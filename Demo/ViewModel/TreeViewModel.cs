using ILikeDapper.Model.Implementation;
using ILikeDapper.Model.Interface;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Demo.ViewModel
{
    public class TreeViewModel : INotifyPropertyChanged
    {
        public TreeViewModel()
        {
          
        }

        public async Task InitTree()
        {
            var provider = new Provider();
           
            provider.CreateTables();
         

            var factory = new Factory();
            var global = factory.CreateDefaultGroup("global");
            for (int i = 0; i < 4; i++)
            {
                var material = factory.CreateDefaultGroup(i.ToString(), global);
                global.Groups.Add(material);
                for (int ii = 0; ii < 4; ii++)
                {
                    material.Groups.Add(factory.CreateDefaultGroup(ii.ToString(), material));
                }
            }

    

            await saveAll(global, provider);

            var tempTree = new ObservableCollection<TreeItemViewModel>();
           
            var rootGroup = await readAll(1, provider);
            var rootViewModel = new TreeItemViewModel(rootGroup.Name);
            tempTree.Add(rootViewModel);

            rootViewModel.Children = new();
            foreach (var item in rootGroup.Groups)
            {
                var vm = new TreeItemViewModel(item.Name);
                vm.Children = new();
                foreach(var subg in item.Groups)
                {
                    var subgVm = new TreeItemViewModel(item.Name);
                    subgVm.Children = new();

                    vm.Children.Add(subgVm);
                }
                rootViewModel.Children.Add(vm);
            }
            this.TreeItems = tempTree;
        }

        private  async Task saveAll(IGroup group, Provider provider)
        {
            var converter = new AttributeConverter();
            await provider.InsertGroup(group);
            foreach (var item in group.Groups)
            {
                await saveAll(item, provider);
            }
            foreach (var item in group.Parameters)
            {
                await provider.InsertParameter(item);
                foreach (var attribute in item.Attributes)
                {
                    await provider.InsertExtendedAttribute(attribute);
                }
            }
        }

        private  async Task<IGroup> readAll(int rootId, Provider provider)
        {
            var root = await provider.GetGroupWithSubGroupsAndParameter(rootId);
            List<IGroup> temp = new();
            foreach (var group in root.Groups)
            {
                var fullSub = await readAll(group.Id.Value, provider);
                temp.Add(fullSub);
            }
            root.Groups = temp;

            foreach (var param in root.Parameters)
            {

                var attributes = await provider.GetAllAttributesForParameter(param.Id.Value);
                foreach (var attribute in attributes)
                {
                    param.Attributes.Add(attribute);
                }
            }

            return root;
        }

        private ObservableCollection<TreeItemViewModel> _treeItems;

        public ObservableCollection<TreeItemViewModel> TreeItems
        {
            get { return _treeItems; }
            set { _treeItems = value; OnPropertyChanged(); }
        }



        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}

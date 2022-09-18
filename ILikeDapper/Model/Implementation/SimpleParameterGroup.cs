using ILikeDapper.Model.Interface;

namespace ILikeDapper.Model.Implementation
{
    public class SimpleParameterGroup : IGroup
    {

        public List<IParameter> Parameters { get; set; } = new List<IParameter>();
        public List<IGroup> Groups { get; set; } = new List<IGroup>();
        public int? Id { get; set ; }
        public IAttribute? Parent { get; set; }
        public int? ParentId { get; set ; }
        public string Name { get; set; } = "";
        public int? Type { get; set; }
    }
}

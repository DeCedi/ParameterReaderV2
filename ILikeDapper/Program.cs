using ILikeDapper.Model.Implementation;
using ILikeDapper.Model.Interface;
using System;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var provider = new Provider();
            Console.WriteLine("creating tables...");
            provider.CreateTables();
            Console.WriteLine("tables created...");

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

            Console.WriteLine("saving all parameters...");

            await saveAll(global, provider);

            Console.WriteLine("all parameters saved");


             //Task.Run(async () => { await readAll(1, provider); });
             //Task.Run(async () => { await readAll(1, provider); });
             //Task.Run(async () => 
             //{  
             //    var main = await readAll(1, provider);
             //    main.Name = "oldFart";
             //    await saveAll(main, provider);   
             //});

            Console.WriteLine("reading...");

            var rootGroup = await readAll(1,provider);

            Console.WriteLine("reading completed!");
            Console.ReadKey();
        }

        private static async Task saveAll(IGroup group, Provider provider)
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

        private static async Task<IGroup> readAll(int rootId, Provider provider)
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
                foreach(var attribute in attributes)
                {
                    param.Attributes.Add(attribute);
                }
            }

            return root;
        }
    }
}
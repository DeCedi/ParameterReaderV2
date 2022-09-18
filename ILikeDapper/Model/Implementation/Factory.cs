using ILikeDapper.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILikeDapper.Model.Implementation
{
    public class AttributeConverter
    {
        public IExtendedAttribute Convert(IValueAttribute<string?> attribute)
        {
            var CompleteAttribute = new ExtendedAttribute() {StringAttribute=attribute.Value,Name=attribute.Name, Parent= attribute.Parent,Id=attribute.Id,Type=attribute.Type};
           
            return CompleteAttribute;
        }

        public IExtendedAttribute Convert(IValueAttribute<int?> attribute)
        {
            var CompleteAttribute = new ExtendedAttribute() { IntAttribute = attribute.Value, Name = attribute.Name, Parent = attribute.Parent, Id = attribute.Id, Type = attribute.Type };

            return CompleteAttribute;
        }

        public IExtendedAttribute Convert(IValueAttribute<double?> attribute)
        {
            var CompleteAttribute = new ExtendedAttribute() { DoulbeAttribute = attribute.Value, Name = attribute.Name, Parent = attribute.Parent, Id = attribute.Id, Type = attribute.Type };

            return CompleteAttribute;
        }
        public IExtendedAttribute Convert(IValueAttribute<bool?> attribute)
        {
            var CompleteAttribute = new ExtendedAttribute() { BoolAttribute = attribute.Value, Name = attribute.Name, Parent = attribute.Parent, Id = attribute.Id, Type = attribute.Type };

            return CompleteAttribute;
        }
    }

    public class Factory
    {

        public IParameter CreateDefaultParameter(string name, IGroup parent)
        {
            var parameter = new SimpleParameter() {Name= name, Parent=parent};
            
            parameter.Attributes.Add(new ExtendedAttribute() { Name = name+"A", StringAttribute = "A", Type = 2, Parent = parameter });
            parameter.Attributes.Add(new ExtendedAttribute() { Name = name+"B", StringAttribute = "B", Type = 2, Parent = parameter });
            parameter.Attributes.Add(new ExtendedAttribute() { Name = name+"D", StringAttribute = "D", Type = 2, Parent = parameter });
            parameter.Attributes.Add(new ExtendedAttribute() { Name = name+"E", StringAttribute = "E", Type = 2, Parent = parameter });
            parameter.Attributes.Add(new ExtendedAttribute() { Name = name+"F", StringAttribute = "F", Type = 2, Parent = parameter });
            parameter.Attributes.Add(new ExtendedAttribute() { Name = name+"G", StringAttribute = "G", Type = 2, Parent = parameter });
            parameter.Attributes.Add(new ExtendedAttribute() { Name = name+"H", StringAttribute = "H", Type = 2, Parent = parameter });
            parameter.Attributes.Add(new ExtendedAttribute() { Name = name+"I", StringAttribute = "I", Type = 2, Parent = parameter });
            parameter.Attributes.Add(new ExtendedAttribute() { Name = name+"J", StringAttribute = "J", Type = 2, Parent = parameter });
            parameter.Attributes.Add(new ExtendedAttribute() { Name = name+"K", StringAttribute = "K", Type = 2, Parent = parameter });
          
            return parameter;
        }

        public SimpleParameterGroup CreateDefaultGroup(string name, IGroup? parent=null)
        {
            var group = new SimpleParameterGroup() {Name=name,Parent=parent };
            group.Parameters.Add(CreateDefaultParameter("P1",group));
            group.Parameters.Add(CreateDefaultParameter("P2", group));
            group.Parameters.Add(CreateDefaultParameter("P3", group));
            group.Parameters.Add(CreateDefaultParameter("P4", group));
            group.Parameters.Add(CreateDefaultParameter("P5", group));
            group.Parameters.Add(CreateDefaultParameter("P6", group));
            group.Parameters.Add(CreateDefaultParameter("P7", group));
            group.Parameters.Add(CreateDefaultParameter("P8", group));
            group.Parameters.Add(CreateDefaultParameter("P9", group));
            group.Parameters.Add(CreateDefaultParameter("P10", group));
            group.Parameters.Add(CreateDefaultParameter("P11", group));
            group.Parameters.Add(CreateDefaultParameter("P12", group));
            group.Parameters.Add(CreateDefaultParameter("P13", group));
            group.Parameters.Add(CreateDefaultParameter("P14", group));
            group.Parameters.Add(CreateDefaultParameter("P15", group));
            group.Parameters.Add(CreateDefaultParameter("P16", group));
            return group;
        }
    }
}

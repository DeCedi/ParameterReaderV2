using ILikeDapper.Model.Interface;

namespace ILikeDapper.Model.Implementation
{
    internal class ExtendedAttribute:IExtendedAttribute 
    {
        public string? StringAttribute { get; set; }
        public double? DoulbeAttribute { get; set; }
        public int? IntAttribute { get; set; }
        public bool? BoolAttribute { get; set; }

        public int? Id { get; set; }
        public int? ParentId { get; set; }
        public int? Type { get; set; }
        public string Name { get; set; }
        public IAttribute? Parent { get; set; }
    }

   
}

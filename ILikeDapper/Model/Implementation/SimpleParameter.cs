using ILikeDapper.Model.Interface;

namespace ILikeDapper.Model.Implementation
{
    public class SimpleParameter : IParameter
    {
        public List<IExtendedAttribute> Attributes { get; set ; } = new ();
        public int? Id { get; set; }
        public IAttribute? Parent { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; } = "";
        public int? Type { get; set; }
    }
}

using ILikeDapper.Model.Interface;

namespace ILikeDapper.Model.Implementation
{
    internal class SimpleAttribute : IAttribute
    {
        public int? Id { get ; set ; }
        public int? ParentId { get; set; }
        public int? Type { get; set ; }
        public string Name { get ; set ; }
        public IAttribute? Parent { get ; set ; }
    }
}

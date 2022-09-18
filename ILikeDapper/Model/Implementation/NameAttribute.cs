using ILikeDapper.Model.Interface;

namespace ILikeDapper.Model.Implementation
{
    internal class NameAttribute : IValueAttribute<string>
    {
        public string Value { get; set; }
        public int? Id { get; set ; }
        public IAttribute? Parent { get; set; }

        public string Name { get; set; }
        public int? Type { get; set; }
    }
}

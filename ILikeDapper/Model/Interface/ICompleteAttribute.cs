namespace ILikeDapper.Model.Interface
{
    public interface IExtendedAttribute:IAttribute
    {
        public string? StringAttribute { get; set; }
        public double? DoulbeAttribute { get; set; }
        public int? IntAttribute { get; set; }
        public bool? BoolAttribute { get; set; }
    }
}


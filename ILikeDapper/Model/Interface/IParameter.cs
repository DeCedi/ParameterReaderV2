namespace ILikeDapper.Model.Interface
{
    public interface IParameter : IAttribute
    {
        public List<IExtendedAttribute> Attributes { get; set; }
    }
}

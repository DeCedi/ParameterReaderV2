namespace ILikeDapper.Model.Interface
{
    public interface IGroup : IAttribute
    {
        public List<IParameter> Parameters { get; set; }
        public List<IGroup> Groups { get; set; }
    }
}

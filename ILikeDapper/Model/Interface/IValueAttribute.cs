namespace ILikeDapper.Model.Interface
{
    public interface IValueAttribute<T>:IAttribute
    {
        public T Value { get; set; }
    }
}

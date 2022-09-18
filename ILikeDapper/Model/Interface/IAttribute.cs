namespace ILikeDapper.Model.Interface
{
    public interface IAttribute
    {
        public int? Id { get; set; }
        public int? Type { get; set; }   
        public string Name { get; set; }    
        public IAttribute? Parent { get; set; }
    }
}


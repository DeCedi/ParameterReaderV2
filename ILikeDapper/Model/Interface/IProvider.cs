namespace ILikeDapper.Model.Interface
{
    public interface IProvider
    {
        public Task<List<IGroup>> GetAllGroups();
    }
}

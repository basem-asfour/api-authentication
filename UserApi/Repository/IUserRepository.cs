using UserApi.Model;

namespace UserApi.Repository
{

    public interface IUserRepository<TEntity>
    {
        IList<TEntity>? List();
        TEntity? Find(string id);
        void Add(TEntity entity);
        void Edit(string id, TEntity entity);
        UserInfo? Login(ViewModel.LoginVM loginVM);
    }
}

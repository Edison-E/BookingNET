namespace BookPro.Domain.interfaces;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAll();
    Task<T> GetById(int id);
    Task<bool> Insert(T entity);
    Task<bool> Update(T entity, int id);
    Task<bool> Delete(int id);

}

namespace BookPro.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly ApplicationDbContext _context;
    public Repository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<T>> GetAll() =>
        await _context.Set<T>().ToListAsync();

    public async Task<T> GetById(int id) => await _context.Set<T>().FindAsync(id);

    public async Task<bool> Insert(T entity)
    {
         await _context.Set<T>().AddAsync(entity);
         bool result = await _context.SaveChangesAsync() > 0;
         return result;     
    }

    public async Task<bool> Update(T entity, int id)
    {
        T existEntity = await _context.Set<T>().FindAsync(id);

        if (existEntity == null)
        {
            return false;
        }

        _context.Entry(existEntity).CurrentValues.SetValues(entity);

        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> Delete(int id)
    {
        var entity = await GetById(id);

        _context.Set<T>().Remove(entity);

        return await _context.SaveChangesAsync() > 0;
    }
}

namespace WebApplicationExample.Services;
public interface IProductsRepository
{
    Task<IEnumerable<object>> GetProductsAsync();
}

namespace WebApplicationExample.Services;
public class ProductsRepository : IProductsRepository
{
    public async Task<IEnumerable<object>> GetProductsAsync()
    {
        await Task.Delay(1000);
        return Enumerable.Range(1, 10).Select(x => $"Product {x}");
    }
}

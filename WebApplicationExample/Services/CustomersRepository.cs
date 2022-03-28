namespace WebApplicationExample.Services;
public class CustomersRepository : ICustomersRepository
{
    public async Task<object> GetCustomerByIdAsync(Guid id)
    {
        await Task.Delay(1000);
        return $"Cusomer {id}";
    }

    public IEnumerable<object> GetCustomers()
    {
        Thread.Sleep(1000);
        return Enumerable.Range(1, 10).Select(x => $"Customer {x}");
    }
}

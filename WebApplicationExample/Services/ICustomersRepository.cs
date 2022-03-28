namespace WebApplicationExample.Services;
public interface ICustomersRepository
{
    IEnumerable<object> GetCustomers();
    Task<object> GetCustomerByIdAsync(Guid id);
}

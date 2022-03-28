using Microsoft.AspNetCore.Mvc;
using WebApplicationExample.Services;

namespace WebApplicationExample.Controllers;
[ApiController]
[Route("[controller]")]
public class ShopController : ControllerBase
{
    private readonly ILogger<ShopController> _logger;
    private readonly IProductsRepository _productsRepository;
    private readonly ICustomersRepository _customersRepository;

    public ShopController(ILogger<ShopController> logger,
        IProductsRepository productsService,
        ICustomersRepository customersService)
    {
        _logger = logger;
        _productsRepository = productsService;
        _customersRepository = customersService;
    }

    [HttpGet("products")]
    // This implementation is correct. It calls synchronous method
    // with await to get a result and returns Task higher as well.
    public async Task<IActionResult> GetProductsAsync()
    {
        var products = await _productsRepository.GetProductsAsync();
        _logger.LogDebug($"Products count: {products.Count()}");

        return Ok(products);
    }

    [HttpGet("customers")]
    // This one is wrong. In fact we wrap synchronous operation by a 
    // Task object, so we lock a thread here. If possible GetCustomers
    // method should be implemented in asynchronous way and then called
    // with await.
    public Task<IActionResult> GetCustomersAsync()
    {
        var customers = _customersRepository.GetCustomers();
        _logger.LogDebug($"Customers count: {customers.Count()}");

        return Task.FromResult((IActionResult)Ok(customers));
    }

    [HttpGet("customers/{id}")]
    // This one needs correction as well. We should change returned type
    // to Task<IActionResult> and call repository in the same way 
    // GetProductsAsync calls its. We now unnecessarily lock a thread on
    // GetResult call.
    public IActionResult GetCustomersById(Guid id)
    {
        var customer = _customersRepository.GetCustomerByIdAsync(id)
            .GetAwaiter()
            .GetResult();
        _logger.LogDebug($"Customer: {customer}");

        return Ok(customer);
    }
}

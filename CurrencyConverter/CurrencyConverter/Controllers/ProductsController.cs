using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
namespace CurrencyConverter.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly HttpClient client;
        private readonly CurrencyConverterClassLibrary.CurrencyConverterClassLibrary lib = new CurrencyConverterClassLibrary.CurrencyConverterClassLibrary();

        public ProductsController(IHttpClientFactory factory)
        {
            client = factory.CreateClient("productsAPI");
        }

        [HttpGet]
        [Route("{productName}/price", Name = "GetPriceOfProduct")]
        async public Task<IActionResult> Get(string productName, [FromQuery] string targetCurrency)
        {
            string productsCSV = await client.GetStringAsync("Prices.csv");
            string exchangeRatesCSV = await client.GetStringAsync("ExchangeRates.csv");

            var products = lib.ParseProductsCSV(productsCSV);
            var exchangeRates = lib.ParseExchangeRatesCSV(exchangeRatesCSV);

            var price = lib.GetPriceInTargetCurrency(targetCurrency, products[productName], exchangeRates);
            return Ok(new APIResponse() { Price = price });
        }
    }
}

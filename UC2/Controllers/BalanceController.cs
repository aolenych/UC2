using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace UC2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BalanceController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public BalanceService _balanceService { get; set; }
        public BalanceTransactionService _balanceTransactionService { get; set; }

        public BalanceController(IConfiguration configuration)
        {
            _configuration = configuration;
            _balanceService = new BalanceService();
            _balanceTransactionService = new BalanceTransactionService();
        }

        [HttpGet("GetBalance")]
        public ActionResult GetBalance()
        {

            var requestOptions = new RequestOptions
            {
                ApiKey = _configuration["Stripe:SecretKey"],

            };
            try
            {
                var response = _balanceService.Get(requestOptions);
                return Ok(response);
            }
            catch (StripeException exception)
            {
                return BadRequest(exception.StripeResponse);
            }
            catch (Exception)
            {
                return BadRequest("An error occurred while processing the request.");
            }

        }

        [HttpGet("GetBalanceTransactions")]
        public ActionResult GetBalanceTransactions()
        {
            var balanceTransactionListOptions = new BalanceTransactionListOptions
            {
                Limit = 5
            };

            var requestOptions = new RequestOptions
            {
                ApiKey = _configuration["Stripe:SecretKey"]
            };
            try
            {
                StripeList<BalanceTransaction> res = _balanceTransactionService.List(balanceTransactionListOptions, requestOptions);
                return Ok(res);
            }
            catch (StripeException exception)
            {
                return BadRequest(exception.StripeResponse);
            }
            catch (Exception)
            {
                return BadRequest("An error occurred while processing the request.");
            }
        }
    }
}
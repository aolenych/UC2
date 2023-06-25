using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Stripe;
using UC2.Controllers;

namespace UC2.Tests.Controllers
{
    [TestFixture]
    public class BalanceControllerTests
    {
        private Mock<IConfiguration> _configurationMock;
        private Mock<BalanceService> _balanceServiceMock;
        private Mock<BalanceTransactionService> _balanceTransactionServiceMock;
        private BalanceController _balanceController;

        [SetUp]
        public void Setup()
        {
            _configurationMock = new Mock<IConfiguration>();
            _configurationMock.Setup(c => c["Stripe:SecretKey"]).Returns("secret_key");

            _balanceServiceMock = new Mock<BalanceService>();
            _balanceTransactionServiceMock = new Mock<BalanceTransactionService>();
            _balanceController = new BalanceController(_configurationMock.Object);

            _balanceController._balanceService = _balanceServiceMock.Object;
            _balanceController._balanceTransactionService = _balanceTransactionServiceMock.Object;
        }

        [Test]
        public void GetBalance_OkResultTest()
        {
            // Arrange
            var resp = new Balance();
            _balanceServiceMock.Setup(s => s.Get(It.IsAny<RequestOptions>())).Returns(resp);

            // Act
            var result = _balanceController.GetBalance() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Value, Is.EqualTo(resp));

        }

        [Test]
        public void GetBalance_StripeExceptionResultTest()
        {
            // Arrange
            var resp = new StripeException() { StripeResponse = new StripeResponse(System.Net.HttpStatusCode.BadRequest, null, "badResp") };
            _balanceServiceMock.Setup(s => s.Get(It.IsAny<RequestOptions>())).Throws(resp); ;

            // Act
            var result = _balanceController.GetBalance() as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Value, Is.EqualTo(resp.StripeResponse));

        }

        [Test]
        public void GetBalance_DefaultExceptionResultTest()
        {
            // Arrange
            var exceptionMessage = "An error occurred while processing the request.";
            _balanceServiceMock.Setup(s => s.Get(It.IsAny<RequestOptions>())).Throws(new Exception()); ;

            // Act
            var result = _balanceController.GetBalance() as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Value, Is.EqualTo(exceptionMessage));

        }

        [Test]
        public void GetBalanceTransactions_OkResultTest()
        {
            // Arrange
            var resp = new StripeList<BalanceTransaction>();
            _balanceTransactionServiceMock.Setup(s => s.List(It.IsAny<BalanceTransactionListOptions>(), It.IsAny<RequestOptions>())).Returns(resp);

            // Act
            var result = _balanceController.GetBalanceTransactions() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Value, Is.EqualTo(resp));
        }

        [Test]
        public void GetBalanceTransactions_StripeExceptionResultTest()
        {
            // Arrange
            var resp = new StripeException() { StripeResponse = new StripeResponse(System.Net.HttpStatusCode.BadRequest, null, "badResp") };
            _balanceTransactionServiceMock.Setup(s => s.List(It.IsAny<BalanceTransactionListOptions>(), It.IsAny<RequestOptions>())).Throws(resp);

            // Act
            var result = _balanceController.GetBalanceTransactions() as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Value, Is.EqualTo(resp.StripeResponse));

        }

        [Test]
        public void GetBalanceTransactions_DefaultExceptionResultTest()
        {
            // Arrange
            var exceptionMessage = "An error occurred while processing the request.";
            _balanceTransactionServiceMock.Setup(s => s.List(It.IsAny<BalanceTransactionListOptions>(), It.IsAny<RequestOptions>())).Throws(new Exception());

            // Act
            var result = _balanceController.GetBalanceTransactions() as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Value, Is.EqualTo(exceptionMessage));

        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;
using RouletteAPI4.Interfaces;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace RouletteAPI4.Tests
{
    [TestClass]
    public class RouletteControllerTests
    {
        private RouletteController _controller;
        private Mock<IBetRepository> _mockBetRepository;

        [TestInitialize]
        public void Initialize()
        {
            _mockBetRepository = new Mock<IBetRepository>();
            _controller = new RouletteController(_mockBetRepository.Object);
        }

        [TestMethod]
        public void GetBetsFromDB_ReturnsOkResult()
        {
            var result = _controller.GetBetsFromDB();
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void PlaceBet_ValidBet_ReturnsOkResult()
        {
            var result = _controller.PlaceBet("player1", 100, BetType.Red, "red");
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void PlaceBet_InvalidBet_ReturnsBadRequestResult()
        {
            var result = _controller.PlaceBet("player1", -100, BetType.Red, "red");
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public void Spin_NoBetsPlaced_ReturnsOkResult()
        {
            var result = _controller.Spin();
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void Payout_ReturnsOkResult()
        {
            var result = _controller.Payout("player1");
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void ShowPreviousSpins_ReturnsOkResult()
        {
            var result = _controller.ShowPreviousSpins();
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }
    }
}
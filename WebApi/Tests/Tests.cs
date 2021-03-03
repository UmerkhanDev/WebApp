using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Controllers;
using WebApi.Models;
using Xunit;

namespace WebApi.Tests
{
    public class Tests
    {
        private readonly PaymentController paymentController;
        public Tests(PaymentController paymentController)
        {
            this.paymentController = paymentController;
        }

        [Fact]
        public void Get_WhenCalled_ReturnsOkResult()
        {
            var testData = new DTO
            {
                CreditCardNumber = "4245124512451245",
                CardHolder = "umer khan",
                Amount = Convert.ToDecimal(20.00),
                ExpirationDate = Convert.ToDateTime("2030-01-06T17:16:40"),
                SecurityCode = "123"
            };

            var okResult = paymentController.Post(testData) as OkObjectResult;

            Assert.NotNull(okResult);
        }
    }
}

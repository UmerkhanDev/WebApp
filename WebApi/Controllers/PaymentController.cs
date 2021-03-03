using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebApi.ExternalPaymentGateways;
using WebApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {

        private readonly IStripePaymentGateway stripePaymentGateway;

        public PaymentController(IStripePaymentGateway stripePaymentGateway)
        {
            this.stripePaymentGateway = stripePaymentGateway;
        }

        #region Public
        //// GET: api/<PaymentController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/<PaymentController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/<PaymentController>
        [HttpPost]
        public IActionResult Post(DTO paymentInfo)
        {
            try
            {
                ValidateObject(paymentInfo);
                var response = MakePayment(paymentInfo);
                return Ok();
            }
            catch (Exception e)
            {
                //throw new System.Net.WebException("Server in not respondind");
                return BadRequest();
            }
        }

        //// PUT api/<PaymentController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<PaymentController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}

        #endregion

        #region Private

        private async Task<dynamic> MakePayment(DTO paymentInfo)
        {
            decimal min = Convert.ToDecimal(20.00);
            decimal max = Convert.ToDecimal(500.00);

            if (paymentInfo.Amount < min)
            {
                var response = await stripePaymentGateway.StripePay(paymentInfo);
                return response;
            }
            else if (paymentInfo.Amount > min && paymentInfo.Amount < max)
            {
                var response = await stripePaymentGateway.StripePay(paymentInfo);
                return response;
            }
            else
            {
                var response = await stripePaymentGateway.StripePay(paymentInfo);
                return response;
            }
        }

        private void ValidateObject(DTO paymentInfo)
        {
            if (!IsCreditCardInfoValid(paymentInfo.CreditCardNumber, paymentInfo.ExpirationDate, paymentInfo.SecurityCode))
            {
                throw new System.Net.WebException("Credit card info is not valid");
            }
            else if (string.IsNullOrEmpty(paymentInfo.CardHolder))
            {
                throw new System.Net.WebException("Enter name for CardHolder");
            }
            else if (!ValidateAmount(paymentInfo.Amount))
            {
                throw new System.Net.WebException("Please enter amount in the format of 00.00");
            }
        }

        private bool ValidateAmount(decimal amount)
        {
            if (amount.ToString().Contains(".") && amount > 0)
                return true;
            return false;
        }

        private bool IsCreditCardInfoValid(string cardNo, DateTime date, string cvv)
        {
            var cardCheck = new Regex(@"^4[0-9]{12}(?:[0-9]{3})?$");
            var monthCheck = new Regex(@"^(0[1-9]|1[0-2])$");
            var yearCheck = new Regex(@"^20[0-9]{2}$");
            var cvvCheck = new Regex(@"^\d{3}$");

            if (!cardCheck.IsMatch(cardNo)) //check card number is valid
                return false;
            if (!cvvCheck.IsMatch(cvv)) //check cvv is valid as "999"
                return false;

            var expiryDate = date.Month.ToString("D2") + "/" + date.Year;
            var dateParts = expiryDate.Split('/'); //expiry date in from MM/yyyy            
            if (!monthCheck.IsMatch(dateParts[0]) || !yearCheck.IsMatch(dateParts[1]))
                return false; //check date format is valid as "MM/yyyy"

            var year = int.Parse(dateParts[1]);
            var month = int.Parse(dateParts[0]);
            var lastDateOfExpiryMonth = DateTime.DaysInMonth(year, month); //get actual expiry date
            var cardExpiry = new DateTime(year, month, lastDateOfExpiryMonth, 23, 59, 59);

            //check expiry greater than today
            return (cardExpiry > DateTime.Now);
        }

        #endregion
    }
}

using Stripe;
using System;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.PaymentRepository;

namespace WebApi.ExternalPaymentGateways
{
    public class StripePaymentGateway : IStripePaymentGateway
    {
        private readonly IPaymentRepo paymentRepo;
        public StripePaymentGateway(IPaymentRepo paymentRepo)
        {
            this.paymentRepo = paymentRepo;
        }

        #region Public

        public async Task<dynamic> StripePay(DTO paymentInfo)
        {
            try
            {
                Guid uniqueKey = new Guid();
                StripeConfiguration.ApiKey = "sk_test_51IQGqvGzzHo1j6wiwbIoVzYWUlD6KxNeTDJjWvwzFyViz7ozKktPTadUUFluLfKiDDRIHLb2aKR4XsLMkLxy4S6h00K4L35PTG";

                var optionsToken = new TokenCreateOptions
                {
                    Card = new TokenCardOptions
                    {
                        Number = paymentInfo.CreditCardNumber,
                        ExpMonth = paymentInfo.ExpirationDate.Date.Month,
                        ExpYear = paymentInfo.ExpirationDate.Date.Year,
                        Cvc = paymentInfo.SecurityCode,
                    },
                };

                var serviceToken = new TokenService();
                Token stripeToken = await serviceToken.CreateAsync(optionsToken);

                var options = new ChargeCreateOptions
                {
                    Amount = Convert.ToInt64(paymentInfo.Amount),
                    Currency = "usd",
                    Description = "test",
                    Source = stripeToken.Id
                };

                var service = new ChargeService();
                service.Create(options, new RequestOptions
                {
                    IdempotencyKey = uniqueKey.ToString(),
                });
                Charge charge = await service.CreateAsync(options);

                if (charge.Paid)
                {
                    Guid paymentId = paymentRepo.CreatePayment(MapPayment(paymentInfo));
                    Guid statusId = paymentRepo.CreateStatus(MapStatus("success"));
                    paymentRepo.CreatePaymentStatus(MapPaymentStatus(paymentId, statusId));

                    return "success";
                }
                else
                {
                    Guid paymentId = paymentRepo.CreatePayment(MapPayment(paymentInfo));
                    Guid statusId = paymentRepo.CreateStatus(MapStatus("failed"));
                    paymentRepo.CreatePaymentStatus(MapPaymentStatus(paymentId, statusId));
                    return "failed";
                }

            }
            catch (Exception e)
            {
                Guid paymentId = paymentRepo.CreatePayment(MapPayment(paymentInfo));
                Guid statusId = paymentRepo.CreateStatus(MapStatus("exception"));
                paymentRepo.CreatePaymentStatus(MapPaymentStatus(paymentId, statusId));
                return "failed";
            }
        }

        #endregion

        #region Private

        private Payment MapPayment(DTO payment)
        {
            Payment paymentResponse = new Payment();
            paymentResponse.CreditCardNumber = payment.CreditCardNumber;
            paymentResponse.CardHolder = payment.CardHolder;
            paymentResponse.Amount = payment.Amount;
            paymentResponse.ExpirationDate = payment.ExpirationDate;
            paymentResponse.SecurityCode = payment.SecurityCode;

            return paymentResponse;
        }

        private Status MapStatus(string status)
        {
            Status response = new Status();
            response.PaymentStatus = status;

            return response;
        }

        private PaymentStatus MapPaymentStatus(Guid paymentId, Guid statusId)
        {
            PaymentStatus response = new PaymentStatus();
            response.PaymentId = paymentId;
            response.StatusId = statusId;

            return response;
        }

        #endregion
    }
}

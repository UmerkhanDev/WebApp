using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.ExternalPaymentGateways
{
    public interface IStripePaymentGateway
    {
        Task<dynamic> StripePay(DTO paymentInfo);
    }
}

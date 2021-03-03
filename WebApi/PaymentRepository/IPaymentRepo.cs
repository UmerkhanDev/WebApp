using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.PaymentRepository
{
    public interface IPaymentRepo
    {
        Guid CreatePayment(Payment payment);
        Guid CreateStatus(Status status);
        void CreatePaymentStatus(PaymentStatus paymentStatus);
    }
}

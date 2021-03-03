using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.DataContext;
using WebApi.Models;

namespace WebApi.PaymentRepository
{
    public class PaymentRepo : IPaymentRepo
    {
        private MyDbContext context;
        public PaymentRepo(MyDbContext context)
        {
            this.context = context;
        }

        public Guid CreatePayment(Payment payment)
        {
            payment.Id = Guid.NewGuid();
            context.payments.Add(payment);
            context.SaveChanges();

            return payment.Id;
        }

        public Guid CreateStatus(Status status)
        {
            var exsistingStatus = context.status.Where(x => x.paymentStatus == status.paymentStatus).FirstOrDefault();
            if (exsistingStatus != null)
            {
                return exsistingStatus.Id;
            }
            else
            {
                context.status.Add(status);
                context.SaveChanges();

                return status.Id;
            }
        }

        public void CreatePaymentStatus(PaymentStatus paymentStatus)
        {
            context.paymentStatus.Add(paymentStatus);
            context.SaveChanges();
        }

    }
}

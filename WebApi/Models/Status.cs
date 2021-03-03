using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class Status
    {
        public Guid Id { get; set; }
        public string PaymentStatus { get; set; }
        public ICollection<PaymentStatus> paymentStatus { get; set; }
    }
}

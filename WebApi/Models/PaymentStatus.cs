using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class PaymentStatus
    {
        public Guid PaymentId { get; set; }
        public Payment payment { get; set; }
        public Guid StatusId { get; set; }
        public Status status { get; set; }
    }
}

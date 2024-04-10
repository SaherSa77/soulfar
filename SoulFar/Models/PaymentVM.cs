using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SoulFar.Models
{
    public class PaymentVM
    {
        public int OrderID { get; set; }
        public int TotalAmount { get; set; }
        public int CustomerID { get; set; }
        public int paymentID { get; set; }
    }
}
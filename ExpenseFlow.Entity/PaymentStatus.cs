using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseFlow.Entity
{
    public enum PaymentStatus
    {

        /// <summary>
        /// Fiş ödeme durumlarını temsil eden enum.
        /// Manager masraf onayladığında ödenebilir veya bekletilebilir.
        /// </summary>
        

        Paid = 1,//ödendi
        Pending=0//beklemede

    }
}

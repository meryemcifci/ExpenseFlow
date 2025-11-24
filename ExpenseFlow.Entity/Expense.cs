using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseFlow.Entity
{
    public class Expense
    {
        public int Id { get; set; }

        public string Description { get; set; } // Masraf açıklaması

        public decimal Amount { get; set; } // Tutar

        public DateTime Date { get; set; } // Harcama Tarihi

        public string ReceiptImage { get; set; } // Fişin dosya yolu (Örn: "/img/fis1.jpg")

        public ExpenseStatus Status { get; set; } = ExpenseStatus.Pending; // Varsayılan: Bekliyor

       
        // 1. Bu masrafı kim yaptı?
        public int UserId { get; set; }
        public AppUser User { get; set; }

        // 2. Bu masraf ne için yapıldı?
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}

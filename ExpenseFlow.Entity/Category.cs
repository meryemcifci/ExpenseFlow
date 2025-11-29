using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseFlow.Entity
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } // "Yemek", "Ulaşım", "Konaklama"
        public string? Description { get; set; }

      
        // Bir kategoride birden çok masraf olabilir.
        public List<Expense> Expenses { get; set; }
    }
}

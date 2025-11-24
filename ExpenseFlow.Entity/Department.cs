using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseFlow.Entity
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; } //"Yazılım", "Muhasebe"

      
        // Bir departmanda birden çok çalışan olur.
        public List<AppUser> Users { get; set; }
    }
}

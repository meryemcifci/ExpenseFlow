using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseFlow.Entity
{
    public class AppRole:IdentityRole<int>
    {
        //IdentityRole sınıfı zaten Id ve Name özelliklerini içeriyor.
        //belki ilerde ekleme yaparım.
    }
}


using Microsoft.AspNetCore.Identity;

namespace ExpenseFlow.Entity
{
    public class AppRole:IdentityRole<int>
    {
        //IdentityRole sınıfı zaten Id ve Name özelliklerini içeriyor.
        //belki ilerde ekleme yaparım.
    }
}


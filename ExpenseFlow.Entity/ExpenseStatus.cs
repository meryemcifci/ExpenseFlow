using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseFlow.Entity
{

    /// <summary>
    /// Kullanıcı masraf durumlarını temsil eden enum.
    /// Çalışan masraf eklediğinde masraf durumu "Bekliyor" olarak ayarlanır.
    /// Yönetici onayladığında "Onaylandı", reddettiğinde ise "Reddedildi" olarak güncellenir.
    /// </summary>

    public enum ExpenseStatus
    {
        Pending = 1,    // Bekliyor
        Approved = 2,   // Onaylandı
        Rejected = 3    //Reddedildi
    }
}

using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ExpenseFlow.Core.ViewModels
{
//    PROFİLE SAYFASINDA KULLANICI PROFİLİ DÜZENLE BUTONUN BASINCA İÇİNDEKİ VERİLERİ TAŞIYAN MODEL
    public class UpdateProfileViewModel
    {
            public int UserId { get; set; }   

            public string AboutMe { get; set; }

            public string Address { get; set; }
            public string City { get; set; }
            public string Country { get; set; }
            public string District { get; set; }



    }
}

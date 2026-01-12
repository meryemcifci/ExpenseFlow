using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

public class UpdateProfilePhotoViewModel
{
    public int UserId { get; set; }

    [Required(ErrorMessage = "Profil fotoğrafı seçilmelidir.")]
    public IFormFile ProfileImage { get; set; }
}

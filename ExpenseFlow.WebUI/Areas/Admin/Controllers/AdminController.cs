using ExpenseFlow.Business.Abstract;
using ExpenseFlow.Business.DTOs;
using ExpenseFlow.Entity;
using ExpenseFlow.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ExpenseFlow.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly IDepartmentService _departmentService;
        private readonly IUserService _userService;


        public AdminController(ILogger<AdminController> logger,UserManager<AppUser> userManager, IDepartmentService departmentService,IUserService userService)
        {
            _logger = logger;
            _userManager = userManager;
            _departmentService = departmentService;
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> PromoteToManager(string userId)
        {
            Console.WriteLine("POST ACTION ÇALIŞTI");
            //zaten UI'da buton yok
            var currentUserId = _userManager.GetUserId(User);

            if (userId == currentUserId)
            {
                TempData["Error"] = "Admin kendi rolünü değiştiremez.";
                return RedirectToAction("Index");
            }

            //  Kullanıcıyı bul
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            //Kullanıcının departmanı var mı?
            if (user.DepartmentId == null)
            {
                TempData["Error"] = "Bu kullanıcının bir departmanı yok, yönetici yapılamaz!";
                return RedirectToAction("Index"); // Kullanıcı listesine dön
            }

            // İŞ KURALI KONTROLÜ 
            try
            {
                // Eğer o departmanda zaten manager varsa bu satır HATA fırlatır ve catch'e düşeriz.
                _departmentService.CheckIfDepartmentHasManager((int)user.DepartmentId);
            }
            catch (InvalidOperationException ex)
            {
                // Hata mesajını ekrana bas
                //Zatem yönetici varken yeni yönetici atayamazsın.
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index");
            }

            // ROL DEĞİŞİMİ 
            // Önce mevcut rollerini al (Employee)
            var userRoles = await _userManager.GetRolesAsync(user);

            // Employee rolünü sil
            await _userManager.RemoveFromRolesAsync(user, userRoles);

            // Manager rolünü ekle
            var result = await _userManager.AddToRoleAsync(user, "Manager");

            if (result.Succeeded)
            {
                TempData["Success"] = $"{user.UserName} başarıyla departman yöneticisi yapıldı.";
                
            }
            else
            {
                TempData["Error"] = "Rol atama işlemi sırasında bir hata oluştu.(Manager)";
            }
          

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PromoteToAccountant(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["Error"] = "Kullanıcı bulunamadı.";
                return RedirectToAction("Index");
            }

            var roles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, roles);

            var result = await _userManager.AddToRoleAsync(user, "Accountant");

            if (result.Succeeded)
            {
                TempData["Success"] = $"{user.UserName} başarıyla Muhasebeci yapıldı.";
            }
            else
            {
                // ⛔ Burada hata varsa gör
                TempData["Error"] = string.Join(" | ", result.Errors.Select(e => e.Description));
            }

            return RedirectToAction("Index");
        }



        public IActionResult Index()
        {
            var userList = _userService.GetUserListWithDetails();

            return View(userList);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

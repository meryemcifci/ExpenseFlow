#region Using Directives
using ExpenseFlow.Business.Abstract;
using ExpenseFlow.Business.Mapping;
using ExpenseFlow.Business.Services;
using ExpenseFlow.Data.Abstract;
using ExpenseFlow.Data.Concrete;
using ExpenseFlow.Data.Context;
using ExpenseFlow.DataAccess.Abstract;
using ExpenseFlow.DataAccess.Concrete;
using ExpenseFlow.Entity;
using ExpenseFlow.WebUI.Hubs;
using ExpenseFlow.WebUI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

#endregion

var builder = WebApplication.CreateBuilder(args);

#region DbContext and Identity
// DbContext
builder.Services.AddDbContext<ExpenseFlowContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<AppUser, AppRole>() 
    .AddEntityFrameworkStores<ExpenseFlowContext>()
    .AddDefaultTokenProviders();

#endregion

#region Servisler
//Servisler
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IExpenseService, ExpenseService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IReportService, ReportService>();

// WebUI içinde bulunan arayüz için hazırlanan servis
builder.Services.AddScoped<INotificationPublisher, SignalRNotificationPublisher>();

//SignalR
builder.Services.AddSignalR();
#endregion

#region Mapping

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

#endregion

#region Data Access Layers
//Dal
builder.Services.AddScoped<IExpenseDal, ExpenseDal>();
builder.Services.AddScoped<ICategoryDal, CategoryDal>();
builder.Services.AddScoped<IDepartmentDal, DepartmentDal>();
builder.Services.AddScoped<IUserDal, UserDal>();
builder.Services.AddScoped<INotificationDal, NotificationDal>();
builder.Services.AddScoped<IReportDal, ReportDal>();


#endregion

// Add services to the container.
builder.Services.AddControllersWithViews();

#region Cookie Ayarları
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Auth/Login";
    options.LogoutPath = "/Auth/Logout";

    options.Cookie.Name = "ExpenseFlow.Auth";
    options.Cookie.HttpOnly = true;

    options.ExpireTimeSpan = TimeSpan.FromDays(7);  // 7 gün hatırlasın
    options.SlidingExpiration = true;               //Her girişte süre uzasın
});
#endregion

var app = builder.Build();


#region Seed Data
//admin ekleme
//using (var scope = app.Services.CreateScope())
//{
//    var serviceProvider = scope.ServiceProvider;
//    try
//    {
//        var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
//        var roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();
//        var context = serviceProvider.GetRequiredService<ExpenseFlowContext>(); // Context'i de çağırdık

//        // 1. ROLLERİ KONTROL ET / OLUŞTUR
//        string[] roleNames = { "Admin", "Manager", "Employee" };
//        foreach (var roleName in roleNames)
//        {
//            if (!await roleManager.RoleExistsAsync(roleName))
//            {
//                await roleManager.CreateAsync(new AppRole { Name = roleName });
//            }
//        }

//        // 2. ÖNCE DEPARTMAN OLUŞTUR (HATA BURADAN KAYNAKLANIYORDU!)
//        // Eğer veritabanında hiç departman yoksa, Admin için bir tane oluşturalım.
//        int adminDepartmentId;
//        var existingDept = await context.Departments.FirstOrDefaultAsync(d => d.Name == "Yönetim");

//        if (existingDept == null)
//        {
//            var newDept = new Department { Name = "Yönetim" }; // Entity ismin 'Department' ise onu kullan
//            context.Departments.Add(newDept);
//            await context.SaveChangesAsync(); // Kaydet ki ID oluşsun
//            adminDepartmentId = newDept.Id;
//        }
//        else
//        {
//            adminDepartmentId = existingDept.Id;
//        }

//        // 3. ADMIN KULLANCISINI OLUŞTUR
//        var adminEmail = "admin@expenseflow.com";
//        var adminUser = await userManager.FindByEmailAsync(adminEmail);

//        if (adminUser == null)
//        {
//            var newAdmin = new AppUser
//            {
//                UserName = "admin",
//                Email = adminEmail,
//                FirstName = "System",
//                LastName = "Admin",

//                // ARTIK GEÇERLİ BİR DEPARTMAN ID VERİYORUZ:
//                DepartmentId = adminDepartmentId,

//                EmailConfirmed = true,
//                PhoneNumberConfirmed = true
//            };

//            var result = await userManager.CreateAsync(newAdmin, "Admin123!");

//            if (result.Succeeded)
//            {
//                await userManager.AddToRoleAsync(newAdmin, "Admin");
//                Console.WriteLine("\n*** Admin kullanıcısı, Departmanı ve Rolleri başarıyla eklendi! ***\n");
//            }
//            else
//            {
//                Console.ForegroundColor = ConsoleColor.Red;
//                foreach (var error in result.Errors)
//                {
//                    Console.WriteLine($"[IDENTITY HATA] {error.Code}: {error.Description}");
//                }
//                Console.ResetColor();
//            }
//        }
//    }
//    catch (Exception ex)
//    {
//        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
//        logger.LogError(ex, "Seed Data hatası!");
//    }
//}


//Rol ve Admin Kullanıcı Oluşturma (oluşturduktan sonra youm satırına aldım.)
//using (var scope = app.Services.CreateScope())
//{
//    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

//    string[] roles = { "Employee", "Manager", "Accountant", "Admin" };

//    foreach (var role in roles)
//    {
//        if (!await roleManager.RoleExistsAsync(role))
//        {
//            await roleManager.CreateAsync(new AppRole { Name = role });
//        }
//    }
//}

//Departman Ekleme(oluşturduktan sonra youm satırına aldım.)
//using (var scope = app.Services.CreateScope())
//{
//    var context = scope.ServiceProvider.GetRequiredService<ExpenseFlowContext>();

//    if (!context.Departments.Any())
//    {
//        context.Departments.AddRange(
//            new Department { Name = "IT" },
//            new Department { Name = "Muhasebe" },
//            new Department { Name = "İnsan Kaynakları" },
//            new Department { Name = "Finans" },
//            new Department { Name = "Ar-Ge" },
//            new Department { Name = "Hukuk" }
//        );

//        context.SaveChanges();
//    }
//}


//kategori ekledik
//using (var scope = app.Services.CreateScope())
//{
//    var context = scope.ServiceProvider.GetRequiredService<ExpenseFlowContext>();

//    if (!context.Categories.Any())
//    {
//        context.Categories.AddRange(
//            new Category { Name = "Ulaşım" },
//            new Category { Name = "Yemek" },
//            new Category { Name = "Konaklama" },
//            new Category { Name = "Ofis Gideri" },
//            new Category { Name = "Diğer" }
//        );

//        context.SaveChanges();
//    }
//}


////Manager Kullanıcı
//using (var scope = app.Services.CreateScope())
//{
//    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
//    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

//    // Manager rolü yoksa oluştur
//    if (!await roleManager.RoleExistsAsync("Manager"))
//    {
//        await roleManager.CreateAsync(new AppRole { Name = "Manager" });
//    }

//    var existingUser = await userManager.FindByEmailAsync("testmanager1@gmail.com");

//    if (existingUser == null)
//    {
//        var user = new AppUser
//        {
//            UserName = "testmanager1@gmail.com",
//            Email = "testmanager1@gmail.com",
//            FirstName = "Test",
//            LastName = "Manager",
//            DepartmentId = 3,
//            EmailConfirmed = true
//        };

//        var result = await userManager.CreateAsync(user, "TestManager123#");

//        if (result.Succeeded)
//        {
//            await userManager.AddToRoleAsync(user, "Manager");
//        }
//    }
//}
#endregion

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Employee/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();
app.MapHub<NotificationHub>("/notificationHub");


#region Map
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

#endregion

app.Run();

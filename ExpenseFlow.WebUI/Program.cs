using ExpenseFlow.Business.Abstract;
using ExpenseFlow.Business.Services;
using ExpenseFlow.Data.Abstract;
using ExpenseFlow.Data.Concrete;
using ExpenseFlow.Data.Context;
using ExpenseFlow.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// DbContext
builder.Services.AddDbContext<ExpenseFlowContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<AppUser, AppRole>() 
    .AddEntityFrameworkStores<ExpenseFlowContext>()
    .AddDefaultTokenProviders();

//Servisler
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IExpenseService, ExpenseService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();


//Dal
builder.Services.AddScoped<IExpenseDal, ExpenseDal>();
builder.Services.AddScoped<ICategoryDal, CategoryDal>();

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Auth/Login";
    options.LogoutPath = "/Auth/Logout";

    options.Cookie.Name = "ExpenseFlow.Auth";
    options.Cookie.HttpOnly = true;

    options.ExpireTimeSpan = TimeSpan.FromDays(7);  // 7 gün hatırlasın
    options.SlidingExpiration = true;               //Her girişte süre uzasın
});

var app = builder.Build();


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

//Departman Ekleme (oluşturduktan sonra youm satırına aldım.)
//using (var scope = app.Services.CreateScope())
//{
//    var context = scope.ServiceProvider.GetRequiredService<ExpenseFlowContext>();

//    if (!context.Departments.Any())
//    {
//        context.Departments.AddRange(
//            new Department { Name = "IT" },
//            new Department { Name = "Muhasebe" },
//            new Department { Name = "İnsan Kaynakları" }
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

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();

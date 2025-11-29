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

    options.ExpireTimeSpan = TimeSpan.FromDays(7);  // 7 gün hatýrlasýn
    options.SlidingExpiration = true;               //Her giriþte süre uzasýn
});

var app = builder.Build();


//Rol ve Admin Kullanýcý Oluþturma (oluþturduktan sonra youm satýrýna aldým.)
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

//Departman Ekleme (oluþturduktan sonra youm satýrýna aldým.)
//using (var scope = app.Services.CreateScope())
//{
//    var context = scope.ServiceProvider.GetRequiredService<ExpenseFlowContext>();

//    if (!context.Departments.Any())
//    {
//        context.Departments.AddRange(
//            new Department { Name = "IT" },
//            new Department { Name = "Muhasebe" },
//            new Department { Name = "Ýnsan Kaynaklarý" }
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
//            new Category { Name = "Ulaþým" },
//            new Category { Name = "Yemek" },
//            new Category { Name = "Konaklama" },
//            new Category { Name = "Ofis Gideri" },
//            new Category { Name = "Diðer" }
//        );

//        context.SaveChanges();
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

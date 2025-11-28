using ExpenseFlow.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExpenseFlow.Data.Context
{
    public class ExpenseFlowContext : IdentityDbContext<AppUser,AppRole, int>
    {
        
        public ExpenseFlowContext(DbContextOptions<ExpenseFlowContext> options) : base(options)
        {

        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Expense> Expenses { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
          
        }

    }
}

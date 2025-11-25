using ExpenseFlow.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Formats.Tar;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseFlow.Data.Context
{
    public class ExpenseFlowContext : IdentityDbContext<AppUser, IdentityRole<int>, int>
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

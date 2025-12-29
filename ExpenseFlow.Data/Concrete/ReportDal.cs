using ExpenseFlow.Data.Abstract;
using ExpenseFlow.Data.Context;
using ExpenseFlow.Entity;
using Microsoft.EntityFrameworkCore;
using System;

namespace ExpenseFlow.Data.Concrete
{
    public class ReportDal : IReportDal
    {
        private readonly ExpenseFlowContext _context;

        public ReportDal(ExpenseFlowContext context)
        {
            _context = context;
        }
        public async Task<List<Expense>> GetUserExpenseReportDataAsync(int userId)
        {
            return await _context.Expenses
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .Include(x => x.Category)
                .Include(x => x.User)
                    .ThenInclude(u => u.Department)
                .OrderByDescending(x => x.Date)
                .ToListAsync();
        }
    }
}

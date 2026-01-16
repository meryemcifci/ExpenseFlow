using ExpenseFlow.Data.Abstract;
using ExpenseFlow.Data.Context;
using ExpenseFlow.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Cryptography.X509Certificates;

namespace ExpenseFlow.Data.Concrete
{
    public class ExpenseDal : IExpenseDal
    {
        private readonly ExpenseFlowContext _context;

        public ExpenseDal(ExpenseFlowContext context)
        {
            _context = context;
        }


        public void Delete(int id)
        {
            try
            {
                var expense = _context.Expenses.Find(id);
                _context.Expenses.Remove(expense);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Masraf ekleme işlemi sırasında bir hata oluştu. Tekrar deneyiniz.");
                throw;
            }
        }

        public List<Expense> GetPendingWithCategory()
        {
            return _context.Expenses
                .Include(x => x.Category)
                .Where(x => x.Status == ExpenseStatus.Pending)
                .ToList();
        }

        public Expense GetById(int id)
        {
            try
            {
                return _context.Expenses
                 .Include(x => x.Category)
                 .FirstOrDefault(x => x.Id == id);




            }
            catch (Exception ex)
            {
                Console.WriteLine("Masraf bulma işlemi sırasında bir hata oluştu.");
                throw;
            }
        }

        public List<Expense> GetList()
        {
            try
            {
                return _context.Expenses.ToList();


            }
            catch (Exception ex)
            {
                Console.WriteLine("Masraf listemele işlemi sırasında bir hata oluştu.");
                throw;
            }
        }

        public void Insert(Expense expense)
        {
            try
            {
                _context.Expenses.Add(expense);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Masraf ekleme işlemi sırasında bir hata oluştu. Tekrar deneyiniz.");
                throw;
            }
        }

        public void Update(Expense expense)
        {
            try
            {
                _context.Expenses.Update(expense);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Masraf güncelleme işlemi sırasında bir hata oluştu. Tekrar deneyiniz.");
                throw;
            }
        }

        public List<Expense> GetApproveWithCategory()
        {
            return _context.Expenses
                .Include(x => x.Category)
                .Where(x => x.Status == ExpenseStatus.Approved)
                .ToList();
        }

        public List<Expense> GetRejectWithCategory()
        {
            return _context.Expenses
                .Include(x => x.Category)
                .Where(x => x.Status == ExpenseStatus.Approved)
                .ToList();
        }

        public async Task UpdatePaymentStatusAsync(int expenseId, PaymentStatus status)
        {
            var expense = await _context.Expenses
                .Where(e => e.Id == expenseId && e.Status == ExpenseStatus.Approved)
                .FirstOrDefaultAsync();

            if (expense != null)
            {
                expense.PaymentStatus = status;
                await _context.SaveChangesAsync();
            }
        }

        public List<Expense> GetApprovedExpenses()
        {
            return _context.Expenses
            .Include(e => e.Category)
            .Where(e => e.Status == ExpenseStatus.Approved)
            .OrderByDescending(e => e.Date)
            .ToList();
        }

        public decimal TotalAmount(int userId)
        {
            return _context.Expenses
              .Where(e => e.UserId == userId)
              .Sum(e => e.Amount);

        }

        public decimal TotalPaidAmount(int userId)
        {
            return _context.Expenses
               .Where(x =>
                   x.UserId == userId &&
                   x.PaymentStatus == PaymentStatus.Paid)
               .Select(x => (decimal?)x.Amount)
               .Sum() ?? 0;
        }

        public decimal TotalPendingAmount(int userId)
        {
            return _context.Expenses
              .Where(x =>
                  x.UserId == userId &&
                  x.PaymentStatus == PaymentStatus.Pending)
              .Select(x => (decimal?)x.Amount)
              .Sum() ?? 0;
        }

        public List<int> GetMonthlyExpenseCounts(int userId)
        {
            var result = new List<int>();
            var year = DateTime.Now.Year;
            for (int month = 1; month <= 12; month++)
            {
                var startDate = new DateTime(year, month, 1);
                var endDate = startDate.AddMonths(1);
                var count = _context.Expenses
                  .Where(x =>
                      x.UserId == userId &&
                      x.Date >= startDate &&
                      x.Date < endDate)
                  .Count();
                result.Add(count);

            }
            return result;
            

        }


        public List<int> GetWeeklyExpenseCounts(int userId)
        {
            var result = new List<int>();
            var now = DateTime.Now;
            var startOfMonth = new DateTime(now.Year, now.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1);
            var currentStart = startOfMonth;

            while (currentStart < endOfMonth)
            {
                var currentEnd = currentStart.AddDays(7);

                var count = _context.Expenses
                    .Where(x =>
                        x.UserId == userId &&
                        x.Date >= currentStart &&
                        x.Date < currentEnd)
                    .Count();

                result.Add(count);

                currentStart = currentEnd;
            }

            return result;
        }

        public decimal GetTotalAmountByPaymentStatus(int userId, PaymentStatus paymentStatus)
        {
            return _context.Expenses
               .Where(x => x.UserId == userId && x.PaymentStatus == paymentStatus)
               .Sum(x => (decimal?)x.Amount) ?? 0;
        }

        public decimal GetTotalAmountByExpenseStatus(int userId, ExpenseStatus expenseStatus)
        {
            return _context.Expenses
               .Where(x => x.UserId == userId && x.Status == expenseStatus)
               .Sum(x => (decimal?)x.Amount) ?? 0;
        }

        public List<(string CategoryName, decimal TotalAmount)> GetTotalAmountByCategory(int userId)
        {
            return _context.Expenses
                .Where(x => x.UserId == userId)
                .GroupBy(x => x.Category.Name)
                .Select(g => new
                {
                    CategoryName = g.Key,
                    TotalAmount = g.Sum(x => x.Amount)
                })
                .AsEnumerable()
                .Select(x => (x.CategoryName, x.TotalAmount))
                .ToList();
        }

        public async Task<List<Expense>> GetFilteredExpensesWithDetailsAsync(int? employeeId, DateTime? start, DateTime? end, int? status, int? categoryId, int departmentId)
        {
            var query = _context.Expenses
                .Include(x => x.User)
                .Include(x => x.Category)
                .Where(x => x.User.DepartmentId == departmentId)
                .AsQueryable();

            // Filtreler dolu geldiyse query'ye ekle
            if (employeeId.HasValue)
                query = query.Where(x => x.UserId == employeeId);

            if (start.HasValue)
                query = query.Where(x => x.Date >= start.Value);

            if (end.HasValue)
                query = query.Where(x => x.Date <= end.Value);

            if (status.HasValue)
                query = query.Where(x => x.Status == (ExpenseStatus)status.Value);

            if (categoryId.HasValue)
                query = query.Where(x => x.CategoryId == categoryId.Value);

            return await query.OrderByDescending(x => x.Date).ToListAsync();
        }

        public async Task<Dictionary<string, int>> GetExpenseCountsByCategoryAsync(int departmentId)
        {
            var counts = await _context.Expenses
                .Include(x => x.Category)
                .Include(x => x.User)
                .Where(x => x.User.DepartmentId == departmentId)
                .GroupBy(x => x.Category.Name)
                .Select(g => new { CategoryName = g.Key, Count = g.Count() })
                .ToListAsync();

            return counts.ToDictionary(x => x.CategoryName, x => x.Count);
        }

        public List<Expense> GetPaidExpenses()
        {
            return _context.Expenses
                .Include(x => x.Category)
                .Where(x =>
                    x.Status == ExpenseStatus.Approved &&
                    x.PaymentStatus == PaymentStatus.Paid
                )
                .ToList();
        }
    }
}

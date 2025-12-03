using ExpenseFlow.Data.Abstract;
using ExpenseFlow.Data.Context;
using ExpenseFlow.Entity;
using Microsoft.EntityFrameworkCore;

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
                var expense= _context.Expenses.Find(id);
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
            catch (Exception ex )
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
    }
}

using ExpenseFlow.Business.Abstract;
using ExpenseFlow.Data.Abstract;
using ExpenseFlow.Entity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseFlow.Business.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseDal _expenseDal;
    

        public ExpenseService( IExpenseDal expenseDal)
        {
           
            _expenseDal = expenseDal;

        }

        public List<Expense> GetApprovedExpenses()
        {
            return _expenseDal.GetApprovedExpenses();
        }

        public List<Expense> GetApproveWithCategory()
        {
            return _expenseDal.GetApproveWithCategory();
        }

        public List<Expense> GetPendingWithCategory()
        {
            return _expenseDal.GetPendingWithCategory();
        }

        public List<Expense> GetRejectWithCategory()
        {
            return _expenseDal.GetRejectWithCategory();
        }

        public decimal TotalPaidAmount(int userId)
        {
            return _expenseDal.TotalPaidAmount(userId);
        }

        public decimal TotalPendingAmount(int userId)
        {
            return _expenseDal.TotalPendingAmount(userId);
        }

        public void TDelete(int id)
        {
            throw new NotImplementedException();
        }

        public Expense TGetById(int id)
        {
            return _expenseDal.GetById(id);
        }

        public List<Expense> TGetList()
        {
            return _expenseDal.GetList();
        }

        public void TInsert(Expense expense)
        {
            expense.Date = DateTime.Now;
            expense.Status= ExpenseStatus.Pending;
            _expenseDal.Insert(expense);
            
           
        }

        public decimal TotalAmount(int userId)
        {
            return _expenseDal.TotalAmount(userId);
        }

        public void TUpdate(Expense expense)
        {
            _expenseDal.Update(expense);
        }

        public async Task UpdatePaymentStatusAsync(int expenseId, PaymentStatus status)
        {
            await _expenseDal.UpdatePaymentStatusAsync(expenseId, status);
        }


        public List<int> GetMonthlyExpenseCounts(int userId)
        {
            return _expenseDal.GetMonthlyExpenseCounts(userId);
        }

        public List<int> GetWeeklyExpenseCounts(int userId)
        {
            return _expenseDal.GetWeeklyExpenseCounts(userId);

        }

        public decimal GetPaidAmount(int userId)
        {
            return _expenseDal.GetTotalAmountByPaymentStatus(userId, PaymentStatus.Paid);
        }

        public decimal GetPendingAmount(int userId)
        {
            return _expenseDal.GetTotalAmountByPaymentStatus(userId, PaymentStatus.Pending);
        }

        public decimal GetRejectedAmount(int userId)
        {
            return _expenseDal.GetTotalAmountByExpenseStatus(userId, ExpenseStatus.Rejected);
        }
        public List<(string CategoryName, decimal TotalAmount)> GetExpenseAmountsByCategory(int userId)
        {
            return _expenseDal.GetTotalAmountByCategory(userId);
        }

    }


}

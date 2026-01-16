using ExpenseFlow.Business.Abstract;
using ExpenseFlow.Core.ViewModels;
using ExpenseFlow.Data.Abstract;
using ExpenseFlow.Data.Concrete;
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
        private readonly IUserDal _userDal;

        public ExpenseService(IExpenseDal expenseDal, IUserDal userDal)
        {
            _expenseDal = expenseDal;
            _userDal = userDal;
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

       

        public async Task<Dictionary<string, int>> GetExpenseCountsByCategoryAsync(int managerId)
        {
            var manager = await _userDal.GetByIdAsync(managerId);
            int deptId = manager.DepartmentId;
            return await _expenseDal.GetExpenseCountsByCategoryAsync(deptId);
        }

        public async Task<List<ExpenseListViewModel>> GetDashboardExpensesAsync(int? employeeId, DateTime? start, DateTime? end, int? status, int? categoryId, int managerId)
        {
            var manager = await _userDal.GetByIdAsync(managerId);
            int deptId = manager.DepartmentId;

            var expenses = await _expenseDal.GetFilteredExpensesWithDetailsAsync(employeeId, start, end, status, categoryId, deptId);

            // ViewModel'e map'le ve geri dön
            return expenses.Select(x => new ExpenseListViewModel
            {
                Id = x.Id,
                EmployeeName = x.User.FirstName + " " + x.User.LastName,
                Date = x.Date,
                CategoryName = x.Category.Name,
                Amount = x.Amount,
                Status = (int)x.Status,

            }).ToList();
        }

        public List<Expense> GetPaidExpenses()
        {
            return _expenseDal.GetPaidExpenses();
        }

        public Task<Dictionary<string, int>> GetApprovedExpenseCountByDepartmentAsync()
        {
            return _expenseDal.GetApprovedExpenseCountByDepartmentAsync();
        }

        public async Task<Dictionary<string, int>> GetExpenseCountsByCategoryAsync()
        {
            return await _expenseDal.GetExpenseCountsByCategoryForAllAsync();
        }

    }


}

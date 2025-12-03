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

        public void TUpdate(Expense expense)
        {
            _expenseDal.Update(expense);
        }

        

    }
}

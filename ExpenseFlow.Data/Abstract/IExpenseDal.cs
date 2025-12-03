using ExpenseFlow.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseFlow.Data.Abstract
{
    public interface IExpenseDal
    {
        //Buraya Expense ile ilgili metotlar eklenecek
        void Insert(Expense expense);
        void Update(Expense expense);
        void Delete(int id);
        Expense GetById(int id);
        List<Expense> GetList();
        List<Expense> GetPendingWithCategory();
        List<Expense> GetApproveWithCategory();
        List<Expense> GetRejectWithCategory();

    }
}

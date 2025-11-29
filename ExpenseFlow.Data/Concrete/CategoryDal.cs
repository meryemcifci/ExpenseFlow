using ExpenseFlow.Data.Abstract;
using ExpenseFlow.Data.Context;
using ExpenseFlow.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseFlow.Data.Concrete
{
    public class CategoryDal : ICategoryDal
    {
        private readonly ExpenseFlowContext _context;

        public CategoryDal(ExpenseFlowContext context)
        {
            _context = context;
        }

        public List<Category> GetList()
        {
            return _context.Categories.ToList();
        }
    }

}

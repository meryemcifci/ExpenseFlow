using ExpenseFlow.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseFlow.Data.Abstract
{
    public interface ICategoryDal
    {
        List<Category> GetList();

    }
}

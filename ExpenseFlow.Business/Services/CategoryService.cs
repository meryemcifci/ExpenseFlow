using ExpenseFlow.Business.Abstract;
using ExpenseFlow.Data.Abstract;
using ExpenseFlow.Entity;

namespace ExpenseFlow.Business.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryDal _categoryDal;

        public CategoryService(ICategoryDal categoryDal)
        {
            _categoryDal = categoryDal;
        }

        public List<Category> TGetList()
        {
            return _categoryDal.GetList();
        }
    }

}

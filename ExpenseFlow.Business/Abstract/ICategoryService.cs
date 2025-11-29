using ExpenseFlow.Entity;

namespace ExpenseFlow.Business.Abstract
{
    public interface ICategoryService
    {
        List<Category> TGetList();
    }
}

using ExpenseFlow.Entity;

namespace ExpenseFlow.DataAccess.Abstract
{
    public interface IDepartmentDal
    {
        bool IsManagerExistsInDepartment(int departmentId);

        List<Department> GetSelectableDepartments();


    }
}
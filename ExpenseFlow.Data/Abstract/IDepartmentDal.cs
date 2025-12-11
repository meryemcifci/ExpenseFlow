namespace ExpenseFlow.DataAccess.Abstract
{
    public interface IDepartmentDal
    {
        bool IsManagerExistsInDepartment(int departmentId);
    }
}
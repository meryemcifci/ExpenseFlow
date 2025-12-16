using ExpenseFlow.Entity;

namespace ExpenseFlow.Business.Abstract
{
    public interface IDepartmentService
    {
        // Eğer void döneceksek, içeride hata (Exception) fırlatacağız demektir.
        // Eğer bool döneceksek, Controller tarafında if/else ile kontrol edeceğiz.

        void CheckIfDepartmentHasManager(int departmentId);

        List<Department> GetSelectableDepartments();

    }
}

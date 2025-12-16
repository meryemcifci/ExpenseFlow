using ExpenseFlow.Business.Abstract;
using ExpenseFlow.DataAccess.Abstract;
using ExpenseFlow.DataAccess.Concrete;
using ExpenseFlow.Entity;

namespace ExpenseFlow.Business.Services
{
    public class DepartmentService : IDepartmentService
    {

        private readonly IDepartmentDal _departmentDal;
        public DepartmentService(IDepartmentDal departmentDal)
        {
            _departmentDal = departmentDal;
        }

        public void CheckIfDepartmentHasManager(int departmentId)
        {
            bool isManagerExists = _departmentDal.IsManagerExistsInDepartment(departmentId);

            if (isManagerExists)
            {
                
                throw new InvalidOperationException("Bu departmanın zaten bir yöneticisi (Manager) bulunmaktadır. Yeni yönetici atamadan önce mevcut olanı görevden almalısınız.");
            }
        }

        public List<Department> GetSelectableDepartments()
        {
            return _departmentDal.GetSelectableDepartments();
        }
    }
}


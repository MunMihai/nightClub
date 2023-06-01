using nightClub.Domain.Entities.Staff;
using nightClub.Domain.Entities.User;
using System.Collections.Generic;

namespace nightClub.BusinessLogic.Interfaces
{
    public interface IStaff
    {
        List<StaffModel> GetStaff();
        UResponse AddEmployee(StaffModel data);
        StaffModel GetStaffById(int id);
        UResponse UpdateEmployee(StaffModel data);
        void DeleteEmployee(int id);
    }
}

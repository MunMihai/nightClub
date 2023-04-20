﻿using nightClub.Domain.Entities.Staff;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using nightClub.BusinessLogic.DBModel;
using nightClub.Domain.Entities.User;

namespace nightClub.BusinessLogic.Core
{
    public class ContentApi
    {
        internal List<StaffModel> GetStaffList()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SDbTable, StaffModel>();
            });
            IMapper mapper = config.CreateMapper();
            using (var db = new StaffContext())
            {
                var result = db.Staff.ToList();
                var staffData = mapper.Map<List<StaffModel>>(result);
                return staffData;
            }
        }

        internal UResponse AddNewEmployee(StaffModel data)
        {
            SDbTable result;
            using (var db = new StaffContext())
                result = db.Staff.FirstOrDefault(u => u.PhoneNumb == data.PhoneNumb);
            if (result != null)
                return new UResponse { Status = false, StatusMsg = "Employee already added!" };

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<StaffModel, SDbTable>();
            });
            IMapper mapper = config.CreateMapper();
            result = mapper.Map<SDbTable>(data);

            using (var db = new StaffContext())
            {
                db.Staff.Add(result);
                db.SaveChanges();
            }
            return new UResponse { Status = true };
        }

        internal StaffModel GetById(int id)
        {
            SDbTable result;
            using (var db = new StaffContext())
                result = db.Staff.FirstOrDefault(u => u.Id == id);
            if (result != null)
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<SDbTable, StaffModel>();
                });
                IMapper mapper = config.CreateMapper();
                return mapper.Map<StaffModel>(result);
            }
            else
            {
                return null;
            }
        }
        internal UResponse Update(int id, StaffModel data)
        {
            SDbTable result;
            using (var db = new StaffContext())
                result = db.Staff.FirstOrDefault(u => u.Id == id);
            if (result == null)
                return new UResponse { Status = false, StatusMsg = "An Error occur at updating" };

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<StaffModel, SDbTable>();
            });
            IMapper mapper = config.CreateMapper();
            result = mapper.Map<SDbTable>(data);

            using (var db = new StaffContext())
            {
                db.Staff.AddOrUpdate(result);
                db.SaveChanges();
            }
            return new UResponse { Status = true };
        }

    }
}

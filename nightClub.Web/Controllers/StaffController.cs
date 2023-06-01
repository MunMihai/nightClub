using AutoMapper;
using nightClub.BusinessLogic.Interfaces;
using nightClub.Domain.Entities.Staff;
using nightClub.Web.Filters;
using nightClub.Web.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace nightClub.Web.Controllers
{
    public class StaffController : BaseController
    {
        private readonly IStaff _staffBL;

        public StaffController()
        {
            var bl = new BusinessLogic.BusinessLogic();
            _staffBL = bl.GetStaffBL();
        }

        //GET: Staff
        public ActionResult Index()
        {
            SessionStatus();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<StaffModel, Staff>();
            });
            IMapper mapper = config.CreateMapper();

            var staff = mapper.Map<List<Staff>>(_staffBL.GetStaff());
            return View(staff);
        }
        // GET: Staff/Create
        [AdminMod]
        public ActionResult Create()
        {
            SessionStatus();
            return View();
        }

        [AdminMod]
        [HttpPost]
        public ActionResult Create(Staff employee)
        {
            SessionStatus();
            if (ModelState.IsValid)
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<Staff, StaffModel>());
                IMapper mapper = config.CreateMapper();
                var data = mapper.Map<StaffModel>(employee);

                var employeeAdded = _staffBL.AddEmployee(data);
                if (employeeAdded.Status)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", employeeAdded.StatusMsg);
                    return View();
                }
            }
            return View();
        }

        // GET: Staff/Details/1
        [AdminMod]
        public ActionResult Details(int id)
        {
            SessionStatus();
            var empDetails = _staffBL.GetStaffById(id);
            if (empDetails != null)
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<StaffModel, Staff>());
                IMapper mapper = config.CreateMapper();
                var data = mapper.Map<Staff>(empDetails);
                return View(data);
            }

            return View("NotFound");
        }
        [Authenticated]
        // GET: Staff/EmployeeDetail/1
        public ActionResult EmployeeDetail(int id)
        {
            SessionStatus();
            var empDetails = _staffBL.GetStaffById(id);
            if (empDetails != null)
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<StaffModel, Staff>());
                IMapper mapper = config.CreateMapper();
                var data = mapper.Map<Staff>(empDetails);
                return View(data);
            }

            return View("NotFound");
        }

        // GET: Staff/Edit/1
        [AdminMod]
        public ActionResult Edit(int id)
        {
            SessionStatus();
            var empDetails = _staffBL.GetStaffById(id);
            if (empDetails != null)
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<StaffModel, Staff>());
                IMapper mapper = config.CreateMapper();
                var data = mapper.Map<Staff>(empDetails);
                return View(data);
            }

            return View("NotFound");
        }

        [AdminMod]
        [HttpPost]
        public ActionResult Edit(Staff employee)
        {
            SessionStatus();
            if (ModelState.IsValid)
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<Staff, StaffModel>());
                IMapper mapper = config.CreateMapper();
                var data = mapper.Map<StaffModel>(employee);

                var employeeAdded = _staffBL.UpdateEmployee(data);
                if (employeeAdded.Status)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", employeeAdded.StatusMsg);
                    return View();
                }
            }
            return View();
        }

        // GET: Staff/Delete/1
        [AdminMod]
        public ActionResult Delete(int id)
        {
            SessionStatus();
            var empDetails = _staffBL.GetStaffById(id);
            if (empDetails != null)
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<StaffModel, Staff>());
                IMapper mapper = config.CreateMapper();
                var data = mapper.Map<Staff>(empDetails);
                return View(data);
            }

            return View("NotFound");
        }

        [AdminMod]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var empDetails = _staffBL.GetStaffById(id);
            if (empDetails == null) return View("NotFound");
            _staffBL.DeleteEmployee(id);
            return RedirectToAction("Index");
        }
    }
}
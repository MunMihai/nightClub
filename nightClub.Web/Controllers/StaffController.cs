using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using nightClub.Domain.Enums;
using nightClub.Web.Models;

namespace nightClub.Web.Controllers
{
    public class StaffController : Controller
    {
        private List<Staff> _staffList = new List<Staff>();
        // GET: Staff
        public StaffController()
        {
            _staffList.Add(new Staff {ImageUrl= "https://cdn-icons-png.flaticon.com/512/3135/3135715.png", FirstName = "Ion", LastName = "Muntean", Role = SRole.Bartender, PayRate = 100, ContactInfo = "ionmun@gmail.com" });
            _staffList.Add(new Staff { FirstName = "Mihai", LastName = "Muntean", Role = SRole.Waiter, PayRate = 200, ContactInfo = "mihmun@gmail.com" });

        }
        public ActionResult Index()
        {
            var staff = _staffList.ToList();
           return View(staff);
        }
    }
}
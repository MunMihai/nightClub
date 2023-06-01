using AutoMapper;
using nightClub.BusinessLogic.Interfaces;
using nightClub.Domain.Entities.User;
using nightClub.Web.Models;
using System;
using System.Web.Mvc;

namespace nightClub.Web.Controllers
{
    public class RegisterController : Controller
    {
        private readonly ISession _sessionBL;

        public RegisterController()
        {
            var bl = new BusinessLogic.BusinessLogic();
            _sessionBL = bl.GetSessionBL();
        }

        // GET: Register
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(UserRegister register)
        {
            if (ModelState.IsValid)
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<UserRegister, URegisterData>();
                });
                IMapper mapper = config.CreateMapper();
                var data = mapper.Map<URegisterData>(register);

                data.LoginIp = Request.UserHostAddress;
                data.LoginDateTime = DateTime.Now;

                var userRegister = _sessionBL.UserRegister(data);

                if (userRegister.Status)
                {
                    //ULoginData data0 = new ULoginData
                    //{
                    //    Credential = register.Username,
                    //    Password = register.Password,
                    //    LoginIp = Request.UserHostAddress,
                    //    LoginDateTime = DateTime.Now
                    //};

                    return RedirectToAction("Index", "Login");
                }
                else
                {
                    ModelState.AddModelError("", userRegister.StatusMsg);
                    return View();
                }
            }
            return View();
        }
    }
}
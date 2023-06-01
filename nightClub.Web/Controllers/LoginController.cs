using AutoMapper;
using nightClub.BusinessLogic.Interfaces;
using nightClub.Domain.Entities.User;
using nightClub.Web.Models;
using System;
using System.Web;
using System.Web.Mvc;

namespace nightClub.Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly ISession _sessionBL;

        public LoginController()
        {
            var bl = new BusinessLogic.BusinessLogic();
            _sessionBL = bl.GetSessionBL();
        }


        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(UserLogin login)
        {
            if (ModelState.IsValid)
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<UserLogin, ULoginData>();
                });
                IMapper mapper = config.CreateMapper();
                var data = mapper.Map<ULoginData>(login); // Mapam Credential si Password

                data.LoginIp = Request.UserHostAddress;
                data.LoginDateTime = DateTime.Now;

                var userLogin = _sessionBL.UserLogin(data);
                if (userLogin.Status)
                {
                    HttpCookie cookie = _sessionBL.GenCookie(login.Credential);
                    ControllerContext.HttpContext.Response.Cookies.Add(cookie);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", userLogin.StatusMsg);
                    return View();
                }
            }
            return View();
        }

    }
}
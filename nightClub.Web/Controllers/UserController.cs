using AutoMapper;
using nightClub.BusinessLogic.Interfaces;
using nightClub.Domain.Entities.Ticket;
using nightClub.Domain.Entities.User;
using nightClub.Web.Extension;
using nightClub.Web.Filters;
using nightClub.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace nightClub.Web.Controllers
{
    [Authenticated]
    public class UserController : BaseController
    {
        private readonly ITicketBooking _ticketBL;
        public UserController()
        {
            var bl = new BusinessLogic.BusinessLogic();
            _ticketBL = bl.GetTicketBookingBL();
        }
        // GET: User
        public ActionResult Index()
        {
            SessionStatus();
            if ((string)System.Web.HttpContext.Current.Session["LoginStatus"] != "login")
            {
                return RedirectToAction("Index", "Login");
            }

            var user = System.Web.HttpContext.Current.GetMySessionObject();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserMinimal, UserData>();
            });
            IMapper mapper = config.CreateMapper();
            var data = mapper.Map<UserData>(user);

            return View(data);
        }

        [Authenticated]
        public ActionResult Logout()
        {
            Session.Abandon();
            System.Web.HttpContext.Current.Session.Clear();
            if (ControllerContext.HttpContext.Request.Cookies.AllKeys.Contains("X-KEY"))
            {
                var cookie = ControllerContext.HttpContext.Request.Cookies["X-KEY"];
                if (cookie != null)
                {
                    cookie.Expires = DateTime.Now.AddDays(-1);
                    ControllerContext.HttpContext.Response.Cookies.Add(cookie);
                }
            }
            System.Web.HttpContext.Current.Session["LoginStatus"] = "logout";
            return RedirectToAction("Index", "Home");
        }

        [Authenticated]
        public ActionResult TicketBookings(int userId, int? eventId)
        {
            SessionStatus();
            var configure = new MapperConfiguration(cfg =>
                cfg.CreateMap<TicketModel, Ticket>());
            IMapper mapper = configure.CreateMapper();

            var bookings = mapper.Map<List<Ticket>>(_ticketBL.GetByUserId(userId, eventId));
            return View(bookings);

        }

        [Authenticated]
        public ActionResult CancelBooking(int id)
        {
            var booking = _ticketBL.GetById(id);
            if (booking == null) return View("NotFound");
            _ticketBL.Delete(id);
            var user = System.Web.HttpContext.Current.GetMySessionObject();

            return RedirectToAction("TicketBookings", new { userid = user.Id });
        }
    }
}
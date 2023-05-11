﻿using AutoMapper;
using nightClub.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using nightClub.BusinessLogic.Interfaces;
using nightClub.Domain.Entities.Event;
using nightClub.Web.Filters;
using System.Net.Sockets;

namespace nightClub.Web.Controllers
{
    public class EventController : BaseController
    {
        private readonly IEvent _eventBl;

        public EventController()
        {
            var bl = new BusinessLogic.BusinessLogic();
            _eventBl = bl.GetEventBL();
        }

        // GET: Event
        public ActionResult Index()
        {
            SessionStatus();
            var config = new MapperConfiguration(cfg => cfg.CreateMap<EventModel, Event>());
            IMapper mapper = config.CreateMapper();

            var eventsList = mapper.Map<List<Event>>(_eventBl.GetAll());
            return View(eventsList);
        }

        //GET: Event/Create
        public ActionResult Create()
        {
            SessionStatus();
            return View();
        }

        [HttpPost]
        public ActionResult Create(Event newEvent)
        {
            SessionStatus();
            if (ModelState.IsValid)
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<Event, EventModel>());
                IMapper mapper = config.CreateMapper();

                var eventAdded = _eventBl.Add(mapper.Map<EventModel>(newEvent));
                if (eventAdded.Status)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", eventAdded.StatusMsg);
                    return View();
                }
            }

            return View();
        }

        //Pagina de detalii pt Admin
        // GET: Event/Details/1      
        public ActionResult Details(int id)
        {
            SessionStatus();
            var evDetails = _eventBl.GetById(id);
            if (evDetails != null)
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<EventModel, Event>());
                IMapper mapper = config.CreateMapper();
                var data = mapper.Map<Event>(evDetails);
                return View(data);
            }

            return View("NotFound");
        }

        //Pagina de detalii pt Utilizatori
        // GET: Event/EventDetail/1 
        public ActionResult EventDetail(int id)
        {
            SessionStatus();
            var evDetails = _eventBl.GetById(id);
            if (evDetails != null)
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<EventModel, Event>());
                IMapper mapper = config.CreateMapper();
                var data = mapper.Map<Event>(evDetails);
                return View(data);
            }

            return View("NotFound");
        }

        // GET: Event/Edit/1 
        public ActionResult Edit(int id)
        {
            SessionStatus();
            var evDetails = _eventBl.GetById(id);
            if (evDetails != null)
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<EventModel, Event>());
                IMapper mapper = config.CreateMapper();
                var data = mapper.Map<Event>(evDetails);
                return View(data);
            }

            return View("NotFound");
        }

        [HttpPost]
        public ActionResult Edit(Event newEvent)
        {
            SessionStatus();
            if (ModelState.IsValid)
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<Event, EventModel>());
                IMapper mapper = config.CreateMapper();

                var eventAdded = _eventBl.Update(mapper.Map<EventModel>(newEvent));
                if (eventAdded.Status)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", eventAdded.StatusMsg);
                    return View();
                }
            }

            return View();
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var eventDetail = _eventBl.GetById(id);
            if (eventDetail == null) return View("NotFound");
            _eventBl.Delete(id);
            return RedirectToAction("Index");
        }

        //GET: Event/BookTicket/1
        public ActionResult BookTicket(int id)
        {
            SessionStatus();
            if ((string)System.Web.HttpContext.Current.Session["LoginStatus"] != "login")
            {
                return RedirectToAction("Index", "Login");
            }

            var config = new MapperConfiguration(cfg => cfg.CreateMap<EventModel, EDbTable>());
            IMapper mapper = config.CreateMapper();

            var evDetails = _eventBl.GetById(id);
            ViewBag.Event = evDetails;
            if (evDetails != null)
            {
                var ticket = new Ticket
                {
                    UserId = ViewBag.CurrentUser.Id,
                    FullName = ViewBag.CurrentUser.Username,
                    Email = ViewBag.CurrentUser.Email,
                    Quantity = 1,
                    EventId = evDetails.Id,
                };
                return View(ticket);
            }

            return View("NotFound");
        }

        [HttpPost]
        public ActionResult BookTicket(Ticket ticket)
        {
            SessionStatus();
            var eventDetail = _eventBl.GetById(ticket.EventId);
            ViewBag.Event = eventDetail;

            if (ModelState.IsValid)
            {
                if (eventDetail == null) return View("NotFound");

                //Conectarea cu business logica
            }
            return View(ticket);
        }
    }
}
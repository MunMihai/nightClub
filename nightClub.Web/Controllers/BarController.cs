using AutoMapper;
using nightClub.BusinessLogic.Implimentations;
using nightClub.BusinessLogic.Interfaces;
using nightClub.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using nightClub.Domain.Entities.Bar;
using nightClub.Domain.Entities.Gallery;
using Microsoft.Ajax.Utilities;
using System.Globalization;

namespace nightClub.Web.Controllers
{
    public class BarController : BaseController
    {
        public readonly IBar _barBL;

        public BarController()
        {
            var bl = new BusinessLogic.BusinessLogic();
            _barBL = bl.GetBarBL();
        }

        //GET: Bar
        public ActionResult Index()
        {

            SessionStatus();
            IMapper mappeer = new MapperConfiguration(cfg =>
                cfg.CreateMap<PhotoBar, Bar>()).CreateMapper();
            var bar = mappeer.Map<List<Bar>>(_barBL.GetBars());
            return View(bar);
        }
        public ActionResult SortByCategory()
        {

            SessionStatus();
            IMapper mappeer = new MapperConfiguration(cfg =>
                cfg.CreateMap<PhotoBar, Bar>()).CreateMapper();
            var bar = mappeer.Map<List<Bar>>(_barBL.GetBarsByCategory());
            return View(bar);
        }
        public ActionResult SortByAlcohol()
        {

            SessionStatus();
            IMapper mappeer = new MapperConfiguration(cfg =>
                cfg.CreateMap<PhotoBar, Bar>()).CreateMapper();
            var bar = mappeer.Map<List<Bar>>(_barBL.GetBarsByAlcohol());
            return View(bar);
        }
        public ActionResult SortByPrice()
        {

            SessionStatus();
            IMapper mappeer = new MapperConfiguration(cfg =>
                cfg.CreateMap<PhotoBar, Bar>()).CreateMapper();
            var bar = mappeer.Map<List<Bar>>(_barBL.GetBarsByPrice());
            return View(bar);
        }
        public ActionResult Create()
        {
            SessionStatus();
            return View();
        }
        [HttpPost]
        public ActionResult Create(Bar bar)
        {
            SessionStatus();
            if (ModelState.IsValid)
            {
                IMapper mappeer = new MapperConfiguration(cfg =>
                    cfg.CreateMap<Bar, PhotoBar>()).CreateMapper();
                var data = mappeer.Map<PhotoBar>(bar);

                var newPhoto = _barBL.Add(data);
                if (newPhoto.Status)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", newPhoto.StatusMsg);
                    return View();
                }
            }
            return View();
        }

        public ActionResult Details(int id)
        {
            SessionStatus();
            var bar = _barBL.GetBarById(id);
            if (bar != null)
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<PhotoBar, Bar>());
                IMapper mapper = config.CreateMapper();
                var data = mapper.Map<Bar>(bar);
                return View(data);
            }

            return View("NotFound");
        }

        public ActionResult Edit(int id)
        {
            SessionStatus();
            var bar = _barBL.GetBarById(id);
            if (bar != null)
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<PhotoBar, Bar>());
                IMapper mapper = config.CreateMapper();
                var data = mapper.Map<Bar>(bar);
                return View(data);
            }

            return View("NotFound");
        }

        [HttpPost]
        public ActionResult Edit(Bar bar)
        {
            SessionStatus();
            if (ModelState.IsValid)
            {
                IMapper mappeer = new MapperConfiguration(cfg =>
                    cfg.CreateMap<Bar, PhotoBar>()).CreateMapper();
                var data = mappeer.Map<PhotoBar>(bar);

                var uBar = _barBL.Update(data);
                if (uBar.Status)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", uBar.StatusMsg);
                    return View();
                }
            }
            return View();
        }

        // GET: Bar/Delete/1
        public ActionResult Delete(int id)
        {
            SessionStatus();
            var bar = _barBL.GetBarById(id);
            if (bar != null)
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<PhotoBar, Bar>());
                IMapper mapper = config.CreateMapper();
                var data = mapper.Map<Bar>(bar);
                return View(data);
            }

            return View("NotFound");
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var empDetails = _barBL.GetBarById(id);
            if (empDetails == null) return View("NotFound");
            _barBL.DeleteBar(id);
            return RedirectToAction("Index");
        }

        public ActionResult Search(string searchQuery)
        {
            SessionStatus();

            if (string.IsNullOrEmpty(searchQuery))
            {
                ModelState.AddModelError("", "Vă rugăm să introduceți un termen de căutare valid.");
                return RedirectToAction("Index");
            }

            var searchResults = _barBL.SearchProducts(searchQuery);

            return View("SearchResults", searchResults);
        }
    }
}
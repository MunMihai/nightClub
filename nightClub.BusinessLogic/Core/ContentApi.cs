using AutoMapper;
using nightClub.BusinessLogic.DBModel;
using nightClub.BusinessLogic.Implimentations;
using nightClub.Domain.Entities.Bar;
using nightClub.Domain.Entities.Contact;
using nightClub.Domain.Entities.Event;
using nightClub.Domain.Entities.Gallery;
using nightClub.Domain.Entities.Staff;
using nightClub.Domain.Entities.Ticket;
using nightClub.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Remoting.Contexts;

namespace nightClub.BusinessLogic.Core
{
    public class ContentApi
    {
        //Get List
        internal List<StaffModel> GetList()
        {
            List<SDbTable> context;

            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<SDbTable, StaffModel>()).CreateMapper();
            using (var db = new StaffContext())
            {
                context = db.Staff.ToList();
            }
            return mapper.Map<List<StaffModel>>(context);
        }
        internal List<PhotoModel> GetAllPhoto()
        {
            List<PDbTable> context;

            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<PDbTable, PhotoModel>()).CreateMapper();
            using (var db = new GalleryContext())
            {
                context = db.Photos.ToList();
            }
            return mapper.Map<List<PhotoModel>>(context);
        }
        internal List<EventModel> GetAllEvents()
        {
            List<EDbTable> context;

            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<EDbTable, EventModel>()).CreateMapper();
            using (var db = new EventContext())
            {
                context = db.Events.ToList();
            }
            return mapper.Map<List<EventModel>>(context);
        }
        internal List<TicketModel> GetAllTicketBookings(string searchCriteria)
        {
            List<TDbTable> context;

            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<TDbTable, TicketModel>()).CreateMapper();
            using (var db = new EventContext())
            {
                if (!string.IsNullOrEmpty(searchCriteria))
                {
                    if (int.TryParse(searchCriteria, out int searchInt))
                    {
                        // Search by integer if the search criteria is a valid integer
                        context = db.Tickets.Where(e => e.EventId == searchInt).ToList();
                    }
                    else
                    {
                        // Search by string if the search criteria is not a valid integer
                        context = db.Tickets.Where(e => e.FullName.Contains(searchCriteria)).ToList();
                    }
                }
                else
                {
                     context = db.Tickets.ToList();
                }
            }
            return mapper.Map<List<TicketModel>>(context);
        }
        //AddNewEntity
        internal UResponse AddNewEntity(StaffModel data)
        {
            SDbTable context;

            using (var db = new StaffContext())
                context = db.Staff.FirstOrDefault(u => u.PhoneNumb == data.PhoneNumb);
            if (context != null)
                return new UResponse { Status = false, StatusMsg = "Employee already added!" };

            IMapper mapper = new MapperConfiguration(cfg => cfg.CreateMap<StaffModel, SDbTable>()).CreateMapper();
            context = mapper.Map<SDbTable>(data);

            using (var db = new StaffContext())
            {
                db.Staff.Add(context);
                db.SaveChanges();
            }
            return new UResponse { Status = true };
        }

        internal UResponse AddEntity(PhotoModel photo)
        {
            IMapper mapper = new MapperConfiguration(cfg => cfg.CreateMap<PhotoModel, PDbTable>()).CreateMapper();
            PDbTable context = mapper.Map<PDbTable>(photo);

            context.Date = DateTime.Now;
            using (var db = new GalleryContext())
            {
                db.Photos.Add(context);
                db.SaveChanges();
            }
            return new UResponse { Status = true };
        }
        internal UResponse AddEvent(EventModel newEvent)
        {
            IMapper mapper = new MapperConfiguration(cfg => cfg.CreateMap<EventModel, EDbTable>()).CreateMapper();
            var context = mapper.Map<EDbTable>(newEvent);
            context.TicketsLeft = context.TotalTicketsNumber;
            using (var db = new EventContext())
            {
                db.Events.Add(context);
                db.SaveChanges();
            }
            return new UResponse { Status = true };
        }
        internal UResponse BookTicket(int ticketEventId, TicketModel ticketModel)
        {
            EDbTable eventEF;
            using (var db = new EventContext())
            {
                eventEF = db.Events.FirstOrDefault(e => e.Id == ticketEventId);
            }
            if (eventEF == null)
            {
                throw new ArgumentException($"The event with ID {ticketEventId} does not exist.");
            }
            if (eventEF.TicketsLeft < ticketModel.Quantity)
            {
                return new UResponse { Status = false, StatusMsg = $"There are not enough tickets available for the event {eventEF.Title}." };
            }
            ticketModel.TotalPrice = ticketModel.Quantity * eventEF.TicketPrice;

            var config = new MapperConfiguration(cfg => cfg.CreateMap<TicketModel, TDbTable>());
            IMapper mapper = config.CreateMapper();
            var ticketEF = mapper.Map<TDbTable>(ticketModel);

            using (var db = new EventContext())
            {
                eventEF.TicketsLeft -= ticketEF.Quantity;
                db.Tickets.Add(ticketEF);
                db.Entry(eventEF).State = EntityState.Modified;
                db.SaveChanges();
            }

            return new UResponse { Status = true };
        }

        //GetById
        internal StaffModel GetEmployee(int id)
        {
            SDbTable context;
            using (var db = new StaffContext())
                context = db.Staff.FirstOrDefault(u => u.Id == id);
            IMapper mapper = new MapperConfiguration(cfg => cfg.CreateMap<SDbTable, StaffModel>()).CreateMapper();

            return context != null ? mapper.Map<StaffModel>(context) : null;
        }
        internal PhotoModel GetPhotoById(int id)
        {
            PDbTable context;
            using (var db = new GalleryContext())
                context = db.Photos.FirstOrDefault(u => u.Id == id);
            IMapper mapper = new MapperConfiguration(cfg => cfg.CreateMap<PDbTable, PhotoModel>()).CreateMapper();

            return context != null ? mapper.Map<PhotoModel>(context) : null;
        }
        internal EventModel GetEventById(int id)
        {
            EDbTable context;
            using (var db = new EventContext())
                context = db.Events.FirstOrDefault(u => u.Id == id);
            IMapper mapper = new MapperConfiguration(cfg => cfg.CreateMap<EDbTable, EventModel>()).CreateMapper();

            return context != null ? mapper.Map<EventModel>(context) : null;
        }
        internal TicketModel GetTicketById(int id)
        {
            TDbTable context;
            using (var db = new EventContext())
                context = db.Tickets.FirstOrDefault(u => u.Id == id);
            IMapper mapper = new MapperConfiguration(cfg => cfg.CreateMap<TDbTable, TicketModel>()).CreateMapper();

            return context != null ? mapper.Map<TicketModel>(context) : null;
        }
        internal List<TicketModel> GetTicketUserById(int userId , int? eventId )
        {
            List<TDbTable> context;
            using (var db = new EventContext())
            {
                if (eventId.HasValue)
                {
                    context= db.Tickets.Where(t=>(t.UserId==userId && t.EventId == eventId)).ToList();
                }
                else
                {
                    context = db.Tickets.Where(b => b.UserId == userId).ToList();
                }
            }
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<TDbTable, TicketModel>()).CreateMapper();
            return mapper.Map<List<TicketModel>>(context);
        }
        //Update
        internal UResponse Update(StaffModel data)
        {
            if (GetEmployee(data.Id) == null)
                return new UResponse { Status = false, StatusMsg = "An Error occur at updating" };

            IMapper mapper = new MapperConfiguration(cfg => cfg.CreateMap<StaffModel, SDbTable>()).CreateMapper();
            var result = mapper.Map<SDbTable>(data);

            using (var db = new StaffContext())
            {
                db.Staff.AddOrUpdate(result);
                db.SaveChanges();
            }
            return new UResponse { Status = true };
        }
        internal UResponse UpdatePhoto(PhotoModel data)
        {
            if (GetPhotoById(data.Id) == null)
                return new UResponse { Status = false, StatusMsg = "An Error occur at updating" };

            IMapper mapper = new MapperConfiguration(cfg => cfg.CreateMap<PhotoModel, PDbTable>()).CreateMapper();
            var result = mapper.Map<PDbTable>(data);
            result.Date = DateTime.Now;

            using (var db = new GalleryContext())
            {
                db.Photos.AddOrUpdate(result);
                db.SaveChanges();
            }
            return new UResponse { Status = true };
        }
        internal UResponse UpdateEvent(EventModel data)
        {
            EventModel currentEvent = GetEventById(data.Id);
            if (currentEvent == null)
                return new UResponse { Status = false, StatusMsg = "An Error occur at updating" };

            int bookedTickets = currentEvent.TotalTicketsNumber - currentEvent.TicketsLeft;
            if (data.TotalTicketsNumber < bookedTickets)
            {
                return new UResponse { Status = false, StatusMsg = $"This amount of tickets is allready booked. {bookedTickets} tickets!" };
            }

            IMapper mapper = new MapperConfiguration(cfg => cfg.CreateMap<EventModel, EDbTable>()).CreateMapper();
            var result = mapper.Map<EDbTable>(data);

            using (var db = new EventContext())
            {
                result.TicketsLeft = result.TotalTicketsNumber - bookedTickets;
                db.Entry(result).State = EntityState.Modified;
                db.SaveChanges();
            }
            return new UResponse { Status = true };
        }

        //Delete
        internal void DeleteStaff(int id)
        {
            using (var db = new StaffContext())
            {
                var staff = db.Staff.FirstOrDefault(u => u.Id == id);
                if (staff != null)
                {
                    db.Staff.Remove(staff);
                    db.SaveChanges();
                }
            }
        }
        internal void DeletePhoto(int id)
        {
            using (var db = new GalleryContext())
            {
                var photo = db.Photos.FirstOrDefault(p => p.Id == id);
                if (photo != null)
                {
                    db.Photos.Remove(photo);
                    db.SaveChanges();
                }
            }
        }
        internal void DeleteEvent(int id)
        {
            using (var db = new EventContext())
            {
                var entity = db.Events.FirstOrDefault(p => p.Id == id);
                if (entity == null) return;

                //Sterge toate rezervarile la acest eveniment
                var tickets = db.Tickets.Where(p => p.EventId == id).ToList();
                db.Tickets.RemoveRange(tickets);

                //Sterge evenimentul
                db.Events.Remove(entity);
                db.SaveChanges();
            }
        }

        internal void DeleteTicket(int id)
        {
            using (var db = new EventContext())
            {
                var booking = db.Tickets.FirstOrDefault(p => p.Id == id);
                var bEvent = db.Events.FirstOrDefault(e => e.Id == booking.EventId);
                if (booking == null) return;
                if (bEvent != null)
                {
                    bEvent.TicketsLeft += booking.Quantity;
                    db.Entry(bEvent).State = EntityState.Modified;
                }
                db.Tickets.Remove(booking);
                db.SaveChanges();
            }
        }

        //Bar
        internal List<PhotoBar> GetBarsPhoto()
        {
            List<BarDbTable> context;

            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<BarDbTable, PhotoBar>()).CreateMapper();
            using (var db = new BarContext())
            {
                context = db.Bars.ToList();
            }
            return mapper.Map<List<PhotoBar>>(context);
        }

        internal List<PhotoBar> GetBarsPhotoByCategory()
        {
            List<BarDbTable> context;

            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<BarDbTable, PhotoBar>()).CreateMapper();
            using (var db = new BarContext())
            {
                context = db.Bars.OrderBy(p=>p.Category).ToList();
            }
            return mapper.Map<List<PhotoBar>>(context);
        }
        internal List<PhotoBar> GetBarsPhotoByPrice()
        {
            List<BarDbTable> context;

            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<BarDbTable, PhotoBar>()).CreateMapper();
            using (var db = new BarContext())
            {
                context = db.Bars.OrderBy(p => p.Price).ToList();
            }
            return mapper.Map<List<PhotoBar>>(context);
        }
        internal List<PhotoBar> GetBarsPhotoByAlcohol()
        {
            List<BarDbTable> context;

            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<BarDbTable, PhotoBar>()).CreateMapper();
            using (var db = new BarContext())
            {
                context = db.Bars.OrderBy(p => p.Alcohol).ToList();
            }
            return mapper.Map<List<PhotoBar>>(context);
        }
        internal UResponse AddBarEntity(PhotoBar photo)
        {
            IMapper mapper = new MapperConfiguration(cfg => cfg.CreateMap<PhotoBar, BarDbTable>()).CreateMapper();
            BarDbTable context = mapper.Map<BarDbTable>(photo);

            context.Date = DateTime.Now;
            using (var db = new BarContext())
            {
                db.Bars.Add(context);
                db.SaveChanges();
            }
            return new UResponse { Status = true };
        }

        internal PhotoBar GetBarPhotoById(int id)
        {
            BarDbTable context;
            using (var db = new BarContext())
                context = db.Bars.FirstOrDefault(u => u.Id == id);
            IMapper mapper = new MapperConfiguration(cfg => cfg.CreateMap<BarDbTable, PhotoBar>()).CreateMapper();

            return context != null ? mapper.Map<PhotoBar>(context) : null;
        }

        internal UResponse UpdateBarPhoto(PhotoBar data)
        {
            if (GetBarPhotoById(data.Id) == null)
                return new UResponse { Status = false, StatusMsg = "An Error occur at updating" };

            IMapper mapper = new MapperConfiguration(cfg => cfg.CreateMap<PhotoBar, BarDbTable>()).CreateMapper();
            var result = mapper.Map<BarDbTable>(data);
            result.Date = DateTime.Now;

            using (var db = new BarContext())
            {
                db.Bars.AddOrUpdate(result);
                db.SaveChanges();
            }
            return new UResponse { Status = true };
        }

        internal void DeleteBarPhoto(int id)
        {
            using (var db = new BarContext())
            {
                var photo = db.Bars.FirstOrDefault(p => p.Id == id);
                if (photo != null)
                {
                    db.Bars.Remove(photo);
                    db.SaveChanges();
                }
            }
        }
        internal List<PhotoBar> SearchBarProducts(string search)
        {
            List<BarDbTable> context;

            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<BarDbTable, PhotoBar>()).CreateMapper();
            using (var db = new BarContext())
            {
                if (!string.IsNullOrEmpty(search))
                {
                    context = db.Bars.Where(e => e.Title.Contains(search)).ToList();
                }
                else
                {
                    context = db.Bars.ToList();
                }
            }
            return mapper.Map<List<PhotoBar>>(context);
        }
    }
}

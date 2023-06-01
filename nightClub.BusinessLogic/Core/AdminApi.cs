using System;
using AutoMapper;
using nightClub.BusinessLogic.DBModel;
using nightClub.Domain.Entities.User;
using System.Collections.Generic;
using System.Linq;
using nightClub.Domain.Enums;
using System.Runtime.Remoting.Contexts;

namespace nightClub.BusinessLogic.Core
{
    public class AdminApi
    {
        internal List<UserMinimal> GetUsers(string searchCriteria)
        {
            List<UDbTable> users;
            var configure = new MapperConfiguration(cfg => cfg.CreateMap<UDbTable, UserMinimal>());
            IMapper mapper = configure.CreateMapper();

            using (var db = new UserContext())
            {
                if (!string.IsNullOrEmpty(searchCriteria))
                {
                    if (Enum.TryParse(searchCriteria, out URole searchInt))
                    {
                        // Search by integer if the search criteria is a valid integer
                        users = db.Users.Where(e => e.Level == searchInt).ToList();
                    }
                    else
                    {
                        // Search by string if the search criteria is not a valid integer
                        users = db.Users.Where(u => u.Username.Contains(searchCriteria)).ToList();
                    }
                }
                else
                {
                    users = db.Users.ToList();
                }
            }
            return mapper.Map<List<UserMinimal>>(users);
        }

        internal UserMinimal GetUserById(int id)
        {
            UDbTable user;
            using (var db = new UserContext())
                user = db.Users.FirstOrDefault(u => u.Id == id);
            IMapper mapper = new MapperConfiguration(cfg => cfg.CreateMap<UDbTable, UserMinimal>()).CreateMapper();

            return user != null ? mapper.Map<UserMinimal>(user) : null;
        }

        internal void DeleteUser(int id)
        {
            UDbTable user;
            bool confDel = false;
            using (var db = new UserContext())
            {
                user = db.Users.FirstOrDefault(p => p.Id == id);
                if (user != null && user.Level != URole.Admin)
                {
                    db.Users.Remove(user);
                    db.SaveChanges();
                    confDel = true;
                }
            }
            if (confDel)
            {
                using (var db = new SessionContext())
                {
                    var session =
                        db.Sessions.FirstOrDefault(s => s.Username == user.Username || s.Username == user.Email);
                    if (session != null)
                    {
                        db.Sessions.Remove(session);
                        db.SaveChanges();
                    }
                }
                using (var db = new EventContext())
                {
                    var tickets = db.Tickets.Where(t => t.UserId == id).ToList();
                    db.Tickets.RemoveRange(tickets);
                    db.SaveChanges();
                }
            }
        }
    }
}

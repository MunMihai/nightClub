using AutoMapper;
using nightClub.BusinessLogic.DBModel;
using nightClub.Domain.Entities.Contact;
using nightClub.Domain.Entities.User;
using nightClub.Domain.Enums;
using nightClub.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace nightClub.BusinessLogic.Core
{
    public class UserApi
    {
        internal UResponse UserLoginAction(ULoginData data)
        {
            UDbTable result;
            var pass = LoginHelper.HashGen(data.Password);

            using (var db = new UserContext())
            {
                result = new EmailAddressAttribute().IsValid(data.Credential)
                    ? db.Users.FirstOrDefault(u => u.Email == data.Credential && u.Password == pass) 
                    : db.Users.FirstOrDefault(u => u.Username == data.Credential && u.Password == pass);
            }

            if (result == null)
            {
                return new UResponse { Status = false, StatusMsg = "The Username or Password is Incorrect" };
            }

            using (var todo = new UserContext())// cand facem schimbari la un context din baza de date
            {
                result.LasIp = data.LoginIp;
                result.LastLogin = data.LoginDateTime;
                todo.Entry(result).State = EntityState.Modified;
                todo.SaveChanges();
            }

            return new UResponse { Status = true };

        }
        internal UResponse UserRegisterAction(URegisterData data)
        {
            UDbTable result;
            data.Level = URole.User;
            using (var db = new UserContext())
                result = db.Users.FirstOrDefault(u => u.Username == data.Username);
            if (result != null)
                return new UResponse { Status = false, StatusMsg = "The Username already taken!" };

            using (var db = new UserContext())
                result = db.Users.FirstOrDefault(u => u.Email == data.Email);
            if (result != null)
                return new UResponse { Status = false, StatusMsg = "Account with a such Email already exists!" };

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<URegisterData, UDbTable>()
                    .ForMember(dest => dest.LastLogin, opt => opt.MapFrom(src => src.LoginDateTime)) // Mapam proprietatea LoginDateTime catre LastLogin
                    .ForMember(dest => dest.LasIp, opt => opt.MapFrom(src => src.LoginIp)) // Mapam proprietatea LoginIp catre LasIp
                    .ForMember(dest => dest.Password, opt => opt.MapFrom(src => LoginHelper.HashGen(src.Password))); //Mapam proprietatea Password  utilizând LoginHelper.HashGen() pe valoarea din câmpul Password din URegisterData.
            });
            IMapper mapper = config.CreateMapper();
            result = mapper.Map<UDbTable>(data);

            using (var db = new UserContext())
            {
                db.Users.Add(result);
                db.SaveChanges();
            }

            return new UResponse { Status = true };
        }

        internal void AddNewReview(ReviewModel review)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReviewModel, RDbTable>();
            });
            IMapper mapper = config.CreateMapper();
            var result = mapper.Map<RDbTable>(review);

            using (var db = new ReviewContext())
            {
                db.Reviews.Add(result);
                db.SaveChanges();
            }
        }
        internal List<ReviewModel> GetReviewList()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<RDbTable, ReviewModel>();
            });
            IMapper mapper = config.CreateMapper();

            using (var db = new ReviewContext())
            {
                var result = db.Reviews.ToList();
                var reviewData = mapper.Map<List<ReviewModel>>(result);
                return reviewData;
            }
        }
        internal HttpCookie Cookie(string loginCredential)
        {
            var validate = new EmailAddressAttribute();
            if (validate.IsValid(loginCredential))
            {
                UDbTable user;
                using (var db = new UserContext())
                {
                    user = db.Users.FirstOrDefault(u => u.Email == loginCredential);
                }
                if (user != null) loginCredential = user.Username;
            }

            var apiCookie = new HttpCookie("X-KEY")
            {
                Value = CookieGenerator.Create(loginCredential)
            };

            Session currentSession;
            using (var db = new SessionContext())
            {
                currentSession = db.Sessions.FirstOrDefault(s => s.Username == loginCredential);

                if (currentSession != null) //Update
                {
                    currentSession.CookieString = apiCookie.Value;
                    currentSession.ExpireTime = DateTime.Now.AddMinutes(60);
                    using (var todo = new SessionContext())
                    {
                        todo.Entry(currentSession).State = EntityState.Modified;
                        todo.SaveChanges();
                    }
                }
                else //Insert
                {
                    db.Sessions.Add(new Session
                    {
                        Username = loginCredential,
                        CookieString = apiCookie.Value,
                        ExpireTime = DateTime.Now.AddMinutes(60)
                    });
                    db.SaveChanges();
                }
            }

            return apiCookie;
        }

        internal UserMinimal UserCookie(string cookie)
        {
            Session session;

            using (var db = new SessionContext())
            {
                session = db.Sessions.FirstOrDefault(s => s.CookieString == cookie && s.ExpireTime > DateTime.Now);
            }

            UDbTable currentUser;
            if (session == null) return null;
            using (var db = new UserContext())
            {
                currentUser = db.Users.FirstOrDefault(u => u.Username == session.Username);
            }

            if (currentUser == null) return null;
            var configure = new MapperConfiguration(cfg => cfg.CreateMap<UDbTable, UserMinimal>());
            IMapper mapper = configure.CreateMapper();
            var userMinimal = mapper.Map<UserMinimal>(currentUser);

            return userMinimal;
        }

    }
}
//HOST, CONTENT TYPE, MINE, COOKIES -Sunt campurile protocolului HTTP 
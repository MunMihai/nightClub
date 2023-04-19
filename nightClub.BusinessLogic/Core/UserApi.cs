using AutoMapper;
using nightClub.BusinessLogic.DBModel;
using nightClub.Domain.Entities.User;
using nightClub.Helpers;
using nightClub.BusinessLogic.DBModel;
using nightClub.Domain.Entities.User;
using nightClub.Helpers;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;
using nightClub.Domain.Entities.Contact;
using nightClub.Domain.Enums;
using System.Collections.Generic;

namespace nightClub.BusinessLogic.Core
{
    public class UserApi
    {
        internal UResponse UserLoginAction(ULoginData data)
        {
            UDbTable result;
            var validate = new EmailAddressAttribute();
            if (validate.IsValid(data.Credential))
            {
                var pass = LoginHelper.HashGen(data.Password);
                using (var db = new UserContext())
                {
                    result = db.Users.FirstOrDefault(u => u.Email == data.Credential && u.Password == pass);
                }

                if (result == null)
                {
                    return new UResponse { Status = false, StatusMsg ="The Username or Password is Incorrect"};
                }

                using (var todo = new UserContext())
                {
                    result.LasIp = data.LoginIp;
                    result.LastLogin = data.LoginDateTime;
                    todo.Entry(result).State = EntityState.Modified;
                    todo.SaveChanges();
                }

                return new UResponse { Status = true };
            }
            else
            {
                var pass = LoginHelper.HashGen(data.Password);
                using (var db = new UserContext())
                {
                    result = db.Users.FirstOrDefault(u => u.Username == data.Credential && u.Password == pass);
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
                    .ForMember(dest => dest.Id, opt => opt.Ignore()) // Id este generat automat in baza de date si nu trebuie mapat
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
        //internal HttpCookie Cookie(string loginCredential)
        //{
        //    var apiCookie = new HttpCookie("X-KEY")
        //    {
        //        Value = CookieGenerator.Create(loginCredential)
        //    };

        //    using (var db = new SessionContext())
        //    {
        //        Session curent;
        //        var validate = new EmailAddressAttribute();
        //        if (validate.IsValid(loginCredential))
        //        {
        //            curent = (from e in db.Sessions where e.Username == loginCredential select e).FirstOrDefault();
        //        }
        //        else
        //        {
        //            curent = (from e in db.Sessions where e.Username == loginCredential select e).FirstOrDefault();
        //        }

        //        if (curent != null) //Update
        //        {
        //            curent.CookieString = apiCookie.Value;
        //            curent.ExpireTime = DateTime.Now.AddMinutes(60);
        //            using (var todo = new SessionContext())
        //            {
        //                todo.Entry(curent).State = EntityState.Modified;
        //                todo.SaveChanges();
        //            }
        //        }
        //        else //Insert
        //        {
        //            db.Sessions.Add(new Session
        //            {
        //                Username = loginCredential,
        //                CookieString = apiCookie.Value,
        //                ExpireTime = DateTime.Now.AddMinutes(60)
        //            });
        //            db.SaveChanges();
        //        }
        //    }

        //    return apiCookie;
        //}

        //internal UserMinimal UserCookie(string cookie)
        //{
        //    Session session;
        //    UDbTable curentUser;

        //    using (var db = new SessionContext())
        //    {
        //        session = db.Sessions.FirstOrDefault(s => s.CookieString == cookie && s.ExpireTime > DateTime.Now);
        //    }

        //    if (session == null) return null;
        //    using (var db = new UserContext())
        //    {
        //        var validate = new EmailAddressAttribute();
        //        if (validate.IsValid(session.Username))
        //        {
        //            curentUser = db.Users.FirstOrDefault(u => u.Email == session.Username);
        //        }
        //        else
        //        {
        //            curentUser = db.Users.FirstOrDefault(u => u.Username == session.Username);
        //        }
        //    }

        //    if (curentUser == null) return null;
        //    var configure = new MapperConfiguration(cfg => cfg.CreateMap<UDbTable, UserMinimal>());
        //    IMapper mapper = configure.CreateMapper();
        //    var userMinimal = mapper.Map<UserMinimal>(curentUser);

        //    return userMinimal;
        //}

    }
    //internal AddNewReview(ReviewModel review)
    //{
        
    //}
}
//HOST, CONTENT TYPE, MINE, COOKIES -Sunt campurile protocolului HTTP 
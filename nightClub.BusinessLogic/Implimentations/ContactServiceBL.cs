﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nightClub.BusinessLogic.Core;
using nightClub.BusinessLogic.Interfaces;
using nightClub.Domain.Entities.Contact;

namespace nightClub.BusinessLogic.Implimentations
{
    public class ContactServiceBL: UserApi, IContactService
    {
        public void AddReview(Review review)
        {
            throw new NotImplementedException();
        }

        public List<Review> GetReviews()
        {
            throw new NotImplementedException();
        }
    }
}

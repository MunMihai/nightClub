using nightClub.BusinessLogic.Core;
using nightClub.BusinessLogic.Interfaces;
using nightClub.Domain.Entities.Contact;
using System.Collections.Generic;

namespace nightClub.BusinessLogic.Implimentations
{
    public class ContactServiceBL : UserApi, IContactService
    {
        public void AddReview(ReviewModel review)
        {
            //SendToMail();
            AddNewReview(review);
        }

        public List<ReviewModel> GetReviews()
        {
            return GetReviewList();
        }
    }
}

using nightClub.BusinessLogic.Implimentations;
using nightClub.BusinessLogic.Interfaces;

namespace nightClub.BusinessLogic
{
    public class BusinessLogic
    {
        public ISession GetSessionBL()
        {
            return new SessionBL();
        }

        public IContactService GetContactBL()
        {
            return new ContactServiceBL();
        }

        public IStaff GetStaffBL()
        {
            return new StaffBL();
        }

        public IGallery GetGalleryBL()
        {
            return new GalleryBL();
        }

        public IUser GetUserBL()
        {
            return new UserBL();
        }
        public IEvent GetEventBL()
        {
            return new EventBL();
        }
        public ITicketBooking GetTicketBookingBL()
        {
            return new TicketBookingBL();
        }
        public IBar GetBarBL()
        {
            return new BarBL();
        }
    }
}

using nightClub.Domain.Entities.Gallery;
using nightClub.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nightClub.Domain.Entities.Bar;

namespace nightClub.BusinessLogic.Interfaces
{
    public interface IBar
    {
        List<PhotoBar> GetBars();
        List<PhotoBar> GetBarsByCategory();
        List<PhotoBar> GetBarsByPrice();
        List<PhotoBar> GetBarsByAlcohol();
        PhotoBar GetBarById(int id);
        UResponse Add(PhotoBar newPhoto);
        UResponse Update(PhotoBar data);
        void DeleteBar(int id);
        List<PhotoBar> SearchProducts(string search);
    }
}

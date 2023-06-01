using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nightClub.BusinessLogic.Core;
using nightClub.BusinessLogic.Interfaces;
using nightClub.Domain.Entities.Bar;
using nightClub.Domain.Entities.Gallery;
using nightClub.Domain.Entities.User;

namespace nightClub.BusinessLogic.Implimentations
{
    public class BarBL : ContentApi, IBar
    {
        public List<PhotoBar> GetBars()
        {
            return GetBarsPhoto();
        }

        public PhotoBar GetBarById(int id)
        {
            return GetBarPhotoById(id);
        }

        public UResponse Add(PhotoBar photo)
        {
            return AddBarEntity(photo);
        }

        public UResponse Update(PhotoBar data)
        {
            return UpdateBarPhoto(data);
        }

        public void DeleteBar(int id)
        {
            DeleteBarPhoto(id);
        }
    }
}

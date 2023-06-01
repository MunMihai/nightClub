using nightClub.BusinessLogic.Core;
using nightClub.BusinessLogic.Interfaces;
using nightClub.Domain.Entities.Gallery;
using nightClub.Domain.Entities.User;
using System.Collections.Generic;

namespace nightClub.BusinessLogic.Implimentations
{
    public class GalleryBL : ContentApi, IGallery
    {
        public List<PhotoModel> GetAll()
        {
            return GetAllPhoto();
        }

        public PhotoModel GetById(int id)
        {
            return GetPhotoById(id);
        }

        public UResponse Add(PhotoModel photo)
        {
            return AddEntity(photo);
        }

        public UResponse Update(PhotoModel data)
        {
            return UpdatePhoto(data);
        }

        public void Delete(int id)
        {
            DeletePhoto(id);
        }


    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhotoAlbum;

namespace PhotoAlbumTests
{
    class RandomObjects
    {
        private Random rnd = new Random();

        public List<string> GenerateTags(int numberOfTags)
        {
            var list = new List<string>();
            for (int i = 0; i < numberOfTags; i++)
            {
                list.Add(Path.GetRandomFileName().Replace(".",""));
            }
            return list;
        }

        public Photo CreatePhoto(string absolutePath)
        {
            var randomPhotoName = "/" + rnd.Next().ToString() + ".jpg";
            new Bitmap(1, 1).Save(absolutePath + randomPhotoName, ImageFormat.Jpeg);
            var photo = new Photo();
            photo.Open(new Uri(absolutePath + randomPhotoName));
            return photo;
        }

        public string GetRandomName()
        {
            return Path.GetRandomFileName().Replace(".", "");
        }
    }
}

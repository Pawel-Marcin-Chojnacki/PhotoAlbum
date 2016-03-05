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
    /// <summary>
    /// Generate random objects for testing.
    /// </summary>
    class RandomObjects
    {
        private Random rnd = new Random();

        /// <summary>
        /// Generates list of random tags 
        /// </summary>
        /// <param name="numberOfTags">Amount of tags to generate.</param>
        /// <returns></returns>
        public List<string> GenerateTags(int numberOfTags)
        {
            var list = new List<string>();
            for (int i = 0; i < numberOfTags; i++)
            {
                list.Add(Path.GetRandomFileName().Replace(".",""));
            }
            return list;
        }

        /// <summary>
        /// Creates photo object as a file with random name.
        /// </summary>
        /// <param name="absolutePath">Directory in which photo will be created.</param>
        /// <returns>Generated Photo object.</returns>
        public Photo CreatePhoto(string absolutePath)
        {
            var randomPhotoName = "/" + rnd.Next().ToString() + ".jpg";
            new Bitmap(1, 1).Save(absolutePath + randomPhotoName, ImageFormat.Jpeg);
            var photo = new Photo();
            photo.Open(new Uri(absolutePath + randomPhotoName));
            return photo;
        }

        /// <summary>
        /// Gets a random name (eg. for file or a directory).
        /// </summary>
        /// <returns>8 characters long, random string.</returns>
        public string GetRandomName()
        {
            return Path.GetRandomFileName().Replace(".", "");
        }
    }
}

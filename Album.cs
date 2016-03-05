using System;
using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAlbum
{
    /// <summary>
    /// Represents album. It's reflection of physical directory with photos.
    /// </summary>
    public class Album
    {
#region properties

        /// <summary>
        /// Physical location of an album.
        /// </summary>
        private Uri Location { get; set; }

        /// <summary>
        /// Album name (same as directory name).
        /// </summary>
        private string Name { get; set; }

        /// <summary>
        /// Creation date of an album in program.
        /// </summary>
        public DateTime CreationDate { get; }

        /// <summary>
        /// All photos managed by album.
        /// </summary>
        private List<Photo> Photos;

        public readonly Tag _tags = new Tag();

        private bool albumCreated = false;
#endregion

        /// <summary>
        /// Create album with customized parameters.
        /// </summary>
        /// <param name="location">Physical location of album.</param>
        /// <param name="name">Name of an album.</param>
        /// <param name="creationDate">Creation date (optional)</param>
        public Album(Uri location, string name, DateTime creationDate )
        {
            Photos = new List<Photo>();
            Location = location;
            Name = name;
            CreationDate = creationDate;
            if (!Directory.Exists(Location.AbsolutePath))
            {
                Directory.CreateDirectory(Location.AbsolutePath);
            }
            albumCreated = true;
        }

        /// <summary>
        /// Creates nameless album without physical location.
        /// </summary>
        public Album()
        {
            Photos = new List<Photo>();
            CreationDate = DateTime.Now;
        }

        /// <summary>
        /// Create new album.
        /// </summary>
        /// <param name="location">Physical location of album.</param>
        /// <param name="name">Name of an album.</param>
        public void Create(Uri location, string name)
        {
            if (albumCreated)
                return;
            Location = location;
            Name = name;
            if (!Directory.Exists(Location.AbsolutePath))
            {
                Directory.CreateDirectory(Location.AbsolutePath);
            }
        }

        public void ChangeName(string newName)
        {
            //foreach (var photo in Photos)
            //{
            //    photo.Data.Dispose();
            //}
           Directory.Move(Location.AbsolutePath, GetParentUriString(Location) + "/" + newName);
            //foreach (var photo in Photos)
            //    photo.Data = photo.
        }

        private string GetParentUriString(Uri uri)
        {
            return uri.AbsoluteUri.Remove(uri.AbsoluteUri.Length - uri.Segments.Last().Length);
        }

        /// <summary>
        /// Collect all tags in photos.
        /// </summary>
        public void AddTagsFromPhotosInAlbum()
        {
            foreach (var photo in Photos)
            {
                foreach (var tag in photo.Tags)
                {
                    _tags.Add(tag);
                }
            }
        }


        /// <summary>
        /// Deletes album and all files in it.
        /// </summary>
        public void Delete()
        {
            this.Dispose();
        }

        private void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                try
                {
                    foreach (var picture in Photos)
                    {
                        picture.Delete();
                    }

                    Directory.Delete(Location.AbsolutePath, true);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
        
    }


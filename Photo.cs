using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAlbum 
{
    /// <summary>
    /// Holds all necessary information about photo.
    /// </summary>
    public class Photo : IDisposable
    {
        #region properties
        /// <summary>
        /// Physical location of file. Path with filename and extension.
        /// </summary>
        public Uri Location { get; set; }

        /// <summary>
        /// Name of the photo.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Capture date of photo. Stored in EXIF.
        /// </summary>
        public DateTime CaptureDate { get; set; }

        /// <summary>
        /// Description of the file. Stored in EXIF.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Tags associated with photo. Stored in EXIF.
        /// </summary>
        public List<string> Tags { get; set; }

        /// <summary>
        /// Picture data and all it's properties.
        /// </summary>
        public Bitmap Data { get; set; }

        private PropertyItem Property { get; set; }

        private bool loaded = false;

        private bool tagPropertyExist = false;

        private bool creationTimePropertyExist = false;

        private bool descriptionPropertyExist = false;
        
        /// <summary>
        /// Picture capture date in string format.
        /// </summary>
        public string OriginalCaptureDate;

        public  bool tagExist = false;

#endregion

        public Photo()
        {
            Tags = new List<string>();
        }
        
        /// <summary>
        /// Deletes photo from memory. Also deletes file from disk.
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {
            try
            {
                Data.Dispose();
                File.Delete(Location.AbsolutePath);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Remove white character at the end of the property.
        /// </summary>
        /// <param name="text">Property in text format.</param>
        /// <returns></returns>
        private string RemoveEndCharacter(string text)
        {
            if (text.EndsWith("\0"))
                return text.Remove(text.Length - 1);
            return text;
        }

        /// <summary>
        /// Save changes to the file, so Data and file are in sync.
        /// </summary>
        private void SaveChanges()
        {
            Data.Dispose();
            if (System.IO.File.Exists(Location.AbsolutePath))
                System.IO.File.Delete(Location.AbsolutePath);
            System.IO.File.Move(Location.AbsolutePath + ".tmp", Location.AbsolutePath);
            Data = (Bitmap)Image.FromFile(Location.AbsolutePath);
        }

        /// <summary>
        /// Opens file to allow any modification on it.
        /// </summary>
        /// <param name="path">Location to photo.</param>
        /// <returns></returns>
        public string Open(Uri path)
        {
            Location = path;
            try
            {
                Data = (Bitmap)Image.FromFile(path.AbsolutePath);
                GetCorrectCaptureDate();
                Name = path.LocalPath;
                var properties = Data.PropertyItems;
                tagPropertyExist = IsPropertySet(properties, 0x9c9e);
                creationTimePropertyExist = IsPropertySet(properties, 0x9003);
                descriptionPropertyExist = IsPropertySet(properties, 0x9c9c);
                if (tagPropertyExist)
                {
                    Tags = Encoding.Unicode.GetString(Data.GetPropertyItem(0x9c9e).Value).Split(';').ToList();
                    var correctLastTag = RemoveEndCharacter(Tags.Last());
                    Tags.RemoveAt(Tags.Count-1);
                    Tags.Add(correctLastTag);
                    tagExist = true;
                }
                else Tags = new List<string>();
                
                loaded = true;
            }
            catch (FileNotFoundException)
            {
                return "Could not open file.";
            }
            return "ok";
        }

        /// <summary>
        /// Chcecks wether Tag property tag is set.
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        private bool IsPropertySet(PropertyItem[] properties, int id)
        {
            foreach (var property in properties)
            {
                if (property.Id == id)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Sets new value for description in photo.
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public bool SetDecription(string description)
        {
            if (!loaded) return false;

            using (Data)
            {
                var propItem = Data.PropertyItems.FirstOrDefault();
                var propertyValue = Encoding.Unicode.GetBytes(description);
                Data.SetPropertyItem(SetProperty(propItem, 1, propertyValue, 0x9c9c));
                Data.Save(Location.AbsolutePath+".tmp");
                Description = description;
            }
            SaveChanges();
            return true;
        }

        /// <summary>
        /// Overwrite current tags with a new ones.
        /// </summary>
        /// <param name="tags">List of new tags.</param>
        /// <returns></returns>
        public bool SetTags(List<string> tags)
        {
            if (!loaded) return false;

            string data = String.Empty;
            data = String.Join(";", tags);
            using (Data)
            {
                
                var propItem = Data.PropertyItems.FirstOrDefault();
                var propertyValue = Encoding.Unicode.GetBytes(data);
                Data.SetPropertyItem(SetProperty(propItem, 1, propertyValue, 0x9c9e));
                Data.Save(Location.AbsolutePath + ".tmp");
                Tags = tags;
            }

            SaveChanges();
            return true;
        }

        /// <summary>
        /// Add tags to existing list.
        /// </summary>
        /// <param name="tags">List of new tags.</param>
        /// <returns></returns>
        public bool AddTags(List<string> tags)
        {

            if (!loaded) return false;
            if (tagPropertyExist)
                Tags = Tags.Concat(tags).Distinct().ToList();
            else
                Tags = tags;
            string data = String.Join(";", Tags);
            using (Data)
            {
                var propItem = Data.PropertyItems.FirstOrDefault();
                var propertyValue = Encoding.Unicode.GetBytes(data);
                Data.SetPropertyItem(SetProperty(propItem, 1, propertyValue, 0x9c9e));
                Data.Save(Location.AbsolutePath + ".tmp");
                
            }

            SaveChanges();
            return true;
        }
        
        /// <summary>
        /// Create new photo.
        /// </summary>
        /// <returns>Wheter creating was successful </returns>
        private bool Create()
        {
            if (Location != null && Name != null && CaptureDate != null && Data != null)
            {
                
                Data.Save(Location.AbsolutePath + Name);
                if (Description != null)
                {
                    SetDecription(Description);
                }
            }
            return false;
        }

        /// <summary>
        /// Gets capture date from EXIF of file.
        /// Saves value in OriginalCaptureDate property.
        /// </summary>
        public void GetCorrectCaptureDate()
        {
            if (creationTimePropertyExist)
            {
                OriginalCaptureDate = RemoveEndCharacter(Encoding.ASCII.GetString(Data.GetPropertyItem(0x9003).Value));
            }
            else
            {
                SetCaptureDate(DateTime.Now);
                creationTimePropertyExist = true;
            }
        }

        /// <summary>
        /// Overwrites old capture date with a new one.
        /// Use when you want to correct date.
        /// </summary>
        /// <param name="captureDate"></param>
        public void SetCaptureDate(DateTime captureDate)
        {
            if (!loaded) return;
            using (Data)
            {
                if(creationTimePropertyExist) 
                    Data.RemovePropertyItem(0x9003);
                var propItem = Data.PropertyItems.FirstOrDefault();
                var time = captureDate.ToString("yyyy:MM:dd HH:mm:ss");
                var propertyValue = Encoding.ASCII.GetBytes(time);
                Data.SetPropertyItem(SetProperty(propItem, 2, propertyValue, 0x9003));
                Data.Save(Location.AbsolutePath + ".tmp");
            }

            SaveChanges();
            creationTimePropertyExist = true;
            GetCorrectCaptureDate();

        }

        private PropertyItem SetProperty(PropertyItem property, short type, byte[] value, int id)
        {
            property.Value = value;
            property.Id = id;
            property.Len = value.Length;
            property.Type = type;
            return property;
        }

        public void Dispose()
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
                    Data.Dispose();
                    File.Delete(Location.AbsolutePath);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

    }
}

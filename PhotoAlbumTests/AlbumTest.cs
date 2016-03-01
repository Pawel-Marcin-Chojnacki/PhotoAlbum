using System;
using System.Text;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhotoAlbum;

namespace PhotoAlbumTests
{
    /// <summary>
    /// Summary description for AlbumTests
    /// </summary>
    [TestClass]
    public class AlbumTest
    {

        RandomObjects random = new RandomObjects();

        [TestMethod]
        public void CreateAlbumTest()
        {
            // arrange 
            var name = random.GetRandomName();
            Uri location = new Uri(Directory.GetCurrentDirectory() + name);

            // act
            var album = new Album(location, name, DateTime.Now);
            
            // assert
            Assert.IsTrue(Directory.Exists(location.AbsolutePath));
        }

        [TestMethod]
        public void CreateEmptyAlbumTest()
        {
            var album = new Album();
            Assert.AreEqual(DateTime.Now,album.CreationDate);
        }

        [TestMethod]
        public void AddPhotoToAlbumTest()
        {
            var name = random.GetRandomName();
            
            Uri location = new Uri(Directory.GetCurrentDirectory() + name);

            // act
            var album = new Album(location, name, DateTime.Now);
            album.Photos.Add(random.CreatePhoto(location.AbsolutePath));
            
            //Assert
            Assert.AreEqual(1, album.Photos.Count);
        }

        [TestMethod]
        public void AddTagsFromPhotosInAlbumTest()
        {
            var name = random.GetRandomName();
            var tagsPerPicture = 4;
            Uri location = new Uri(Directory.GetCurrentDirectory() + "/" + name);
            var album = new Album(location, name, DateTime.Now);
            
            album.Photos.Add(random.CreatePhoto(location.AbsolutePath));
            album.Photos.Add(random.CreatePhoto(location.AbsolutePath));
            foreach (var photo in album.Photos)
                photo.AddTags(random.GenerateTags(tagsPerPicture));
            album.AddTagsFromPhotosInAlbum();

            Assert.AreEqual(tagsPerPicture * album.Photos.Count, album._tags.Count);
        }

        

        [TestMethod]
        public void SetTagsFromPhotosInAlbumTest()
        {
            var name = random.GetRandomName();
            Uri location = new Uri(Directory.GetCurrentDirectory() + "/" + name);
            var album = new Album(location, name, DateTime.Now);
            var tagsPerPicture = 4;
            album.Photos.Add(random.CreatePhoto(location.AbsolutePath));
            album.Photos.Add(random.CreatePhoto(location.AbsolutePath));
            foreach (var photo in album.Photos)
                photo.AddTags(random.GenerateTags(tagsPerPicture));
            //act
            album.AddTagsFromPhotosInAlbum();

            // assert
            Assert.AreEqual(tagsPerPicture*album.Photos.Count, album._tags.Count);

        }

        [TestMethod]
        public void DeleteAlbumTest()
        {
            var name = random.GetRandomName();
            Uri location = new Uri(Directory.GetCurrentDirectory() + "/" + name);
            var album = new Album(location, name, DateTime.Now);
            album.Photos.Add(random.CreatePhoto(location.AbsolutePath));
            album.Photos.Add(random.CreatePhoto(location.AbsolutePath));

            album.Delete();

            Assert.AreEqual(false,Directory.Exists(location.AbsolutePath));
        }

    }
}

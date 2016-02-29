using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
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
        [TestMethod]
        public void CreateAlbumTest()
        {

            // arrange 
            var name = "Clocks";
            Uri location = new Uri(Directory.GetCurrentDirectory() + name);

            // act
            var album = new Album(location, name, DateTime.Now);
            
            // assert
            Assert.IsTrue(Directory.Exists(location.AbsolutePath));
        }

        [TestMethod]
        public void CreateEmptyAlbumTest()
        {
            //arrange
            // act
            var album = new Album();
            //Thread.Sleep(1000);

            //assert
            Assert.AreEqual(DateTime.Now,album.CreationDate);
        }

        [TestMethod]
        public void AddPhotoToAlbumTest()
        {
            var name = "Clocks";
            Uri location = new Uri(Directory.GetCurrentDirectory() + name);
            var photo = new Photo();
            var photoLocation = new Uri(location.AbsolutePath + "/Untitled.jpg");

            // act
            var album = new Album(location, name, DateTime.Now);
            photo.Open(photoLocation);
            album.Photos.Add(photo);
            
            //Assert
            Assert.AreEqual(1, album.Photos.Count);
            album.Photos[0].Delete();

            album.Photos.Remove(photo);
            Assert.AreEqual(0, album.Photos.Count);
        }

        [TestMethod]
        public void AddTagsFromPhotosInAlbumTest()
        {
            var name = "Clocks";
            Uri location = new Uri(Directory.GetCurrentDirectory() + "/" + name);
            var album = new Album(location, name, DateTime.Now);

            File.Copy(Directory.GetCurrentDirectory() + "/Untitled.jpg", Directory.GetCurrentDirectory() + "/" + name + "/Untitled.jpg", true);
            File.Copy(Directory.GetCurrentDirectory() + "/Untitled.jpg", Directory.GetCurrentDirectory() + "/" + name + "/Untitled2.jpg", true);
            var photo = new Photo();
            var photoLocation = new Uri(location.AbsolutePath + "/Untitled.jpg");
            var photo2 = new Photo();
            var photoLocation2 = new Uri(location.AbsolutePath + "/Untitled2.jpg");
            photo.Open(photoLocation);
            album.Photos.Add(photo);
            photo2.Open(photoLocation2);
            album.Photos.Add(photo2);

            List<string> tags = new List<string>()
            {
                "outside",
                "manual",
                "morning"
            };
            var addTagsResult = album.Photos[0].AddTags(tags);
            List<string> tags2 = new List<string>()
            {
                "plan",
                "schedule",
                "university"
            };
            var addTagsResult2 = album.Photos[1].AddTags(tags2);


            //act
            album.AddTagsFromPhotosInAlbum();

            // assert
            Assert.AreEqual("work;clock;outside;manual;morning;plan;schedule;university", String.Join(";",Tag.Tags));

        }


        [TestMethod]
        public void SetTagsFromPhotosInAlbumTest()
        {
            var name = "Lights";
            Uri location = new Uri(Directory.GetCurrentDirectory() + "/" + name);
            var album = new Album(location, name, DateTime.Now);

            File.Copy(Directory.GetCurrentDirectory() + "/Untitled.jpg", Directory.GetCurrentDirectory() + "/" + name + "/Untitled.jpg", true);
            File.Copy(Directory.GetCurrentDirectory() + "/Untitled.jpg", Directory.GetCurrentDirectory() + "/" + name + "/Untitled2.jpg", true);
            var photo = new Photo();
            var photoLocation = new Uri(location.AbsolutePath + "/Untitled.jpg");
            var photo2 = new Photo();
            var photoLocation2 = new Uri(location.AbsolutePath + "/Untitled2.jpg");
            photo.Open(photoLocation);
            album.Photos.Add(photo);
            photo2.Open(photoLocation2);
            album.Photos.Add(photo2);

            List<string> tags = new List<string>()
            {
                "outside",
                "manual",
                "morning"
            };
            var addTagsResult = album.Photos[0].SetTags(tags);
            List<string> tags2 = new List<string>()
            {
                "plan",
                "schedule",
                "university"
            };
            var addTagsResult2 = album.Photos[1].SetTags(tags2);

            //act
            album.AddTagsFromPhotosInAlbum();

            // assert
            Assert.AreEqual("outside;manual;morning;plan;schedule;university", String.Join(";", Tag.Tags));

        }

        [TestMethod]
        public void DeleteAlbumTest()
        {
            var name = "Street";
            Uri location = new Uri(Directory.GetCurrentDirectory() + "/" + name);
            var album = new Album(location, name, DateTime.Now);
            File.Copy(Directory.GetCurrentDirectory() + "/Untitled.jpg", Directory.GetCurrentDirectory() + "/" + name + "/Untitled.jpg", true);
            File.Copy(Directory.GetCurrentDirectory() + "/Untitled.jpg", Directory.GetCurrentDirectory() + "/" + name + "/Untitled2.jpg", true);
            var photo = new Photo();
            var photoLocation = new Uri(location.AbsolutePath + "/Untitled.jpg");
            var photo2 = new Photo();
            var photoLocation2 = new Uri(location.AbsolutePath + "/Untitled2.jpg");
            photo.Open(photoLocation);
            album.Photos.Add(photo);
            photo2.Open(photoLocation2);
            album.Photos.Add(photo2);

            album.Delete();
            

        }

    }
}

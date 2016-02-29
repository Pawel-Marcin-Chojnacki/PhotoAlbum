using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhotoAlbum;

namespace PhotoAlbumTests
{
    [TestClass]
    public class PhotoTest
    {

        [TestInitialize()]
        public void CreateEmptyPhoto()
        {
            (new Bitmap(1, 1)).Save(Directory.GetCurrentDirectory() + "/Untitled.jpg", ImageFormat.Jpeg);
        }


        [TestMethod]
        public void OpenPhotoTest()
        {
            // arrange
            Uri photoPath = new Uri(Directory.GetCurrentDirectory() + "/Untitled.jpg");
            Photo somePhoto = new Photo();
            // act
            var openResult = somePhoto.Open(photoPath);
      
            // assert
            Assert.AreEqual("ok", openResult);
        }

        [TestMethod]
        public void CheckLocationPropertyTest()
        {
            // arrange
            Uri photoPath = new Uri(Directory.GetCurrentDirectory() + "/Untitled.jpg");
            Photo somePhoto = new Photo();
            
            // act
            var openResult = somePhoto.Open(photoPath);
            var locationResult = somePhoto.Location.AbsolutePath;
            // assert
            Assert.AreEqual(Directory.GetCurrentDirectory() + "/Untitled.jpg", locationResult);
        }

        [TestMethod]
        public void OpenNotExistingPhotoTest()
        {
            // arrange
            Uri photoPath = new Uri(Directory.GetCurrentDirectory() + "/fakefile.jpg");
            Photo somePhoto = new Photo();
            // act
            var openResult = somePhoto.Open(photoPath);

            // assert
            Assert.AreEqual("Could not open file.", openResult);
        }

        [TestMethod]
        public void SetDescriptionTest()
        {
            // arrange
            Uri photoPath = new Uri(Directory.GetCurrentDirectory() + "/Untitled.jpg");
            Photo somePhoto = new Photo();
            // act
            var openResult = somePhoto.Open(photoPath);
            var setDescriptionResult = somePhoto.SetDecription("My description test");

            //assert
            Assert.AreEqual(true, setDescriptionResult);
        }

        [TestMethod]
        public void SetTagsTest()
        {
            // arrange
            Uri photoPath = new Uri(Directory.GetCurrentDirectory() + "/Untitled.jpg");
            Photo somePhoto = new Photo();
            List<string> tags = new List<string>()
            {
                "work",
                "clock"
            };
            // act
            var openResult = somePhoto.Open(photoPath);
            var setTagsResult = somePhoto.SetTags(tags);

            // assert
            Assert.AreEqual(true, setTagsResult);
        }

        [TestMethod]
        public void AddTagsTest()
        {
            //arrange
            Uri photoPath = new Uri(Directory.GetCurrentDirectory() + "/Untitled.jpg");

            Photo somePhoto = new Photo();
            List<string> tags = new List<string>()
            {
                "outside",
                "manual",
                "morning"
            };
            // act
            var openResult = somePhoto.Open(photoPath);
            var addTagsResult = somePhoto.AddTags(tags);

            // assert
            Assert.AreEqual(true, addTagsResult);
        }

        [TestMethod]
        public void TagExtistTest()
        {
            //arrange
            Uri photoPath = new Uri(Directory.GetCurrentDirectory() + "/Untitled.jpg");
            Photo somePhoto = new Photo();

            //act 
            var openResult = somePhoto.Open(photoPath);

            // assert
            Assert.AreEqual(true, somePhoto.tagExist);
        }

        [TestMethod]
        public void DeleteTest()
        {
            //arrange
            Uri photoPath = new Uri(Directory.GetCurrentDirectory() + "/Untitled.jpg");
            Uri photoPathCopy = new Uri(Directory.GetCurrentDirectory() + "/" + "Untitled Copy.jpg");
            File.Copy(photoPath.AbsolutePath,photoPathCopy.AbsolutePath);
            Photo somePhoto = new Photo();

            //act 
            var openResult = somePhoto.Open(photoPathCopy);
            somePhoto.Delete();

            // assert
            Assert.IsFalse(File.Exists(photoPathCopy.AbsolutePath));
        }

        [TestMethod]
        public void GetCaptureDateTest()
        {
            Uri photoPath = new Uri(Directory.GetCurrentDirectory() + "/Untitled.jpg");
            Photo somePhoto = new Photo();

            //act 
            var openResult = somePhoto.Open(photoPath);
            
            Assert.AreEqual("2016:01:17 17:14:32", somePhoto.OriginalCaptureDate);
        }

        [TestMethod]
        public void SetCaptureDateTest()
        {
            Uri photoPath = new Uri(Directory.GetCurrentDirectory() + "/Untitled.jpg");
            Photo somePhoto = new Photo();
            DateTime captureTime = new DateTime(2016,1,17,17,14,32);
            var openResult = somePhoto.Open(photoPath);

            somePhoto.SetCaptureDate(captureTime);
            Assert.AreEqual("2016:01:17 17:14:32", somePhoto.OriginalCaptureDate);
        }

    }
}

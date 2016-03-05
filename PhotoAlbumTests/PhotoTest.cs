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
        private readonly RandomObjects _random = new RandomObjects();

        [TestMethod]
        public void OpenPhotoTest()
        {
            var name = "/" + _random.GetRandomName() + ".jpg";
            (new Bitmap(1, 1)).Save(Directory.GetCurrentDirectory() + name, ImageFormat.Jpeg);
            // arrange
            Uri photoPath = new Uri(Directory.GetCurrentDirectory() + name);
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
            Photo somePhoto = new Photo();
            
            // act
            somePhoto.Open(photoPath);
            var locationResult = somePhoto.Location.AbsolutePath;
            // assert
            Assert.AreEqual(new Uri(Directory.GetCurrentDirectory()).AbsolutePath + name, locationResult);
            somePhoto.Delete();
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
            var name = "/" + _random.GetRandomName() + ".jpg";
            (new Bitmap(1, 1)).Save(Directory.GetCurrentDirectory() + name, ImageFormat.Jpeg);
            Uri photoPath = new Uri(Directory.GetCurrentDirectory() + name);
            Photo somePhoto = new Photo();
            // act
            somePhoto.Open(photoPath);
            var setDescriptionResult = somePhoto.SetDecription("My description test");

            //assert
            Assert.AreEqual(true, setDescriptionResult);
            somePhoto.Delete();
        }

        [TestMethod]
        public void SetTagsTest()
        {
            // arrange
            var name = "/" + _random.GetRandomName() + ".jpg";
            (new Bitmap(1, 1)).Save(Directory.GetCurrentDirectory() + name, ImageFormat.Jpeg);
            Uri photoPath = new Uri(Directory.GetCurrentDirectory() + name);
            Photo somePhoto = new Photo();
            List<string> tags = new List<string>()
            {
                "work",
                "clock"
            };
            // act
            somePhoto.Open(photoPath);
            var setTagsResult = somePhoto.SetTags(tags);

            // assert
            Assert.AreEqual(true, setTagsResult);
        }

        [TestMethod]
        public void AddTagsTest()
        {
            Photo somePhoto = _random.CreatePhoto(Directory.GetCurrentDirectory());
            somePhoto.AddTags(_random.GenerateTags(3));
            Assert.AreEqual(3, somePhoto.Tags.Count);
            somePhoto.Delete();
        }

        [TestMethod]
        public void TagExistTest()
        {
            //arrange
            Photo somePhoto = _random.CreatePhoto(Directory.GetCurrentDirectory());

            //act 
            somePhoto.AddTags(_random.GenerateTags(3));

            // assert
            Assert.AreEqual(true, somePhoto.tagExist);
        }

        [TestMethod]
        public void DeleteTest()
        {
            //arrange
            Photo somePhoto = _random.CreatePhoto(Directory.GetCurrentDirectory());
            var location = somePhoto.Location;
            //act 
            somePhoto.Delete();

            // assert
            Assert.IsFalse(File.Exists(location.AbsolutePath));
        }

        [TestMethod]
        public void GetCaptureDateTest()
        {
            var somePhoto = _random.CreatePhoto(Directory.GetCurrentDirectory());
            var captureTime = new DateTime(2016, 3, 17, 17, 14, 33);
            somePhoto.SetCaptureDate(captureTime);

            Assert.AreEqual("2016:03:17 17:14:33", somePhoto.OriginalCaptureDate);
        }

        [TestMethod]
        public void SetCaptureDateTest()
        {
            var somePhoto = _random.CreatePhoto(Directory.GetCurrentDirectory());
            //DateTime captureTime = new DateTime(2016,1,17,17,14,32);
            somePhoto.SetCaptureDate(new DateTime(2016, 1, 17, 17, 14, 32));
            Assert.AreEqual("2016:01:17 17:14:32", somePhoto.OriginalCaptureDate);
        }

    }
}

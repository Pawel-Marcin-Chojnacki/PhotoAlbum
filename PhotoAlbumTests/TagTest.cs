using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhotoAlbum;

namespace PhotoAlbumTests
{
    [TestClass]
    public class TagTest
    {
        [TestMethod]
        public void SuggestTest()
        {
            // Arrange
            Tag.Tags.Add("morning");
            Tag.Tags.Add("work");
            Tag.Tags.Add("monday");
            Tag.Tags.Add("message");
            Tag.Tags.Add("wall");
            Tag.Tags.Add("symphony");


            // Act
            var suggestion = Tag.Suggest("mo");

            // Assert
            Assert.AreEqual("morning;monday", String.Join(";",suggestion));
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAlbum
{
    public static class Tag 
    {
        /// <summary>
        /// List of tags used withing all photos across all albums.
        /// </summary>
        public static List<string> Tags = new List<string>();

        /// <summary>
        /// Adds new tag to list if it doesn't exist already.
        /// </summary>
        /// <param name="tag"></param>
        public static void Add(string tag)
        {
            if (!Tags.Contains(tag))
            { 
                Tags.Add(tag);
            }
        }

        /// <summary>
        /// Get list of tags by given prefix.
        /// </summary>
        /// <param name="prefix">Prefix of a tags to look for.</param>
        /// <returns></returns>
        public static List<string> Suggest(string prefix)
        {
            Predicate<string> prefixFinder = s => s.StartsWith(prefix);
            return Tags.FindAll(prefixFinder);
        }

    }
}

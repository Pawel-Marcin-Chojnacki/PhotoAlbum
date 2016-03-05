using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAlbum
{
    public class Tag
    {
        /// <summary>
        /// List of tags used withing all photos across all albums.
        /// </summary>
        public readonly List<string> Tags;

        public int Count { get; private set; }

        public Tag()
        {
            Tags = new List<string>();
            Count = 0;
        }

        /// <summary>
        /// Adds new tag to list if it doesn't exist already.
        /// </summary>
        /// <param name="tag"></param>
        public void Add(string tag)
        {
            if (!Tags.Contains(tag))
            { 
                Tags.Add(tag);
                Count++;
            }
        }

        /// <summary>
        /// Get list of tags by given prefix.
        /// </summary>
        /// <param name="prefix">Prefix of a tags to look for.</param>
        /// <returns></returns>
        public List<string> Suggest(string prefix)
        {
            Predicate<string> prefixFinder = s => s.StartsWith(prefix);
            return Tags.FindAll(prefixFinder);
        }

    }
}

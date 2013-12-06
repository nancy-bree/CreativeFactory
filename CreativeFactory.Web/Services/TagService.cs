using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CreativeFactory.Web.Services
{
    /// <summary>
    /// Defines all operations with tags.
    /// </summary>
    public static class TagService
    {
        /// <summary>
        /// Gets list of tags from comma separated string.
        /// </summary>
        /// <param name="tags">Comma separated string of tags.</param>
        /// <returns>List of tag names.</returns>
        public static List<string> SplitTags(string tags)
        {
            var list = new List<string>(tags.Trim().Split(','));
            list.Remove(string.Empty);
            return list;
        }

        /// <summary>
        /// Defines CSS class name for tag cloud.
        /// </summary>
        /// <param name="tag">A number of photos related to tag.</param>
        /// <param name="articles">A number of all photos.</param>
        /// <returns>CSS class name.</returns>
        public static string GetTagClass(int tag, int articles)
        {
            var result = (tag * 100) / articles;
            if (result <= 1)
                return "tag1";
            if (result <= 4)
                return "tag2";
            if (result <= 8)
                return "tag3";
            if (result <= 12)
                return "tag4";
            if (result <= 18)
                return "tag5";
            if (result <= 30)
                return "tag6";
            return result <= 50 ? "tag7" : "tag8";
        }
    }
}
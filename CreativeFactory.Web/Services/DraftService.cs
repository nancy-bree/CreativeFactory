using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using CreativeFactory.Web.Properties;

namespace CreativeFactory.Web.Services
{
    public static class DraftService
    {
        private static string GetDraftPath(string filename)
        {
            return Path.Combine(HttpContext.Current.Server.MapPath(Settings.Default.Drafts), filename);
        }

        public static void SaveDraft(string filename, string content)
        {
            var path = GetDraftPath(filename);
            File.WriteAllText(path, content);
        }

        public static void DeleteDraft(string filename)
        {
            var draftPath = GetDraftPath(filename);
            if (File.Exists(draftPath))
            {
                File.Delete(draftPath);
            }
        }

        public static string GetDraft(string filename)
        {
            var path = GetDraftPath(filename);
            if (File.Exists(path))
            {
                return File.ReadAllText(path);
            }
            return String.Empty;
        }
    }
}
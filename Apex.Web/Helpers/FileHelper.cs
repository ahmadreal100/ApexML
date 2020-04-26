using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Apex.Service.Extensions;
using Apex.Shared.Extensions;
using Apex.Shared.Helpers;

namespace Apex.Web.Helpers
{
    public class FileHelper
    {
        private static HttpServerUtility Server => HttpContext.Current.Server;
        private const string UserFileRoot = "UserFiles";
        private const string TempName = "Temp";

        public static string GetTempPath()
        {
            return $"/{UserFileRoot}/{TempName}/";
        }
        public static string CopyFile(string filePath, string folderName)
        {
            if (!Regex.IsMatch(filePath.IsNeu(""), @"\/Temp\/[^\/]+$"))
                return filePath;

            RemovePastTempFiles();

            var from = Server.MapPath(filePath);
            var end = $"/{UserFileRoot}/{folderName}/";

            if (!File.Exists(from)) return filePath;
            if (!CheckDirectory(Server.MapPath(end))) return filePath;

            var name = new FileInfo(from).Name;
            File.Copy(from, Server.MapPath($"{end}{name}"), true);
            return $"{end}{name}";
        }
        public static bool RemoveFile(string fileUrl)
        {
            try
            {
                if (fileUrl.IsNeu())
                    return true;

                RemovePastTempFiles();

                var dir = fileUrl.Contains(":") ? fileUrl : Server.MapPath(fileUrl);
                if (File.Exists(dir))
                    File.Delete(dir);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool CheckDirectory(string dir)
        {
            try
            {
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string CreateSafePath(string path, string extension) => CreateSafePath(path, extension, out _);
        public static string CreateSafePath(string path, string extension, out string name)
        {
            var fileName = $"{DateHelper.TimeStamp}";
            name = string.Empty;

            if (!CheckDirectory(path)) return null;

            while (File.Exists($"{path}{fileName}.{extension}"))
                fileName = $"{decimal.Parse(fileName) + 1}";
            name = $"{fileName}.{extension}";
            return $"{path}{fileName}.{extension}";
        }

        public static void RemovePastTempFiles()
        {
            var tempPath = Server.MapPath(GetTempPath());
            if (!Directory.Exists(tempPath))
                return;

            //Delete all of Temp files that past 6 days.
            var today = DateHelper.Now;
            var tsToDel = Directory.GetFiles(tempPath).Where(f => (today - new FileInfo(f).NameAsDate()).TotalDays > 6).ToList();
            tsToDel.ForEach(File.Delete);
        }

    }
}
using System.IO;

namespace Apex.Service.ViewModels
{
    public class FileBase
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public long ContentLength { get; set; }
        public string ContentBase64 { get; set; }

        public void SaveAs(string fileName)
        {
            using (var tempStream = new MemoryStream())
            {
                File.WriteAllBytes(fileName, tempStream.ToArray());
            }
        }
    }
}
namespace Apex.Web.Models
{
    public class AjaxResult
    {
        public AjaxResult(bool status = false, string message = "")
        {
            Status = status;
            Message = message;
        }
        public AjaxResult(bool status, object data)
        {
            Status = status;
            Data = data;
        }
        public AjaxResult(bool status, string message, object data)
        {
            Status = status;
            Message = message;
            Data = data;
        }
        public AjaxResult(string message)
        {
            Message = message;
        }

        public bool Status { get; set; }
        public object Data { get; set; }
        public object ExData { get; set; }
        public string Message { get; set; }
    }
    public class UploadResult
    {
        public string Path { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
    }
}
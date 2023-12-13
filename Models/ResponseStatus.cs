namespace POSWebsite.Models
{
    public class ResponseStatus
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public string Data { get; set; }

        public ResponseStatus(bool status, string message)
        {
            Status = status;
            Message = message;
        }

        public ResponseStatus(bool status, string message, string data)
        {
            Status = status;
            Message = message;
            Data = data;
        }
    }
}

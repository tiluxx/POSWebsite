namespace POSWebsite.Models
{
    public class ResponseStatus
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public string Data { get; set; }
        public SalesReport SalesReport { get; set; }
        public string AgentSelection { get; set; }
        public string TimelineSelection { get; set; }
        public string QuarterSelection { get; set; }
        public string QuarterYearSelection { get; set; }
        public string YearSelection { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public List<Inventory> Inventories { get; set; }
        public object Payload { get; set; }

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

        public ResponseStatus(bool status, string message, object payload)
        {
            Status = status;
            Message = message;
            Payload = payload;
        }

        public ResponseStatus(bool status, string message, SalesReport salesReport)
        {
            Status = status;
            Message = message;
            SalesReport = salesReport;
        }

        public ResponseStatus(bool status, string message, List<Inventory> inventories)
        {
            Status = status;
            Message = message;
            Inventories = inventories;
        }

        public ResponseStatus(bool status, string message, string timelineSelection, string quarterSelection, string quarterYearSelection, string yearSelection, string fromDate, string toDate)
        {
            Status = status;
            Message = message;
            TimelineSelection = timelineSelection;
            QuarterSelection = quarterSelection;
            QuarterYearSelection = quarterYearSelection;
            YearSelection = yearSelection;
            FromDate = fromDate;
            ToDate = toDate;
        }

        
        public ResponseStatus(bool status, string message, string agentSelection, string fromDate, string toDate)
        {
            Status = status;
            Message = message;
            AgentSelection = agentSelection;
            FromDate = fromDate;
            ToDate = toDate;
        }
    }
}

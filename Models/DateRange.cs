namespace POSWebsite.Models
{
    public interface IRange
    {
        DateTime Start { get; }
        DateTime End { get; }
        bool WithInRange(DateTime value);
        bool WithInRange(IRange range);
    }

    public class DateRange : IRange
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public DateRange(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

        public bool WithInRange(DateTime value)
        {
            return (Start <= value) && (value <= End);
        }

        public bool WithInRange(IRange range)
        {
            return (Start <= range.Start) && (range.End <= End);
        }
    }
}

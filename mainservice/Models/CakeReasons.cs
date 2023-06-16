using CakeOTron.Service;

namespace CakeOTron
{
    public class CakeReason
    {
        public String Reason { get; set; }

        public ReferenceDate ReferenceDate { get; set; }
    }

    public class ReferenceDate
    {
        public DateTimeOffset Date { get; set; }

        public string Description { get; set; }
    }
}

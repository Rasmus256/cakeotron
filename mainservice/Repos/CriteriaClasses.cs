using System.Linq;

namespace CakeOTron.Service
{
    
    public abstract class Criteria
    {
        protected long units;
        protected abstract string unitNamePlural { get; }

        protected Criteria(long days)
        {
            this.units = days;
        }

        public virtual string Prettyreason(DateTimeOffset lookupDate) => $"This date was {units} {unitNamePlural} ago today!";

        public abstract bool MakesDateSpecial(DateTimeOffset lookupDate);

    }
    public class DaysSince : Criteria
    {
        public DaysSince(long days) : base(days){}

        protected override string unitNamePlural => "days";

        public override bool MakesDateSpecial(DateTimeOffset lookupDate)
        {
            TimeSpan dateTimeOffset = lookupDate.Subtract(DateTimeOffset.UtcNow);
            return (long)dateTimeOffset.TotalDays == -units;

        }

    }

    public class HoursSince : Criteria
    {
        public HoursSince(long hours) : base(hours){}

        protected override string unitNamePlural => "hours";

        public override bool MakesDateSpecial(DateTimeOffset lookupDate)
        {
            TimeSpan dateTimeOffset = lookupDate.Subtract(DateTimeOffset.UtcNow);
            return (long)dateTimeOffset.TotalDays == -(long)units / 24;

        }
    }

    public class MinutesSince : Criteria
    {

        public MinutesSince(long hours) : base(hours){}

        protected override string unitNamePlural => "minutes";

        public override bool MakesDateSpecial(DateTimeOffset lookupDate)
        {
            TimeSpan dateTimeOffset = lookupDate.Subtract(DateTimeOffset.UtcNow);
            return (long)dateTimeOffset.TotalDays == -(long)units / (24 * 60);

        }
    }


    public class SecondsSince : Criteria
    {

        public SecondsSince(long hours) : base(hours){}

        protected override string unitNamePlural => "seconds";

        public override bool MakesDateSpecial(DateTimeOffset lookupDate)
        {
            TimeSpan dateTimeOffset = lookupDate.Subtract(DateTimeOffset.UtcNow);
            return (long)dateTimeOffset.TotalDays == -(long)units / (24 * 60 * 60);

        }
    }

    public class FuzzydaysSince : Criteria
    {

        public FuzzydaysSince(long days) : base(days){}

        public long DaysAway(DateTimeOffset lookupDate)
        {
            TimeSpan dateTimeOffset = lookupDate.Subtract(DateTimeOffset.UtcNow);
            return ((long)dateTimeOffset.TotalDays) + units;
        }

        public override bool MakesDateSpecial(DateTimeOffset lookupDate)
        {
            var v = DaysAway(lookupDate);
            return v != 0 && v <= 20 && v >= -20;

        }
        public override string Prettyreason(DateTimeOffset lookupDate)
        {
            var offshoot = DaysAway(lookupDate);
            var msg = offshoot > 0 ? $"Will happen in {Math.Abs(offshoot)} days!" : $"Happened {Math.Abs(offshoot)} days ago!";
            return $"This date was (almost) {units} days ago today! {msg}";
        }

        protected override string unitNamePlural => "days";
    }

    public class MonthsSince : Criteria
    {

        public MonthsSince(long months):base(months){}

        protected override string unitNamePlural => "Months";

        public override bool MakesDateSpecial(DateTimeOffset lookupDate)
        {
            if ((DateTimeOffset.MaxValue.Subtract(lookupDate).TotalDays / 365 * 12) < units)
                return false;
            DateTimeOffset dateTimeOffset = lookupDate.AddMonths((int)units);
            return dateTimeOffset.Day == DateTime.UtcNow.Day && dateTimeOffset.Month == DateTime.UtcNow.Month && dateTimeOffset.Year == DateTime.UtcNow.Year;

        }
    }

    public class Anniversary : Criteria
    {
        public Anniversary() : base(0){}

        public override string Prettyreason(DateTimeOffset lookupDate) => "Today is the anniversary of this date!";

        protected override string unitNamePlural => throw new NotImplementedException();

        public override bool MakesDateSpecial(DateTimeOffset lookupDate)
        {
            return lookupDate.Day == DateTimeOffset.UtcNow.Day
                && lookupDate.Month == DateTimeOffset.UtcNow.Month;
        }
    }

    public class IsRepeating : Criteria
    {
        public IsRepeating(string Format) : base(0) { this.Format = Format; }

        string Format = "";
        public override bool MakesDateSpecial(DateTimeOffset lookupDate)
        {

            var s2 = lookupDate.ToString(Format);
            char c = s2[0];
            int longest = 1;
            for (int i = 1, j = 0; i < s2.Count(); i++)
            {
                if (c == s2[i])
                {
                    j++;
                    longest = Math.Max(longest, j);
                }
                else
                {
                    j = 0;
                }
            }
            return longest > 2;

        }
        protected override string unitNamePlural => throw new NotImplementedException();
        public override string Prettyreason(DateTimeOffset lookupDate) => $"Today's date contains more than 2 repeating decimals when formatted as {Format}!";
    }
    public class IsPalindrome : Criteria
    {

        public IsPalindrome(string Format) : base(0) { this.Format = Format; }

        string Format = "";
        protected override string unitNamePlural => throw new NotImplementedException();
        public override bool MakesDateSpecial(DateTimeOffset lookupDate)
        {
            var s2 = lookupDate.ToString(Format);
            return s2 == string.Join("", s2.Reverse());
        }
        public override string Prettyreason(DateTimeOffset lookupDate) => $"Today's date is a palindrome when formatted as {Format}!";
    }
}
using System.Linq;
using System;

namespace CakeOTron.Service
{
    public class CriteriaRepo
    {
        public static IEnumerable<Criteria> criteria()
        {
            var returnValue = new List<Criteria> { };
            var SignificantNumbers = new HashSet<long> { 100, 1000, 10000, 10000000, 12345, 1234, 123, 12, 4321, 321, 54321, 10, 11, 12, 24, 36, 48, 12121, 21212, 12221, 21112, 12321,121,1234321,12345421};
            var digits = Enumerable.Range(0,10);
            for (int i = 1; i < 10; i++){
                for (int j = 1; j<6; j++) {
                    SignificantNumbers.Add(Math.Power(i,j));
                }
            }
            List<string> significantStrings = new List<string> { };
            for (int i = 1; i < 10; i++)
            {
                significantStrings.AddRange(digits.Select(p => new string($"{p}"[0], i)));
                significantStrings.AddRange(digits.Select(p => p+ new string($"{p}"[0], i)));

            }
            for (int i = 0; i < digits.Count(); i++)
            {
                for (int j = i; j < digits.Count(); j++)
                {
                    var sToAdd = String.Join("",digits.Skip(i).Take(j));
                    significantStrings.Add(sToAdd);
                }

            }
            var strings = significantStrings.Distinct().Where(p => p!= "").ToList();
            strings.ForEach(p => SignificantNumbers.Add(long.Parse(p)));
            strings.ForEach(p => SignificantNumbers.Add(long.Parse(string.Join("", p.Reverse()))));
            returnValue.AddRange(SignificantNumbers.Select(p => new DaysSince(p)).ToList());
            returnValue.AddRange(SignificantNumbers.Select(p => new FuzzydaysSince(p)).ToList());
            returnValue.AddRange(SignificantNumbers.Select(p => new MonthsSince(p)).ToList());
            returnValue.AddRange(SignificantNumbers.Select(p => new HoursSince(p)).ToList());
            returnValue.AddRange(SignificantNumbers.Select(p => new MinutesSince(p)).ToList());
            returnValue.AddRange(SignificantNumbers.Select(p => new SecondsSince(p)).ToList());
            returnValue.Add(new Anniversary());
            return returnValue;
        }
        public static IEnumerable<CriteriaForCurrentDate> criteriaNoDate()
        {
            var returnValue = new List<CriteriaForCurrentDate> { };
            var dateFormats = new HashSet<string> {"yyyyMMdd","ddMMyyyy","MMddyyyy","yyyyddMM","yyyy", "yyyyMM", "MMyyyy", "ddMM", "MMdd"};
            returnValue.AddRange(dateFormats.Select(p => new IsRepeating(p)));
            returnValue.AddRange(dateFormats.Select(p => new IsPalindrome(p)));
            return returnValue;
        }
    }

    public abstract class Criteria
    {
        protected long units;
        protected abstract string unitNamePlural { get; }

        protected Criteria(long days)
        {
            this.units = days;
        }

        public virtual string Prettyreason => $"This date was {units} {unitNamePlural} ago today!";

        public abstract bool MakesDateSpecial(DateTimeOffset lookupDate);

    }
    public interface CriteriaForCurrentDate
    {
        string Prettyreason { get; }

        public bool MakesDateSpecial();

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
        private DateTimeOffset lastInvocation;

        public long DaysAway(DateTimeOffset lookupDate)
        {
            TimeSpan dateTimeOffset = lookupDate.Subtract(DateTimeOffset.UtcNow);
            return ((long)dateTimeOffset.TotalDays) + units;
        }

        public override bool MakesDateSpecial(DateTimeOffset lookupDate)
        {
            lastInvocation = lookupDate;
            TimeSpan dateTimeOffset = lookupDate.Subtract(DateTimeOffset.UtcNow);
            var v = ((long)dateTimeOffset.TotalDays) + units;
            return v != 0 && v <= 20 && v >= -20;

        }
        public override string Prettyreason
        {
            get
            {
                var offshoot = DaysAway(lastInvocation);
                var msg = offshoot > 0 ? $"Will happen in {Math.Abs(offshoot)} days!" : $"Happened {Math.Abs(offshoot)} days ago!";
                return $"This date was (almost) {units} days ago today! {msg}";
            }
        }

        protected override string unitNamePlural => throw new NotImplementedException();
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

        public override string Prettyreason => "Today is the anniversary of this date!";

        protected override string unitNamePlural => throw new NotImplementedException();

        public override bool MakesDateSpecial(DateTimeOffset lookupDate)
        {
            return lookupDate.Day == DateTimeOffset.UtcNow.Day
                && lookupDate.Month == DateTimeOffset.UtcNow.Month;
        }
    }

    public class IsRepeating : CriteriaForCurrentDate
    {
        public IsRepeating(string Format) { this.Format = Format; }

        string Format = "";
        public bool MakesDateSpecial()
        {
            var lookupDate = DateTimeOffset.UtcNow;

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
        public string Prettyreason { get => $"Today's date contains more than 2 repeating decimals when formatted as {Format}!"; }
    }
    public class IsPalindrome : CriteriaForCurrentDate
    {

        public IsPalindrome(string Format) { this.Format = Format; }

        string Format = "";
        public bool MakesDateSpecial()
        {
            var lookupDate = DateTimeOffset.UtcNow;

            var s2 = lookupDate.ToString(Format);
            return s2 == string.Join("", s2.Reverse());

        }
        public string Prettyreason { get => $"Today's date is a palindrome when formatted as {Format}!"; }
    }
}

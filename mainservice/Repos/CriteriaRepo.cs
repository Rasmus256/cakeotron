using System.Linq;

namespace CakeOTron.Service
{
    public class CriteriaRepo
    {
        private static List<Criteria> criteriaCache = new List<Criteria>();
        private static List<string> GetSignificantStrings() {
            List<string> significantStrings = new List<string> { };
            for (int i = 1; i < 10; i++)
            {
                significantStrings.AddRange(Enumerable.Range(0,10).Select(p => new string($"{p}"[0], i)));
                significantStrings.AddRange(Enumerable.Range(0,10).Select(p => p+ new string($"{p}"[0], i)));

            }
            foreach( int i in  Enumerable.Range(0,10))
            {
                foreach (int j in Enumerable.Range(i,10))
                {
                    var sToAdd = String.Join("",Enumerable.Range(0,10).Skip(i).Take(j));
                    significantStrings.Add(sToAdd);
                }

            }
            return significantStrings.Distinct().Where(p => p!= "").ToList();
        }
        public static IEnumerable<Criteria> criteria()
        {
            var returnValue = criteriaCache;
            if (returnValue.Any()) {
                return returnValue;
            }
            var SignificantNumbers = new HashSet<long> { 100, 1000, 10000, 10000000, 12345, 1234, 123, 12, 4321, 321, 54321, 10, 11, 12, 24, 36, 48, 12121, 21212, 12221, 21112, 12321,121,1234321,12345421};
            for (int i = 1; i < 10; i++){
                for (int j = 1; j<6; j++) {
                    SignificantNumbers.Add(long.Parse($"{i}"+new String('0', j)));
                }
            }
            var strings = GetSignificantStrings();
            strings.ForEach(p => SignificantNumbers.Add(long.Parse(p)));
            strings.ForEach(p => SignificantNumbers.Add(long.Parse(string.Join("", p.Reverse()))));

            returnValue.AddRange(SignificantNumbers.Select(p => new DaysSince(p)).ToList());
            returnValue.AddRange(SignificantNumbers.Select(p => new FuzzydaysSince(p)).ToList());
            returnValue.AddRange(SignificantNumbers.Select(p => new MonthsSince(p)).ToList());
            returnValue.AddRange(SignificantNumbers.Select(p => new HoursSince(p)).ToList());
            returnValue.AddRange(SignificantNumbers.Select(p => new MinutesSince(p)).ToList());
            returnValue.AddRange(SignificantNumbers.Select(p => new SecondsSince(p)).ToList());
            returnValue.Add(new Anniversary());
            returnValue.RemoveAll(item => item == null);
            return returnValue;
        }
        public static IEnumerable<Criteria> criteriaForToday()
        {
            var returnValue = new List<Criteria> { };
            var dateFormats = new HashSet<string> {"yyyyMMdd","ddMMyyyy","MMddyyyy","yyyyddMM","yyyy", "yyyyMM", "MMyyyy", "ddMM", "MMdd"};
            returnValue.AddRange(dateFormats.Select(p => new IsRepeating(p)));
            returnValue.AddRange(dateFormats.Select(p => new IsPalindrome(p)));
            return returnValue;
        }
    }
}

namespace CakeOTron.Service
{
    public class ReferenceRepo
    {
        public static IEnumerable<ReferenceDate> references()
        {
            var returnValue = new List<ReferenceDate> { };
            returnValue.Add(new ReferenceDate() { Description = "RDP Birthday", Date = new DateTimeOffset(1991, 6, 20, 0, 0, 0, TimeSpan.Zero) });
            returnValue.Add(new ReferenceDate() { Description = "Maj Birthday", Date = new DateTimeOffset(1991, 5, 15, 0, 0, 0, TimeSpan.Zero) });
            returnValue.Add(new ReferenceDate() { Description = "Mor Kirsten Birthday", Date = new DateTimeOffset(1961, 5, 23, 0, 0, 0, TimeSpan.Zero) });
            returnValue.Add(new ReferenceDate() { Description = "Far Frank Birthday", Date = new DateTimeOffset(1960, 3, 25, 0, 0, 0, TimeSpan.Zero) });
            returnValue.Add(new ReferenceDate() { Description = "Alberte Birthday", Date = new DateTimeOffset(2017, 3, 4, 0, 0, 0, TimeSpan.Zero) });
            returnValue.Add(new ReferenceDate() { Description = "Gry Birthday", Date = new DateTimeOffset(2018, 12, 22, 0, 0, 0, TimeSpan.Zero) });
            returnValue.Add(new ReferenceDate() { Description = "Mathias Birthday", Date = new DateTimeOffset(1992, 4, 24, 0, 0, 0, TimeSpan.Zero) });
            returnValue.Add(new ReferenceDate() { Description = "Stefan Birthday", Date = new DateTimeOffset(1991, 9, 11, 0, 0, 0, TimeSpan.Zero) });
            returnValue.Add(new ReferenceDate() { Description = "Wedding Anniversary", Date = new DateTimeOffset(2016, 2, 12, 0, 0, 0, TimeSpan.Zero) });
            returnValue.Add(new ReferenceDate() { Description = "Kærestedag", Date = new DateTimeOffset(2012, 8, 12, 0, 0, 0, TimeSpan.Zero) });
            returnValue.Add(new ReferenceDate() { Description = "Ansat ved Trifork", Date = new DateTimeOffset(2019, 1, 3, 0, 0, 0, TimeSpan.Zero) });
            returnValue.Add(new ReferenceDate() { Description = "Maj Ansat ved VBC", Date = new DateTimeOffset(2021, 7, 1, 0, 0, 0, TimeSpan.Zero) });
            returnValue.Add(new ReferenceDate() { Description = "Frodos fødselsdag", Date = new DateTimeOffset(2015, 9, 7, 0, 0, 0, TimeSpan.Zero) });
            returnValue.Add(new ReferenceDate() { Description = "Morten Mikkelsens fødselsdag", Date = new DateTimeOffset(2015, 9, 7, 0, 0, 0, TimeSpan.Zero) });
            returnValue.Add(new ReferenceDate() { Description = "Idas fødselsdag", Date = new DateTimeOffset(1992, 7, 14, 0, 0, 0, TimeSpan.Zero) });
            returnValue.Add(new ReferenceDate() { Description = "Bos fødselsdag", Date = new DateTimeOffset(1999, 2, 13, 0, 0, 0, TimeSpan.Zero) });
            returnValue.Add(new ReferenceDate() { Description = "Ansat ved Bankdata", Date= new DateTimeOffset(2022,12,1,0,0,0,TimeSpan.Zero)});
            return returnValue;
        }
    }
    public class ReferenceDate
    {
        public DateTimeOffset Date { get; set; }

        public string Description { get; set; }
    }

}

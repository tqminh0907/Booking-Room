using Org.BouncyCastle.Asn1.Ocsp;
using System.Text.RegularExpressions;

namespace Booking_Room.Utils
{
    public static class RegexHandle
    {
        public static MatchCollection DateRangeSplit(string dateRange)
        {
            string pattern = "\\d{1,2}\\/\\d{1,2}\\/\\d{4,4}";
            Regex myRegex = new Regex(pattern);
            MatchCollection dates = myRegex.Matches(dateRange);
            return dates;
        }
    }
}

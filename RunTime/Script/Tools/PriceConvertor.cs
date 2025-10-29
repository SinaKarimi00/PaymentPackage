using System;
using System.Linq;

namespace Mirka.Payment.Runtime.Script.Tools
{
    public static class PriceConvertor
    {
        public static string ExtractPriceFromPersianMarkets(string price, int decimalPlaces = 1)
        {
            // Remove non-numeric characters (like "ریال")
            var cleanedPrice = new string(price.Where(c => char.IsDigit(c) || c == ',').ToArray());
            var arabicPrice = ConvertPersianDigitToEnglishDigit(cleanedPrice);
            return FormatPriceWithCommasInRtl(arabicPrice, decimalPlaces);
        }

        private static string ConvertPersianDigitToEnglishDigit(string price)
        {
            string[] persianDigits = { "۰", "۱", "۲", "۳", "۴", "۵", "۶", "۷", "۸", "۹" };
            string[] arabicDigits = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

            for (var i = 0; i < persianDigits.Length; i++)
            {
                price = price.Replace(persianDigits[i], arabicDigits[i]);
            }

            return price;
        }

        private static string FormatPriceWithCommasInRtl(string price, int decimalPlaces)
        {
            price = price.Replace(",", "");

            if (long.TryParse(price, out var parsedPrice))
            {
                var result = parsedPrice / decimalPlaces;
                var formattedPrice = result.ToString("#,0");
                return ReverseStringWithCommas(formattedPrice);
            }

            throw new FormatException("Invalid price format.");
        }

        private static string ReverseStringWithCommas(string input)
        {
            var parts = input.Split(',');
            Array.Reverse(parts);
            return string.Join(",", parts);
        }
    }
}
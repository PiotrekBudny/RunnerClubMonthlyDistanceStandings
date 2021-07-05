namespace StravaClubMonthlyDistanceStandings
{
    public static class StringExtensions
    {
        public static string RemoveMeasureUnitFromString(this string value)
        {
            var regex = new[]
            {
                "/km", "km", "m", "/m"
            };

            foreach (var unit in regex)
            {
                if (value.Contains(unit))
                {
                    value = value.Replace(unit, "");
                }
            }

            return value;
        }

        public static string GetActivityTypeSubstring(this string value)
        {
            var indexOfLetterAfterLastSpace = 0;
            for (var index = 0; index < value.Length; index++)
            {
                if (value[index] == ' ' && value[index-1] == '–') indexOfLetterAfterLastSpace = index+1;
            }

            return value.Substring(indexOfLetterAfterLastSpace);
        }
    }
}
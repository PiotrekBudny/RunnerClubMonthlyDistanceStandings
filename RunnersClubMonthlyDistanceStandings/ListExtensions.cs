using System.Collections.Generic;

namespace RunnersClubMonthlyDistanceStandings
{
    public static class ListExtensions
    {
        public static bool ValidateIfUrlIsAlreadyOnList(this List<string> values, string url)
        {
            var isOnList = false;
            
            foreach (var value in values)
            {
                if (value.Contains(url))
                    isOnList = true;
            }

            return isOnList;
        }
    }
}
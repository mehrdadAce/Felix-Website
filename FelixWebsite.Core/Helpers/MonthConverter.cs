using FelixWebsite.Core.App_GlobalResources;
using System;

namespace FelixWebsite.Core.Helpers
{
    public static class MonthConverter
    {
        public static string ConvertToYearsAndMonths(this string monthsInput)
        {
            try
            {
                var amountOfMonths = int.Parse(monthsInput);

                var years = amountOfMonths / 12;
                var months = amountOfMonths % 12;
                string yearsString = null;
                string monthsString = null;

                if (years > 0)
                {
                    yearsString = years + " " + FelixResources.experience_years;
                }

                if (months > 0)
                {
                    if (months > 1)
                    {
                        monthsString = months + " " + FelixResources.experience_months;
                    }
                    else
                    {
                        monthsString = months + " " + FelixResources.experience_month;
                    }
                }

                if (yearsString != null && monthsString != null)
                {
                    return yearsString + " & " + monthsString;
                }

                if (yearsString != null)
                {
                    return yearsString;
                }

                if (monthsString != null)
                {
                    return monthsString;
                }

                return FelixResources.experience_none;
            }
            catch (FormatException)
            {
                return FelixResources.experience_none;
            }
        }
    }
}

using System;

namespace CourseLibrary.API.Helper
{
    public static class DateTimeOffsetExtension
    {
        public static int GetCurrantAge(this DateTimeOffset DateOfBarth)
        {
            var age = DateTime.UtcNow.Year - DateOfBarth.Year;
            if (DateTime.UtcNow < DateOfBarth.AddYears(age))
            {
                age--;
            }
            return age;
        }
    }
}

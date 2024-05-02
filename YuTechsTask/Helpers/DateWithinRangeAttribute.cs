using System.ComponentModel.DataAnnotations;

namespace YuTechsTask.Helpers
{
    public class DateWithinRangeAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null || !(value is DateTime))
            {
                return false;
            }

            DateTime publicationDate = (DateTime)value;
            DateTime today = DateTime.Today;
            DateTime oneWeekFromToday = today.AddDays(7);

            return publicationDate >= today && publicationDate <= oneWeekFromToday;
        }
    }
}

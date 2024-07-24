namespace DXKumaBot.Utils;

public static class DateTimeExtensions
{
    public static bool IsSameWeek(this DateTime date1, DateTime date2)
    {
        return date1.Date.AddDays(-Convert.ToInt32(date1.DayOfWeek)) ==
               date2.Date.AddDays(-Convert.ToInt32(date2.DayOfWeek));
    }
}
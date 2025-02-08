namespace ProjectLayer.Models.Structure
{
    /// <summary>
    /// متد های عمومی
    /// </summary>
    public class Publics
    {
        /// <summary>
        /// دریافت توکن یک بار مصرف
        /// </summary>
        public static string GetToken()
        {
            byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            byte[] key = Guid.NewGuid().ToByteArray();
            return Convert.ToBase64String(time.Concat(key).ToArray());
        }

        public static object DateFormat(object value)
        {
            return Convert.ToDateTime(value).ToString("dd, MMMM yyyy");
        }

        public static object DateFormatShort(object value)
        {
            return Convert.ToDateTime(value).ToString("ddd, dd MMM");
        }

        public static object DateFormatShort2(object value)
        {
            return Convert.ToDateTime(value).ToString("dd MMM");
        }

        public static object DateFormatShortTiming(object value)
        {
            if (value == null) return null;

            //string _return = "";

            var time = (DateTime.Now - (DateTime)value).TotalSeconds;

            switch (time)
            {
                case < 59:
                    return "Now";

                case < 3600:
                    return (DateTime.Now - (DateTime)value).Minutes + " minutes ago";

                case < 86400:
                    {
                        if ((DateTime)value > Convert.ToDateTime(DateTime.Now.ToShortDateString()))
                            return "Today " + Convert.ToDateTime(value).ToString("hh:mm tt");
                        else
                            return Convert.ToDateTime(value).ToString("dd MMM hh:mm tt");
                    }

                default:
                    return Convert.ToDateTime(value).ToString("dd MMM hh:mm tt");
            }

        }

        public static object DateFormatShortByTime(object value)
        {
            return Convert.ToDateTime(value).ToString("dd MMM hh:mm tt");
        }

        public static object DateTimerFormat(object value)
        {
            return Convert.ToDateTime(value).ToString("yyyy-MM-dd hh:mm:ss");
        }

        public static object DateTimeFormat(object value)
        {
            return Convert.ToDateTime(value).ToString("dd, MMMM yyyy hh:mm tt");
        }

        public static object TimeFormat(object value)
        {
            return Convert.ToDateTime(value).ToString("hh:mm tt");
        }

        public static object CurrencyFormat(object value, string addition = "$ ", int DecimalNum = 2)
        {
            return addition + Convert.ToDecimal(value).ToString("N" + DecimalNum);
        }

        public static object CurrencyFormat2(object value, string addition = "$", int DecimalNum = 2)
        {
            return Convert.ToDecimal(value).ToString("N" + DecimalNum) + addition;
        }
    }
}

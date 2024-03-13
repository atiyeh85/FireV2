using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace System
{
    /// <summary>
    /// Extensions
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// Check for valid persian date
        /// </summary>
        /// <param name="dateStr"></param>
        /// <returns></returns>
        public static bool isValidDate(this string dateStr, string separator = "/")
        {
            bool result = false;
            string[] datePart = dateStr.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries);

            if (datePart.Length == 3)
            {
                int y;
                int m;
                int d;

                result = int.TryParse(datePart[0], out y)
                    && int.TryParse(datePart[1], out m)
                    && int.TryParse(datePart[2], out d)
                    && ((y >= 1300) && (y <= 9999))
                    && ((m > 0) && (m < 13))
                    && ((d > 0) && (d < 32));
            }

            return result;
        }
        /// <summary>
        /// Check for valid persian date
        /// </summary>
        /// <param name="timeStr"></param>
        /// <returns></returns>
        public static bool isValidTime(this string timeStr, string separator = ":")
        {
            bool result = false;
            string[] datePart = timeStr.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries);

            if (datePart.Length == 3)
            {
                int h;
                int m;
                int s;

                result = int.TryParse(datePart[0], out h) &&
                    int.TryParse(datePart[1], out m) &&
                    int.TryParse(datePart[2], out s) &&
                    ((h >= 0) && (h < 24)) &&
                    ((m >= 0) && (m < 60)) &&
                    ((s >= 0) && (s < 60));
            }

            return result;
        }

        /// <summary>
        /// To Date Time
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime? toDateTime(this object date)
        {
            DateTime? result = null;

            if (null != date)
                result = Convert.ToDateTime(date);

            return result;
        }

        /// <summary>
        /// Empty String date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool isEmptyDate(this string date)
        {
            bool result = true;

            if ((date ?? "").Length > 0)
                result = (date.Replace("/", "").Trim().Length == 0);

            return result;
        }

        /// <summary>
        /// Get Compare
        /// </summary>
        static public int compareDate(this string firstDate, string secondDate)
        {
            int res = 0;
            string[] datePart = null;
            string[] datePart2 = null;
            PersianCalendar pC = new PersianCalendar();

            datePart = firstDate.Replace(" ", "").Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            datePart2 = secondDate.Replace(" ", "").Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);

            if ((datePart.Length > 2) && (datePart2.Length > 2))
            {
                DateTime d1 = pC.ToDateTime(Convert.ToInt32(datePart[0]), Convert.ToInt32(datePart[1]), Convert.ToInt32(datePart[2]), 0, 0, 0, 0);
                DateTime d2 = pC.ToDateTime(Convert.ToInt32(datePart2[0]), Convert.ToInt32(datePart2[1]), Convert.ToInt32(datePart2[2]), 0, 0, 0, 0);

                if (d1 < d2)
                    res = -1;
                else if (d1 > d2)
                    res = 1;
                else
                    res = 0;
            }
            else
                res = -2;

            return res;
        }

        /// <summary>
        /// Persian To Miladi
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        static public DateTime persianToMiladi(this string date)
        {
            DateTime result = new DateTime();

            // Correct date
            date = (date ?? "").Replace("-", "/");

            // Get date part
            if (null == date)
                date = "";
            else
                date = date.Split(' ')[0];

            // split date to y, m and d
            string[] val = date.Replace("//", "").Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);

            if (val.Length > 2)
            {
                PersianCalendar pc = new PersianCalendar();

                int y;
                int m;
                int d;

                int.TryParse(val[0].Replace(" ", ""), out y);
                int.TryParse(val[1].Replace(" ", ""), out m);
                int.TryParse(val[2].Replace(" ", ""), out d);

                //Create a Persian calendar class
                PersianCalendar pc2 = new PersianCalendar();

                // Create a date using the Persian calendar.
                DateTime wantedDate = pc2.ToDateTime(1395, 4, 22, 12, 30, 0, 0);

                try
                {
                    result = new DateTime(y, m, d, pc);
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    if ((12 == m) && (30 == d))
                        d--;
                }
                result = new DateTime(y, m, d, pc);
            }

            return result;
        }


        /// <summary>
        /// Persian To Miladi
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        static public DateTime persianToMiladi(this string date, ref bool status)
        {
            DateTime result = new DateTime();

            if (null == date)
                date = "";

            string[] val = date.Replace(" ", "").Replace("//", "").Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);

            if (val.Length > 2)
            {
                PersianCalendar pc = new PersianCalendar();

                int y;
                int m;
                int d;

                int.TryParse(val[0], out y);
                int.TryParse(val[1], out m);
                int.TryParse(val[2], out d);

                result = pc.ToDateTime(y, m, d, 0, 0, 0, 0);
                status = true;
            }
            else
                status = false;

            return result;
        }

        /// <summary>
        /// Get Time
        /// </summary>
        static public string getTimeStr(this DateTime date, string separtor = ":")
        {
            string res = "{1}{0}{2}{0}{3}";

            res = string.Format(res, separtor, date.Hour.ToString("D2"), date.Minute.ToString("D2"), date.Second.ToString("D2"));

            return res;
        }

        /// <summary>
        /// Get Week Day
        /// </summary>
        public static string getWeekDay(this DateTime p)
        {
            PersianCalendar pc = new PersianCalendar();
            string[] weeks = new string[]{
				"يك شنبه"
				,"دو شنبه"
				,"سه شنبه"
				,"چهار شنبه"
				,"پنج شنبه"
				,"جمعه"
				,"شنبه"
			};

            return weeks[(((int)pc.GetDayOfWeek(p)) % 7)];
        }

        /// <summary>
        /// Persian To Miladi - As String
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        static public string persianToMiladiAsString(this string date)
        {
            string result = "";
            string[] val = date.Split('/');

            if (val.Length > 2)
            {
                PersianCalendar pc = new PersianCalendar();

                int y;
                int m;
                int d;

                int.TryParse(val[0], out y);
                int.TryParse(val[1], out m);
                int.TryParse(val[2], out d);

                DateTime dt = pc.ToDateTime(y, m, d, 0, 0, 0, 0);
                result = string.Format("{0}/{1}/{2}", dt.Year.ToString("D4"), dt.Month.ToString("D2"), dt.Day.ToString("D2"));
            }

            return result;
        }

        /// <summary>
        /// Get Beginning of the date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string getYearStart(this DateTime date)
        {
            string result = "";

            result = string.Format("{0}/{1}/{2}", date.toPersianDate().Substring(0, 4), 1.ToString("D2"), 1.ToString("D2"));

            return result;
        }

        /// <summary>
        /// Get Beginning of the date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string getYearEnd(this DateTime date)
        {
            string result = "";

            result = string.Format("{0}/{1}/{2}", date.toPersianDate().Substring(0, 4), 12, 30);

            return result;
        }

        /// <summary>
        /// Return month number
        /// </summary>
        /// <param name="monthName"></param>
        /// <returns></returns>
        public static string getMonthNum(this string monthName)
        {
            string monthRes = "";
            List<string> arr = new string[] { "فروردين", "ارديبهشت", "خرداد", "تير", "مرداد", "شهريور", "مهر", "آبان", "آذر", "دي", "بهمن", "اسفند" }.ToList<string>();

            monthRes = (arr.IndexOf(monthName) + 1).ToString("D2");

            return monthRes;
        }

        #region Persian Date
        /// <summary>
        /// To Persian DateTime
        /// </summary>
        /// <param name="date"></param>
        /// <param name="dateSeparator"></param>
        /// <param name="timeSeparator"></param>
        /// <returns></returns>
        public static string toPersianDateTime(this DateTime date, string dateSeparator = "/", string timeSeparator = ":")
        {
            string result = "";
            PersianCalendar pc = new PersianCalendar();

            if (date != null)
                result = string.Format("{0}{6}{1}{6}{2} {3}{7}{4}{7}{5}",
                    pc.GetYear(date).ToString("D4"),
                    pc.GetMonth(date).ToString("D2"),
                    pc.GetDayOfMonth(date).ToString("D2"),
                    date.Hour.ToString("D2"),
                    date.Minute.ToString("D2"),
                    date.Second.ToString("D2"),
                    dateSeparator,
                    timeSeparator
                    );
            //else
            //    result = date.ToString();

            return result;
        }

        /// <summary>
        /// To Persian Date
        /// </summary>
        /// <param name="date"></param>
        /// <param name="dateSeparator"></param>
        /// <returns></returns>
        public static string toPersianDate(this DateTime date, string dateSeparator = "/")
        {
            string result = "";
            PersianCalendar pc = new PersianCalendar();

            if (date != null)
                try
                {
                    result = string.Format("{0}{3}{1}{3}{2}",
                        pc.GetYear(date).ToString("D4"),
                        pc.GetMonth(date).ToString("D2"),
                        pc.GetDayOfMonth(date).ToString("D2"),
                        dateSeparator
                    );
                }
                catch (Exception ex)
                {
                }
            //else
            //    result = date.ToString();

            return result;
        }

        /// <summary>
        /// To Persian Time
        /// </summary>
        /// <param name="date"></param>
        /// <param name="dateSeparator"></param>
        /// <returns></returns>
        public static string toPersianTime(this DateTime date, string timeSeparator = ":", bool fullHourCase = true)
        {
            string result = "";
            PersianCalendar pc = new PersianCalendar();

            if (date != null)
                result = date.ToString(string.Format("{1}{0}mm{0}ss {2}", timeSeparator, (fullHourCase ? "HH" : "hh"), (fullHourCase ? "" : "tt")));

            return result;
        }

        /// <summary>
        /// Get Persian year
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static int getPersianYear(this DateTime date)
        {
            int result = 0;

            if (null != date)
                result = (new PersianCalendar()).GetYear(date);

            return result;
        }

        /// <summary>
        /// Get Persian month
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static int getPersianMonth(this DateTime date)
        {
            int result = 0;

            if (null != date)
                result = (new PersianCalendar()).GetMonth(date);

            return result;
        }

        /// <summary>
        /// Get Persian day
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static int getPersianDay(this DateTime date)
        {
            int result = 0;

            if (null != date)
                result = (new PersianCalendar()).GetDayOfMonth(date);

            return result;
        }

        /// <summary>
        /// Get Persian Month name
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string toPersianMonthName(this int number)
        {
            string result = "";
            string[] month = new string[] {
                "فروردين", "ارديبهشت", "خرداد",
                "تير", "مرداد", "شهريور",
                "مهر", "آبان", "آذر",
                "دي", "بهمن", "اسفند"
            };

            number--;
            if ((number > -1) && (number < 12))
                result = month[number];

            return result;
        }


        /// <summary>
        /// Shamsi To Miladi
        /// </summary>
        /// <returns></returns>
        public static DateTime persianToMiladiDateTime(this string date)
        {
            DateTime result = new DateTime();

            if (null == date)
                date = "";

            string[] val = date.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            string[] dateVal = (val.Length > 0 ? val[0].Split('/') : null);
            string[] timeVal = (val.Length > 1 ? val[1].Split(':') : null);

            if ((dateVal.Length > 2) && (timeVal.Length > 2))
            {
                PersianCalendar pc = new PersianCalendar();

                int y;
                int m;
                int d;
                int h;
                int mi;
                int s;
                int mil;

                int.TryParse(dateVal[0], out y);
                int.TryParse(dateVal[1], out m);
                int.TryParse(dateVal[2], out d);
                int.TryParse(timeVal[0], out h);
                int.TryParse(timeVal[1], out mi);
                int.TryParse(timeVal[2], out s);
                if (timeVal.Length > 3)
                    int.TryParse(timeVal[3], out mil);
                else
                    mil = 0;

                result = pc.ToDateTime(y, m, d, h, mi, s, mil);
            }

            return result;
        }
        #endregion
    }
}

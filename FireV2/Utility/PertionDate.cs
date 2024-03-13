using FireV2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FireV2.Utility
{
    public class PertionDate
    {
    

        public static string Today()
        {
            DateTime dt = DateTime.Now;

            System.Globalization.PersianCalendar p = new System.Globalization.PersianCalendar();

            return p.GetYear(dt).ToString() + "/" + p.GetMonth(dt).ToString("0#") + "/" + p.GetDayOfMonth(dt).ToString("0#");
        }
        public static string ConvertNumbersToEnglish(string str)
        {
            return str.Replace("۰", "0").Replace("۱", "1").Replace("۲", "2").Replace("۳", "3").Replace("۴", "4").Replace("۵", "5").Replace("۶", "6").Replace("۷", "7").Replace("۸", "8").Replace("۹", "9");
        }
        public static DateTime ShamsiToDateTime(string StartDate)
        {
            var Date = ConvertNumbersToEnglish(StartDate);
            DateTime DateTimeStr = DateTime.Parse(Date);

            return DateTimeStr;

        }
        public static int CurrentMonth()
        {
            DateTime dt = DateTime.Now;

            System.Globalization.PersianCalendar p = new System.Globalization.PersianCalendar();

            return p.GetMonth(dt);
        }
        public static string MathAge(string Birthdate)
        {
            var DateNow = Utility.PertionDate.Today();
            var yearOfBirthdate = Convert.ToInt32(Birthdate.Substring(0, 4));
            var yearOfDateNow = Convert.ToInt32(DateNow.Substring(0, 4));
            var Age =Convert.ToString( yearOfDateNow - yearOfBirthdate);
            return Age;
        }
        public static int GetAgeN(string date)
        {
            try
            {
                var YearNow =Convert.ToDateTime( Today()).Year;
                var Yeardate = Convert.ToInt32(date.Substring(0, 4));
                var age = YearNow - Yeardate;
                return age;
            }
            catch (Exception ex)
            {
            }
            return 1;
        }
        public static int GetAgeRangeId(string date, int year)
        {
            try
            {
                StoreDb db = new StoreDb();
                var Yeardate =Convert.ToInt32( date.Substring(0,4));
                var age = year - Yeardate;
                return db.AgeRenges.Where(a => age >= a.MinRenge && age <= a.MaxReng).FirstOrDefault().AgeRengeId;
            }
            catch (Exception ex)
            {
            }
            return 1;
        }
        public static int GetAgeRangeId(int age)
        {
            try
            {
                StoreDb db = new StoreDb();
                return db.AgeRenges.Where(a => age >= a.MinRenge && age <= a.MaxReng).FirstOrDefault().AgeRengeId;
            }
            catch (Exception ex)
            {
            }
            return 1;
        }
        public static int  GetAge(string date, int Date2)
        {
            try
            {
                var Yeardate = Convert.ToInt32(date.Substring(0, 4));
                var age = Date2 - Yeardate;
                return age;
            }
            catch (Exception ex)
            {
            }
            return 1;
        }
    }
}
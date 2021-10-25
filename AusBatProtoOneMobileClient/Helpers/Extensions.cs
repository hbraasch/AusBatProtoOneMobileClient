using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xamarin.Forms;
using static AusBatProtoOneMobileClient.Models.KeyTree;

namespace TreeApp.Helpers
{
    public static class Extensions
    {

        public static bool IsEmpty(this string value)
        {
            return String.IsNullOrEmpty(value);
        }

        public static bool IsNotEmpty(this string value)
        {
            return !String.IsNullOrEmpty(value);
        }

        public static bool IsEmpty<T>(this List<T> list)
        {
            if (list == null) return true;
            return list.Count == 0;
        }

        public static string GetUniqueFullFilename(this string filePath)
        {
            if (File.Exists(filePath))
            {
                string folderPath = Path.GetDirectoryName(filePath);
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                string fileExtension = Path.GetExtension(filePath);
                int number = 1;

                Match regex = Regex.Match(fileName, @"^(.+) \((\d+)\)$");

                if (regex.Success)
                {
                    fileName = regex.Groups[1].Value;
                    number = int.Parse(regex.Groups[2].Value);
                }

                do
                {
                    number++;
                    string newFileName = $"{fileName} ({number}){fileExtension}";
                    filePath = Path.Combine(folderPath, newFileName);
                }
                while (File.Exists(filePath));
            }

            return filePath;
        }


        public static string CompleteMessage(this Exception exp)
        {
            string message = string.Empty;
            Exception innerException = exp;
            string seperator = "";

            do
            {
                if (message.IsNotEmpty())
                {
                    seperator = " ----> ";
                }
                message = message + seperator + (string.IsNullOrEmpty(innerException.Message) ? string.Empty : innerException.Message);
                innerException = innerException.InnerException;
            }
            while (innerException != null);

            return message;
        }

        static Stopwatch timer = new Stopwatch();
        /// <summary>
        /// Generate event if double tapped faster than TIMEOUT value. Not perfect however because too fast tapping is missed
        /// </summary>
        /// <param name="listview"></param>
        /// <param name="onDoubleTapped"></param>
        public static void ItemDoubleTapped(this ListView listview, Action onDoubleTapped)
        {
            const int TIMEOUT = 600;           
            timer.Start();
            long mylatesttap = 0;
            listview.ItemTapped += (s, e) => {
                EvalateDoubleTap();
            };

            void EvalateDoubleTap()
            {
                var now = timer.ElapsedMilliseconds;
                var timesince = now - mylatesttap;
                if ((timesince < TIMEOUT) && (timesince > 0))
                {
                    // Double tap   
                    onDoubleTapped?.Invoke();
                }
                else
                {
                    // Too much time to be a doubletap
                }

                mylatesttap = timer.ElapsedMilliseconds;

            }
        }


        public static bool IsValidEmail(this string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                        RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        public static List<T> ToRandomOrder<T>(this List<T> list)
        {
            Random rand = new Random();
            return list.Select(x => new { x = x, r = rand.Next() }).OrderBy(x => x.r).Select(x => x.x).ToList();
        }

        public static string ToAndroidFilenameFormat(this string text)
        {
            var result = text.Replace("-", "_");
            result = result.Replace(" ", "_");
            return result.ToLower();

        }

        public static string ToUpperFirstChar(this string rawText)
        {
            var firstChar = rawText[0].ToString().ToUpper();
            return $"{firstChar}{rawText.Substring(1)}";
        }

    }
}

using System;
using System.IO;

namespace MakeAndroidFilenames
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine($"No file extention e.g. [*.json] parameter");
                return;
            }
            var pathFullname = AppDomain.CurrentDomain.BaseDirectory;
            Console.WriteLine($"Converting files in directory: [{pathFullname}]");

            foreach (var fullFilename in Directory.GetFiles(pathFullname, args[0]))
            {
                var filename = Path.GetFileName(fullFilename);
                var path = Path.GetDirectoryName(fullFilename);
                var androidName = FormatToAndroidFilename(filename);
                var newFullFilename = Path.Combine(path, androidName);
                File.Move(fullFilename, newFullFilename, true);
                Console.WriteLine($"Converting file [{filename} to [{androidName}]");
            }
            Console.WriteLine($"Done");
        }

        public static string FormatToAndroidFilename(string text)
        {
            var result = text.Replace("-", "_");
            result = result.Replace(" ", "_");
            return result.ToLower();

        }
    }
}

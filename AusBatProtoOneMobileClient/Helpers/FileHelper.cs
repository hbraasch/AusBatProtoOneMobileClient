using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace AusBatProtoOneMobileClient.Helpers
{
    public class FileHelper
    {
        public static Stream GetStreamFromFile(string filename)
        {
            var assembly = typeof(App).GetTypeInfo().Assembly;

            var stream = assembly.GetManifestResourceStream("AusBatProtoOneMobileClient." + filename);

            return stream;
        }
    }
}

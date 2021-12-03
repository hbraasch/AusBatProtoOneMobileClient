using AusBatProtoOneMobileClient;
using AusBatProtoOneMobileClient.Models;
using Mobile.Helpers;
using Newtonsoft.Json;
using System.IO;
using System.Reflection;

namespace TreeApps.Models
{
    class Version
    {
        private static Stream GetStreamFromFile(string filename)
        {
            var assembly = typeof(App).GetTypeInfo().Assembly;

            var stream = assembly.GetManifestResourceStream("AusBatProtoOneMobileClient." + filename);

            return stream;
        }

        public static DbaseVersion Get(string filePath)
        {
            #region *// Read from source JSON
            using (Stream stream = GetStreamFromFile(filePath))
            {
                if (stream == null)
                {
                    throw new BusinessException($"Version file at [{filePath}] does not exist");
                }

                using (StreamReader reader = new StreamReader(stream))
                {
                    string versionJson = reader.ReadToEnd();
                    if (string.IsNullOrEmpty(versionJson))
                    {
                        throw new BusinessException($"No data inside version file at [{filePath}]");
                    }
                    try
                    {
                        return JsonConvert.DeserializeObject<DbaseVersion>(versionJson);
                    }
                    catch (System.Exception ex)
                    {
                        throw new BusinessException($"Problem parsing version file at [{filePath}]. {ex.Message}");
                    };
                }
            }
            #endregion
        }
    }
}

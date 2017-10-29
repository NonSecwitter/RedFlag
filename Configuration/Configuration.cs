using Newtonsoft.Json;
using System.IO;

namespace AppConfiguration
{
    public class Configuration
    {
        static string ConfigDirectory = @"./Configurations/";

        static string DBConfigFileName = @"DBConfig.json";
        static string DBConfigPath = ConfigDirectory + DBConfigFileName;

        private DatabaseConfiguration _dbConfig;
        public DatabaseConfiguration DBConfig
        {
            get
            {
                return _dbConfig;
            }
        }

        public Configuration()
        {
            ReadConfigurations();
        }

        ~Configuration()
        {
            File.WriteAllText(DBConfigPath, JsonConvert.SerializeObject(DBConfig));
        }

        private void ReadConfigurations()
        {
            string json = null;

            while (json == null)
            {
                try
                {
                    json = File.ReadAllText(DBConfigPath);
                    _dbConfig = JsonConvert.DeserializeObject<DatabaseConfiguration>(json);
                }
                catch (FileNotFoundException)
                {
                    File.WriteAllText(DBConfigPath,"");
                }
                catch (DirectoryNotFoundException)
                {
                    Directory.CreateDirectory(ConfigDirectory);
                }
            }
        }
    }
}
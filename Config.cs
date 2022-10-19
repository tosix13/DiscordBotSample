using System.IO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace DiscordBot.NetCore
{
    public class Config
    {
        [Serializable()]
        public class ConfigInfo
        {
            public string token { get; set; }
            public string loglevel { get; set; }
            public string rakuten_applicationID { get; set; }
            public ulong channelID { get; set; }
        }

        private const string CONFIG_FILE_PATH = "./config.json";

        public static ConfigInfo GetConfigJSON()
        {
            StreamReader file = File.OpenText(CONFIG_FILE_PATH);
            JsonSerializer serializer = new JsonSerializer();
            var configInfo = (ConfigInfo)serializer.Deserialize(file, typeof(ConfigInfo));
            file.Close();
            return configInfo;
        }
    }
}

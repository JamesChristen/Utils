using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Common.Configuration
{
    public static class ConfigurationProvider
    {
        public static string GetConnectionString(string key)
        {
            IConfigurationRoot configuration = GetConfigRoot();
            string result = configuration.GetConnectionString(key);
            if (string.IsNullOrEmpty(result))
            {
                throw new ArgumentException($"Could not find connection string for {key}");
            }
            return result;
        }

        public static string GetConfigValue(string key)
        {
            IConfigurationRoot configuration = GetConfigRoot();
            string result = configuration[key];
            if (string.IsNullOrEmpty(result))
            {
                throw new ArgumentException($"Could not find config value for {key}");
            }
            return result;
        }

        public static string[] GetArray(string key) => GetArray(key, x => x.Value);

        public static T[] GetArray<T>(string key, Func<IConfigurationSection, T> evaluator)
        {
            IConfigurationSection section = GetConfigRoot().GetSection(key);
            try
            {
                T[] arr = section.GetChildren().ToArray().Select(evaluator).ToArray();
                return arr;
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Could not convert section to {typeof(T)}", ex);
            }
        }

        public static T GetConfigValue<T>(string key)
        {
            IConfigurationRoot configuration = GetConfigRoot();
            try
            {
                return JsonConvert.DeserializeObject<T>(configuration[key]);
            }
            catch
            {
                throw new InvalidOperationException($"Failed to convert config value for key {key} => {configuration[key]} to type {typeof(T)}");
            }
        }

        public static T GetConfigValue<T>(string key, Func<string, T> converter)
        {
            string value = GetConfigValue(key);
            try
            {
                return converter(value);
            }
            catch
            {
                throw new InvalidOperationException($"Failed to convert config value {key} => {value} to type {typeof(T)}");
            }
        }

        private static JObject _configObject;

        public static T GetConfigObject<T>(string key) where T : class, new()
        {
            if (_configObject == null)
            {
                string json;
                try
                {
                    json = File.ReadAllText(GetConfigFile().FullName);
                }
                catch
                {
                    throw new IOException($"Error reading from config file");
                }

                try
                {
                    _configObject = JObject.Parse(json);
                }
                catch
                {
                    throw new InvalidOperationException($"Failed to parse json into {nameof(JObject)}");
                }
            }

            if (_configObject.ContainsKey(key))
            {
                JToken t = _configObject[key];
                T result = t.ToObject<T>();
                return result;
            }
            else
            {
                throw new ArgumentException($"Could not find config value for {key}");
            }
        }

        private static IConfigurationRoot _configRoot;

        private static IConfigurationRoot GetConfigRoot()
        {
            if (_configRoot == null)
            {
                FileInfo file = GetConfigFile();
                IConfigurationBuilder builder =
                    new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                                              .AddJsonFile(file.Name, optional: true, reloadOnChange: false);

                _configRoot = builder.Build();
            }
            return _configRoot;
        }

        private static FileInfo GetConfigFile()
        {
            string configFile = "appsettings.json";
            string directory = AppDomain.CurrentDomain.BaseDirectory;
            FileInfo file = new(Path.Combine(directory, configFile));
            if (!file.Exists)
            {
                throw new FileNotFoundException($"Cannot locate config file {file.FullName}");
            }
            return file;
        }
    }
}

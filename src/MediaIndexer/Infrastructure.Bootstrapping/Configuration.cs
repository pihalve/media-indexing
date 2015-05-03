using System;
using System.Configuration;

namespace Pihalve.MediaIndexer.Infrastructure.Bootstrapping
{
    public static class Configuration
    {
        public static T GetAppSetting<T>(string name)
        {
            var setting = ConfigurationManager.AppSettings[name];
            if (string.IsNullOrWhiteSpace(setting))
            {
                throw new ConfigurationErrorsException(string.Format("Invalid app setting '{0}'", name));
            }

            if (typeof(T) == typeof(string))
            {
                return (T)(object)setting;
            }
            if (typeof(T) == typeof(int))
            {
                int number;
                if (int.TryParse(setting, out number))
                {
                    return (T)(object)number;
                }
            }

            throw new NotSupportedException(string.Format("Data type not supported: {0}", typeof(T)));
        }
    }
}

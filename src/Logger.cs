using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NetNLog = NLog;

namespace VL.NLog
{
    public class Logger
    {
        private static string _defaultConfigName = "NLog.config";

        private static readonly object _configMx = new object();
        private static volatile bool _configured = false;

        private static Dictionary<string, NetNLog.Logger> _loggers = new Dictionary<string, NetNLog.Logger>();

        private NetNLog.Logger _logger;

        public static string GetDefaultConfigPath()
        {
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var configPath = Path.Combine(assemblyFolder, _defaultConfigName);
            return configPath;
        }

        public static void LoadDefaultConfig()
        {
            LoadConfig(GetDefaultConfigPath());
        }

        public static void LoadConfig(string configPath)
        {
            NetNLog.Config.ConfigurationItemFactory.Default = new NetNLog.Config.ConfigurationItemFactory(new Assembly[] { });

            LogManager.Setup()
                .SetupExtensions(s => s.AutoLoadAssemblies(false))
                .LoadConfigurationFromFile(configPath, optional: false);

            _configured = true;
        }

        public Logger(string name)
        {
            if (!_configured)
            {
                lock (_configMx)
                {
                    if (!_configured)
                    {
                        LoadDefaultConfig();
                    }
                }
            }

            if (_loggers.ContainsKey(name))
            {
                _logger = _loggers[name];
            }
            else
            {
                var logger = LogManager.GetLogger(name);
                _loggers.Add(name, logger);
                _logger = logger;
            }
        }

        public void Trace(LogLevel logLevel, string message)
        {
            _logger.Log(Map(logLevel), message);
        }

        public void Trace(LogLevel logLevel, Exception exception)
        {
            _logger.Log(Map(logLevel), exception);
        }

        public void Trace(LogLevel logLevel, Exception exception, string message)
        {
            _logger.Log(Map(logLevel), exception, message);
        }

        private NetNLog.LogLevel Map(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Debug:
                    return NetNLog.LogLevel.Debug;
                case LogLevel.Info:
                    return NetNLog.LogLevel.Info;
                case LogLevel.Warning:
                    return NetNLog.LogLevel.Warn;
                case LogLevel.Error:
                    return NetNLog.LogLevel.Error;
                default:
                    throw new NotImplementedException("Unrecognized LogLevel: " + logLevel);
            }
        }


        public void Debug(string message)
        {
            Trace(LogLevel.Debug, message);
        }

        public void Info(string message)
        {
            Trace(LogLevel.Info, message);
        }

        public void Warning(string message)
        {
            Trace(LogLevel.Warning, message);
        }

        public void Error(string message)
        {
            Trace(LogLevel.Error, message);
        }
    }
}
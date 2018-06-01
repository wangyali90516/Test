using System;
using System.Text;
using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;

namespace RedisToTableTool
{
    public class NLogger
    {
        /// <summary>
        ///     The application logger
        /// </summary>
        private static readonly Lazy<ILogger> ApplicationLogger = new Lazy<ILogger>(() => InitApplicationLogger());

        /// <summary>
        ///     Initializes the <see cref="NLogger" /> class.
        /// </summary>
        static NLogger()
        {
            LoggingConfiguration config = new LoggingConfiguration();

            FileTarget target = new FileTarget
            {
                AutoFlush = true,
                CreateDirs = true,
                Encoding = Encoding.UTF8,
                FileName = Layout.FromString(AppDomain.CurrentDomain.BaseDirectory + "Logs\\${shortdate}.log"),
                Layout = Layout.FromString("${date} [${level:format=FirstCharacter}] ${message} ${onexception:${exception:format=tostring}"),
                ArchiveAboveSize = 1024,
                ArchiveEvery = FileArchivePeriod.Minute,
                ArchiveNumbering = ArchiveNumberingMode.Sequence,
                ArchiveFileName = "${basedir}/Logs/archives/${shortdate}.{#####}.log"
            };
            config.AddTarget("ApplicationTarget", target);
            config.LoggingRules.Add(new LoggingRule("ApplicationLogger", LogLevel.Info, target));
            LogManager.Configuration = config;
        }

        /// <summary>
        ///     Gets the logger.
        /// </summary>
        /// <value>The logger.</value>
        protected static ILogger Logger => ApplicationLogger.Value;

        /// <summary>
        ///     Initializes the application logger.
        /// </summary>
        /// <returns>NLog.ILogger.</returns>
        private static ILogger InitApplicationLogger()
        {
            return LogManager.GetLogger("ApplicationLogger");
        }
    }
}
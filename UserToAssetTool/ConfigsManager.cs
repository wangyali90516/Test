using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace UserToAssetTool
{
    public static class ConfigsManager
    {
        /// <summary>
        ///     Redis用20160614add
        ///     The lazy connection
        /// </summary>
        private static readonly Lazy<ConnectionMultiplexer> LazyConnection;

        /// <summary>
        ///     Redis用20160614add
        ///     Initializes static members of the <see cref="ConfigsManager" /> class.
        ///     *意導入的namespace,別導入maolab的
        /// </summary>
        static ConfigsManager()
        {
            ConfigurationOptions options = GetConfigurationOptions();
            LazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(options));
        }

        /// <summary>
        ///     Redis用20160614add
        ///     Gets the redis connection multiplexer.
        /// </summary>
        /// <value>The redis connection multiplexer.</value>
        private static ConnectionMultiplexer RedisConnectionMultiplexer
        {
            get { return LazyConnection.Value; }
        }

        /// <summary>
        ///     Redis用20160614add
        ///     Gets the biz redis client.
        /// </summary>
        /// <returns>IDatabase.</returns>
        public static IDatabase GetBizRedisClient()
        {
            return RedisConnectionMultiplexer.GetDatabase(2, new object());
        }

        /// <summary>
        ///     Redis用20160614add
        ///     Gets the configuration options.
        /// </summary>
        /// <returns>ConfigurationOptions.</returns>
        private static ConfigurationOptions GetConfigurationOptions()
        {
            ConfigurationOptions options = ConfigurationOptions.Parse(ConfigurationManager.AppSettings["Redis"], true);
            options.AbortOnConnectFail = false;
            options.AllowAdmin = true;
            options.ConnectRetry = 10;
            options.ConnectTimeout = 20000;
            options.DefaultDatabase = 0;
            options.ResponseTimeout = 20000;
            options.Ssl = false;
            options.SyncTimeout = 20000;
            return options;
        }

    }
}

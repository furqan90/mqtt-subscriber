using log4net.Appender;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MQTTSubscribeClient
{
    public class RollingFileByMaxAgeAppender : RollingFileAppender
    {
        //IConfiguration _configuration;
        //LoggingSettings _loggingSettings = new LoggingSettings();
        //public RollingFileByMaxAgeAppender() : base()
        //{
        //    IConfigurationBuilder builder = new ConfigurationBuilder()
        //    .SetBasePath(Directory.GetCurrentDirectory())
        //    .AddJsonFile("appsettings.json");

        //    _configuration = builder.Build();
        //    _configuration.GetSection("Logging").Bind(_loggingSettings);
        //}

        protected override void AdjustFileBeforeAppend()
        {
            base.AdjustFileBeforeAppend();

            //var maxAgeRollBackups = _loggingSettings.MaxAgeRollBackups;

            var maxAgeRollBackups = 3;

            foreach (string file in Directory.GetFiles(Path.GetDirectoryName(File)))
            {
                if (System.IO.File.GetLastWriteTime(file) < DateTime.Today.AddDays(-1 * maxAgeRollBackups))
                    DeleteFile(file);
            }
        }
    }
}


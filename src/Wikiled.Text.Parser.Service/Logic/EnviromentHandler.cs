using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Wikiled.Common.Utilities.Config;

namespace Wikiled.Text.Parser.Service.Logic
{
    public class EnviromentHandler : IEnviromentHandler
    {
        private static readonly object syncRoot = new object();

        private readonly IHostingEnvironment environment;

        private readonly IApplicationConfiguration appConfig;

        public readonly DocumentsConfig config;

        public EnviromentHandler(IApplicationConfiguration appConfig, IHostingEnvironment environment, DocumentsConfig config)
        {
            this.appConfig = appConfig ?? throw new ArgumentNullException(nameof(appConfig));
            this.environment = environment ?? throw new ArgumentNullException(nameof(environment));
            this.config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public string GetFileName(string name)
        {
            var path = config.Save;
            if (!Path.IsPathRooted(path))
            {
                string webRootPath = environment.ContentRootPath;
                path = Path.Combine(webRootPath, path);
            }

            var fileName = $"{Path.GetFileNameWithoutExtension(name)}_{appConfig.Now:yyyyMMddhhmmss}{Path.GetExtension(name)}";
            if (!Directory.Exists(path))
            {
                lock (syncRoot)
                {
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                }
            }

            return Path.Combine(path, fileName);
        }
    }
}

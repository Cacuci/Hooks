namespace Hooks
{
    sealed class AppConfig
    {
        public AppSettings Setting { get; set; }        

        private static AppConfig _appConfig;

        public static AppConfig Instance()
        {
            if (_appConfig is null)
            {
                _appConfig = InitOptions<AppConfig>();
            }

            return _appConfig;
        }

        private static T InitOptions<T>() where T : new()
        {
            var config = InitConfig();

            return config.Get<T>();
        }

        private static IConfigurationRoot InitConfig()
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env}.json", true, true)
                .AddEnvironmentVariables();

            return builder.Build();
        }
    }
}

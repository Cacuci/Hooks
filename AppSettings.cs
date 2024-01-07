namespace Hooks
{
    public class AppSettings
    {  
        public string UserName { get; set; }
        
        public string Password { get; set; }

        public string Host { get; set; }

        public string UrlCrumb { get; set; }

        public IEnumerable<Connection> Jobs { get; set; }
    }
}

using Hooks;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/hooks", async () =>
{
    using (HttpClient client = new HttpClient())
    {
        string userName = AppConfig.Instance().Setting.UserName,
               password = AppConfig.Instance().Setting.Password,
               host = AppConfig.Instance().Setting.Host,
               urlCrumb = host + AppConfig.Instance().Setting.UrlCrumb;
        
        var builds = AppConfig.Instance().Setting.Jobs;                

        using (var requestCrumb = new HttpRequestMessage(HttpMethod.Get, urlCrumb))
        {
            requestCrumb.Headers.Add("User-Agent", userName);
            requestCrumb.Headers.Add("Authorization", password);

            var result = await client.SendAsync(requestCrumb);

            if (result is not null)
            {
                using (var responseStream = new StreamReader(result.Content.ReadAsStream()))
                {
                    string response = responseStream.ReadToEnd();

                    builds.ToList().ForEach(build =>
                    {
                        var url = host + build.Url;

                        using (var requestBuild = new HttpRequestMessage(HttpMethod.Post, url))
                        {
                            var crumb = JsonConvert.DeserializeObject<Crumbs>(response);

                            if (crumb is not null)
                            {
                                requestBuild.Headers.Add("Accept", "application/json");
                                requestBuild.Headers.Add("Jenkins-Crumb", crumb.Crumb);
                                requestBuild.Headers.Add("Jenkins_User", crumb.CrumbRequestField);

                                client.Send(requestBuild);
                            }
                        }
                    });
                }
            }
        }
    }
});

app.Run();

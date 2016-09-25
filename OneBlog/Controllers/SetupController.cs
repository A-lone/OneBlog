using System.Web.Http;

public class SetupController : ApiController
{
    public string Get(string version)
    {
        var updater = new OneBlog.Core.Data.Services.UpdaterService();
        return updater.Check(version);
    }
}

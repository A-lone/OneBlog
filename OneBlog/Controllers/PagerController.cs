using System.Web.Http;

public class PagerController : ApiController
{
    public string Get()
    {
        return OneBlog.Core.Pager.Render();
    }
}

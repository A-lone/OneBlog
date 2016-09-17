using System;
using OneBlog.Core;
using OneBlog.Core.Web.Controls;
using OneBlog.Core.Web.Extensions;

/// <summary>
/// Sends emails to newsletter subscribers
/// </summary>
[Extension("Sends emails to newsletter subscribers", "3.3.0.0", "OneBlog.NET")]
public class SendNewsletters
{
    #region Constructors and Destructors

    static SendNewsletters()
    {
        Post.Published += Post_Published;
    }

    private static void Post_Published(object sender, EventArgs e)
    {
        if (!ExtensionManager.ExtensionEnabled("SendNewsletters"))
            return;

        var publishable = (IPublishable)sender;
        OneBlog.NET.Custom.Widgets.Newsletter.SendEmails(publishable);
    }

    #endregion
}
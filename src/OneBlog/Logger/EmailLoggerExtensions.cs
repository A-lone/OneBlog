using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using One.Services;

namespace One.Logger
{
  public static class EmailLoggerExtensions
  {
    public static ILoggerFactory AddEmail(this ILoggerFactory factory, 
                                          IMailService mailService, 
                                          Func<string, LogLevel, bool> filter = null)
    {
      factory.AddProvider(new EmailLoggerProvider(filter, mailService));
      return factory;
    }

    public static ILoggerFactory AddEmail(this ILoggerFactory factory, IMailService mailService, LogLevel minLevel)
    {
      return AddEmail(
          factory,
          mailService,
          (_, logLevel) => logLevel >= minLevel);
    }
  }
}

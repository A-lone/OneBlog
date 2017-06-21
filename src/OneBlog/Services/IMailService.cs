using System.Threading.Tasks;

namespace One.Services
{
  public interface IMailService
  {
    Task SendMail(string template, string name, string email, string subject, string msg);
  }
}
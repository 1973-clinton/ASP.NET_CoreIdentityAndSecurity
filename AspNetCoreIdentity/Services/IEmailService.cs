using System.Threading.Tasks;

namespace AspNetCoreIdentity.Services
{
    public interface IEmailService
    {
        Task Send(string from, string to, string subject, string body);
    }
}
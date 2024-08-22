using Hiwu.SpecificationPattern.Application.DataTransferObjects.Email;

namespace Hiwu.SpecificationPattern.Application.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendAsync(EmailRequest request);
    }
}

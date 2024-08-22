using System.ComponentModel.DataAnnotations;

namespace Hiwu.SpecificationPattern.Application.DataTransferObjects.Account
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}

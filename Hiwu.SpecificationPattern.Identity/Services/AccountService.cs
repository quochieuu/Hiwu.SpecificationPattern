using Hiwu.SpecificationPattern.Application.DataTransferObjects.Account;
using Hiwu.SpecificationPattern.Application.DataTransferObjects.Email;
using Hiwu.SpecificationPattern.Application.Enums;
using Hiwu.SpecificationPattern.Application.Exceptions;
using Hiwu.SpecificationPattern.Application.Interfaces.Services;
using Hiwu.SpecificationPattern.Application.Wrappers;
using Hiwu.SpecificationPattern.Domain.Settings;
using Hiwu.SpecificationPattern.Identity.Entities;
using Hiwu.SpecificationPattern.Shared.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Hiwu.SpecificationPattern.Identity.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JWTSettings _jwtSettings;
        private readonly IEmailService _emailService;
        public AccountService(UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IOptions<JWTSettings> jwtSettings,
            SignInManager<ApplicationUser> signInManager,
            IEmailService emailService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _jwtSettings = jwtSettings.Value;
            _emailService = emailService;
        }

        public async Task<BaseResponse<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email.Trim());

            if (user == null)
            {
                throw new ApiException($"You are not registered with '{request.Email}'.") { StatusCode = (int)HttpStatusCode.BadRequest };
            }

            if (!user.EmailConfirmed)
            {
                throw new ApiException($"Account Not Confirmed for '{request.Email}'.") { StatusCode = (int)HttpStatusCode.BadRequest };
            }

            var signInResult = await _signInManager.PasswordSignInAsync(user, request.Password, false, lockoutOnFailure: false);
            if (!signInResult.Succeeded)
            {
                throw new ApiException($"Invalid Credentials for '{request.Email}'.") { StatusCode = (int)HttpStatusCode.BadRequest };
            }

            var ipAddress = IpHelper.GetIpAddress();
            var jwtSecurityToken = await GenerateJWToken(user, ipAddress);

            var response = new AuthenticationResponse
            {
                Id = user.Id.ToString(),
                JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Email = user.Email,
                UserName = user.UserName,
                Roles = (await _userManager.GetRolesAsync(user).ConfigureAwait(false)).ToList(),
                IsVerified = user.EmailConfirmed,
                RefreshToken = await GenerateRefreshToken(user)
            };

            return new BaseResponse<AuthenticationResponse>(response, $"Authenticated {user.UserName}");
        }

        public async Task<BaseResponse<string>> ConfirmEmailAsync(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ApiException("User not found.") { StatusCode = (int)HttpStatusCode.NotFound };
            }

            var decodedCode = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, decodedCode);

            if (result.Succeeded)
            {
                return new BaseResponse<string>(
                    user.Id.ToString(),
                    message: $"Account confirmed for {user.Email}. You can now use the /api/Account/authenticate endpoint."
                );
            }

            throw new ApiException($"An error occurred while confirming {user.Email}.")
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        }

        public async Task ForgotPasswordAsync(ForgotPasswordRequest request, string uri)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null) return;

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var route = "api/account/reset-password/";
            var enpointUri = new Uri(string.Concat($"{uri}/", route));
            var emailRequest = new EmailRequest()
            {
                Body = $"You have to send a request to the '{enpointUri}' service with reset token - {code}",
                To = request.Email,
                Subject = "Reset Password",
            };

            await _emailService.SendAsync(emailRequest);
        }

        public async Task<BaseResponse<string>> LogoutAsync(string userEmail)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user != null)
            {
                await _userManager.RemoveAuthenticationTokenAsync(user, "MyApp", "RefreshToken");
            }

            await _signInManager.SignOutAsync();

            return new BaseResponse<string>(userEmail, message: $"Logout.");
        }

        public async Task<BaseResponse<AuthenticationResponse>> RefreshTokenAsync(RefreshTokenRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new ApiException($"You are not registered with '{request.Email}'.") { StatusCode = (int)HttpStatusCode.BadRequest };
            }

            if (!user.EmailConfirmed)
            {
                throw new ApiException($"Account Not Confirmed for '{request.Email}'.") { StatusCode = (int)HttpStatusCode.BadRequest };
            }

            var refreshToken = await _userManager.GetAuthenticationTokenAsync(user, "MyApp", "RefreshToken");
            var isValid = await _userManager.VerifyUserTokenAsync(user, "MyApp", "RefreshToken", request.Token);
            if (!refreshToken.Equals(request.Token) || !isValid)
            {
                throw new ApiException($"Your token is not valid.") { StatusCode = (int)HttpStatusCode.BadRequest };
            }

            var ipAddress = IpHelper.GetIpAddress();
            var jwtSecurityToken = await GenerateJWToken(user, ipAddress);
            var response = new AuthenticationResponse
            {
                Id = user.Id.ToString(),
                JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Email = user.Email,
                UserName = user.UserName,
                Roles = (await _userManager.GetRolesAsync(user).ConfigureAwait(false)).ToList(),
                IsVerified = user.EmailConfirmed,
                RefreshToken = await GenerateRefreshToken(user)
            };

            await _signInManager.SignInAsync(user, false);
            return new BaseResponse<AuthenticationResponse>(response, $"Authenticated {user.UserName}");
        }

        public async Task<BaseResponse<string>> RegisterAsync(RegisterRequest request, string uri)
        {
            ApplicationUser findUser = await _userManager.FindByNameAsync(request.UserName);
            if (findUser != null)
            {
                throw new ApiException($"Username '{request.UserName}' is already taken.") { StatusCode = (int)HttpStatusCode.BadRequest };
            }
            findUser = await _userManager.FindByEmailAsync(request.Email);
            if (findUser != null)
            {
                throw new ApiException($"Email {request.Email} is already registered.") { StatusCode = (int)HttpStatusCode.BadRequest };
            }
            ApplicationUser newUser = new ApplicationUser
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName
            };
            var result = await _userManager.CreateAsync(newUser, request.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, Roles.Basic.ToString());
                var verificationUri = await SendVerificationEmail(newUser, uri);

                return new BaseResponse<string>(newUser.Id.ToString(), message: $"User Registered. Please confirm your account by visiting this URL {verificationUri}");
            }
            else
            {
                throw new ApiException($"{result.Errors}") { StatusCode = (int)HttpStatusCode.InternalServerError };
            }
        }

        public async Task<BaseResponse<string>> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null) throw new ApiException($"You are not registered with '{request.Email}'.");

            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);
            if (result.Succeeded)
            {
                return new BaseResponse<string>(request.Email, message: $"Password Resetted.");
            }
            else
            {
                throw new ApiException($"Error occured while reseting the password. Please try again.");
            }
        }

        //public async Task<List<ApplicationUser>> GetUsers()
        //{
        //    return await _userManager.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role).ToListAsync(); //lazzyloading
        //}

        private async Task<string> SendVerificationEmail(ApplicationUser newUser, string uri)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
            var encodedCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var route = "api/account/confirm-email/";
            var endpointUri = new Uri(string.Concat($"{uri}/", route));
            var verificationUri = QueryHelpers.AddQueryString(endpointUri.ToString(), "userId", newUser.Id.ToString());
            verificationUri = QueryHelpers.AddQueryString(verificationUri, "code", encodedCode);

            var emailRequest = new EmailRequest
            {
                From = "quochieuu@gmail.com",
                To = newUser.Email,
                Body = $"Please confirm your account by visiting this URL: {verificationUri}",
                Subject = "Confirm Registration"
            };
            await _emailService.SendAsync(emailRequest);

            return verificationUri;
        }

        private async Task<JwtSecurityToken> GenerateJWToken(ApplicationUser user, string ipAddress)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var roleClaims = new List<Claim>();

            for (int i = 0; i < roles.Count; i++)
            {
                roleClaims.Add(new Claim("roles", roles[i]));
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id.ToString()),
                new Claim("ip", ipAddress)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }

        private async Task<string> GenerateRefreshToken(ApplicationUser user)
        {
            await _userManager.RemoveAuthenticationTokenAsync(user, "MyApp", "RefreshToken");

            var newRefreshToken = await _userManager.GenerateUserTokenAsync(user, "MyApp", "RefreshToken");
            var result = await _userManager.SetAuthenticationTokenAsync(user, "MyApp", "RefreshToken", newRefreshToken);

            if (!result.Succeeded)
            {
                throw new ApiException("An error occurred while setting the refresh token.")
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }

            return newRefreshToken;
        }
    }
}

using Microsoft.AspNetCore.Identity;
using MyMusic.Models.Data;
using MyMusic.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyMusic.Interfaces
{
    public interface IUserService
    {
        Task<AuthenticationResult> RegisterUserAsync(UserRegistrationViewModel user);

        Task<AuthenticationResult> LoginAsync(string email, string password);

        Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken);

        Task<ApplicationUser> GetUserAsync(string id);
    }
}

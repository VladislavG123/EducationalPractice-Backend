using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using GreenPoint.Dotnet.Contracts.Dtos;
using GreenPoint.Dotnet.Contracts.Options;
using GreenPoint.Dotnet.DataAccess.Models;
using GreenPoint.Dotnet.DataAccess.Models.Abstract;
using GreenPoint.Dotnet.DataAccess.Providers;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace GreenPoint.Dotnet.Services
{
    public class UserAuthenticationService 
    {
        private readonly UserProvider _userProvider;

        private SecretOption SecretOptions { get; }

        public UserAuthenticationService(UserProvider userProvider, IOptions<SecretOption> secretOptions)
        {
            SecretOptions = secretOptions.Value;
            this._userProvider = userProvider;
        }

        /// <summary>
        ///     Getting a token before creating a new user
        /// </summary>
        /// <param name="newUser">Data transfer object for registration new user</param>
        /// <exception cref="ArgumentException">User is already exists</exception>
        /// <returns></returns>
        public async Task<string> Register(UserRegistrationDto newUser)
        {
            // Try to get a data from the newUser parameter
            var user = await _userProvider.FirstOrDefault(x => x.Email == newUser.Email);

            if (user is not null)
                throw new ArgumentException("This user is already exists");


            // Add new user to table
            await _userProvider.Add(new User
            {
                Username = newUser.Username,
                Email = newUser.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(newUser.Password)
            });

            return GenerateJwtToken(newUser.Email);
        }



        /// <summary>
        ///     Get Jwt token by exited user
        /// </summary>
        /// <param name="emailOrPhone"></param>
        /// <param name="password"></param>
        /// <exception cref="ArgumentException">User is not found</exception>
        /// <returns>Jwt token</returns>
        public async Task<string> Authenticate(string email, string password)
        {
            // Find data by arguments
            var user = await _userProvider.GetByEmailOrPhone(email);

            // if user is not found, throw exception
            if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
                throw new ArgumentException("Incorrect password");

            return GenerateJwtToken(user.Email);
        }


        public string GenerateJwtToken(string email)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(SecretOptions.JwtSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email, email),
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(securityToken);
        }


        /// <summary>
        ///    Token decryption
        /// </summary>
        /// <param name="token"></param>
        /// <exception cref="ArgumentException">throws when could not parse claims</exception>
        /// <returns>Owner's data</returns>
        private UserClaimsDto DecryptToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();

            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            if (tokenS?.Claims is List<Claim> claims)
            {
                return new UserClaimsDto()
                {
                    Email = claims[0].Value
                };
            }

            throw new ArgumentException();
        }

        /// <summary>
        /// Gets User by headers from Request
        /// Usage in controllers: 
        /// GetUserByHeaders(Request.Headers[HeaderNames.Authorization].ToArray())
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        public async Task<User> GetUserByHeaders(string[] headers)
        {
            var token = headers[0].Replace("Bearer ", "");

            return await _userProvider.GetByEmailOrPhone(DecryptToken(token).Email);
        }
    }
}

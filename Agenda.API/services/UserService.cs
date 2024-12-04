using System.Security.Claims;
using Agenda.API.interfaces;
using Agenda.API.models;
using Agenda.API.validators;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace Agenda.API.services;

public class UserService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public UserService(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public Task<bool> CreateAsync(UserModel user)
    {
        return _userRepository.CreateAsync(user);
    }

    public Task<UserModel?> GetByIdAsync(string cpf)
    {
        return _userRepository.GetByIdAsync(cpf);
    }

    public Task<bool> UpdatePasswordAsync(string password, string id)
    {
        try
        {
            return _userRepository.UpdatePasswordAsync(password, id);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public Task<bool> DeleteByIdAsync(string id)
    {
        try
        {
            return _userRepository.DeleteByIdAsync(id);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public byte[] GetPasswordHash(string password, byte[] key, byte[] salt)
    {
        byte[] cipheredText;
        using (Aes aes = Aes.Create())
        {
            ICryptoTransform encryptor = aes.CreateEncryptor(key, salt);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                    {
                        streamWriter.Write(password);
                    }
                    cipheredText = memoryStream.ToArray();
                }
            }
        }
        return cipheredText;
    }

    public string GeneretaToken(string username)
    {
        string secretKey = _configuration.GetValue<string>("Jwt:KeySecret")!;
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim("email_verified", "true")
            ]),
            Expires = DateTime.UtcNow.AddMinutes(15),
            SigningCredentials = credentials,
            Issuer = _configuration.GetValue<string>("Jwt:Issuer"),
            Audience = _configuration.GetValue<string>("Jwt:Audience")
        };

        var tokenHandler = new JsonWebTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return token;
    }
}
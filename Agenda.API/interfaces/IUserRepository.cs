using Agenda.API.models;

namespace Agenda.API.interfaces;

public interface IUserRepository
{
    Task<bool> CreateAsync(UserModel user);

    Task<UserModel?> GetByIdAsync(string cpf);

    Task<bool> UpdatePasswordAsync(string password, string id);

    Task<bool> DeleteByIdAsync(string id);
}
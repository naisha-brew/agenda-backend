using Agenda.API.Database;
using Agenda.API.interfaces;
using Agenda.API.models;
using Dapper;

namespace Agenda.API.repository;

public class UserRepository : IUserRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public UserRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<bool> CreateAsync(UserModel user)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync().ConfigureAwait(false);
        try
        {
            await connection.ExecuteAsync("""
                                          INSERT INTO Users (email,userpwd,username) 
                                          VALUES(@Email,@Userpwd,@UserName)
                                          """,user);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public async Task<UserModel?> GetByIdAsync(string email)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync().ConfigureAwait(false);
        try
        {
            var result = await connection
                .QuerySingleOrDefaultAsync<UserModel>("""
                                                      SELECT * FROM Users WHERE email = @email limit 1
                                                      """, new { email });
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }

    public async Task<bool> UpdatePasswordAsync(string password, string email)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync().ConfigureAwait(false);
        try
        {
            var result = GetByIdAsync(email).Result;
            if (result == null)
            {
                throw new Exception("User not found");
            }
            await connection.ExecuteAsync("""
                                          UPDATE Users SET userpwd = @password
                                          where email = @email
                                          """, new {email, password});
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public async Task<bool> DeleteByIdAsync(string id)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync().ConfigureAwait(false);
        try
        {
            var result = GetByIdAsync(id).Result;
            if (result == null)
            {
                throw new Exception("User not found");
            }
            await connection.ExecuteAsync("""
                                          DELETE FROM Users WHERE cpf = @id
                                          """, new {id});
            return true;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}
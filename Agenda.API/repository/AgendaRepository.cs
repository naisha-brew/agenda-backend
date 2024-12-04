using Agenda.API.Database;
using Agenda.API.interfaces;
using Dapper;

namespace Agenda.API.models;

public class AgendaRepository : IAgendaRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public AgendaRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<bool> CreateAgendaAsync(AgendaModel agenda)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync().ConfigureAwait(false);
        try
        {
            await connection.ExecuteAsync("""
                                    INSERT INTO Agenda (agendaId, email, username, phone, useremail)
                                    values (@agendaId, @email, @username, @phone, @useremail)
                                    """, agenda);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public async Task<List<AgendaModel>> GetAgendaByCpfAsync(string email)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync().ConfigureAwait(false);
        try
        {
            var result = connection.QueryAsync<AgendaModel>("""
                                                       select * from Agenda where useremail = @email  
                                                       """, new { email }).Result.ToList();
            if (result.Count > 0)
            {
                return result;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        return new List<AgendaModel>();
    }

    public async Task<bool> UpdateAgendaAsync(AgendaModel agenda)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync().ConfigureAwait(false);
        try
        {
            await connection.QueryAsync("""
                                  update Agenda set email = @email, username = @username, phone = @phone
                                  where agendaId = @agendaId
                                  """, agenda);
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
            await connection.QueryAsync("""
                                        delete from Agenda where agendaId = @id::uuid
                                        """, new { id });
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }
}
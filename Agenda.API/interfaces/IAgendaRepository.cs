using Agenda.API.models;

namespace Agenda.API.interfaces;

public interface IAgendaRepository
{
    Task<bool> CreateAgendaAsync(AgendaModel agenda);

    Task<List<AgendaModel>> GetAgendaByCpfAsync(string cpf);

    Task<bool> UpdateAgendaAsync(AgendaModel agenda);

    Task<bool> DeleteByIdAsync(string id);
}
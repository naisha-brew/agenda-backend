using Agenda.API.dto;
using Agenda.API.interfaces;
using Agenda.API.models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Agenda.API.services;

public class AgendaService
{
    private readonly IAgendaRepository _agendaRepository;

    public AgendaService(IAgendaRepository agendaRepository)
    {
        _agendaRepository = agendaRepository;
    }

    public Task<bool> CreateAgendaAsync(AgendaRequest agenda)
    {
        try
        {
            var id = Guid.NewGuid();
            AgendaModel agendaModel = new AgendaModel
            {
                AgendaId = id,
                Email = agenda.Email,
                Username = agenda.UserName,
                Phone = agenda.Phone,
                UserEmail = agenda.UserEmail
            };
            
            var result = _agendaRepository.CreateAgendaAsync(agendaModel);
            if (result.Result)
            {
                return Task.FromResult(true);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        return Task.FromResult(false);
    }

    public Task<List<AgendaModel>> GetAgendaByCpfAsync(string email)
    {
        try
        {
            var result = _agendaRepository.GetAgendaByCpfAsync(email);
            if (result.Result.Count > 0)
            {
                return Task.FromResult(result.Result.ToList());
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        return Task.FromResult(new List<AgendaModel>());
    }

    public Task<bool> UpdateAgendaAsync(AgendaModel agenda)
    {
        try
        {
            return _agendaRepository.UpdateAgendaAsync(agenda);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public Task<bool> DeleteByIdAsync(string id)
    {
        try
        {
            return _agendaRepository.DeleteByIdAsync(id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
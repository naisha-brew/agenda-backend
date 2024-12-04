using Agenda.API.dto;
using Agenda.API.interfaces;
using Agenda.API.models;
using Agenda.API.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Agenda.API.controller;
[ApiController]
[Route("api/[controller]")]
[Authorize]
[EnableCors("AllowAll")]
public class AgendaController : ControllerBase
{
    private readonly AgendaService _agendaService;

    public AgendaController(AgendaService agendaService)
    {
        _agendaService = agendaService;
    }

    [HttpPost]
    [Route("createAgenda")]
    public IActionResult CreateAgenda(AgendaRequest agenda)
    {
        try
        {
            var result = _agendaService.CreateAgendaAsync(agenda);
            if (result.Result)
            {
                return Ok("User add to Agenda");
            }
            return BadRequest("User already exists");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet]
    [Route("getAllAgenda")]
    public IActionResult GetAllAgenda(string email)
    {
        try
        {
            var result = _agendaService.GetAgendaByCpfAsync(email);
            if (result.Result.Count > 0)
            {
                return Ok(result.Result);
            }
            return NotFound("Agenda not found");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpPut]
    [Route("updateAgenda")]
    public IActionResult UpdateAgenda(AgendaModel agenda)
    {
        try
        {
            var result = _agendaService.UpdateAgendaAsync(agenda);
            if (result.Result)
            {
                return Ok("Agenda updated");
            }
            return BadRequest("Agenda not found");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpDelete]
    [Route("deleteAgenda")]
    public IActionResult DeleteAgenda(string id)
    {
        try
        {
            var result = _agendaService.DeleteByIdAsync(id);
            if (result.Result)
            {
                return Ok("Agenda deleted");
            }
            return BadRequest("Agenda not found");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
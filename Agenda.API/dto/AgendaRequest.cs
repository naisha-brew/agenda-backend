namespace Agenda.API.dto;

public class AgendaRequest
{
    public required string Email { get; set; }
    
    public required string UserName { get; set; }
    
    public required string Phone { get; set; }
    
    public required string UserEmail { get; set; }
}
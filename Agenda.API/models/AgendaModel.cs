using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agenda.API.models;

[Table("Agenda")]
public class AgendaModel
{
    public AgendaModel()
    {
        
    }
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public required Guid AgendaId { get; set; }
    
    [Column(TypeName = "varchar(100)")]
    public required string Email { get; set; }
    
    [Column(TypeName = "varchar(100)")]
    public required string Username { get; set; }
    
    [Column(TypeName = "varchar(11)")]
    public required string Phone { get; set; }
    
    [Column(TypeName = "varchar(11)")]
    [EmailAddress]
    public required string UserEmail { get; set; }
}
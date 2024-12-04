using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Agenda.API.models;

[Index(nameof(Username), IsUnique = true)]
[Table("Users")]
public class UserModel
{
    [Key]
    [Column(TypeName = "varchar(100)")]
    public required string Email { get; set; }
    
    [Column(TypeName = "varchar(20)")]
    public required string Userpwd { get; set; }
    
    [Column(TypeName = "varchar(50)")]
    public required string Username { get; set; }
    
}
using Agenda.API.models;
using Microsoft.EntityFrameworkCore;

namespace Agenda.API.Database;

public class AgendaDbContext : DbContext
{
    public AgendaDbContext(DbContextOptions<AgendaDbContext> options) : base(options)
    {
        
    }
    DbSet<AgendaModel> Agenda { get; set; }
    DbSet<UserModel> User { get; set; }
}
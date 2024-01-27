using Servindustria.Models;
using Microsoft.EntityFrameworkCore;

namespace Servindustria.Data;

public class ServindustriaDBContext : DbContext {
    public ServindustriaDBContext(DbContextOptions<ServindustriaDBContext> options) : base(options) {}
    
    public DbSet<User> Users { get; set; }
}
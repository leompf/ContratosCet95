using ContratosCet95.Web.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ContratosCet95.Web.Data;

public class DataContext : IdentityDbContext<User>
{

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {          
    }
}

using DLyah_Boutique_System.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DLyah_Boutique_System.Data;

public class IdentityOnlyDbContext : IdentityDbContext<UserModel, IdentityRole<int>, int> {
    public IdentityOnlyDbContext(DbContextOptions<IdentityOnlyDbContext> options) : base(options) {
    }

    protected override void OnModelCreating(ModelBuilder builder) {
        base.OnModelCreating(builder);
        // Podemos adicionar configurações específicas do Identity aqui se necessário, mas geralmente não precisa.
    }
}
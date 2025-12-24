using Flashcards.Models;
using Microsoft.EntityFrameworkCore;

namespace Flashcards.Data;

public class DatabaseContext : DbContext
{
    // Define DbSets for your entities here
    public DbSet<Flashcard> Flashcards { get; set; }
    public DbSet<Stack> Stacks { get; set; }
    public DbSet<StudySession> StudySessions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options
            .UseSqlServer("Server=localhost;Database=Flashcards;User Id=flashcard;Password=Password1;Encrypt=False;TrustServerCertificate=True;");
    }
}

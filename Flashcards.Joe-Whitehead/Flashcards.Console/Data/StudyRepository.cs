using Flashcards.Models;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;

namespace Flashcards.Data;

internal class StudyRepository
{
    public async Task AddSessionAsync(StudySession session)
    {
        using var db = new DatabaseContext();
        db.StudySessions.Add(session);
        await db.SaveChangesAsync();
        AnsiConsole.MarkupLine("[green]Study session saved successfully![/]");
    }
}

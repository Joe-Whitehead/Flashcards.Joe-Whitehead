using Flashcards.Models;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;

namespace Flashcards.Data;

internal class FlashcardRepository
{
    internal async Task<bool> InsertFlashcardAsync(Flashcard flashcard)
    {
        using var db = new DatabaseContext();
        try
        {
            db.Flashcards.Add(flashcard);
            await db.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            AnsiConsole.Markup($"Failed to add Flashard: {ex.Message}");
            return false;
        }
    }

    internal async Task<bool> UpdateFlashcardAsync(Flashcard flashcard)
    {
        using var db = new DatabaseContext();
        try
        {
            db.Flashcards.Update(flashcard);
            await db.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            AnsiConsole.Markup($"Failed to update Flashcard: {ex.Message}");
            return false;
        }
    }

    internal async Task<bool> DeleteFlashcardAsync(Flashcard flashcard)
    {
        using var db = new DatabaseContext();
        try
        {
            db.Flashcards.Remove(flashcard);
            await db.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            AnsiConsole.Markup($"Failed to delete Flashcard: {ex.Message}");
            return false;
        }
    }

    internal async Task<Flashcard?> GetFlashcardByIdAsync(int id)
    {
        using var db = new DatabaseContext();
        try
        {
            return await db.Flashcards.FirstOrDefaultAsync(f => f.Id == id);
        }
        catch (Exception ex)
        {
            AnsiConsole.Markup($"Failed to retrieve Flashcard: {ex.Message}");
            return null;
        }
    }

    internal async Task<List<Flashcard>> GetAllFlashcardsAsync()
    {
        using var db = new DatabaseContext();
        try
        {
            return await db.Flashcards.ToListAsync();
        }
        catch (Exception ex)
        {
            AnsiConsole.Markup($"Failed to retrieve Flashcards: {ex.Message}");
            return new List<Flashcard>();
        }
    }
}

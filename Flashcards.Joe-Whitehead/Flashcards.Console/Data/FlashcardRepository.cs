using Flashcards.Models;
using Spectre.Console;

namespace Flashcards.Data;

internal class FlashcardRepository
{
    internal bool InsertFlashcard(Flashcard flashcard)
    {
        using var db = new DatabaseContext();
        try
        {
            db.Flashcards.Add(flashcard);
            db.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            AnsiConsole.Markup($"Failed to add Flashard: {ex.Message}");
            return false;
        }
    }

    internal bool UpdateFlashcard(Flashcard flashcard)
    {
        using var db = new DatabaseContext();
        try
        {
            db.Flashcards.Update(flashcard);
            db.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            AnsiConsole.Markup($"Failed to update Flashcard: {ex.Message}");
            return false;
        }
    }

    internal bool DeleteFlashcard(Flashcard flashcard)
    {
        using var db = new DatabaseContext();
        try
        {
            db.Flashcards.Remove(flashcard);
            db.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            AnsiConsole.Markup($"Failed to delete Flashcard: {ex.Message}");
            return false;
        }
    }

    internal Flashcard? GetFlashcardById(int id)
    {
        using var db = new DatabaseContext();
        try
        {
            return db.Flashcards.FirstOrDefault(f => f.Id == id);
        }
        catch (Exception ex)
        {
            AnsiConsole.Markup($"Failed to retrieve Flashcard: {ex.Message}");
            return null;
        }
    }

    internal List<Flashcard> GetAllFlashcards()
    {
        using var db = new DatabaseContext();
        try
        {
            return db.Flashcards.ToList();
        }
        catch (Exception ex)
        {
            AnsiConsole.Markup($"Failed to retrieve Flashcards: {ex.Message}");
            return new List<Flashcard>();
        }
    }
}

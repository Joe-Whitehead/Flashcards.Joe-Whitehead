using Flashcards.Models;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;

namespace Flashcards.Data;

public class StackRepository
{
    public List<Stack> GetAllStacks()
    {
        using var db = new DatabaseContext();
        try
        {
            return db.Stacks
                .Include(s => s.Flashcards)
                .ToList();
        }
        catch (Exception ex)
        {
            throw new Exception($"Unexpected error retrieving stacks: {ex.Message}", ex);
        }
    }

    public List<string> GetAllStackNames()
    {
        using var db = new DatabaseContext();
        try
        {
            return db.Stacks
                .Select(s => s.Name)
                .ToList();
        }
        catch (Exception ex)
        {
            throw new Exception($"Unexpected error retrieving stack names: {ex.Message}", ex);
        }
    }

    public Stack GetStack(int id)
    {
        using var db = new DatabaseContext();
        try
        {
            return db.Stacks
                .Include(s => s.Flashcards)
                .Single(s => s.Id == id);
                
        }
        catch (InvalidOperationException)
        {
            throw new InvalidOperationException($"No stack exists with ID: {id}.");
        }
        catch (Exception ex)
        {
            throw new Exception($"Unexpected error retrieving stack: {ex.Message}", ex);
        }        
    }

    public bool AddStack(Stack stack)
    {
        using var db = new DatabaseContext();
        try
        {
            db.Stacks.Add(stack);
            db.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            AnsiConsole.Markup($"Failed to add Stack: {ex.Message}");
            return false;
        }            
    }

    public bool UpdateStack(Stack stack)
    {
        using var db = new DatabaseContext();
        try
        {
            db.Stacks.Update(stack);
            db.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            AnsiConsole.Markup($"Failed to update Stack: {ex.Message}");
            return false;
        }
    }

    public bool DeleteStack(Stack stack)
    {
        using var db = new DatabaseContext();
        try
        {
            db.Stacks.Remove(stack);
            db.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            AnsiConsole.Markup($"Failed to delete Stack: {ex.Message}");
            return false;
        }
    }
}

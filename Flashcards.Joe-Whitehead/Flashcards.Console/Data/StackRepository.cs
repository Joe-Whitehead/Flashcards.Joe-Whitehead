using Flashcards.Models;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;

namespace Flashcards.Data;

public class StackRepository
{
    public async Task<List<Stack>> GetAllStacksAsync()
    {
        using var db = new DatabaseContext();
        try
        {
            return await db.Stacks
                .Include(s => s.Flashcards)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Unexpected error retrieving stacks: {ex.Message}", ex);
        }
    }

    public async Task<List<string>> GetAllStackNamesAsync()
    {
        using var db = new DatabaseContext();
        try
        {
            return await db.Stacks
                .Select(s => s.Name)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Unexpected error retrieving stack names: {ex.Message}", ex);
        }
    }

    public async Task<Stack> GetStackAsync(int id)
    {
        using var db = new DatabaseContext();
        try
        {
            return await db.Stacks
                .Include(s => s.Flashcards)
                .SingleAsync(s => s.Id == id);
                
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

    public async Task<bool> AddStackAsync(Stack stack)
    {
        using var db = new DatabaseContext();
        try
        {
            db.Stacks.Add(stack);
            await db.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            AnsiConsole.Markup($"Failed to add Stack: {ex.Message}");
            return false;
        }            
    }

    public async Task<bool> UpdateStackAsync(Stack stack)
    {
        using var db = new DatabaseContext();
        try
        {
            db.Stacks.Update(stack);
            await db.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            AnsiConsole.Markup($"Failed to update Stack: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteStackAsync(Stack stack)
    {
        using var db = new DatabaseContext();
        try
        {
            db.Stacks.Remove(stack);
            await db.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            AnsiConsole.Markup($"Failed to delete Stack: {ex.Message}");
            return false;
        }
    }
}

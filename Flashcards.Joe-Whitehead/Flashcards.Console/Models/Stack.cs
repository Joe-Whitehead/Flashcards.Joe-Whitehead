using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Flashcards.DTOs;

namespace Flashcards.Models;

public class Stack
{
    public int Id { get; init; }        
    public string Name { get; set; }
    public DateTime CreatedAt { get; init; }
    public List<Flashcard> Flashcards { get; set; }

    public bool AddFlashcard(Flashcard card)
    {
        ArgumentNullException.ThrowIfNull(card);
        if (Flashcards.Any(c => c.Id == card.Id))
        {
            return false; // Flashcard with the same ID already exists
        }
        Flashcards.Add(card);
        return true;
    }

    public bool RemoveFlashcard(int flashcardId)
    {
        var card = Flashcards.FirstOrDefault(c => c.Id == flashcardId);
        if (card == null)
        {
            return false;
        }
        return Flashcards.Remove(card);            
    }

    public StackDTO ToDto()
    {
        return new StackDTO
        {
            Name = this.Name,
            Flashcards = this.Flashcards.Select((f, j) => new FlashcardDTO
            {
                Question = f.Question,
                Answer = f.Answer
            }).ToList()
        };
    }

}

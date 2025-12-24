using Flashcards.Models;
namespace Flashcards.DTOs;

public class FlashcardDTO
{
    public string Question { get; set; }
    public string Answer { get; set; }


    public static FlashcardDTO ToDto(Flashcard flashcard)
    {
        return new FlashcardDTO
        {
            Question = flashcard.Question,
            Answer = flashcard.Answer
        };
    }

    public static string ToDisplayString(Flashcard flashcard)
    {
        return $"Q: {flashcard.Question} | A: {flashcard.Answer}";
    }
}

        
        

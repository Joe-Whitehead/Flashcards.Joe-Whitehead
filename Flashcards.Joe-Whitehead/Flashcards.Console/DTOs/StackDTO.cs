using Flashcards.Models;
namespace Flashcards.DTOs;

public class StackDTO
{
    public string Name { get; set; }
    public List<FlashcardDTO> Flashcards { get; set; }


   /* public static StackDTO ToDto(Stack stack)
    {
        return new StackDTO
        {
            Name = stack.Name,
            Flashcards = stack.Flashcards.Select((f, j) => new FlashcardDTO
            {
                Question = f.Question,
                Answer = f.Answer
            }).ToList()
        };
    }*/

    public static string ToDisplayString(Stack stack)
    {
        return $"{stack.Name} (Cards: {stack.Flashcards?.Count ?? 0})";
    }
}

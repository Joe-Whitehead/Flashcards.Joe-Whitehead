using Flashcards.Models;
using Flashcards.Data;
using Flashcards.DTOs;

namespace Flashcards.Controllers;

public class StackController
{
    private readonly StackRepository _stackRepository;
    private readonly FlashcardRepository _flashcardRepository;

    public StackController()
    {
        _stackRepository = new StackRepository();
        _flashcardRepository = new FlashcardRepository();
    }

    public List<StackDTO> GetAllStackDtos()
    {
        return GetAllStacks()
        .OrderBy(s => s.Id)
        .Select(s => s.ToDto())
        .ToList();
    }

    public List<Stack> GetAllStacks()
    {
        return _stackRepository.GetAllStacks()
        .OrderBy(s => s.Id)
        .ToList();
    }

    public List<string> GetAllStackNames()
    {
        return _stackRepository.GetAllStackNames();
    }

    public Stack GetStackById(int id)
    {
        return _stackRepository.GetStack(id);
    }

    public bool AddNewStack(StackDTO stack)
    {
        var newStack = new Stack
        {
            Name = stack.Name,
            CreatedAt = DateTime.Now,
            Flashcards = MapFlashcards(stack.Flashcards)
        };
        List<Stack> existingStacks = _stackRepository.GetAllStacks();

        return _stackRepository.AddStack(newStack);           
    }

    private List<Flashcard> MapFlashcards(List<FlashcardDTO> flashcardDtos)
    {
        var flashcards = flashcardDtos.Select(dto => new Flashcard            
        {
            Question = dto.Question,
            Answer = dto.Answer,
            CreatedAt = DateTime.Now,
            LastUpdated = DateTime.Now             
        }).ToList();
        return flashcards;
    }

    public bool EditStack(Stack stack)
    {
        var existingStack = _stackRepository.GetStack(stack.Id);
        if (existingStack == null)
        {
            return false;
        }
        existingStack.Name = stack.Name;            
        return _stackRepository.UpdateStack(existingStack);
    }

    public bool AddFlashcardToStack(int stackid, FlashcardDTO flashcardDto)
    {
        var stack = _stackRepository.GetStack(stackid);
        if (stack == null)
        {
            return false;
        }
        return _flashcardRepository.InsertFlashcard(new Flashcard
        {
            Question = flashcardDto.Question,
            Answer = flashcardDto.Answer,
            StackId = stack.Id,
            CreatedAt = DateTime.Now,
            LastUpdated = DateTime.Now
        });           
    }

    public bool DeleteStack(int id)
    {
        var existingStack = _stackRepository.GetStack(id);
        if (existingStack == null)
        {
            return false;
        }           
        return _stackRepository.DeleteStack(existingStack);
    }
}


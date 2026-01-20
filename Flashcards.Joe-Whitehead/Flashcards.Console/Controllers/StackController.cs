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

    public async Task<List<StackDTO>> GetAllStackDtosAsync()
    {
        var allStackDtos = await _stackRepository.GetAllStacksAsync();

        return allStackDtos
        .OrderBy(s => s.Id)
        .Select(s => s.ToDto())
        .ToList();
    }

    public async Task<List<Stack>> GetAllStacksAsync()
    {
        var allStacks = await _stackRepository.GetAllStacksAsync();

        return allStacks
        .OrderBy(s => s.Id)
        .ToList();
    }

    public async Task<List<string>> GetAllStackNamesAsync()
    {
        return await _stackRepository.GetAllStackNamesAsync();
    }

    public async Task<Stack> GetStackByIdAsync(int id)
    {
        return await _stackRepository.GetStackAsync(id);
    }

    public async Task<bool> AddNewStackAsync(StackDTO stack)
    {
        var newStack = new Stack
        {
            Name = stack.Name,
            CreatedAt = DateTime.Now,
            Flashcards = MapFlashcards(stack.Flashcards)
        };

        return await _stackRepository.AddStackAsync(newStack);           
    }

    private List<Flashcard> MapFlashcards(List<FlashcardDTO> flashcardDtos)
    {
       return flashcardDtos
            .Select(dto => new Flashcard            
            {
                Question = dto.Question,
                Answer = dto.Answer,
                CreatedAt = DateTime.Now,
                LastUpdated = DateTime.Now             
            })
            .ToList();
    }

    public async Task<bool> EditStackAsync(Stack stack)
    {
        var existingStack = await _stackRepository.GetStackAsync(stack.Id);
        if (existingStack == null)
        {
            return false;
        }
        existingStack.Name = stack.Name;            
        return await _stackRepository.UpdateStackAsync(existingStack);
    }

    public async Task<bool> AddFlashcardToStackAsync(int stackid, FlashcardDTO flashcardDto)
    {
        var stack = await _stackRepository.GetStackAsync(stackid);
        if (stack == null)
        {
            return false;
        }
        return await _flashcardRepository.InsertFlashcardAsync(new Flashcard
        {
            Question = flashcardDto.Question,
            Answer = flashcardDto.Answer,
            StackId = stack.Id,
            CreatedAt = DateTime.Now,
            LastUpdated = DateTime.Now
        });           
    }

    public async Task<bool> DeleteStackAsync(int id)
    {
        var existingStack = await _stackRepository.GetStackAsync(id);
        if (existingStack == null)
        {
            return false;
        }           
        return await _stackRepository.DeleteStackAsync(existingStack);
    }
}


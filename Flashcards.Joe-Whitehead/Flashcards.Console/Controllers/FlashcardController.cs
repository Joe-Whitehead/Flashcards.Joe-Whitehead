using Flashcards.Data;
using Flashcards.DTOs;

namespace Flashcards.Controllers;

public class FlashcardController
{
    private readonly FlashcardRepository _flashcardRepository;
    
    public FlashcardController() 
    { 
        _flashcardRepository = new FlashcardRepository();
    }      

    public async Task<bool> UpdateFlashcardAsync(int id, FlashcardDTO flashcardDto)
    {
        var existingFlashcard = await _flashcardRepository.GetFlashcardByIdAsync(id);
        if (existingFlashcard == null)
        {
            return false;
        }
        existingFlashcard.Question = flashcardDto.Question;
        existingFlashcard.Answer = flashcardDto.Answer;
        existingFlashcard.LastUpdated = DateTime.UtcNow;
        return await _flashcardRepository.UpdateFlashcardAsync(existingFlashcard);
    }

    public async Task<List<FlashcardDTO>> GetAllFlashcardsAsync()
    {
        var flashcards = await _flashcardRepository.GetAllFlashcardsAsync();               

        var flashcardDtos = flashcards
            .OrderBy(f => f.Id)
            .Select((f, i) => new FlashcardDTO
            {
                Question = f.Question,
                Answer = f.Answer
            })
            .ToList();

        return flashcardDtos;

    }

    public async Task<bool> DeleteFlashcardAsync(int id)
    {
        var existingFlashcard = await _flashcardRepository.GetFlashcardByIdAsync(id);
        if (existingFlashcard == null)
        {
            return false;
        }
        return await _flashcardRepository.DeleteFlashcardAsync(existingFlashcard);
    }
}

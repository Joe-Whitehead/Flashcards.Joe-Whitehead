using Flashcards.Data;
using Flashcards.DTOs;
using Flashcards.Models;

namespace Flashcards.Controllers;

internal class StudyController
{
    private readonly StudyRepository _studyRepository;

    public StudyController()
    {
        _studyRepository = new StudyRepository();
    }

    public async Task<StudySession> CreateNewSessionAsync()
    {
        var session = new StudySession
        {
            StartedAt = DateTime.Now
        };
        await _studyRepository.AddSessionAsync(session);
        return session;
    }
}

namespace Flashcards.Models;

public class StudySession
{
    public int Id { get; init; }
    public DateTime StartedAt { get; set; } 
    public DateTime? EndedAt { get; set; }
    public List<int> ReviewedFlashcardIds { get; set; }
    public int TotalFlashcards { get; set; }
    public TimeSpan Duration { get; set; }

    public bool StartSession()
    {
        if (StartedAt != default)            
            return false; // Session already started
        
        StartedAt = DateTime.Now;
        return true;
    }

    public bool ReviewFlashcard(int flashcardId)
    {
        if (ReviewedFlashcardIds.Contains(flashcardId))            
            return false; // Flashcard already reviewed in this session
        
        ReviewedFlashcardIds.Add(flashcardId);
        return true;
    }

    public int GetReviewedCount() => ReviewedFlashcardIds.Count;        

    public bool EndSession()
    {
        if (StartedAt == default)            
            return false; // Session not started yet
        
        if (EndedAt != null)            
            return false; // Session already ended
        
        EndedAt = DateTime.Now;
        Duration = EndedAt.Value - StartedAt;
        TotalFlashcards = GetReviewedCount();
        return true;
    }
}

namespace Flashcards.DTOs;

internal class StudyDTO
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public TimeSpan Duration { get; init; }
    public int FlashcardsStudied { get; set; }
    public int StacksStudied { get; set; }        
}

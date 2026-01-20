
namespace Flashcards.Models;

    internal record StudyCard(
        int FlashcardId,
        string Question,
        string Answer,
        int StackId,
        string StackName
        );



using System.ComponentModel.DataAnnotations;

namespace Flashcards.Enums;

internal enum MainMenu
{
    [Display(Name = "Study Flashcards")]
    StudyFlashcards = 1,
    [Display(Name = "Manage Stacks & Flashcards")]
    ManageStacksCards = 2,
    [Display(Name = "View Statistics")]
    ViewStatistics = 3,
    [Display(Name = "Settings")]
    Settings = 4,
    [Display(Name = "Exit Application")]
    ExitApplication = 5
}

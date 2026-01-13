using System.ComponentModel.DataAnnotations;

namespace Flashcards.Enums;

internal enum StudyMenu
{
    [Display(Name ="Study All Stacks (Randomised)")]
    StudyAllStacks = 1,
    [Display(Name = "Study Single Stack")]
    StudySingleStack = 2,
    [Display(Name = "Study Stack Selection")]
    StudyStackSelection = 3,
    [Display(Name = "Back to Main Menu")]
    BackToMainMenu = 4
}

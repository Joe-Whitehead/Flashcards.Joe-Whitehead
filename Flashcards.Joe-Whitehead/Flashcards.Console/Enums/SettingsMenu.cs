using System.ComponentModel.DataAnnotations;
namespace Flashcards.Enums;

internal enum SettingsMenu
{
    [Display(Name = "Insert Test Data")]
    SeedDatabase = 1,
    [Display(Name = "Clear All Data")]
    ClearDatabase,
    [Display(Name = "Back to Main Menu")]
    BackToMainMenu
}

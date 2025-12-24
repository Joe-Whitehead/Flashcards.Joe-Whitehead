using System.ComponentModel.DataAnnotations;

namespace Flashcards.Enums;

internal enum StackMenu
{
    [Display(Name = "Create New Stack")]
    AddStack = 1,
    [Display(Name = "Edit Existing Stacks")]
    EditExistingStack,
    [Display(Name = "Delete Stack")]
    DeleteStack,
    [Display(Name = "View All Stacks")]
    ViewAllStacks,
    [Display(Name = "Return to Main Menu")]
    ReturnToMainMenu
}

internal enum EditStackMenu
{
    [Display(Name = "Rename Stack")]
    RenameStack = 1,
    [Display(Name = "Add Flashcard to Stack")]
    AddFlashcardToStack,
    [Display(Name = "Edit Flashcard in Stack")]
    EditFlashcardInStack,
    [Display(Name = "Delete Flashcard from Stack")]
    DeleteFlashcardFromStack,
    [Display(Name = "Return to Main Menu")]
    ReturnToMainMenu
}

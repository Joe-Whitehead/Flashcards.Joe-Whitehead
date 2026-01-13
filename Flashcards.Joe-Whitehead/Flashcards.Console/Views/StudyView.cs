using Flashcards.Controllers;
using Flashcards.DTOs;
using Flashcards.Enums;
using Flashcards.Helpers;
using Flashcards.Models;
using Spectre.Console;

namespace Flashcards.Views;

internal class StudyView
{
    private readonly StackController _stackController;
    private readonly FlashcardController _flashcardController;
    private readonly StudyController _studyController;

    public StudyView(StackController stackController, FlashcardController flashcardController, StudyController studyController)
    {
        _stackController = stackController;
        _flashcardController = flashcardController;
        _studyController = studyController;
    }

    public async Task RunAsync()
    {
        while (true)
        {
            Console.Clear();
            Display.SetPageTitle("Study");
            Display.ShowTitle("Study Flashcards");
            var menuItems = Display.GetMenuItems<StudyMenu>();
            var selection = Display.PromptMenuSelection(menuItems);
            switch (selection)
            {
                case StudyMenu.StudyAllStacks:
                    var allStacks = await _stackController.GetAllStacksAsync();
                    Display.PressToContinue("Studying ALl Stacks. Press any key to begin.");
                    
                    
                    break;
                case StudyMenu.StudySingleStack:
                    var stacks = await _stackController.GetAllStacksAsync();
                    var stackOptions = Display.GetModelItems(stacks);
                    var selectedStack = Display.PromptMenuSelection(stackOptions);
                    //StudyStack(selectedStack);
                    break;
                case StudyMenu.StudyStackSelection:
                   /* var selectedStacks = _stackController.PromptStackSelection();
                    foreach (var stack in selectedStacks)
                    {
                      / StudyStack(stack);
                    }*/
                    break;
                case StudyMenu.BackToMainMenu:
                    return;
                default:
                    Display.ErrorMessage("Invalid selection. Please try again.");
                    break;
            }
        }
    }

    private void StudySession(StackDTO stack, FlashcardDTO flashcard)
    {        
        Console.Clear();
        Display.ShowTitle("Study Session");
        AnsiConsole.MarkupLine($"[yellow]Q:[/] {flashcard.Question}");
        Display.PressToContinue("Press any key to view the answer...");
        AnsiConsole.MarkupLine($"[green]A:[/] {flashcard.Answer}");
        Display.PressToContinue("Press any key to continue...");
    }
}

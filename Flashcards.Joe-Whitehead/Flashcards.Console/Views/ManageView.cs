using Flashcards.Controllers;
using Flashcards.DTOs;
using Flashcards.Enums;
using Flashcards.Helpers;
using Flashcards.Models;
using Spectre.Console;

namespace Flashcards.Views;

internal class ManageView
{
    private readonly StackController _stackController;
    private readonly FlashcardController _flashcardController;

    public ManageView(StackController stackController, FlashcardController flashcardController)
    {
        _stackController = stackController;
        _flashcardController = flashcardController;
    }

    public async Task RunAsync()
    {
        while (true)
        {
            Console.Clear();
            Display.SetPageTitle("Manage");
            Display.ShowTitle("Manage Stacks & Flashcards");
            var menuItems = Display.GetMenuItems<StackMenu>();
            var selectedOption = Display.PromptMenuSelection(menuItems);

            switch (selectedOption)
            {
                case StackMenu.AddStack:
                    string stackName = Validation.UniqueName(await _stackController.GetAllStackNamesAsync());
                    List<FlashcardDTO> flashcards = [];

                    if (Display.YesNoPrompt("Would you like to add a flashcard to this stack now?"))
                    {
                        while (true)
                        {
                            flashcards.Add(Display.PromptFlashcardDetails());

                            if (!Display.YesNoPrompt("Would you like to add another flashcard?"))
                            {
                                break;
                            }
                        }
                    }
                    await _stackController.AddNewStackAsync(new StackDTO
                    {
                        Name = stackName,
                        Flashcards = flashcards
                    });
                    break;

                case StackMenu.EditExistingStack:
                    var selectedStack = await DisplayStackMenuAsync();
                    await GetStackSubMenuAsync(selectedStack);

                    break;

                case StackMenu.DeleteStack:
                    var stackToDelete = DisplayStackMenuAsync();
                    if (Display.YesNoPrompt("Are you sure you want to delete this stack? (This will also delete any Flashcards in the stack)"))
                    {
                        await _stackController.DeleteStackAsync(stackToDelete.Id);
                        Display.SuccessMessage("Stack deleted successfully");
                    }
                    else
                    {
                        Display.CancelledMessage("Deletion cancelled.");
                    }
                    Display.PressToContinue();
                    break;

                case StackMenu.ViewAllStacks:
                    Console.Clear();
                    List<StackDTO> stacks = await _stackController.GetAllStackDtosAsync();
                    int counter = 1;
                    foreach (StackDTO stack in stacks)
                    {
                        Display.FullStackView(stack);
                        Display.PressToContinue($"Next Stack [[Page {counter++} of {stacks.Count}]]");
                        AnsiConsole.Clear();
                    }
                    Display.ShowPanelMessage("End of Stacks", "You have reached the end of the stacks.", Color.Green);
                    Display.PressToContinue();
                    break;

                case StackMenu.ReturnToMainMenu:
                    return;
            }
        }
    }

    private async Task GetStackSubMenuAsync(Stack stack)
    {
        bool exitSubMenu = false;

        while (!exitSubMenu)
        {
            Dictionary<string, Flashcard> flashcards;
            Flashcard selectedFlashcard;
            Console.Clear();
            Display.SetPageTitle("Manage");
            Display.ShowTitle("Stack Management Menu");

            Display.StackOverview(stack.ToDto());

            var menuItems = Display.GetMenuItems<EditStackMenu>();
            var selectedOption = Display.PromptMenuSelection(menuItems);


            switch (selectedOption)
            {
                case EditStackMenu.RenameStack:
                    string prevName = stack.Name;
                    stack.Name = Validation.UniqueName(await _stackController.GetAllStackNamesAsync());
                    if (await _stackController.EditStackAsync(stack))
                    {
                        Console.Clear();
                        Display.SuccessMessage($"Stack renamed from {prevName} to {stack.Name}.");
                    }
                    Display.PressToContinue();
                    await RefreshStackAsync(stack);
                    break;

                case EditStackMenu.AddFlashcardToStack:
                    var flashcardDto = Display.PromptFlashcardDetails();
                    if (await _stackController.AddFlashcardToStackAsync(stack.Id, flashcardDto))
                    {
                        Display.SuccessMessage("Flashcard added successfully.");
                    }
                    Display.PressToContinue();
                    await RefreshStackAsync(stack);
                    break;

                case EditStackMenu.EditFlashcardInStack:
                    flashcards = Display.GetModelItems(stack.Flashcards);
                    selectedFlashcard = Display.PromptMenuSelection<Flashcard>(flashcards);
                    var updatedFlashcardDto = Display.PromptFlashcardDetails(selectedFlashcard.Question, selectedFlashcard.Answer);

                    if (await _flashcardController.UpdateFlashcardAsync(selectedFlashcard.Id, updatedFlashcardDto))
                    {
                        Display.SuccessMessage("Flashcard updated successfully.");
                    }
                    Display.PressToContinue();
                    await RefreshStackAsync(stack);
                    break;

                case EditStackMenu.DeleteFlashcardFromStack:
                    flashcards = Display.GetModelItems(stack.Flashcards);
                    selectedFlashcard = Display.PromptMenuSelection<Flashcard>(flashcards);

                    Display.WarningMessage("You are about to delete the following flashcard");
                    Display.SingleFlashcardView(selectedFlashcard);

                    if (Display.YesNoPrompt("Are you sure you want to delete this flashcard?"))
                    {
                        await _flashcardController.DeleteFlashcardAsync(selectedFlashcard.Id);
                        Display.SuccessMessage("Flashcard deleted successfully.");
                    }
                    else
                    {
                        Display.CancelledMessage("Deletion cancelled.");
                    }
                    Display.PressToContinue();
                    await RefreshStackAsync(stack);
                    break;

                case EditStackMenu.ReturnToMainMenu:
                    exitSubMenu = true;
                    break;
            }
        }
    }

    private async Task RefreshStackAsync(Stack stack)
    {
        var updatedStack = await _stackController.GetStackByIdAsync(stack.Id);

        // Mutate the original object
        stack.Flashcards = updatedStack.Flashcards;
    }

    private async Task<Stack> DisplayStackMenuAsync()
    {
        var stackList = await _stackController.GetAllStacksAsync();
        var stackMenuItems = Display.GetModelItems(stackList);
        return Display.PromptMenuSelection<Stack>(stackMenuItems);
    }
}
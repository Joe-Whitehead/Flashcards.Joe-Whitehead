using Flashcards.Controllers;
using Flashcards.DTOs;
using Flashcards.Enums;
using Flashcards.Helpers;
using Flashcards.Models;
using Spectre.Console;
using System.Diagnostics;

namespace Flashcards.Views
{
    internal class MainView
    {
        private readonly StackController _stackController;
        private readonly FlashcardController _flashcardController;

        public MainView()
        {
            _stackController = new StackController();
            _flashcardController = new FlashcardController();
        }

        public void Run()
        {
            while (true)
            {
                //Clear the Console and set the main titles
                Console.Clear();
                Display.SetPageTitle("Home");
                Display.ShowTitle("Main Menu");

                //Get our Main menu items and prompt user for selection
                var menuItems = Display.GetMenuItems<MainMenu>();
                var selectedOption = Display.PromptMenuSelection(menuItems);

                //Handle user selection
                switch (selectedOption)
                {
                    case MainMenu.StudyFlashcards:
                        break;

                    case MainMenu.ManageStacksCards:
                        ManageStacksAndCards();
                        break;

                    case MainMenu.ViewStatistics:
                        break;

                    case MainMenu.Settings:
                        SettingsView.Run();
                        break;

                    case MainMenu.ExitApplication:
                        Console.Clear();
                        AnsiConsole.MarkupLine("[green]Thank you for using Flashcards! Goodbye![/]");
                        Environment.Exit(0);
                        break;
                }
            }
        }

        private void ManageStacksAndCards()
        {
            Console.Clear();
            Display.SetPageTitle("Manage");
            Display.ShowTitle("Manage Stacks & Flashcards");
            var menuItems = Display.GetMenuItems<StackMenu>();
            var selectedOption = Display.PromptMenuSelection(menuItems);

            switch (selectedOption)
            {
                case StackMenu.AddStack:
                    string stackName = Validation.UniqueName(_stackController.GetAllStackNames());
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
                    _stackController.AddNewStack(new StackDTO
                    {
                        Name = stackName,
                        Flashcards = flashcards
                    });
                    break;

                case StackMenu.EditExistingStack:
                    GetStackSubMenu(DisplayStackMenu());
                    break;

                case StackMenu.DeleteStack:
                    var stackToDelete = DisplayStackMenu();
                    if (Display.YesNoPrompt("Are you sure you want to delete this stack? (This will also delete any Flashcards in the stack)"))
                    {
                        _stackController.DeleteStack(stackToDelete.Id);
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
                    List<StackDTO> stacks = _stackController.GetAllStackDtos();
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

        private void GetStackSubMenu(Stack stack)
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
                        stack.Name = Validation.UniqueName(_stackController.GetAllStackNames());
                        if (_stackController.EditStack(stack))
                        {
                            Console.Clear();
                            Display.SuccessMessage($"Stack renamed from {prevName} to {stack.Name}.");
                        }
                        Display.PressToContinue();
                        RefreshStack(stack);
                        break;

                    case EditStackMenu.AddFlashcardToStack:
                        var flashcardDto = Display.PromptFlashcardDetails();
                        if (_stackController.AddFlashcardToStack(stack.Id, flashcardDto))
                        {
                            Display.SuccessMessage("Flashcard added successfully.");
                        }
                        Display.PressToContinue();
                        RefreshStack(stack);
                        break;

                    case EditStackMenu.EditFlashcardInStack:
                        flashcards = Display.GetModelItems(stack.Flashcards);
                        selectedFlashcard = Display.PromptMenuSelection<Flashcard>(flashcards);
                        var updatedFlashcardDto = Display.PromptFlashcardDetails(selectedFlashcard.Question, selectedFlashcard.Answer);

                        if (_flashcardController.UpdateFlashcard(selectedFlashcard.Id, updatedFlashcardDto))
                        {
                            Display.SuccessMessage("Flashcard updated successfully.");
                        }
                        Display.PressToContinue();
                        RefreshStack(stack);
                        break;

                    case EditStackMenu.DeleteFlashcardFromStack:
                        flashcards = Display.GetModelItems(stack.Flashcards);
                        selectedFlashcard = Display.PromptMenuSelection<Flashcard>(flashcards);

                        Display.WarningMessage("You are about to delete the following flashcard");
                        Display.SingleFlashcardView(selectedFlashcard);

                        if (Display.YesNoPrompt("Are you sure you want to delete this flashcard?"))
                        {
                            _flashcardController.DeleteFlashcard(selectedFlashcard.Id);
                            Display.SuccessMessage("Flashcard deleted successfully.");
                        }
                        else
                        {
                            Display.CancelledMessage("Deletion cancelled.");
                        }
                        Display.PressToContinue();
                        RefreshStack(stack);
                        break;

                    case EditStackMenu.ReturnToMainMenu:
                        exitSubMenu = true;
                        break;
                }
            }
        }

        private void RefreshStack(Stack stack)
        {
            var updatedStack = _stackController.GetStackById(stack.Id);

            // Mutate the original object
            stack.Flashcards = updatedStack.Flashcards;
        }

        private Stack DisplayStackMenu()
        {
            var stackList = _stackController.GetAllStacks();
            var stackMenuItems = Display.GetModelItems(stackList);
            return Display.PromptMenuSelection<Stack>(stackMenuItems);
        }
    }
}

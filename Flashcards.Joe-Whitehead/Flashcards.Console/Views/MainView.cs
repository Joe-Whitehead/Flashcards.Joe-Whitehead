using Flashcards.Controllers;
using Flashcards.Enums;
using Flashcards.Helpers;
using Spectre.Console;

namespace Flashcards.Views;

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
                    var manageView = new ManageView(_stackController, _flashcardController);
                    manageView.Run();
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
}
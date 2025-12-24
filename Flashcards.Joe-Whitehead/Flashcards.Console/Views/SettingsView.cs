using Spectre.Console;
using Flashcards.Enums;
using Flashcards.Helpers;
using Flashcards.Controllers;

namespace Flashcards.Views;

internal static class SettingsView
{
    public static void Run()
    {
        while (true)
        {
            //Clear the Console and set the main titles
            Console.Clear();
            Display.SetPageTitle("Settings");
            Display.ShowTitle("Settings Menu");

            //Get our Settings menu items and prompt user for selection
            var menuItems = Display.GetMenuItems<SettingsMenu>();
            var selectedOption = Display.PromptMenuSelection(menuItems);

            //Handle user selection
            switch (selectedOption)
            {
                case SettingsMenu.SeedDatabase:
                    if (SettingsController.IsDatabaseSeeded())
                    {
                        AnsiConsole.MarkupLine("[yellow]Database already seeded. Would you like to clear and re-seed?[/]");
                        var confirm = Display.YesNoPrompt("This will erase all existing data. Are you sure?");
                        if (!confirm)
                        {
                            AnsiConsole.MarkupLine("[cyan]Seeding cancelled.[/]");
                            Display.PressToContinue();
                            break;
                        }
                        var clear = SettingsController.ClearDatabase();
                        if (clear)
                        {
                            AnsiConsole.MarkupLine("[green]Database cleared successfully![/]");
                            var reseed = SettingsController.SeedDatabase();
                            if (reseed)
                            {
                                AnsiConsole.MarkupLine("[green]Database seeded successfully![/]");
                            }
                        }
                        else
                        {
                            AnsiConsole.MarkupLine("[red]Failed to clear database. Seeding aborted.[/]");

                        }
                        Display.PressToContinue();
                        break;
                    }
                    var seeded = SettingsController.SeedDatabase();
                    if (seeded)
                    {
                        AnsiConsole.MarkupLine("[green]Database seeded successfully![/]");
                    }

                    Display.PressToContinue();
                    break;

                case SettingsMenu.ClearDatabase:
                    var cleared = SettingsController.ClearDatabase();
                    if (cleared)
                    {
                        AnsiConsole.MarkupLine("[green]Database cleared successfully![/]");
                    }
                    Display.PressToContinue();
                    break;

                case SettingsMenu.BackToMainMenu:
                    return;
            }
        }
    }
}

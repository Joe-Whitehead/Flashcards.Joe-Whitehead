using Flashcards.DTOs;
using Flashcards.Models;
using Spectre.Console;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Flashcards.Helpers;

internal static class Display
{
    public static Dictionary<string, TEnum> GetMenuItems<TEnum>()
        where TEnum : struct, Enum
    {
        return Enum.GetValues<TEnum>()
            .ToDictionary(option => GetEnumDisplayName(option), option => option);
    }

    private static string GetEnumDisplayName(Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attribute = field?.GetCustomAttributes(typeof(DisplayAttribute), false)
            .FirstOrDefault() as DisplayAttribute;

        return attribute?.Name ?? value.ToString();
    }

    public static T PromptMenuSelection<T>(Dictionary<string, T> options)
    {
        string typeName = typeof(T).IsEnum ? "item" : typeof(T).Name;
        var selectedItem = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title($"Please select {typeName} from the list:")
                .PageSize(10)
                .HighlightStyle(new Style(Color.Cyan1))
                .AddChoices(options.Keys)
        );
        return options[selectedItem];
    }

    public static Dictionary<string, Stack> GetModelItems(List<Stack> stacks)
    {
        return stacks.ToDictionary(
            stack => StackDTO.ToDisplayString(stack),
            stack => stack
        );
    }

    public static Dictionary<string, Flashcard> GetModelItems(List<Flashcard> flashcards)
    {
        return flashcards.ToDictionary(
            card => FlashcardDTO.ToDisplayString(card),
            card => card
        );
    }

    public static bool YesNoPrompt(string message)
    {
        return AnsiConsole.Confirm(message);
    }

    public static void SetPageTitle(string title)
    {
        Console.Title = $"Flashcards - {title}";
    }

    public static void ShowTitle(string title)
    {
        AnsiConsole.Clear();
        AnsiConsole.MarkupLine($"[underline green]{title}[/]");
    }

    public static void PressToContinue(string message = "Press any key to continue...")
    {
        AnsiConsole.Markup($"[grey]{message}[/]");           
        Console.ReadKey(true);
    }

    public static string PromptInput(string message)
    {
        return AnsiConsole.Ask<string>(message);
    }

    public static FlashcardDTO PromptFlashcardDetails(string? existingQuestion = null, string? existingAnswer = null)
    {
        if (existingQuestion != null && existingAnswer != null)
        {
            AnsiConsole.MarkupLine($"Current Question: [blue]{existingQuestion}[/]");
            AnsiConsole.MarkupLine($"Current Answer: [blue]{existingAnswer}[/]");
        }
        string question = Display.PromptInput($"Enter the question for the flashcard: ");
        string answer = Display.PromptInput("Enter the answer for the flashcard: ");
        return new FlashcardDTO
        {
            Question = question,
            Answer = answer
        };
    }

    public static void ShowPanelMessage(string title, string message, Color colour)
    {
        var panel = new Panel(new Markup($"[white]{message}[/]").Centered())
        {
            Header = new PanelHeader(title, Justify.Center),
            Border = BoxBorder.Rounded,
            BorderStyle = new Style(colour),
            Padding = new Padding(1, 1)
        };
        AnsiConsole.Write(panel);
    }

    public static void SuccessMessage(string message)
    {
        Console.Clear();
        ShowPanelMessage("Success", message, Color.Green);
    }

    public static void CancelledMessage(string message)
    {
        Console.Clear();
        ShowPanelMessage("Cancelled", message, Color.Yellow);
    }

    public static void ErrorMessage(string message)
    {
        Console.Clear();
        ShowPanelMessage("Error", message, Color.Red);
    }

    public static void WarningMessage(string message)
    {
        Console.Clear();
        ShowPanelMessage("Warning", message, Color.Orange1);
    }

    public static void SingleFlashcardView(Flashcard flashcard)
    {
        var table = new Spectre.Console.Table()             
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Red)
            .AddColumn(new TableColumn("[yellow bold u]Question[/]").Centered())
            .AddColumn(new TableColumn("[yellow bold u]Answer[/]").Centered())
            .AddRow(flashcard.Question, flashcard.Answer);
            
        AnsiConsole.Write(table);
    }

    public static void StackOverview(StackDTO stack)
    {
        int counter = 1;
        var flashcardText = new StringBuilder();
        foreach (var card in stack.Flashcards)
        {
            flashcardText.AppendLine($"{counter++}. Q: [blue]{card.Question}[/] | A: [green]{card.Answer}[/]");
            if (counter < stack.Flashcards.Count + 1)
            {
                flashcardText.AppendLine();
            }
        }
        var grid = new Grid()
            .AddColumn()
            .AddColumn();
        grid.AddRow(
            new Panel(
                new Markup(flashcardText.ToString()))
                .Header("Flashcards", Justify.Center)
                .BorderColor(Color.Purple),

            new Panel(
                new Markup($"[bold yellow]Stack Name:[/] [white]{stack.Name}[/]\n[bold yellow]Number of Flashcards:[/] [white]{stack.Flashcards.Count}[/]"))
                .Header("Stack Information", Justify.Center)
                .BorderColor(Color.Green)    
                .Padding(8, 0)
        );
        AnsiConsole.Write(grid);
    }

    public static void FullStackView(StackDTO stack)
    {
        var infoPanel = new Panel
            (
                Align.Center(
                    new Markup(
                        $"[bold yellow]Stack Name:[/] [white]{stack.Name}[/]\n[bold yellow]Number of Flashcards:[/] [white]{stack.Flashcards.Count}[/]"                        
                    )
                )
            )
            .Header("Stack Information", Justify.Center)
            .BorderColor(Color.Green)    
            .Padding(8, 0)
            .Expand();

        var flashcardGrid = new Grid()
            .AddColumn()
            .AddColumn();
        for (int i = 0; i < stack.Flashcards.Count; i += 2)
        {
            var panel1 = CreateFlashcardPanel(stack.Flashcards[i]);
            Panel? panel2 = null;

            if (i + 1 < stack.Flashcards.Count)
                panel2 = CreateFlashcardPanel(stack.Flashcards[i + 1]);

            flashcardGrid.AddRow(panel1, panel2 ?? new Panel("")
            {
                Border = BoxBorder.None,
                Padding = new Padding(0)
            });
        }

        AnsiConsole.Write(infoPanel);
        AnsiConsole.Write(flashcardGrid);
    }

    static Panel CreateFlashcardPanel(FlashcardDTO card)
    {
        var content = new Markup(
            $"[bold blue]Q:[/] {card.Question}\n[bold green]A:[/] {card.Answer}"
        );
        return new Panel(content)
            .Header("Flashcard", Justify.Center)
            .Border(BoxBorder.Rounded)
            .Padding (5, 0)
            .Expand();        
    }
}

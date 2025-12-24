namespace Flashcards.Helpers;

internal static class Validation
{
    public static bool StackExists(string stackName, List<string> existingStacks) => existingStacks.Any(stack =>
            string.Equals(stack, stackName, StringComparison.OrdinalIgnoreCase));

    public static string UniqueName(List<string> stackNames)
    {
        string stackName = Display.PromptInput("Enter the new name for the stack: ");
        while (StackExists(stackName, stackNames))
        {
            Display.ErrorMessage($"A stack with name '{stackName}' already exists.");
            stackName = Display.PromptInput("Please enter a unique name for the new stack: ");
        }
        return stackName;
    }    
    
    public static bool isValidTextInput(string input) => !string.IsNullOrWhiteSpace(input);
}

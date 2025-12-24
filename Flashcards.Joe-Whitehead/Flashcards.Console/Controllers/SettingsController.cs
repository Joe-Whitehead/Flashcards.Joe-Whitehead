using Flashcards.Data;

namespace Flashcards.Controllers;

internal static class SettingsController
{
    public static bool SeedDatabase()
    {
        return SettingsRepository.SeedDatabase();
    }
    
    public static bool ClearDatabase()
    {
        return SettingsRepository.ClearDatabase();
    }

    public static bool IsDatabaseSeeded()
    {
        return SettingsRepository.IsDatabaseSeeded();
    }
}

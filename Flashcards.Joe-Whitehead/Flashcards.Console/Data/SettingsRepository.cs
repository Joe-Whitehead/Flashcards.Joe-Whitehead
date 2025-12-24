using Flashcards.Models;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;

namespace Flashcards.Data;

internal static class SettingsRepository
{
    public static bool SeedDatabase()
    {
        using var db = new DatabaseContext();
        return SeedDatabase(db);
    }

    public static bool SeedDatabase(DatabaseContext db)
    {
        try
        {
            if (!IsDatabaseSeeded(db))
            {
                TestData(db);
                return true;
            }
            else
            {
                throw new InvalidOperationException("Clear Database before re-seeding");
            }
        }
        catch (InvalidOperationException ex)
        {
            AnsiConsole.MarkupLine($"[bold red]{ex.Message}[/]");
            return false;
        }            
    }

    public static bool ClearDatabase()
    {
        using (var db = new DatabaseContext())
        {
            try
            {
                db.Flashcards.RemoveRange(db.Flashcards);
                db.Stacks.RemoveRange(db.Stacks);
                db.SaveChanges();

                //Reset the PK Index
                db.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Flashcards', RESEED, 0)");
                db.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Stacks', RESEED, 0)");
                return true;
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[bold red]error clearing database:[/] [orange]{ex.Message}[/]");                    
                return false;
            }
        }
    }

    public static bool IsDatabaseSeeded()
    {
        using var db = new DatabaseContext();
        return IsDatabaseSeeded(db);
    }
    public static bool IsDatabaseSeeded(DatabaseContext db) =>  db.Stacks.Any() || db.Flashcards.Any();                         

    private static void TestData(DatabaseContext db)
    {
        SeedStacks(db);
        db.SaveChanges(); // Save stacks and flashcards first to generate IDs

        SeedStudySessions(db,5);
        db.SaveChanges(); // Save study sessions
        /*
         * I know calling SaveChanges multiple times is not optimal, but for seeding test data 
         * it won't cause much of an issue and it enables generating StudySessions with valid Flashcard IDs.
         */
    }

    private static void SeedStacks(DatabaseContext db)
    {
            var spanishStack = new Stack
            {
                Name = "Spanish Basics",
                CreatedAt = DateTime.UtcNow,
                Flashcards = new List<Flashcard>
                {
                    new Flashcard { Question = "Hello", Answer = "Hola", CreatedAt = DateTime.UtcNow, LastUpdated = DateTime.UtcNow },
                    new Flashcard { Question = "Thank you", Answer = "Gracias", CreatedAt = DateTime.UtcNow, LastUpdated = DateTime.UtcNow },
                    new Flashcard { Question = "Please", Answer = "Por favor", CreatedAt = DateTime.UtcNow, LastUpdated = DateTime.UtcNow },
                    new Flashcard { Question = "Goodbye", Answer = "Adiós", CreatedAt = DateTime.UtcNow, LastUpdated = DateTime.UtcNow },
                    new Flashcard { Question = "Yes", Answer = "Sí", CreatedAt = DateTime.UtcNow, LastUpdated = DateTime.UtcNow }
                }
            };
            var mathStack = new Stack
            {
                Name = "Basic Math",
                CreatedAt = DateTime.UtcNow,
                Flashcards = new List<Flashcard>
                {
                    new Flashcard { Question = "2 + 2", Answer = "4", CreatedAt = DateTime.UtcNow, LastUpdated = DateTime.UtcNow },
                    new Flashcard { Question = "5 * 3", Answer = "15", CreatedAt = DateTime.UtcNow, LastUpdated = DateTime.UtcNow },
                    new Flashcard { Question = "10 / 2", Answer = "5", CreatedAt = DateTime.UtcNow, LastUpdated = DateTime.UtcNow },
                    new Flashcard { Question = "7 - 4", Answer = "3", CreatedAt = DateTime.UtcNow, LastUpdated = DateTime.UtcNow},
                    new Flashcard { Question = "9 + 6", Answer = "15", CreatedAt = DateTime.UtcNow, LastUpdated = DateTime.UtcNow}
                }
            };
            var generalStack = new Stack
            {
                Name = "General Knowledge",
                CreatedAt = DateTime.UtcNow,
                Flashcards = new List<Flashcard>
                {
                    new Flashcard { Question = "What is the capital of France?", Answer = "Paris", CreatedAt = DateTime.UtcNow, LastUpdated = DateTime.UtcNow },
                    new Flashcard { Question = "Who wrote 'Hamlet'?", Answer = "William Shakespeare", CreatedAt = DateTime.UtcNow, LastUpdated = DateTime.UtcNow },
                    new Flashcard { Question = "What is the largest planet in our solar system?", Answer = "Jupiter", CreatedAt = DateTime.UtcNow, LastUpdated = DateTime.UtcNow },
                    new Flashcard { Question = "What is the boiling point of water?", Answer = "100°C (212°F)", CreatedAt = DateTime.UtcNow, LastUpdated = DateTime.UtcNow },
                    new Flashcard { Question = "Who painted the Mona Lisa?", Answer = "Leonardo da Vinci", CreatedAt = DateTime.UtcNow, LastUpdated = DateTime.UtcNow }
                }
            };
            var englishStack = new Stack
            {
                Name = "English Vocabulary",
                CreatedAt = DateTime.UtcNow,
                Flashcards = new List<Flashcard>
                {
                    new Flashcard { Question = "What is a synonym for 'happy'?", Answer = "Joyful", CreatedAt = DateTime.UtcNow, LastUpdated = DateTime.UtcNow },
                    new Flashcard { Question = "What does 'benevolent' mean?", Answer = "Kind and generous", CreatedAt = DateTime.UtcNow, LastUpdated = DateTime.UtcNow },
                    new Flashcard { Question = "What is the antonym of 'difficult'?", Answer = "Easy", CreatedAt = DateTime.UtcNow, LastUpdated = DateTime.UtcNow },
                    new Flashcard { Question = "Define 'ephemeral'.", Answer = "Lasting for a very short time", CreatedAt = DateTime.UtcNow, LastUpdated = DateTime.UtcNow },
                    new Flashcard { Question = "What is a homophone for 'sea'?", Answer = "See", CreatedAt = DateTime.UtcNow, LastUpdated = DateTime.UtcNow }
                }
            };
            var historyStack = new Stack
            {
                Name = "World History",
                CreatedAt = DateTime.UtcNow,
                Flashcards = new List<Flashcard>
                {
                    new Flashcard { Question = "Who was the first President of the United States?", Answer = "George Washington", CreatedAt = DateTime.UtcNow, LastUpdated = DateTime.UtcNow },
                    new Flashcard { Question = "In which year did World War II end?", Answer = "1945", CreatedAt = DateTime.UtcNow, LastUpdated = DateTime.UtcNow },
                    new Flashcard { Question = "What ancient civilization built the pyramids?", Answer = "The Egyptians", CreatedAt = DateTime.UtcNow, LastUpdated = DateTime.UtcNow },
                    new Flashcard { Question = "Who was known as the 'Maid of Orléans'?", Answer = "Joan of Arc", CreatedAt = DateTime.UtcNow, LastUpdated = DateTime.UtcNow },
                    new Flashcard { Question = "What was the name of the ship that brought the Pilgrims to America?", Answer = "The Mayflower", CreatedAt = DateTime.UtcNow, LastUpdated = DateTime.UtcNow }
                }
            };

            db.Stacks.AddRange(spanishStack, mathStack, generalStack, englishStack, historyStack);           
    }

    private static void SeedStudySessions(DatabaseContext db, int sessionCount)
    {
        var random = new Random();
        var allIds = db.Flashcards.Select(f => f.Id).ToList(); // Dynamically fetch all flashcard IDs

        var sessions = new List<StudySession>();

        for (int i = 0; i < sessionCount; i++)
        {
            var reviewedIds = allIds
                .OrderBy(_ => random.Next())
                .Take(random.Next(5, Math.Min(15, allIds.Count))) // Avoid exceeding available IDs
                .ToList();

            var start = DateTime.Now.AddDays(-i).AddHours(-random.Next(1, 5));
            var duration = TimeSpan.FromMinutes(random.Next(10, 45));
            var end = start + duration;

            sessions.Add(new StudySession
            {
                StartedAt = start,
                EndedAt = end,
                Duration = duration,
                ReviewedFlashcardIds = reviewedIds,
                TotalFlashcards = reviewedIds.Count
            });
        }
        db.StudySessions.AddRange(sessions);
    }
}

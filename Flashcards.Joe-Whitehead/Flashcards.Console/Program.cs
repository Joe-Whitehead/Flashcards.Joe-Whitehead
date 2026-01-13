using Flashcards.Data;
using Flashcards.Views;
using Microsoft.EntityFrameworkCore;

DbContext db = new DatabaseContext();
MainView app = new MainView();

using (db)
{
    await db.Database.MigrateAsync();    
}

await app.RunAsync();


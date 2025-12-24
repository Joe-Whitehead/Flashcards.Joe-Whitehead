using Flashcards.Data;
using Flashcards.Views;
using Microsoft.EntityFrameworkCore;

DbContext db = new DatabaseContext();
MainView app = new MainView();

using (db)
{
    db.Database.Migrate();    
}

app.Run();


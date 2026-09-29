using Data.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Migration
{
    public class DbMigrationService
    {
        private readonly string _programDataPath;

        public DbMigrationService()
        {
            _programDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Data Logger");
        }

        public async Task MigrateAsync(string oldDbPath, string newDbPath)
        {
            string key = Data.DataDependencies.RetrieveEncryptionKey();

            if (string.IsNullOrWhiteSpace(key))
                throw new InvalidOperationException("The SQLCipher key is missing.");

            /* CREATE ALPHA 6 DATABASE */

            var alpha6ConnectionString = new SqliteConnectionStringBuilder
            {
                DataSource = newDbPath,
                Mode = SqliteOpenMode.ReadWriteCreate,
                Password = key
            }.ToString();

            var alpha6Options = new DbContextOptionsBuilder<EntityMaster>().UseSqlite(alpha6ConnectionString).Options;

            await using var alpha6Db = new EntityMaster(alpha6Options);

            await alpha6Db.Database.MigrateAsync();

            /* OPEN ALPHA 5 DATABASE */

            var alpha5ConnectionString = new SqliteConnectionStringBuilder
            {
                DataSource = oldDbPath,
                Mode = SqliteOpenMode.ReadOnly
            }.ToString();

            var alpha5Options = new DbContextOptionsBuilder<EntityMaster>().UseSqlite(alpha5ConnectionString).Options;

            await using var alpha5Db = new EntityMaster(alpha5Options);

            /* DATA MIGRATION */

            var alpha5Accounts = await alpha5Db.Accounts.AsNoTracking().ToListAsync();
            var alpha6AccountIds = await alpha6Db.Accounts.Select(x => x.accountID).ToListAsync();
            alpha6Db.Accounts.AddRange(alpha5Accounts.Where(x => !alpha6AccountIds.Contains(x.accountID)));

            var alpha5Applications = await alpha5Db.Applications.AsNoTracking().ToListAsync();
            var alpha6ApplicationIds = await alpha6Db.Applications.Select(x => x.appID).ToListAsync();
            alpha6Db.Applications.AddRange(alpha5Applications.Where(x => !alpha6ApplicationIds.Contains(x.appID)));

            var alpha5Projects = await alpha5Db.Projects.AsNoTracking().ToListAsync();
            var alpha6ProjectIds = await alpha6Db.Projects.Select(x => x.projectID).ToListAsync();
            alpha6Db.Projects.AddRange(alpha5Projects.Where(x => !alpha6ProjectIds.Contains(x.projectID)));

            var alpha5Outputs = await alpha5Db.Outputs.AsNoTracking().ToListAsync();
            var alpha6OutputIds = await alpha6Db.Outputs.Select(x => x.outputID).ToListAsync();
            alpha6Db.Outputs.AddRange(alpha5Outputs.Where(x => !alpha6OutputIds.Contains(x.outputID)));

            var alpha5Types = await alpha5Db.Types.AsNoTracking().ToListAsync();
            var alpha6TypeIds = await alpha6Db.Types.Select(x => x.typeID).ToListAsync();
            alpha6Db.Types.AddRange(alpha5Types.Where(x => !alpha6TypeIds.Contains(x.typeID)));

            var alpha5PostIts = await alpha5Db.PostIts.AsNoTracking().ToListAsync();
            var alpha6PostItIds = await alpha6Db.PostIts.Select(x => x.postItID).ToListAsync();
            alpha6Db.PostIts.AddRange(alpha5PostIts.Where(x => !alpha6PostItIds.Contains(x.postItID)));

            var alpha5Subjects = await alpha5Db.Subjects.AsNoTracking().ToListAsync();
            var alpha6SubjectIds = await alpha6Db.Subjects.Select(x => x.subjectID).ToListAsync();
            alpha6Db.Subjects.AddRange(alpha5Subjects.Where(x => !alpha6SubjectIds.Contains(x.subjectID)));

            var alpha5Logs = await alpha5Db.Logs.AsNoTracking().ToListAsync();
            var alpha6LogIds = await alpha6Db.Logs.Select(x => x.ID).ToListAsync();
            alpha6Db.Logs.AddRange(alpha5Logs.Where(x => !alpha6LogIds.Contains(x.ID)));

            var alpha5Mediums = await alpha5Db.Mediums.AsNoTracking().ToListAsync();
            var alpha6MediumIds = await alpha6Db.Mediums.Select(x => x.mediumID).ToListAsync();
            alpha6Db.Mediums.AddRange(alpha5Mediums.Where(x => !alpha6MediumIds.Contains(x.mediumID)));

            var alpha5Formats = await alpha5Db.Formats.AsNoTracking().ToListAsync();
            var alpha6FormatIds = await alpha6Db.Formats.Select(x => x.formatID).ToListAsync();
            alpha6Db.Formats.AddRange(alpha5Formats.Where(x => !alpha6FormatIds.Contains(x.formatID)));

            var alpha5Units = await alpha5Db.MeasuringUnits.AsNoTracking().ToListAsync();
            var alpha6UnitIds = await alpha6Db.MeasuringUnits.Select(x => x.unitID).ToListAsync();
            alpha6Db.MeasuringUnits.AddRange(alpha5Units.Where(x => !alpha6UnitIds.Contains(x.unitID)));

            var alpha5CodingLogs = await alpha5Db.CodingLogs.AsNoTracking().ToListAsync();
            var alpha6CodingLogIds = await alpha6Db.CodingLogs.Select(x => x.ID).ToListAsync();
            alpha6Db.CodingLogs.AddRange(alpha5CodingLogs.Where(x => !alpha6CodingLogIds.Contains(x.ID)));

            var alpha5AndroidCodingLogs = await alpha5Db.AndroidCodingLogs.AsNoTracking().ToListAsync();
            var alpha6AndroidCodingLogIds = await alpha6Db.AndroidCodingLogs.Select(x => x.ID).ToListAsync();
            alpha6Db.AndroidCodingLogs.AddRange(alpha5AndroidCodingLogs.Where(x => !alpha6AndroidCodingLogIds.Contains(x.ID)));

            var alpha5GraphicsLogs = await alpha5Db.GraphicsLogs.AsNoTracking().ToListAsync();
            var alpha6GraphicsLogIds = await alpha6Db.GraphicsLogs.Select(x => x.ID).ToListAsync();
            alpha6Db.GraphicsLogs.AddRange(alpha5GraphicsLogs.Where(x => !alpha6GraphicsLogIds.Contains(x.ID)));

            var alpha5FilmLogs = await alpha5Db.FilmLogs.AsNoTracking().ToListAsync();
            var alpha6FilmLogIds = await alpha6Db.FilmLogs.Select(x => x.ID).ToListAsync();
            alpha6Db.FilmLogs.AddRange(alpha5FilmLogs.Where(x => !alpha6FilmLogIds.Contains(x.ID)));

            var alpha5NotesLogs = await alpha5Db.NotesLogs.AsNoTracking().ToListAsync();
            var alpha6NotesLogIds = await alpha6Db.NotesLogs.Select(x => x.ID).ToListAsync();
            alpha6Db.NotesLogs.AddRange(alpha5NotesLogs.Where(x => !alpha6NotesLogIds.Contains(x.ID)));

            var alpha5NoteItems = await alpha5Db.NoteItems.AsNoTracking().ToListAsync();
            var alpha6NoteItemIds = await alpha6Db.NoteItems.Select(x => x.ID).ToListAsync();
            alpha6Db.NoteItems.AddRange(alpha5NoteItems.Where(x => !alpha6NoteItemIds.Contains(x.ID)));

            var alpha5Checklists = await alpha5Db.Checklists.AsNoTracking().ToListAsync();
            var alpha6ChecklistIds = await alpha6Db.Checklists.Select(x => x.logID).ToListAsync();
            alpha6Db.Checklists.AddRange(alpha5Checklists.Where(x => !alpha6ChecklistIds.Contains(x.logID)));

            var alpha5ChecklistItems = await alpha5Db.ChecklistItems.AsNoTracking().ToListAsync();
            var alpha6ChecklistItemIds = await alpha6Db.ChecklistItems.Select(x => x.itemID).ToListAsync();
            alpha6Db.ChecklistItems.AddRange(alpha5ChecklistItems.Where(x => !alpha6ChecklistItemIds.Contains(x.itemID)));

            var alpha5FlexiNotesLogs = await alpha5Db.FlexiNotesLogs.AsNoTracking().ToListAsync();
            var alpha6FlexiNotesLogIds = await alpha6Db.FlexiNotesLogs.Select(x => x.ID).ToListAsync();
            alpha6Db.FlexiNotesLogs.AddRange(alpha5FlexiNotesLogs.Where(x => !alpha6FlexiNotesLogIds.Contains(x.ID)));

            var alpha5Feedback = await alpha5Db.AllFeedback.AsNoTracking().ToListAsync();
            var alpha6FeedbackIds = await alpha6Db.AllFeedback.Select(x => x.feedbackID).ToListAsync();
            alpha6Db.AllFeedback.AddRange(alpha5Feedback.Where(x => !alpha6FeedbackIds.Contains(x.feedbackID)));

            await alpha6Db.SaveChangesAsync();
        }

    }
}

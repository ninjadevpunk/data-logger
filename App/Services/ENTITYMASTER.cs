using Data_Logger_1._3.Models;
using Data_Logger_1._3.Models.App_Models;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace Data_Logger_1._3.Services
{
    public class EntityMaster : DbContext
    {

        public DbSet<ApplicationClass> Applications { get; set; }
        public DbSet<ProjectClass> Projects { get; set; }
        public DbSet<OutputClass> Outputs { get; set; }
        public DbSet<TypeClass> Types { get; set; }
        public DbSet<ACCOUNT> Accounts { get; set; }
        public DbSet<PostIt> PostIts { get; set; }
        public DbSet<SubjectClass> Subjects { get; set; }
        public DbSet<LOG> Logs { get; set; }
        public DbSet<MediumClass> Mediums { get; set; }
        public DbSet<FormatClass> Formats { get; set; }
        public DbSet<MeasuringUnitClass> MeasuringUnits { get; set; }
        public DbSet<CodingLOG> CodingLogs { get; set; }
        public DbSet<AndroidCodingLOG> AndroidCodingLogs { get; set; }
        public DbSet<GraphicsLOG> GraphicsLogs { get; set; }
        public DbSet<FilmLOG> FilmLogs { get; set; }
        public DbSet<NotesLOG> NotesLogs { get; set; }
        public DbSet<NoteItem> NoteItems { get; set; }
        public DbSet<CheckList> Checklists { get; set; }
        public DbSet<CheckListItem> ChecklistItems { get; set; }
        public DbSet<FlexiNotesLOG> FlexiNotesLogs { get; set; }
        public DbSet<FeedbackMessage> AllFeedback { get; set; }


        public EntityMaster(DbContextOptions<EntityMaster> options)
        : base(options)
        {
            Batteries_V2.Init();
        }



        public virtual async Task SaveChangesAsync()
        {
            await base.SaveChangesAsync();
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationClass>()
                .HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.accountID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProjectClass>()
                .HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.accountID)
                .OnDelete(DeleteBehavior.Cascade);

            // LOG

            modelBuilder.Entity<LOG>()
                .HasOne(log => log.Author)
                .WithMany()
                .HasForeignKey(log => log.accountID)
                .OnDelete(DeleteBehavior.Cascade);

            // APPLICATION

            modelBuilder.Entity<ProjectClass>()
                .HasOne(p => p.Application)
                .WithMany()
                .HasForeignKey(p => p.appID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OutputClass>()
                .HasOne(o => o.Application)
                .WithMany()
                .HasForeignKey(o => o.appID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TypeClass>()
                .HasOne(t => t.Application)
                .WithMany()
                .HasForeignKey(t => t.appID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SubjectClass>()
                .HasOne(s => s.Application)
                .WithMany()
                .HasForeignKey(s => s.appID)
                .OnDelete(DeleteBehavior.Cascade);



            modelBuilder.Entity<FeedbackMessage>()
                .HasOne(feed => feed.User)
                .WithMany()
                .HasForeignKey(feed => feed.accountID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<NoteItem>()
                .HasOne(n => n.Checklist)
                .WithOne(c => c.NoteItem)
                .HasForeignKey<NoteItem>(n => n.CheckListID)
                .IsRequired(false);


            AddAdmin(modelBuilder);
            AddApplications(modelBuilder);
            AddProjects(modelBuilder);
            AddOutputs(modelBuilder);
            AddTypes(modelBuilder);
            AddSubjects(modelBuilder);
            AddMediums(modelBuilder);
            AddFormats(modelBuilder);
            AddUnits(modelBuilder);

        }

        



        #region Add Items



        private static void AddAdmin(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ACCOUNT>().HasData(
                new ACCOUNT
                {
                    accountID = 1,
                    ProfilePic = "",
                    FirstName = "admin",
                    LastName = "",
                    Email = "support@datalogger.space",
                    Password = "pcsx2024",
                    IsEmployee = false,
                    CompanyName = "",
                    CompanyAddress = "",
                    CompanyLogo = "",
                    IsOnline = false
                }
            );
        }


        private static void AddApplications(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationClass>().HasData(
                new ApplicationClass { appID = 1, accountID = 1, Name = "Qt Creator", Category = LOG.CATEGORY.CODING, IsDefault = true },
                new ApplicationClass { appID = 2, accountID = 1, Name = "Android Studio", Category = LOG.CATEGORY.CODING, IsDefault = true },
                new ApplicationClass { appID = 3, accountID = 1, Name = "Unknown", Category = LOG.CATEGORY.CODING, IsDefault = true },
                new ApplicationClass { appID = 4, accountID = 1, Name = "Visual Studio Community 2022", Category = LOG.CATEGORY.CODING, IsDefault = true },

                new ApplicationClass { appID = 5, accountID = 1, Name = "Krita", Category = LOG.CATEGORY.GRAPHICS, IsDefault = true },
                new ApplicationClass { appID = 6, accountID = 1, Name = "Inkscape", Category = LOG.CATEGORY.GRAPHICS, IsDefault = true },
                new ApplicationClass { appID = 7, accountID = 1, Name = "Canva", Category = LOG.CATEGORY.GRAPHICS, IsDefault = true },
                new ApplicationClass { appID = 8, accountID = 1, Name = "Adobe Illustrator", Category = LOG.CATEGORY.GRAPHICS, IsDefault = true },

                new ApplicationClass { appID = 9, accountID = 1, Name = "Da Vinci Resolve", Category = LOG.CATEGORY.FILM, IsDefault = true },
                new ApplicationClass { appID = 10, accountID = 1, Name = "Blender 3D/2D", Category = LOG.CATEGORY.FILM, IsDefault = true },
                new ApplicationClass { appID = 11, accountID = 1, Name = "Powerpoint", Category = LOG.CATEGORY.FILM, IsDefault = true },
                new ApplicationClass { appID = 12, accountID = 1, Name = "Shotcut", Category = LOG.CATEGORY.FILM, IsDefault = true },

                new ApplicationClass { appID = 13, accountID = 1, Name = "Unity", Category = LOG.CATEGORY.NOTES, IsDefault = true },
                new ApplicationClass { appID = 14, accountID = 1, Name = "Steam", Category = LOG.CATEGORY.NOTES, IsDefault = true },
                new ApplicationClass { appID = 15, accountID = 1, Name = "Data Logger NOTES", Category = LOG.CATEGORY.NOTES, IsDefault = true },
                new ApplicationClass { appID = 16, accountID = 1, Name = "Data Logger Checklist", Category = LOG.CATEGORY.NOTES, IsDefault = true },
                new ApplicationClass { appID = 17, accountID = 1, Name = "Microsoft Word", Category = LOG.CATEGORY.NOTES, IsDefault = true },
                new ApplicationClass { appID = 18, accountID = 1, Name = "REAPER", Category = LOG.CATEGORY.NOTES, IsDefault = true },
                new ApplicationClass { appID = 19, accountID = 1, Name = "Notepad", Category = LOG.CATEGORY.NOTES, IsDefault = true },
                new ApplicationClass { appID = 20, accountID = 1, Name = "Microsoft Excel", Category = LOG.CATEGORY.NOTES, IsDefault = true },
                new ApplicationClass { appID = 21, accountID = 1, Name = "Microsoft Access", Category = LOG.CATEGORY.NOTES, IsDefault = true },

                new ApplicationClass { appID = 22, accountID = 1, Name = "IntelliJ", Category = LOG.CATEGORY.CODING, IsDefault = true },
                new ApplicationClass { appID = 23, accountID = 1, Name = "PyCharm", Category = LOG.CATEGORY.CODING, IsDefault = true },
                new ApplicationClass { appID = 24, accountID = 1, Name = "WebStorm", Category = LOG.CATEGORY.CODING, IsDefault = true },
                new ApplicationClass { appID = 25, accountID = 1, Name = "Visual Studio Code", Category = LOG.CATEGORY.CODING, IsDefault = true }
                );
        }

        private void AddProjects(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProjectClass>().HasData(
                new ProjectClass { projectID = 1, accountID = 1, appID = 3, Category = LOG.CATEGORY.CODING, Name = "Unnamed Project", IsDefault = true }
            );
        }

        private void AddOutputs(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OutputClass>().HasData(
                new OutputClass { outputID = 1, accountID = 1, Category = LOG.CATEGORY.CODING, appID = 1, Name = "Console Application" },
                new OutputClass { outputID = 2, accountID = 1, Category = LOG.CATEGORY.CODING, appID = 1, Name = "Widgets Application" },
                new OutputClass { outputID = 3, accountID = 1, Category = LOG.CATEGORY.CODING, appID = 1, Name = "QtQuick Application" },
                new OutputClass { outputID = 4, accountID = 1, Category = LOG.CATEGORY.CODING, appID = 2, Name = "APK (*.apk)" },
                new OutputClass { outputID = 5, accountID = 1, Category = LOG.CATEGORY.CODING, appID = 2, Name = "USB" },
                new OutputClass { outputID = 6, accountID = 1, Category = LOG.CATEGORY.CODING, appID = 2, Name = "Emulator (*.exe)" },
                new OutputClass { outputID = 7, accountID = 1, Category = LOG.CATEGORY.CODING, appID = 4, Name = "C# Application (*.exe)" },
                new OutputClass { outputID = 8, accountID = 1, Category = LOG.CATEGORY.CODING, appID = 3, Name = "C++ Application (*.exe)" },
                new OutputClass { outputID = 9, accountID = 1, Category = LOG.CATEGORY.CODING, appID = 3, Name = "Java Application (*.exe)" },
                new OutputClass { outputID = 10, accountID = 1, Category = LOG.CATEGORY.CODING, appID = 3, Name = "Database (*.db)" },
                new OutputClass { outputID = 11, accountID = 1, Category = LOG.CATEGORY.CODING, appID = 3, Name = "Database (SQL, Oracle)" },

                new OutputClass { outputID = 12, accountID = 1, Category = LOG.CATEGORY.GRAPHICS, appID = 5, Name = "PNG" },
                new OutputClass { outputID = 13, accountID = 1, Category = LOG.CATEGORY.GRAPHICS, appID = 5, Name = "JPG" },
                new OutputClass { outputID = 14, accountID = 1, Category = LOG.CATEGORY.GRAPHICS, appID = 6, Name = "SVG" },
                new OutputClass { outputID = 15, accountID = 1, Category = LOG.CATEGORY.GRAPHICS, appID = 5, Name = "GIF" },
                new OutputClass { outputID = 16, accountID = 1, Category = LOG.CATEGORY.GRAPHICS, appID = 7, Name = "PDF" },
                new OutputClass { outputID = 17, accountID = 1, Category = LOG.CATEGORY.GRAPHICS, appID = 8, Name = "TIFF" },
                new OutputClass { outputID = 18, accountID = 1, Category = LOG.CATEGORY.GRAPHICS, appID = 8, Name = "PSD" },
                new OutputClass { outputID = 19, accountID = 1, Category = LOG.CATEGORY.GRAPHICS, appID = 8, Name = "WEBP" },

                new OutputClass { outputID = 20, accountID = 1, Category = LOG.CATEGORY.FILM, appID = 9, Name = "MP4" },
                new OutputClass { outputID = 21, accountID = 1, Category = LOG.CATEGORY.FILM, appID = 9, Name = "AVI" },
                new OutputClass { outputID = 22, accountID = 1, Category = LOG.CATEGORY.FILM, appID = 9, Name = "MKV" },
                new OutputClass { outputID = 23, accountID = 1, Category = LOG.CATEGORY.FILM, appID = 9, Name = "TS" },
                new OutputClass { outputID = 24, accountID = 1, Category = LOG.CATEGORY.FILM, appID = 3, Name = "WEBM" },

                new OutputClass { outputID = 25, accountID = 1, Category = LOG.CATEGORY.NOTES, appID = 17, Name = "PDF" },
                new OutputClass { outputID = 26, accountID = 1, Category = LOG.CATEGORY.NOTES, appID = 17, Name = "WORD" },
                new OutputClass { outputID = 27, accountID = 1, Category = LOG.CATEGORY.NOTES, appID = 17, Name = "TEXT FILE (*.txt)" },
                new OutputClass { outputID = 28, accountID = 1, Category = LOG.CATEGORY.NOTES, appID = 17, Name = "HTML (*.html)" },
                new OutputClass { outputID = 29, accountID = 1, Category = LOG.CATEGORY.NOTES, appID = 17, Name = "XPS (*.xps)" },
                new OutputClass { outputID = 30, accountID = 1, Category = LOG.CATEGORY.NOTES, appID = 20, Name = "EXCEL FILE (*.xlsx)" },
                new OutputClass { outputID = 31, accountID = 1, Category = LOG.CATEGORY.NOTES, appID = 21, Name = "ACCESS FILE (*.accdb)" },

                new OutputClass { outputID = 32, accountID = 1, Category = LOG.CATEGORY.NOTES, Name = "MP3" },
                new OutputClass { outputID = 33, accountID = 1, Category = LOG.CATEGORY.NOTES, Name = "WAV" },
                new OutputClass { outputID = 34, accountID = 1, Category = LOG.CATEGORY.NOTES, Name = "AAC" },
                new OutputClass { outputID = 35, accountID = 1, Category = LOG.CATEGORY.NOTES, Name = "M4A" },
                new OutputClass { outputID = 36, accountID = 1, Category = LOG.CATEGORY.NOTES, Name = "OGG" },

                new OutputClass { outputID = 37, accountID = 1, Category = LOG.CATEGORY.NOTES, Name = "Note" },
                new OutputClass { outputID = 38, accountID = 1, Category = LOG.CATEGORY.NOTES, Name = "Check List" },
                new OutputClass { outputID = 39, accountID = 1, Category = LOG.CATEGORY.NOTES, Name = "PNG" },
                new OutputClass { outputID = 40, accountID = 1, Category = LOG.CATEGORY.NOTES, Name = "JPG" },
                new OutputClass { outputID = 41, accountID = 1, Category = LOG.CATEGORY.NOTES, Name = "EXE (*.exe)" },

                new OutputClass { outputID = 42, accountID = 1, Category = LOG.CATEGORY.CODING, Name = "JavaScript (*.js)" },
                new OutputClass { outputID = 43, accountID = 1, Category = LOG.CATEGORY.CODING, Name = "Kotlin Application (*.kt)" },
                new OutputClass { outputID = 44, accountID = 1, Category = LOG.CATEGORY.CODING, Name = "Python Application (*.py)" },
                new OutputClass { outputID = 45, accountID = 1, Category = LOG.CATEGORY.CODING, Name = "React App (*.jsx | *.js)" },
                new OutputClass { outputID = 46, accountID = 1, Category = LOG.CATEGORY.CODING, Name = "PHP (*.php)" },
                new OutputClass { outputID = 47, accountID = 1, Category = LOG.CATEGORY.CODING, Name = "CSS (*.css)" },
                new OutputClass { outputID = 48, accountID = 1, Category = LOG.CATEGORY.CODING, Name = "Tailwind CSS" }

            );
        }

        private void AddTypes(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TypeClass>().HasData(
                new TypeClass { typeID = 1, accountID = 1, Name = "NONE", Category = LOG.CATEGORY.CODING },
                new TypeClass { typeID = 2, accountID = 1, Name = "Build", Category = LOG.CATEGORY.CODING },
                new TypeClass { typeID = 3, accountID = 1, Name = "Runtime", Category = LOG.CATEGORY.CODING },
                new TypeClass { typeID = 4, accountID = 1, Name = "Sync", Category = LOG.CATEGORY.CODING },
                new TypeClass { typeID = 5, accountID = 1, Name = "Build", Category = LOG.CATEGORY.CODING },
                new TypeClass { typeID = 6, accountID = 1, Name = "Runtime", Category = LOG.CATEGORY.CODING },
                new TypeClass { typeID = 7, accountID = 1, Name = "Compilation", Category = LOG.CATEGORY.CODING },
                new TypeClass { typeID = 8, accountID = 1, Name = "Runtime", Category = LOG.CATEGORY.CODING },

                new TypeClass { typeID = 9, accountID = 1, Name = "NONE", Category = LOG.CATEGORY.GRAPHICS },
                new TypeClass { typeID = 10, accountID = 1, Name = "Artwork", Category = LOG.CATEGORY.GRAPHICS },
                new TypeClass { typeID = 11, accountID = 1, Name = "Doodle", Category = LOG.CATEGORY.GRAPHICS },
                new TypeClass { typeID = 12, accountID = 1, Name = "Graphic Design", Category = LOG.CATEGORY.GRAPHICS },
                new TypeClass { typeID = 13, accountID = 1, Name = "Resource", Category = LOG.CATEGORY.GRAPHICS },
                new TypeClass { typeID = 14, accountID = 1, Name = "Portfolio", Category = LOG.CATEGORY.GRAPHICS },

                new TypeClass { typeID = 15, accountID = 1, Name = "NONE", Category = LOG.CATEGORY.FILM },
                new TypeClass { typeID = 16, accountID = 1, Name = "Film", Category = LOG.CATEGORY.FILM },
                new TypeClass { typeID = 17, accountID = 1, Name = "Doodle", Category = LOG.CATEGORY.FILM },
                new TypeClass { typeID = 18, accountID = 1, Name = "Assignment", Category = LOG.CATEGORY.FILM },

                new TypeClass { typeID = 19, accountID = 1, Name = "Music", Category = LOG.CATEGORY.NOTES },
                new TypeClass { typeID = 20, accountID = 1, Name = "Poetry", Category = LOG.CATEGORY.NOTES },
                new TypeClass { typeID = 21, accountID = 1, Name = "Mantra", Category = LOG.CATEGORY.NOTES },
                new TypeClass { typeID = 22, accountID = 1, Name = "Boardgame", Category = LOG.CATEGORY.NOTES },
                new TypeClass { typeID = 23, accountID = 1, Name = "Video game", Category = LOG.CATEGORY.NOTES },
                new TypeClass { typeID = 24, accountID = 1, Name = "Card game", Category = LOG.CATEGORY.NOTES },
                new TypeClass { typeID = 25, accountID = 1, Name = "Game - Design ONLY", Category = LOG.CATEGORY.NOTES },
                new TypeClass { typeID = 26, accountID = 1, Name = "Document", Category = LOG.CATEGORY.NOTES },
                new TypeClass { typeID = 27, accountID = 1, Name = "Invitation", Category = LOG.CATEGORY.NOTES },
                new TypeClass { typeID = 28, accountID = 1, Name = "Mail", Category = LOG.CATEGORY.NOTES },
                new TypeClass { typeID = 29, accountID = 1, Name = "Report", Category = LOG.CATEGORY.NOTES },
                new TypeClass { typeID = 30, accountID = 1, Name = "Doodle", Category = LOG.CATEGORY.NOTES },
                new TypeClass { typeID = 31, accountID = 1, Name = "Novel", Category = LOG.CATEGORY.NOTES },
                new TypeClass { typeID = 32, accountID = 1, Name = "Questionnaire", Category = LOG.CATEGORY.NOTES },
                new TypeClass { typeID = 33, accountID = 1, Name = "CV", Category = LOG.CATEGORY.NOTES },
                new TypeClass { typeID = 34, accountID = 1, Name = "Web page", Category = LOG.CATEGORY.NOTES },
                new TypeClass { typeID = 35, accountID = 1, Name = "Presentation", Category = LOG.CATEGORY.NOTES },
                new TypeClass { typeID = 36, accountID = 1, Name = "Spreadsheet", Category = LOG.CATEGORY.NOTES },
                new TypeClass { typeID = 37, accountID = 1, Name = "Notes", Category = LOG.CATEGORY.NOTES },
                new TypeClass { typeID = 38, accountID = 1, Name = "Access Database", Category = LOG.CATEGORY.NOTES },
                new TypeClass { typeID = 39, accountID = 1, Name = "Note", Category = LOG.CATEGORY.NOTES },
                new TypeClass { typeID = 40, accountID = 1, Name = "Check List", Category = LOG.CATEGORY.NOTES },
                new TypeClass { typeID = 41, accountID = 1, Name = "Exception", Category = LOG.CATEGORY.CODING }
            );
        }

        private void AddSubjects(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SubjectClass>().HasData(
                new SubjectClass
                {
                    subjectID = 1,
                    accountID = 1,
                    Category = LOG.CATEGORY.CODING,
                    Subject = "No Subject",
                    projectID = 1,
                    appID = 3
                }
            );
        }

        private void AddMediums(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MediumClass>().HasData(
                new MediumClass
                {
                    mediumID = 1,
                    accountID = 1,
                    Medium = "Pencil",
                    Category = LOG.CATEGORY.GRAPHICS
                },
                new MediumClass
                {
                    mediumID = 2,
                    accountID = 1,
                    Medium = "Paint",
                    Category = LOG.CATEGORY.GRAPHICS
                },
                new MediumClass
                {
                    mediumID = 3,
                    accountID = 1,
                    Medium = "Pen",
                    Category = LOG.CATEGORY.GRAPHICS
                },
                new MediumClass
                {
                    mediumID = 4,
                    accountID = 1,
                    Medium = "Pencil Crayon",
                    Category = LOG.CATEGORY.GRAPHICS
                },
                new MediumClass
                {
                    mediumID = 5,
                    accountID = 1,
                    Medium = "Kokie",
                    Category = LOG.CATEGORY.GRAPHICS
                },
                new MediumClass
                {
                    mediumID = 6,
                    accountID = 1,
                    Medium = "Crayon",
                    Category = LOG.CATEGORY.GRAPHICS
                },
                new MediumClass
                {
                    mediumID = 7,
                    accountID = 1,
                    Medium = "Oil Pastel",
                    Category = LOG.CATEGORY.GRAPHICS
                },
                new MediumClass
                {
                    mediumID = 8,
                    accountID = 1,
                    Medium = "Chalk",
                    Category = LOG.CATEGORY.GRAPHICS
                },
                new MediumClass
                {
                    mediumID = 9,
                    accountID = 1,
                    Medium = "A Cappella",
                    Category = LOG.CATEGORY.NOTES
                },
                new MediumClass
                {
                    mediumID = 10,
                    accountID = 1,
                    Medium = "Song",
                    Category = LOG.CATEGORY.NOTES
                },
                new MediumClass
                {
                    mediumID = 11,
                    accountID = 1,
                    Medium = "Orchestral",
                    Category = LOG.CATEGORY.NOTES
                },
                new MediumClass
                {
                    mediumID = 12,
                    accountID = 1,
                    Medium = "Game Engine",
                    Category = LOG.CATEGORY.NOTES
                }
            );
        }

        private void AddFormats(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FormatClass>().HasData(
                new FormatClass { formatID = 1, accountID = 1, Format = "Paper", Category = LOG.CATEGORY.GRAPHICS },
                new FormatClass { formatID = 2, accountID = 1, Format = "Digital Canvas", Category = LOG.CATEGORY.GRAPHICS },
                new FormatClass { formatID = 3, accountID = 1, Format = "Cardboard", Category = LOG.CATEGORY.GRAPHICS },
                new FormatClass { formatID = 4, accountID = 1, Format = "Wall/Street", Category = LOG.CATEGORY.GRAPHICS },
                new FormatClass { formatID = 5, accountID = 1, Format = "CD", Category = LOG.CATEGORY.NOTES },
                new FormatClass { formatID = 6, accountID = 1, Format = "MIDI", Category = LOG.CATEGORY.NOTES },
                new FormatClass { formatID = 7, accountID = 1, Format = "Digital", Category = LOG.CATEGORY.NOTES },
                new FormatClass { formatID = 8, accountID = 1, Format = "Tape", Category = LOG.CATEGORY.NOTES },
                new FormatClass { formatID = 9, accountID = 1, Format = "LP", Category = LOG.CATEGORY.NOTES },
                new FormatClass { formatID = 10, accountID = 1, Format = "EP", Category = LOG.CATEGORY.NOTES },
                new FormatClass { formatID = 11, accountID = 1, Format = "Gramophone", Category = LOG.CATEGORY.NOTES },
                new FormatClass { formatID = 12, accountID = 1, Format = "Digital Download", Category = LOG.CATEGORY.NOTES },
                new FormatClass { formatID = 13, accountID = 1, Format = "Disc", Category = LOG.CATEGORY.NOTES }
            );
        }

        private void AddUnits(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MeasuringUnitClass>().HasData(
                new MeasuringUnitClass { unitID = 1, Unit = "cm" },
                new MeasuringUnitClass { unitID = 2, Unit = "mm" },
                new MeasuringUnitClass { unitID = 3, Unit = "px" },
                new MeasuringUnitClass { unitID = 4, Unit = "in" },
                new MeasuringUnitClass { unitID = 5, Unit = "m" }
            );
        }



        #endregion



        



        
        
    }



}

using Core.Interfaces;
using Core.Models;
using Core.Models.App_Models;
using Core.Utilities;
using Data.Interfaces;
using Data.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DLSTests
{
    public class HANDLERUpdateTests : ENTITYMASTERTests
    {
        private static async Task<CodingLOG> SeedTestDataAsync(EntityMaster master)
        {
            // Author
            var author = new ACCOUNT
            {
                accountID = 1,
                FirstName = "Ninja",
                LastName = "Dev",
                Email = "ninja@test.com",
                Password = "test123",
                ProfilePic = Constants.Avatar
            };

            // Nav Data
            var app1 = new ApplicationClass(author, "Visual Studio");
            var project1 = new ProjectClass(author, "Project Alpha", app1);
            var output1 = new OutputClass(1, "EXE");
            var type1 = new TypeClass(1, "Bug Report");

            master.Applications.Add(app1);
            master.Projects.Add(project1);
            master.Outputs.Add(output1);
            master.Types.Add(type1);
            await master.SaveChangesAsync();

            // Subject
            var subject = new SubjectClass
            {
                Category = LOG.CATEGORY.CODING,
                accountID = author.accountID,
                Subject = "Existing Subject",
                projectID = project1.projectID,
                appID = app1.appID
            };
            master.Subjects.Add(subject);
            await master.SaveChangesAsync();

            // Post-Its
            var postItToUpdate = new PostIt { Subject = subject, subjectID = subject.subjectID, accountID = author.accountID, Error = "Old Error" };
            var postItToDelete = new PostIt { Subject = subject, subjectID = subject.subjectID, accountID = author.accountID, Error = "Delete Error" };

            // The Log
            var originalLog = new CodingLOG
            {
                ID = 1,
                Author = author,
                accountID = author.accountID,
                Application = app1,
                appID = app1.appID,
                Project = project1,
                projectID = project1.projectID,
                Output = output1,
                outputID = output1.outputID,
                Type = type1,
                typeID = type1.typeID,
                PostItList = new List<PostIt> { postItToUpdate, postItToDelete }
            };

            master.Logs.Add(originalLog);
            await master.SaveChangesAsync();

            // Clear tracker so the "Act" phase fetches fresh data
            master.ChangeTracker.Clear();

            return originalLog;
        }

        private static async Task<CodingLOG> SeedTestDataAsync(EntityMaster master, ACCOUNT account,
            List<ApplicationClass>? apps = null, List<ProjectClass>? projects = null,
            List<SubjectClass>? subjects = null, List<PostIt>? postIts = null)
        {
            apps ??= new();
            projects ??= new();
            subjects ??= new();
            postIts ??= new();

            master.Applications.AddRange(apps);
            master.Projects.AddRange(projects);
            master.Subjects.AddRange(subjects);
            await master.SaveChangesAsync();

            var log = new CodingLOG
            {
                ID = 1,
                Author = account,
                accountID = account.accountID,
                Application = apps.FirstOrDefault(),
                appID = apps.FirstOrDefault()?.appID ?? 0,
                Project = projects.FirstOrDefault(),
                projectID = projects.FirstOrDefault()?.projectID ?? 0,
                PostItList = postIts
            };

            master.Logs.Add(log);

            await master.SaveChangesAsync();
            master.ChangeTracker.Clear();

            return log;
        }

        private static async Task TesterUpdateCommandAsync(CodingLOG incomingLog, EntityMaster master, string editorApplicationText,
            string editorProjectText, int userID)
        {
            bool appIsNew = false;
            var account = incomingLog.Author;

            // APPLICATION

            if (string.IsNullOrEmpty(editorApplicationText))
            {
                // Set to "Unknown" default
                incomingLog.appID = 3;
                incomingLog.Application = null;
            }
            else if (incomingLog.Application.Name != editorApplicationText)
            {
                // App may be the same or changed. Changed app may be new or existing
                var app = await master.Applications.FirstOrDefaultAsync(a =>
                    a.Name == editorApplicationText);

                if (app == null)
                {
                    app = new ApplicationClass(editorApplicationText);
                    app.accountID = incomingLog.accountID;
                }

                appIsNew = app.appID == 0;

                if (appIsNew)
                    incomingLog.Application = app;
                else
                {
                    incomingLog.appID = app.appID;
                    incomingLog.Application = null;
                }
            }

            // PROJECT

            if (!appIsNew && incomingLog.Project != null)
            {
                bool projectBelongsToNewApp = incomingLog.Project.appID == incomingLog.appID;

                if (string.IsNullOrEmpty(editorProjectText))
                {
                    // Set to "Unnamed Project" default
                    incomingLog.projectID = 1;
                    incomingLog.Project = null;
                }
                else if (incomingLog.Project.Name != editorProjectText || !projectBelongsToNewApp)
                {
                    var project = await master.Projects.FirstOrDefaultAsync(p =>
                        p.Name == editorProjectText &&
                        p.appID == incomingLog.appID);

                    if (project == null)
                    {
                        project = new ProjectClass(editorProjectText);
                        project.accountID = account.accountID;
                        project.appID = incomingLog.appID;
                    }

                    if (project.projectID == 0)
                        incomingLog.Project = project;
                    else
                    {
                        incomingLog.projectID = project.projectID;
                        incomingLog.Project = null;
                    }
                }
            }
            else
            {
                if (string.IsNullOrEmpty(editorProjectText))
                {
                    // Set to "Unnamed Project" default
                    incomingLog.projectID = 1;
                    incomingLog.Project = null;
                }
                else
                    incomingLog.Project = new ProjectClass(account, editorProjectText, incomingLog.Application!);
            }
        }

        private static async Task TesterPostItUpdateCommandAsync(CodingLOG incomingLog,
            EntityMaster master, ApplicationClass app, ProjectClass project,
            List<PostItEditorInput> inputs)
        {
            foreach (var input in inputs)
            {
                var subject = await master.Subjects.FirstOrDefaultAsync(s =>
                    s.Subject == input.SubjectText &&
                    s.Category == incomingLog.Category);

                bool subjectIsNew = subject == null;

                if (subjectIsNew)
                {
                    subject = new SubjectClass
                    {
                        Subject = input.SubjectText,
                        accountID = incomingLog.accountID,
                        Category = incomingLog.Category,
                        appID = app.appID,
                        projectID = project.projectID
                    };

                    await master.Subjects.AddAsync(subject);
                }

                if (input.ID == 0)
                {
                    var newPostIt = new PostIt
                    {
                        Subject = subjectIsNew ? subject : null,
                        subjectID = subjectIsNew ? 0 : subject.subjectID,
                        Error = input.Error,
                        ERCaptureTime = input.DateFound,
                        Solution = input.Solution,
                        SOCaptureTime = input.DateSolved,
                        Suggestion = input.Suggestion,
                        Comment = input.Comment,
                        logID = incomingLog.ID,
                        accountID = incomingLog.accountID
                    };

                    incomingLog.PostItList.Add(newPostIt);
                }
                else
                {
                    var existing = incomingLog.PostItList.First(p => p.postItID == input.ID);

                    if (subjectIsNew)
                    {
                        existing.Subject = subject;
                    }
                    else
                    {
                        existing.subjectID = subject.subjectID;
                    }

                    existing.Error = input.Error;
                    existing.ERCaptureTime = input.DateFound;
                    existing.Solution = input.Solution;
                    existing.SOCaptureTime = input.DateSolved;
                    existing.Suggestion = input.Suggestion;
                    existing.Comment = input.Comment;
                }
            }

            var inputIds = inputs.Select(i => i.ID).ToList();

            var toRemove = incomingLog.PostItList
                .Where(p => !inputIds.Contains(p.postItID))
                .ToList();

            foreach (var removed in toRemove)
            {
                incomingLog.PostItList.Remove(removed);
            }
        }


        // DTOs

        public sealed class PostItEditorInput
        {
            public int ID { get; init; }
            public string SubjectText { get; init; } = string.Empty;
            public string Error { get; init; } = string.Empty;
            public DateTime DateFound { get; init; }
            public string Solution { get; init; } = string.Empty;
            public DateTime DateSolved { get; init; }
            public string Suggestion { get; init; } = string.Empty;
            public string Comment { get; init; } = string.Empty;
        }






        #region EXISTING TO EXISTING APP





        /// <summary>
        /// #1 + #A1
        /// Test the updating of application in the log to an existing application and changing
        /// project to an existing project.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task UpdateLogAsync_SwitchToExistingAppAndProject()
        {
            var services = new ServiceCollection();

            // In memory database
            services.AddDbContext<EntityMaster>(options => options.UseInMemoryDatabase(GetTestDbName()));

            // AuthService
            services.AddSingleton<IAuthService>((services) => new AuthService(services));

            // DataService
            services.AddSingleton<IDataService>(services => new DataService(services.GetRequiredService<IAuthService>(), services));

            var serviceProvider = services.BuildServiceProvider();

            CodingLOG originalLog;
            string editorApplicationText = string.Empty;
            string editorProjectText = string.Empty;
            int initialAppCount = 0;
            int initialProjectCount = 0;

            ApplicationClass app1, app2;
            ProjectClass project1, project2;

            using (var seedScope = serviceProvider.CreateAsyncScope())
            {
                var master = seedScope.ServiceProvider.GetRequiredService<EntityMaster>();
                originalLog = await SeedTestDataAsync(master);
                Assert.NotEqual(0, originalLog.ID);
                app1 = originalLog.Application;
                app2 = new ApplicationClass(originalLog.Author, "VS Code");
                project1 = originalLog.Project;
                project2 = new ProjectClass(originalLog.Author, "Project Beta", app2);
                editorApplicationText = app2.Name;
                editorProjectText = project2.Name;
                await master.Applications.AddAsync(app2);
                await master.Projects.AddAsync(project2);
                await master.SaveChangesAsync();

                initialAppCount = await master.Applications.CountAsync();
                initialProjectCount = await master.Projects.CountAsync();

                Assert.NotNull(await master.Applications.SingleOrDefaultAsync(a => a.Name == "VS Code"));
                Assert.NotNull(await master.Projects.SingleOrDefaultAsync(p => p.Name == "Project Beta"));
            }



            using (var actScope = serviceProvider.CreateAsyncScope())
            {
                var master = actScope.ServiceProvider.GetRequiredService<EntityMaster>();

                // ----------------------------------------
                // LOAD LOG (simulate it being fetched somewhere else)
                // ----------------------------------------
                var incomingLog = (CodingLOG)await master.Logs
                .Include(l => l.Author)
                .Include(l => l.Application)
                .Include(l => l.Project)
                .Include(l => l.Output)
                .Include(l => l.Type)
                .Include(l => l.PostItList)
                    .ThenInclude(p => p.Subject)
                .FirstAsync(l => l.ID == originalLog.ID);

                // ----------------------------------------
                // DETACH
                // ----------------------------------------
                master.ChangeTracker.Clear();

                // ----------------------------------------
                // APPLY EDITOR CHANGES (now working on detached object)
                // ----------------------------------------

                await TesterUpdateCommandAsync(incomingLog, master, editorApplicationText,
                    editorProjectText, incomingLog.accountID);


                var dataService = serviceProvider.GetRequiredService<IDataService>();

                // Update Log
                await dataService.UpdateLogAsync(incomingLog);
            }



            using (var assertScope = serviceProvider.CreateAsyncScope())
            {
                var master = assertScope.ServiceProvider.GetRequiredService<EntityMaster>();

                var result = (CodingLOG)await master.Logs
                .Include(l => l.Author)
                .Include(l => l.Application)
                .Include(l => l.Project)
                .Include(l => l.Output)
                .Include(l => l.Type)
                .Include(l => l.PostItList)
                    .ThenInclude(p => p.Subject)
                .FirstAsync(l => l.ID == originalLog.ID);

                // App changed
                Assert.NotEqual(app1.Name, result.Application.Name);
                Assert.NotEqual(app1.appID, result.appID);
                Assert.Equal(app2.Name, result.Application.Name);

                // App count hasn't changed
                int appCount = await master.Applications.CountAsync();
                Assert.True(initialAppCount == appCount);

                // Project changed
                Assert.Equal(project2.Name, result.Project.Name);

                // Project's app uses the new existing app
                Assert.Equal(result.Project.appID, result.appID);
                Assert.Equal(app2.appID, result.appID);
                Assert.NotEqual(app1.appID, result.appID);

                // Project count hasn't changed
                int projectCount = await master.Projects.CountAsync();
                Assert.True(initialProjectCount == projectCount);

                // Project ID changes and should not match project1.
                // project2's ID should match because the app is changed to app2
                Assert.NotEqual(project1.projectID, result.projectID);
                Assert.Equal(project2.projectID, result.projectID);

            }
        }




        /// <summary>
        /// #1 + #A2
        /// Test the updating of application in the log to an existing application and changing
        /// project to a NEW project.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task UpdateLogAsync_SwitchToExistingAppAndNewProject()
        {
            var services = new ServiceCollection();

            // In memory database
            services.AddDbContext<EntityMaster>(options => options.UseInMemoryDatabase(GetTestDbName()));

            // Writer
            services.AddScoped((services) => new EntityWriter(services));

            // Handler
            services.AddScoped((services) => new EntityHandler(services));

            var serviceProvider = services.BuildServiceProvider();

            CodingLOG originalLog;
            string editorApplicationText = string.Empty;
            string editorProjectText = string.Empty;
            int initialAppCount = 0;
            int initialProjectCount = 0;

            ApplicationClass app1, app2;
            ProjectClass project1, project2;

            using (var seedScope = serviceProvider.CreateAsyncScope())
            {
                var master = seedScope.ServiceProvider.GetRequiredService<EntityMaster>();
                originalLog = await SeedTestDataAsync(master);
                Assert.NotEqual(0, originalLog.ID);
                app1 = originalLog.Application;
                app2 = new ApplicationClass(originalLog.Author, "VS Code");
                project1 = originalLog.Project;
                project2 = new ProjectClass(originalLog.Author, "Project Beta", app2);
                editorApplicationText = app2.Name;
                editorProjectText = project2.Name;
                await master.Applications.AddAsync(app2);
                await master.SaveChangesAsync();

                initialAppCount = await master.Applications.CountAsync();
                initialProjectCount = await master.Projects.CountAsync();

                Assert.NotNull(await master.Applications.SingleOrDefaultAsync(a => a.Name == "VS Code"));
            }



            using (var actScope = serviceProvider.CreateAsyncScope())
            {
                var master = actScope.ServiceProvider.GetRequiredService<EntityMaster>();

                // ----------------------------------------
                // LOAD LOG (simulate it being fetched somewhere else)
                // ----------------------------------------
                var incomingLog = (CodingLOG)await master.Logs
                .Include(l => l.Author)
                .Include(l => l.Application)
                .Include(l => l.Project)
                .Include(l => l.Output)
                .Include(l => l.Type)
                .Include(l => l.PostItList)
                    .ThenInclude(p => p.Subject)
                .FirstAsync(l => l.ID == originalLog.ID);

                // ----------------------------------------
                // DETACH
                // ----------------------------------------
                master.ChangeTracker.Clear();

                // ----------------------------------------
                // APPLY EDITOR CHANGES (now working on detached object)
                // ----------------------------------------

                await TesterUpdateCommandAsync(incomingLog, master, editorApplicationText,
                    editorProjectText, incomingLog.accountID);


                // Retrieve Dependency
                var handler = serviceProvider.GetRequiredService<EntityHandler>();

                // Update Log
                await handler.UpdateLogAsync(incomingLog);
            }



            using (var assertScope = serviceProvider.CreateAsyncScope())
            {
                var master = assertScope.ServiceProvider.GetRequiredService<EntityMaster>();

                var result = (CodingLOG)await master.Logs
                .Include(l => l.Author)
                .Include(l => l.Application)
                .Include(l => l.Project)
                .Include(l => l.Output)
                .Include(l => l.Type)
                .Include(l => l.PostItList)
                    .ThenInclude(p => p.Subject)
                .FirstAsync(l => l.ID == originalLog.ID);

                // App changed
                Assert.NotEqual(app1.Name, result.Application.Name);
                Assert.NotEqual(app1.appID, result.appID);
                Assert.Equal(app2.Name, result.Application.Name);

                // App count hasn't changed
                int appCount = await master.Applications.CountAsync();
                Assert.True(initialAppCount == appCount);

                // Project changed
                Assert.Equal(project2.Name, result.Project.Name);

                // Project's app uses the new existing app
                Assert.Equal(result.Project.appID, result.appID);
                Assert.Equal(app2.appID, result.appID);
                Assert.NotEqual(app1.appID, result.appID);

                // A new project has been added. Count should be higher
                int projectCount = await master.Projects.CountAsync();
                Assert.True(initialProjectCount < projectCount);

                // Project ID changes and should not match project1 but project2.
                Assert.NotEqual(project1.projectID, result.projectID);
            }
        }




        /// <summary>
        /// #1 + #A3
        /// Test the updating of application in the log to an existing application with an 
        /// unchanged project.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task UpdateLogAsync_SwitchToExistingAppAndProjectUnchanged()
        {
            var services = new ServiceCollection();

            // In memory database
            services.AddDbContext<EntityMaster>(options => options.UseInMemoryDatabase(GetTestDbName()));

            // Writer
            services.AddScoped((services) => new EntityWriter(services));

            // Handler
            services.AddScoped((services) => new EntityHandler(services));

            var serviceProvider = services.BuildServiceProvider();

            CodingLOG originalLog;
            string editorApplicationText = string.Empty;
            string editorProjectText = string.Empty;
            int initialAppCount = 0;

            ApplicationClass app1, app2;
            ProjectClass project1;

            using (var seedScope = serviceProvider.CreateAsyncScope())
            {
                var master = seedScope.ServiceProvider.GetRequiredService<EntityMaster>();
                originalLog = await SeedTestDataAsync(master);
                Assert.NotEqual(0, originalLog.ID);
                app1 = originalLog.Application;
                app2 = new ApplicationClass(originalLog.Author, "VS Code");
                project1 = originalLog.Project;
                editorApplicationText = app2.Name;
                editorProjectText = project1.Name;
                await master.Applications.AddAsync(app2);
                await master.SaveChangesAsync();
                initialAppCount = await master.Applications.CountAsync();

                Assert.NotNull(await master.Applications.SingleOrDefaultAsync(a => a.Name == "VS Code"));
            }



            using (var actScope = serviceProvider.CreateAsyncScope())
            {
                var master = actScope.ServiceProvider.GetRequiredService<EntityMaster>();

                // ----------------------------------------
                // LOAD LOG (simulate it being fetched somewhere else)
                // ----------------------------------------
                var incomingLog = (CodingLOG)await master.Logs
                .Include(l => l.Author)
                .Include(l => l.Application)
                .Include(l => l.Project)
                .Include(l => l.Output)
                .Include(l => l.Type)
                .Include(l => l.PostItList)
                    .ThenInclude(p => p.Subject)
                .FirstAsync(l => l.ID == originalLog.ID);

                // ----------------------------------------
                // DETACH
                // ----------------------------------------
                master.ChangeTracker.Clear();

                // ----------------------------------------
                // APPLY EDITOR CHANGES (now working on detached object)
                // ----------------------------------------

                await TesterUpdateCommandAsync(incomingLog, master, editorApplicationText,
                    editorProjectText, incomingLog.accountID);

                // Retrieve Dependency
                var handler = serviceProvider.GetRequiredService<EntityHandler>();

                // Update Log
                await handler.UpdateLogAsync(incomingLog);
            }



            using (var assertScope = serviceProvider.CreateAsyncScope())
            {
                var master = assertScope.ServiceProvider.GetRequiredService<EntityMaster>();

                var result = (CodingLOG)await master.Logs
                .Include(l => l.Author)
                .Include(l => l.Application)
                .Include(l => l.Project)
                .Include(l => l.Output)
                .Include(l => l.Type)
                .Include(l => l.PostItList)
                    .ThenInclude(p => p.Subject)
                .FirstAsync(l => l.ID == originalLog.ID);

                // App changed
                Assert.NotEqual(app1.Name, result.Application.Name);
                Assert.NotEqual(app1.appID, result.appID);
                Assert.Equal(app2.Name, result.Application.Name);

                // App count hasn't changed
                int appCount = await master.Applications.CountAsync();
                Assert.True(initialAppCount == appCount);

                // Project unchanged
                Assert.Equal(project1.Name, result.Project.Name);

                // Project's app uses the new existing app
                Assert.Equal(app2.appID, result.appID);
                Assert.NotEqual(app1.appID, result.appID);
                Assert.Equal(result.Project.appID, result.appID);

                // Original unchanged project still points to old appID
                Assert.NotEqual(project1.appID, result.appID);

                // Project ID changes because only 1 project in this test
                Assert.NotEqual(project1.projectID, result.Project.projectID);

            }
        }



        #endregion








        #region #2 EXISTING TO NEW APP



        /// <summary>
        /// #2 + #A2
        /// Test the updating of a project to a NEW project in the log with a NEW application.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task UpdateLogAsync_SwitchToNewAppAndNewProject()
        {
            var services = new ServiceCollection();

            // In memory database
            services.AddDbContext<EntityMaster>(options => options.UseInMemoryDatabase(GetTestDbName()));

            // Writer
            services.AddScoped((services) => new EntityWriter(services));

            // Handler
            services.AddScoped((services) => new EntityHandler(services));

            var serviceProvider = services.BuildServiceProvider();

            CodingLOG originalLog;
            const string editorApplicationText = "Komodo IDE";
            const string editorProjectText = "Project Delta";
            int initialAppCount = 0;
            int initialProjectCount = 0;

            using (var seedScope = serviceProvider.CreateAsyncScope())
            {
                var master = seedScope.ServiceProvider.GetRequiredService<EntityMaster>();
                originalLog = await SeedTestDataAsync(master);
                initialAppCount = await master.Applications.CountAsync();
                initialProjectCount = await master.Projects.CountAsync();
            }


            using (var actScope = serviceProvider.CreateAsyncScope())
            {
                var master = actScope.ServiceProvider.GetRequiredService<EntityMaster>();

                // ----------------------------------------
                // LOAD LOG (simulate it being fetched somewhere else)
                // ----------------------------------------
                var incomingLog = (CodingLOG)await master.Logs
                    .Include(l => l.Author)
                    .Include(l => l.Application)
                    .Include(l => l.Project)
                    .Include(l => l.Output)
                    .Include(l => l.Type)
                    .Include(l => l.PostItList)
                        .ThenInclude(p => p.Subject)
                    .FirstAsync(l => l.ID == originalLog.ID);

                // ----------------------------------------
                // DETACH
                // ----------------------------------------
                master.ChangeTracker.Clear();

                // ----------------------------------------
                // APPLY EDITOR CHANGES (now working on detached object)
                // ----------------------------------------

                await TesterUpdateCommandAsync(incomingLog, master, editorApplicationText,
                    editorProjectText, incomingLog.accountID);

                // Retrieve Dependency
                var handler = serviceProvider.GetRequiredService<EntityHandler>();

                // Update Log
                await handler.UpdateLogAsync(incomingLog);
            }

            using (var assertScope = serviceProvider.CreateAsyncScope())
            {
                var master = assertScope.ServiceProvider.GetRequiredService<EntityMaster>();

                var result = (CodingLOG)await master.Logs
                .Include(l => l.Author)
                .Include(l => l.Application)
                .Include(l => l.Project)
                .Include(l => l.Output)
                .Include(l => l.Type)
                .Include(l => l.PostItList)
                    .ThenInclude(p => p.Subject)
                .FirstAsync(l => l.ID == originalLog.ID);

                // App name changed
                Assert.Equal(editorApplicationText, result.Application.Name);
                Assert.NotEqual(originalLog.appID, result.appID);

                // App Count is greater than initial Count
                int appCount = await master.Applications.CountAsync();
                Assert.True(initialAppCount < appCount);

                // Project name changed
                Assert.Equal(editorProjectText, result.Project.Name);
                Assert.NotEqual(originalLog.Project.Name, result.Project.Name);

                // Project has the new app
                Assert.Equal(result.appID, result.Project.appID);

                // Project Count is greater than initial count
                int projectCount = await master.Projects.CountAsync();
                Assert.True(initialProjectCount < projectCount);
            }
        }





        #endregion








        #region #3 UNCHANGED APP



        /// <summary>
        /// #3 + #A1
        /// Test the updating of a project to an existing project in the log with an unchanged
        /// application.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task UpdateLogAsync_SwitchToUnchangedAppAndExistingProject()
        {
            var services = new ServiceCollection();

            // In memory database
            services.AddDbContext<EntityMaster>(options => options.UseInMemoryDatabase(GetTestDbName()));

            // Writer
            services.AddScoped((services) => new EntityWriter(services));

            // Handler
            services.AddScoped((services) => new EntityHandler(services));

            var serviceProvider = services.BuildServiceProvider();

            CodingLOG originalLog;
            string editorApplicationText = string.Empty;
            string editorProjectText = string.Empty;
            int initialProjectCount = 0;

            ApplicationClass? app1;
            ProjectClass project1, project2;

            using (var seedScope = serviceProvider.CreateAsyncScope())
            {
                var master = seedScope.ServiceProvider.GetRequiredService<EntityMaster>();
                originalLog = await SeedTestDataAsync(master);
                app1 = await master.Applications.FirstOrDefaultAsync(a => a.appID == originalLog.appID);
                project1 = originalLog.Project;
                project2 = new ProjectClass(originalLog.Author, "Project Beta", app1);
                editorApplicationText = app1.Name;
                editorProjectText = project2.Name;
                await master.Projects.AddAsync(project2);
                await master.SaveChangesAsync();

                initialProjectCount = await master.Projects.CountAsync();
            }



            using (var actScope = serviceProvider.CreateAsyncScope())
            {
                var master = actScope.ServiceProvider.GetRequiredService<EntityMaster>();

                // ----------------------------------------
                // LOAD LOG (simulate it being fetched somewhere else)
                // ----------------------------------------
                var incomingLog = (CodingLOG)await master.Logs
                    .Include(l => l.Author)
                    .Include(l => l.Application)
                    .Include(l => l.Project)
                    .Include(l => l.Output)
                    .Include(l => l.Type)
                    .Include(l => l.PostItList)
                        .ThenInclude(p => p.Subject)
                    .FirstAsync(l => l.ID == originalLog.ID);

                // ----------------------------------------
                // DETACH
                // ----------------------------------------
                master.ChangeTracker.Clear();

                // ----------------------------------------
                // APPLY EDITOR CHANGES (now working on detached object)
                // ----------------------------------------

                await TesterUpdateCommandAsync(incomingLog, master, editorApplicationText,
                    editorProjectText, incomingLog.accountID);

                // Retrieve Dependency
                var handler = serviceProvider.GetRequiredService<EntityHandler>();

                // Update Log
                await handler.UpdateLogAsync(incomingLog);
            }

            using (var assertScope = serviceProvider.CreateAsyncScope())
            {
                var master = assertScope.ServiceProvider.GetRequiredService<EntityMaster>();

                var result = (CodingLOG)await master.Logs
                .Include(l => l.Author)
                .Include(l => l.Application)
                .Include(l => l.Project)
                .Include(l => l.Output)
                .Include(l => l.Type)
                .Include(l => l.PostItList)
                    .ThenInclude(p => p.Subject)
                .FirstAsync(l => l.ID == originalLog.ID);

                // Project Changed to New Existing
                Assert.Equal(editorProjectText, result.Project.Name);
                Assert.NotEqual(originalLog.Project.Name, result.Project.Name);
                Assert.Equal(project2.projectID, result.projectID);

                // Project Count NOT Changed
                int projectsCount = await master.Projects.CountAsync();
                Assert.True(initialProjectCount == projectsCount);

                // App Unchanged
                Assert.Equal(app1.Name, result.Application.Name);
                Assert.Equal(originalLog.appID, result.appID);
                Assert.Equal(result.Project.appID, result.appID);
            }
        }



        /// <summary>
        /// #3 + #A2
        /// Test the updating of a project to a new project in the log with an existing 
        /// application.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task UpdateLogAsync_SwitchToUnchangedAppAndNewProject()
        {
            var services = new ServiceCollection();

            // In memory database
            services.AddDbContext<EntityMaster>(options => options.UseInMemoryDatabase(GetTestDbName()));

            // Writer
            services.AddScoped((services) => new EntityWriter(services));

            // Handler
            services.AddScoped((services) => new EntityHandler(services));

            var serviceProvider = services.BuildServiceProvider();

            CodingLOG originalLog;
            string editorApplicationText = string.Empty;
            const string editorProjectText = "Project Delta";
            int initialProjectCount = 0;

            ApplicationClass app1;

            using (var seedScope = serviceProvider.CreateAsyncScope())
            {
                var master = seedScope.ServiceProvider.GetRequiredService<EntityMaster>();
                originalLog = await SeedTestDataAsync(master);
                app1 = originalLog.Application;
                editorApplicationText = app1.Name;
                initialProjectCount = await master.Projects.CountAsync();
            }



            using (var actScope = serviceProvider.CreateAsyncScope())
            {
                var master = actScope.ServiceProvider.GetRequiredService<EntityMaster>();

                // ----------------------------------------
                // LOAD LOG (simulate it being fetched somewhere else)
                // ----------------------------------------
                var incomingLog = (CodingLOG)await master.Logs
                    .Include(l => l.Author)
                    .Include(l => l.Application)
                    .Include(l => l.Project)
                    .Include(l => l.Output)
                    .Include(l => l.Type)
                    .Include(l => l.PostItList)
                        .ThenInclude(p => p.Subject)
                    .FirstAsync(l => l.ID == originalLog.ID);

                // ----------------------------------------
                // DETACH
                // ----------------------------------------
                master.ChangeTracker.Clear();

                // ----------------------------------------
                // APPLY EDITOR CHANGES (now working on detached object)
                // ----------------------------------------

                await TesterUpdateCommandAsync(incomingLog, master, editorApplicationText,
                    editorProjectText, incomingLog.accountID);

                // Retrieve Dependency
                var handler = serviceProvider.GetRequiredService<EntityHandler>();

                // Update Log
                await handler.UpdateLogAsync(incomingLog);
            }

            using (var assertScope = serviceProvider.CreateAsyncScope())
            {
                var master = assertScope.ServiceProvider.GetRequiredService<EntityMaster>();

                var result = (CodingLOG)await master.Logs
                .Include(l => l.Author)
                .Include(l => l.Application)
                .Include(l => l.Project)
                .Include(l => l.Output)
                .Include(l => l.Type)
                .Include(l => l.PostItList)
                    .ThenInclude(p => p.Subject)
                .FirstAsync(l => l.ID == originalLog.ID);

                // Project Changed to New
                Assert.Equal(editorProjectText, result.Project.Name);
                Assert.NotEqual(originalLog.Project.Name, result.Project.Name);

                // Project Count Changed
                int projectsCount = await master.Projects.CountAsync();
                Assert.True(initialProjectCount < projectsCount);

                // App Unchanged
                Assert.Equal(app1.Name, result.Application.Name);
                Assert.Equal(originalLog.appID, result.appID);
                Assert.Equal(result.Project.appID, result.appID);
            }
        }




        /// <summary>
        /// #3 + #A3
        /// Test the updating of a project to an unchanged project in the log with an unchanged
        /// application.
        /// </summary>
        [Fact]
        public async Task UpdateLogAsync_SwitchToUnchangedAppAndUnchangedProject()
        {
            var services = new ServiceCollection();

            services.AddDbContext<EntityMaster>(options => options.UseInMemoryDatabase(GetTestDbName()));
            services.AddScoped((services) => new EntityWriter(services));
            services.AddScoped((services) => new EntityHandler(services));

            var serviceProvider = services.BuildServiceProvider();

            CodingLOG originalLog;
            string editorApplicationText = string.Empty;
            string editorProjectText = string.Empty;
            int initialAppCount = 0;
            int initialProjectCount = 0;

            ApplicationClass app1;
            ProjectClass project1;

            using (var seedScope = serviceProvider.CreateAsyncScope())
            {
                var master = seedScope.ServiceProvider.GetRequiredService<EntityMaster>();
                originalLog = await SeedTestDataAsync(master);

                app1 = originalLog.Application;
                project1 = originalLog.Project;

                editorApplicationText = app1.Name;
                editorProjectText = project1.Name;
                initialAppCount = await master.Applications.CountAsync();
                initialProjectCount = await master.Projects.CountAsync();
            }

            using (var actScope = serviceProvider.CreateAsyncScope())
            {
                var master = actScope.ServiceProvider.GetRequiredService<EntityMaster>();

                var incomingLog = (CodingLOG)await master.Logs
                    .Include(l => l.Author)
                    .Include(l => l.Application)
                    .Include(l => l.Project)
                    .Include(l => l.Output)
                    .Include(l => l.Type)
                    .Include(l => l.PostItList)
                        .ThenInclude(p => p.Subject)
                    .FirstAsync(l => l.ID == originalLog.ID);

                master.ChangeTracker.Clear();

                await TesterUpdateCommandAsync(
                    incomingLog,
                    master,
                    editorApplicationText,
                    editorProjectText,
                    incomingLog.accountID);

                var handler = serviceProvider.GetRequiredService<EntityHandler>();
                await handler.UpdateLogAsync(incomingLog);
            }

            using (var assertScope = serviceProvider.CreateAsyncScope())
            {
                var master = assertScope.ServiceProvider.GetRequiredService<EntityMaster>();

                var result = (CodingLOG)await master.Logs
                    .Include(l => l.Author)
                    .Include(l => l.Application)
                    .Include(l => l.Project)
                    .Include(l => l.Output)
                    .Include(l => l.Type)
                    .Include(l => l.PostItList)
                        .ThenInclude(p => p.Subject)
                    .FirstAsync(l => l.ID == originalLog.ID);

                // App unchanged
                Assert.Equal(app1.appID, result.appID);
                Assert.Equal(result.Project.appID, result.appID);

                // App count unchanged
                int appCount = await master.Applications.CountAsync();
                Assert.Equal(initialAppCount, appCount);

                // Project unchanged
                Assert.Equal(project1.Name, result.Project.Name);
                Assert.Equal(project1.projectID, result.projectID);

                // Project count unchanged
                int projectsCount = await master.Projects.CountAsync();
                Assert.Equal(initialProjectCount, projectsCount);

            }
        }



        #endregion






        #region POST ITS



        public static IEnumerable<object[]> LogTestCases()
        {
            var account = new ACCOUNT
            {
                accountID = 1,
                FirstName = "Ninja",
                LastName = "Dev",
                Email = "ninja@test.com",
                Password = "test123",
                ProfilePic = Constants.Avatar
            };

            // ---------- ADD ----------
            {

                var app = new ApplicationClass(account, "Visual Studio");
                var project = new ProjectClass(account, "Project Alpha", app);

                var subject = new SubjectClass(
                    LOG.CATEGORY.CODING,
                    account,
                    "Existing Subject",
                    project,
                    app
                );

                yield return new object[]
                {
                    "Add PostIt",

                    account,
                    new List<ApplicationClass> { app },
                    new List<ProjectClass> { project },
                    new List<SubjectClass> { subject },

                    new List<PostIt>
                    {
                        new PostIt
                        {
                            Subject = subject,
                            subjectID = subject.subjectID,
                            accountID = account.accountID,
                            Error = "Old"
                        }
                    },

                    new List<PostItEditorInput>
                    {
                        new PostItEditorInput
                        {
                            ID = 1,
                            SubjectText = "Existing Subject",
                            Error = "Old"
                        },
                        new PostItEditorInput
                        {
                            ID = 0,
                            SubjectText = "New Subject",
                            Error = "New"
                        }
                    },

                    (Action<CodingLOG>)(result =>
                    {
                        Assert.Equal(2, result.PostItList.Count);
                        Assert.Contains(result.PostItList, p => p.Error == "New");
                    })
                };
            }

            // ---------- UPDATE ----------
            {
                var app = new ApplicationClass(account, "Visual Studio");
                var project = new ProjectClass(account, "Project Alpha", app);

                var subject = new SubjectClass(
                    LOG.CATEGORY.CODING,
                    account,
                    "Existing Subject",
                    project,
                    app
                );

                yield return new object[]
                {
                    "Update PostIt",

                    account,
                    new List<ApplicationClass> { app },
                    new List<ProjectClass> { project },
                    new List<SubjectClass> { subject },

                    new List<PostIt>
                    {
                        new PostIt
                        {
                            postItID = 1,
                            Subject = subject,
                            subjectID = subject.subjectID,
                            accountID = account.accountID,
                            Error = "Old"
                        }
                    },

                    new List<PostItEditorInput>
                    {
                        new PostItEditorInput
                        {
                            ID = 1,
                            SubjectText = "Existing Subject",
                            Error = "Updated"
                        }
                    },

                    (Action<CodingLOG>)(result =>
                    {
                        Assert.Single(result.PostItList);
                        Assert.Contains(result.PostItList, p => p.Error == "Updated");
                    })
                };
            }

            // ---------- DELETE ----------
            {

                var app = new ApplicationClass(account, "Visual Studio");
                var project = new ProjectClass(account, "Project Alpha", app);

                var subjectA = new SubjectClass(
                    LOG.CATEGORY.CODING,
                    account,
                    "A",
                    project,
                    app
                );

                var subjectB = new SubjectClass(
                    LOG.CATEGORY.CODING,
                    account,
                    "B",
                    project,
                    app
                );

                yield return new object[]
                        {
                    "Delete PostIt",

                    account,
                    new List<ApplicationClass> { app },
                    new List<ProjectClass> { project },
                    new List<SubjectClass> { subjectA, subjectB },

                    new List<PostIt>
                    {
                        new PostIt
                        {
                            postItID = 1,
                            Subject = subjectA,
                            subjectID = subjectA.subjectID,
                            accountID = account.accountID,
                            Error = "Keep"
                        },
                        new PostIt
                        {
                            postItID = 2,
                            Subject = subjectB,
                            subjectID = subjectB.subjectID,
                            accountID = account.accountID,
                            Error = "Delete"
                        }
                    },

                    new List<PostItEditorInput>
                    {
                        new PostItEditorInput
                        {
                            ID = 1,
                            SubjectText = "A",
                            Error = "Keep"
                        }
                    },

                    (Action<CodingLOG>)(result =>
                    {
                        Assert.Single(result.PostItList);
                        Assert.DoesNotContain(result.PostItList, p => p.Error == "Delete");
                    })
                };
            }
        }





        /// <summary>
        /// Test PostIt cases
        /// </summary>
        /// <returns></returns>
        [Theory]
        [MemberData(nameof(LogTestCases))]
        public async Task UpdateLogAsync_PostIts_Cases(
            string caseName,
            ACCOUNT account,
            List<ApplicationClass> apps,
            List<ProjectClass> projects,
            List<SubjectClass> subjects,
            List<PostIt> seedPostIts,
            List<PostItEditorInput> inputs,
            Action<CodingLOG> assertAction)
        {
            var services = new ServiceCollection();

            var dbName = $"TestDB_{caseName}_{GetTestDbName()}";
            services.AddDbContext<EntityMaster>(options => options.UseInMemoryDatabase(dbName));

            services.AddScoped((s) => new EntityWriter(s));
            services.AddScoped((s) => new EntityHandler(s));

            var serviceProvider = services.BuildServiceProvider();

            CodingLOG originalLog;

            using (var seedScope = serviceProvider.CreateAsyncScope())
            {
                var master = seedScope.ServiceProvider.GetRequiredService<EntityMaster>();

                originalLog = await SeedTestDataAsync(
                    master,
                    account,
                    apps,
                    projects,
                    subjects,
                    seedPostIts
                );

                Assert.NotEmpty(await master.Logs.ToListAsync());
            }

            using (var actScope = serviceProvider.CreateAsyncScope())
            {
                var master = actScope.ServiceProvider.GetRequiredService<EntityMaster>();

                var incomingLog = (CodingLOG)await master.Logs
                    .Include(l => l.Author)
                    .Include(l => l.Application)
                    .Include(l => l.Project)
                    .Include(l => l.PostItList)
                        .ThenInclude(p => p.Subject)
                    .FirstAsync(l => l.ID == originalLog.ID);

                master.ChangeTracker.Clear();

                // App + Project logic
                await TesterUpdateCommandAsync(
                    incomingLog,
                    master,
                    incomingLog.Application.Name,
                    incomingLog.Project.Name,
                    incomingLog.accountID
                );

                // PostIt logic
                await TesterPostItUpdateCommandAsync(
                    incomingLog,
                    master,
                    incomingLog.Application,
                    incomingLog.Project,
                    inputs
                );

                Console.WriteLine("=== BEFORE UPDATE ===");
                DumpLog(incomingLog);

                var handler = serviceProvider.GetRequiredService<EntityHandler>();
                await handler.UpdateLogAsync(incomingLog);
            }

            using (var assertScope = serviceProvider.CreateAsyncScope())
            {
                var master = assertScope.ServiceProvider.GetRequiredService<EntityMaster>();

                var result = (CodingLOG)await master.Logs
                    .Include(l => l.Author)
                    .Include(l => l.Application)
                    .Include(l => l.Project)
                    .Include(l => l.PostItList)
                        .ThenInclude(p => p.Subject)
                    .FirstAsync(l => l.ID == originalLog.ID);

                Console.WriteLine("=== AFTER UPDATE ===");
                DumpLog(result);

                assertAction(result);
            }
        }





        #endregion

        void DumpLog(CodingLOG log)
        {
            Console.WriteLine($"Log ID: {log.ID}");
            Console.WriteLine($"PostIts count: {log.PostItList.Count}");

            foreach (var p in log.PostItList)
            {
                Console.WriteLine($"- ID: {p.postItID}, Error: {p.Error}");
            }
        }



    }



}

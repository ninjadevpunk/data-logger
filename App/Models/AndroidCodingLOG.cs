
using Data_Logger_1._3.Models.App_Models;
using System.ComponentModel.DataAnnotations.Schema;


namespace Data_Logger_1._3.Models
{

    [Table("AndroidCodingLOG")]
    public class AndroidCodingLOG : CodingLOG
    {
        /* DOCUMENTATION 
            
        Please use this class for Android Studio projects. It will be more useful.
         
         */

        /* ENUMS */



        /// <summary>
        /// Scope of the Android Studio logging. Full for all details, and simple for quick jotting.
        /// </summary>
        public enum SCOPE { FULL, SIMPLE }


        /* MEMBER VARIABLES */



        /// <summary>
        /// Store the selection between a simple Android LOG and full Android LOG
        /// </summary>

        public SCOPE Scope { get; set; } = SCOPE.FULL;


        /// <summary>
        /// Sync time.
        /// </summary>

        public DateTime Sync { get; set; } = new DateTime();

        /// <summary>
        /// The duration of the Gradle Daemon's ignition.
        /// </summary>

        public DateTime StartingGradleDaemon { get; set; }

        /// <summary>
        /// How long it takes for Android Studio to run build.
        /// </summary>

        public DateTime RunBuild { get; set; }

        /// <summary>
        /// How long it takes for Android Studio to load build.
        /// </summary>

        public DateTime LoadBuild { get; set; }

        /// <summary>
        /// How long it takes for Android Studio to configure build.
        /// </summary>

        public DateTime ConfigureBuild { get; set; }

        /// <summary>
        /// How long it takes for Android Studio to finish the last sync step.
        /// </summary>

        public DateTime AllProjects { get; set; }



        /* CONSTRUCTORS */



        public AndroidCodingLOG()
        {
        }

        public AndroidCodingLOG(SCOPE scope, DateTime sync, DateTime startingGradleDaemon,
            DateTime runBuild, DateTime loadBuild, DateTime configureBuild, DateTime allProjects)
        {
            SetApplication();
            Scope = scope;
            Sync = sync;
            StartingGradleDaemon = startingGradleDaemon;
            RunBuild = runBuild;
            LoadBuild = loadBuild;
            ConfigureBuild = configureBuild;
            AllProjects = allProjects;
        }

        public AndroidCodingLOG(int bugs, bool success,
            SCOPE scope, DateTime sync, DateTime startingGradleDaemon,
            DateTime runBuild, DateTime loadBuild, DateTime configureBuild, DateTime allProjects) : base(bugs, success)
        {
            SetApplication();
            Scope = scope;
            Sync = sync;
            StartingGradleDaemon = startingGradleDaemon;
            RunBuild = runBuild;
            LoadBuild = loadBuild;
            ConfigureBuild = configureBuild;
            AllProjects = allProjects;
        }

        public AndroidCodingLOG(int id, ACCOUNT author, ProjectClass projectName, ApplicationClass applicationName, DateTime startTime, DateTime endTime,
            OutputClass output, TypeClass type, List<PostIt> postItList, int bugs, bool success,
            SCOPE scope, DateTime sync, DateTime startingGradleDaemon,
            DateTime runBuild, DateTime loadBuild, DateTime configureBuild, DateTime allProjects) : base(id, author, projectName, applicationName, startTime, endTime, output, type, postItList, bugs, success)
        {
            SetApplication();
            Scope = scope;
            Sync = sync;
            StartingGradleDaemon = startingGradleDaemon;
            RunBuild = runBuild;
            LoadBuild = loadBuild;
            ConfigureBuild = configureBuild;
            AllProjects = allProjects;
        }

        private void SetApplication()
        {
            Application = new(2, new ACCOUNT(1, "", "admin", "", "support@datalogger.co.za", "pcsx2024", false, "", "", "", false), "Android Studio Meerkat 2024.3.1",
                CATEGORY.CODING, true);
        }


        /* OVERLAODS */



        public override bool Equals(object? obj)
        {
            return obj is AndroidCodingLOG lOG &&
                   base.Equals(obj) &&
                   Category == lOG.Category &&
                   ID == lOG.ID &&
                   Author == lOG.Author &&
                   Project == lOG.Project &&
                   Application == lOG.Application &&
                   Start == lOG.Start &&
                   End == lOG.End &&
                   Output == lOG.Output &&
                   Type == lOG.Type &&
                   PostItList.Equals(lOG.PostItList) &&
                   Bugs == lOG.Bugs &&
                   Success == lOG.Success &&
                   Scope == lOG.Scope &&
                   Sync == lOG.Sync &&
                   StartingGradleDaemon == lOG.StartingGradleDaemon &&
                   RunBuild == lOG.RunBuild &&
                   LoadBuild == lOG.LoadBuild &&
                   ConfigureBuild == lOG.ConfigureBuild &&
                   AllProjects == lOG.AllProjects;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(Category);
            hash.Add(ID);
            hash.Add(Author);
            hash.Add(Project);
            hash.Add(Application);
            hash.Add(Start);
            hash.Add(End);
            hash.Add(Output);
            hash.Add(Type);
            hash.Add(PostItList);
            hash.Add(Bugs);
            hash.Add(Success);
            hash.Add(Scope);
            hash.Add(Sync);
            hash.Add(StartingGradleDaemon);
            hash.Add(RunBuild);
            hash.Add(LoadBuild);
            hash.Add(ConfigureBuild);
            hash.Add(AllProjects);
            return hash.ToHashCode();
        }

        public static bool operator ==(AndroidCodingLOG left, AndroidCodingLOG right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(AndroidCodingLOG left, AndroidCodingLOG right)
        {
            return !left.Equals(right);
        }
    }
}

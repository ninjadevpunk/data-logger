using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Data_Logger_1._3.Migrations
{
    /// <inheritdoc />
    public partial class Alpha3InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ACCOUNT",
                columns: table => new
                {
                    accountID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProfilePic = table.Column<string>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    IsEmployee = table.Column<bool>(type: "INTEGER", nullable: false),
                    CompanyName = table.Column<string>(type: "TEXT", nullable: true),
                    CompanyAddress = table.Column<string>(type: "TEXT", nullable: true),
                    CompanyLogo = table.Column<string>(type: "TEXT", nullable: true),
                    IsOnline = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACCOUNT", x => x.accountID);
                });

            migrationBuilder.CreateTable(
                name: "UNIT",
                columns: table => new
                {
                    unitID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Unit = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UNIT", x => x.unitID);
                });

            migrationBuilder.CreateTable(
                name: "APPLICATION",
                columns: table => new
                {
                    appID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    accountID = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Category = table.Column<int>(type: "INTEGER", nullable: false),
                    IsDefault = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_APPLICATION", x => x.appID);
                    table.ForeignKey(
                        name: "FK_APPLICATION_ACCOUNT_accountID",
                        column: x => x.accountID,
                        principalTable: "ACCOUNT",
                        principalColumn: "accountID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CHECKLIST",
                columns: table => new
                {
                    logID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    accountID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CHECKLIST", x => x.logID);
                    table.ForeignKey(
                        name: "FK_CHECKLIST_ACCOUNT_accountID",
                        column: x => x.accountID,
                        principalTable: "ACCOUNT",
                        principalColumn: "accountID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FEEDBACK",
                columns: table => new
                {
                    feedbackID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Category = table.Column<int>(type: "INTEGER", nullable: false),
                    accountID = table.Column<int>(type: "INTEGER", nullable: false),
                    DateReported = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    CanContact = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsAutoFeed = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FEEDBACK", x => x.feedbackID);
                    table.ForeignKey(
                        name: "FK_FEEDBACK_ACCOUNT_accountID",
                        column: x => x.accountID,
                        principalTable: "ACCOUNT",
                        principalColumn: "accountID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FORMAT",
                columns: table => new
                {
                    formatID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    accountID = table.Column<int>(type: "INTEGER", nullable: false),
                    Format = table.Column<string>(type: "TEXT", nullable: false),
                    Category = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FORMAT", x => x.formatID);
                    table.ForeignKey(
                        name: "FK_FORMAT_ACCOUNT_accountID",
                        column: x => x.accountID,
                        principalTable: "ACCOUNT",
                        principalColumn: "accountID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MEDIUM",
                columns: table => new
                {
                    mediumID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    accountID = table.Column<int>(type: "INTEGER", nullable: false),
                    Medium = table.Column<string>(type: "TEXT", nullable: false),
                    Category = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MEDIUM", x => x.mediumID);
                    table.ForeignKey(
                        name: "FK_MEDIUM_ACCOUNT_accountID",
                        column: x => x.accountID,
                        principalTable: "ACCOUNT",
                        principalColumn: "accountID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OUTPUT",
                columns: table => new
                {
                    outputID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    accountID = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    appID = table.Column<int>(type: "INTEGER", nullable: false),
                    Category = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OUTPUT", x => x.outputID);
                    table.ForeignKey(
                        name: "FK_OUTPUT_ACCOUNT_accountID",
                        column: x => x.accountID,
                        principalTable: "ACCOUNT",
                        principalColumn: "accountID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OUTPUT_APPLICATION_appID",
                        column: x => x.appID,
                        principalTable: "APPLICATION",
                        principalColumn: "appID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PROJECT",
                columns: table => new
                {
                    projectID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    accountID = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    appID = table.Column<int>(type: "INTEGER", nullable: false),
                    Category = table.Column<int>(type: "INTEGER", nullable: false),
                    IsDefault = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PROJECT", x => x.projectID);
                    table.ForeignKey(
                        name: "FK_PROJECT_ACCOUNT_accountID",
                        column: x => x.accountID,
                        principalTable: "ACCOUNT",
                        principalColumn: "accountID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PROJECT_APPLICATION_appID",
                        column: x => x.appID,
                        principalTable: "APPLICATION",
                        principalColumn: "appID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TYPE",
                columns: table => new
                {
                    typeID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    accountID = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    appID = table.Column<int>(type: "INTEGER", nullable: false),
                    Category = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TYPE", x => x.typeID);
                    table.ForeignKey(
                        name: "FK_TYPE_ACCOUNT_accountID",
                        column: x => x.accountID,
                        principalTable: "ACCOUNT",
                        principalColumn: "accountID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TYPE_APPLICATION_appID",
                        column: x => x.appID,
                        principalTable: "APPLICATION",
                        principalColumn: "appID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Item",
                columns: table => new
                {
                    itemID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    logID = table.Column<int>(type: "INTEGER", nullable: false),
                    ChecklistlogID = table.Column<int>(type: "INTEGER", nullable: false),
                    accountID = table.Column<int>(type: "INTEGER", nullable: false),
                    IsChecked = table.Column<bool>(type: "INTEGER", nullable: false),
                    Item = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item", x => x.itemID);
                    table.ForeignKey(
                        name: "FK_Item_ACCOUNT_accountID",
                        column: x => x.accountID,
                        principalTable: "ACCOUNT",
                        principalColumn: "accountID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Item_CHECKLIST_ChecklistlogID",
                        column: x => x.ChecklistlogID,
                        principalTable: "CHECKLIST",
                        principalColumn: "logID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subject",
                columns: table => new
                {
                    subjectID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Category = table.Column<int>(type: "INTEGER", nullable: false),
                    accountID = table.Column<int>(type: "INTEGER", nullable: false),
                    Subject = table.Column<string>(type: "TEXT", nullable: false),
                    projectID = table.Column<int>(type: "INTEGER", nullable: false),
                    appID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subject", x => x.subjectID);
                    table.ForeignKey(
                        name: "FK_Subject_ACCOUNT_accountID",
                        column: x => x.accountID,
                        principalTable: "ACCOUNT",
                        principalColumn: "accountID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Subject_APPLICATION_appID",
                        column: x => x.appID,
                        principalTable: "APPLICATION",
                        principalColumn: "appID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Subject_PROJECT_projectID",
                        column: x => x.projectID,
                        principalTable: "PROJECT",
                        principalColumn: "projectID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LOG",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Category = table.Column<int>(type: "INTEGER", nullable: false),
                    accountID = table.Column<int>(type: "INTEGER", nullable: false),
                    projectID = table.Column<int>(type: "INTEGER", nullable: false),
                    appID = table.Column<int>(type: "INTEGER", nullable: false),
                    Start = table.Column<DateTime>(type: "TEXT", nullable: false),
                    End = table.Column<DateTime>(type: "TEXT", nullable: false),
                    outputID = table.Column<int>(type: "INTEGER", nullable: false),
                    typeID = table.Column<int>(type: "INTEGER", nullable: false),
                    Content = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOG", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LOG_ACCOUNT_accountID",
                        column: x => x.accountID,
                        principalTable: "ACCOUNT",
                        principalColumn: "accountID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LOG_APPLICATION_appID",
                        column: x => x.appID,
                        principalTable: "APPLICATION",
                        principalColumn: "appID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LOG_OUTPUT_outputID",
                        column: x => x.outputID,
                        principalTable: "OUTPUT",
                        principalColumn: "outputID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LOG_PROJECT_projectID",
                        column: x => x.projectID,
                        principalTable: "PROJECT",
                        principalColumn: "projectID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LOG_TYPE_typeID",
                        column: x => x.typeID,
                        principalTable: "TYPE",
                        principalColumn: "typeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CodingLOG",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Bugs = table.Column<int>(type: "INTEGER", nullable: false),
                    Success = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodingLOG", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CodingLOG_LOG_ID",
                        column: x => x.ID,
                        principalTable: "LOG",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FilmLOG",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Height = table.Column<double>(type: "REAL", nullable: false),
                    Width = table.Column<double>(type: "REAL", nullable: false),
                    Length = table.Column<string>(type: "TEXT", nullable: false),
                    IsCompleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    Source = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilmLOG", x => x.ID);
                    table.ForeignKey(
                        name: "FK_FilmLOG_LOG_ID",
                        column: x => x.ID,
                        principalTable: "LOG",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GraphicsLOG",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    mediumID = table.Column<int>(type: "INTEGER", nullable: false),
                    formatID = table.Column<int>(type: "INTEGER", nullable: false),
                    Brush = table.Column<string>(type: "TEXT", nullable: false),
                    Height = table.Column<double>(type: "REAL", nullable: false),
                    Width = table.Column<double>(type: "REAL", nullable: false),
                    unitID = table.Column<int>(type: "INTEGER", nullable: false),
                    Size = table.Column<string>(type: "TEXT", nullable: false),
                    DPI = table.Column<double>(type: "REAL", nullable: false),
                    Depth = table.Column<string>(type: "TEXT", nullable: false),
                    IsCompleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    Source = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GraphicsLOG", x => x.ID);
                    table.ForeignKey(
                        name: "FK_GraphicsLOG_FORMAT_formatID",
                        column: x => x.formatID,
                        principalTable: "FORMAT",
                        principalColumn: "formatID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GraphicsLOG_LOG_ID",
                        column: x => x.ID,
                        principalTable: "LOG",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GraphicsLOG_MEDIUM_mediumID",
                        column: x => x.mediumID,
                        principalTable: "MEDIUM",
                        principalColumn: "mediumID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GraphicsLOG_UNIT_unitID",
                        column: x => x.unitID,
                        principalTable: "UNIT",
                        principalColumn: "unitID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotesLOG",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotesLOG", x => x.ID);
                    table.ForeignKey(
                        name: "FK_NotesLOG_LOG_ID",
                        column: x => x.ID,
                        principalTable: "LOG",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostIt",
                columns: table => new
                {
                    postItID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    logID = table.Column<int>(type: "INTEGER", nullable: false),
                    accountID = table.Column<int>(type: "INTEGER", nullable: false),
                    subjectID = table.Column<int>(type: "INTEGER", nullable: false),
                    Error = table.Column<string>(type: "TEXT", nullable: false),
                    Solution = table.Column<string>(type: "TEXT", nullable: false),
                    Suggestion = table.Column<string>(type: "TEXT", nullable: false),
                    Comment = table.Column<string>(type: "TEXT", nullable: false),
                    ERCaptureTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SOCaptureTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostIt", x => x.postItID);
                    table.ForeignKey(
                        name: "FK_PostIt_ACCOUNT_accountID",
                        column: x => x.accountID,
                        principalTable: "ACCOUNT",
                        principalColumn: "accountID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostIt_LOG_logID",
                        column: x => x.logID,
                        principalTable: "LOG",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostIt_Subject_subjectID",
                        column: x => x.subjectID,
                        principalTable: "Subject",
                        principalColumn: "subjectID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AndroidCodingLOG",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Scope = table.Column<int>(type: "INTEGER", nullable: false),
                    Sync = table.Column<DateTime>(type: "TEXT", nullable: false),
                    StartingGradleDaemon = table.Column<DateTime>(type: "TEXT", nullable: false),
                    RunBuild = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LoadBuild = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ConfigureBuild = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AllProjects = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AndroidCodingLOG", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AndroidCodingLOG_CodingLOG_ID",
                        column: x => x.ID,
                        principalTable: "CodingLOG",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FlexiNotesLOG",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    flexinotetype = table.Column<int>(type: "INTEGER", nullable: false),
                    gamingcontext = table.Column<int>(type: "INTEGER", nullable: false),
                    mediumID = table.Column<int>(type: "INTEGER", nullable: false),
                    formatID = table.Column<int>(type: "INTEGER", nullable: false),
                    Bitrate = table.Column<int>(type: "INTEGER", nullable: false),
                    Length = table.Column<string>(type: "TEXT", nullable: false),
                    IsCompleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    Source = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlexiNotesLOG", x => x.ID);
                    table.ForeignKey(
                        name: "FK_FlexiNotesLOG_FORMAT_formatID",
                        column: x => x.formatID,
                        principalTable: "FORMAT",
                        principalColumn: "formatID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FlexiNotesLOG_MEDIUM_mediumID",
                        column: x => x.mediumID,
                        principalTable: "MEDIUM",
                        principalColumn: "mediumID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FlexiNotesLOG_NotesLOG_ID",
                        column: x => x.ID,
                        principalTable: "NotesLOG",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NoteItem",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Subject = table.Column<string>(type: "TEXT", nullable: false),
                    NoteContent = table.Column<string>(type: "TEXT", nullable: false),
                    CheckListID = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoteItem", x => x.ID);
                    table.ForeignKey(
                        name: "FK_NoteItem_CHECKLIST_CheckListID",
                        column: x => x.CheckListID,
                        principalTable: "CHECKLIST",
                        principalColumn: "logID");
                    table.ForeignKey(
                        name: "FK_NoteItem_NotesLOG_ID",
                        column: x => x.ID,
                        principalTable: "NotesLOG",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ACCOUNT",
                columns: new[] { "accountID", "CompanyAddress", "CompanyLogo", "CompanyName", "Email", "FirstName", "IsEmployee", "IsOnline", "LastName", "Password", "ProfilePic" },
                values: new object[] { 1, "", "", "", "support@datalogger.co.za", "admin", false, false, "", "pcsx2024", "" });

            migrationBuilder.InsertData(
                table: "UNIT",
                columns: new[] { "unitID", "Unit" },
                values: new object[,]
                {
                    { 1, "cm" },
                    { 2, "mm" },
                    { 3, "px" },
                    { 4, "in" },
                    { 5, "m" }
                });

            migrationBuilder.InsertData(
                table: "APPLICATION",
                columns: new[] { "appID", "Category", "IsDefault", "Name", "accountID" },
                values: new object[,]
                {
                    { 1, 0, true, "Qt Creator", 1 },
                    { 2, 0, true, "Android Studio", 1 },
                    { 3, 0, true, "Unknown", 1 },
                    { 4, 0, true, "Visual Studio Community 2022", 1 },
                    { 5, 1, true, "Krita", 1 },
                    { 6, 1, true, "Inkscape", 1 },
                    { 7, 1, true, "Canva", 1 },
                    { 8, 1, true, "Adobe Illustrator", 1 },
                    { 9, 2, true, "Da Vinci Resolve", 1 },
                    { 10, 2, true, "Blender 3D/2D", 1 },
                    { 11, 2, true, "Powerpoint", 1 },
                    { 12, 2, true, "Shotcut", 1 },
                    { 13, 3, true, "Unity", 1 },
                    { 14, 3, true, "Steam", 1 },
                    { 15, 3, true, "Data Logger NOTES", 1 },
                    { 16, 3, true, "Data Logger Checklist", 1 },
                    { 17, 3, true, "Microsoft Word", 1 },
                    { 18, 3, true, "REAPER", 1 },
                    { 19, 3, true, "Notepad", 1 },
                    { 20, 3, true, "Microsoft Excel", 1 },
                    { 21, 3, true, "Microsoft Access", 1 },
                    { 22, 0, true, "IntelliJ", 1 },
                    { 23, 0, true, "PyCharm", 1 },
                    { 24, 0, true, "WebStorm", 1 }
                });

            migrationBuilder.InsertData(
                table: "FORMAT",
                columns: new[] { "formatID", "Category", "Format", "accountID" },
                values: new object[,]
                {
                    { 1, 1, "Paper", 1 },
                    { 2, 1, "Digital Canvas", 1 },
                    { 3, 1, "Cardboard", 1 },
                    { 4, 1, "Wall/Street", 1 },
                    { 5, 3, "CD", 1 },
                    { 6, 3, "MIDI", 1 },
                    { 7, 3, "Digital", 1 },
                    { 8, 3, "Tape", 1 },
                    { 9, 3, "LP", 1 },
                    { 10, 3, "EP", 1 },
                    { 11, 3, "Gramophone", 1 },
                    { 12, 3, "Digital Download", 1 },
                    { 13, 3, "Disc", 1 }
                });

            migrationBuilder.InsertData(
                table: "MEDIUM",
                columns: new[] { "mediumID", "Category", "Medium", "accountID" },
                values: new object[,]
                {
                    { 1, 1, "Pencil", 1 },
                    { 2, 1, "Paint", 1 },
                    { 3, 1, "Pen", 1 },
                    { 4, 1, "Pencil Crayon", 1 },
                    { 5, 1, "Kokie", 1 },
                    { 6, 1, "Crayon", 1 },
                    { 7, 1, "Oil Pastel", 1 },
                    { 8, 1, "Chalk", 1 },
                    { 9, 3, "A Cappella", 1 },
                    { 10, 3, "Song", 1 },
                    { 11, 3, "Orchestral", 1 },
                    { 12, 3, "Game Engine", 1 }
                });

            migrationBuilder.InsertData(
                table: "OUTPUT",
                columns: new[] { "outputID", "Category", "Name", "accountID", "appID" },
                values: new object[,]
                {
                    { 1, 0, "Console Application", 1, 1 },
                    { 2, 0, "Widgets Application", 1, 1 },
                    { 3, 0, "QtQuick Application", 1, 1 },
                    { 4, 0, "APK (*.apk)", 1, 2 },
                    { 5, 0, "USB", 1, 2 },
                    { 6, 0, "Emulator (*.exe)", 1, 2 },
                    { 7, 0, "C# Application (*.exe)", 1, 4 },
                    { 8, 0, "C++ Application (*.exe)", 1, 3 },
                    { 9, 0, "Java Application (*.exe)", 1, 3 },
                    { 10, 0, "Database (*.db)", 1, 3 },
                    { 11, 0, "Database (SQL, Oracle)", 1, 3 },
                    { 12, 1, "PNG", 1, 5 },
                    { 13, 1, "JPG", 1, 5 },
                    { 14, 1, "SVG", 1, 6 },
                    { 15, 1, "GIF", 1, 5 },
                    { 16, 1, "PDF", 1, 7 },
                    { 17, 1, "TIFF", 1, 8 },
                    { 18, 1, "PSD", 1, 8 },
                    { 19, 1, "WEBP", 1, 8 },
                    { 20, 2, "MP4", 1, 9 },
                    { 21, 2, "AVI", 1, 9 },
                    { 22, 2, "MKV", 1, 9 },
                    { 23, 2, "TS", 1, 9 },
                    { 24, 2, "WEBM", 1, 3 },
                    { 25, 3, "PDF", 1, 17 },
                    { 26, 3, "WORD", 1, 17 },
                    { 27, 3, "TEXT FILE (*.txt)", 1, 17 },
                    { 28, 3, "HTML (*.html)", 1, 17 },
                    { 29, 3, "XPS (*.xps)", 1, 17 },
                    { 30, 3, "EXCEL FILE (*.xlsx)", 1, 20 },
                    { 31, 3, "ACCESS FILE (*.accdb)", 1, 21 },
                    { 32, 3, "MP3", 1, 3 },
                    { 33, 3, "WAV", 1, 3 },
                    { 34, 3, "AAC", 1, 3 },
                    { 35, 3, "M4A", 1, 3 },
                    { 36, 3, "OGG", 1, 3 },
                    { 37, 3, "Note", 1, 3 },
                    { 38, 3, "Check List", 1, 3 },
                    { 39, 3, "PNG", 1, 3 },
                    { 40, 3, "JPG", 1, 3 },
                    { 41, 3, "EXE (*.exe)", 1, 3 },
                    { 42, 0, "JavaScript (*.js)", 1, 3 },
                    { 43, 0, "Kotlin Application (*.kt)", 1, 3 },
                    { 44, 0, "Python Application (*.py)", 1, 3 },
                    { 45, 0, "React App (*.jsx | *.js)", 1, 3 },
                    { 46, 0, "PHP (*.php)", 1, 3 },
                    { 47, 0, "CSS (*.css)", 1, 3 },
                    { 48, 0, "Tailwind CSS", 1, 3 }
                });

            migrationBuilder.InsertData(
                table: "PROJECT",
                columns: new[] { "projectID", "Category", "IsDefault", "Name", "accountID", "appID" },
                values: new object[] { 1, 0, true, "Unknown", 1, 3 });

            migrationBuilder.InsertData(
                table: "TYPE",
                columns: new[] { "typeID", "Category", "Name", "accountID", "appID" },
                values: new object[,]
                {
                    { 1, 0, "NONE", 1, 3 },
                    { 2, 0, "Build", 1, 3 },
                    { 3, 0, "Runtime", 1, 3 },
                    { 4, 0, "Sync", 1, 3 },
                    { 5, 0, "Build", 1, 3 },
                    { 6, 0, "Runtime", 1, 3 },
                    { 7, 0, "Compilation", 1, 3 },
                    { 8, 0, "Runtime", 1, 3 },
                    { 9, 1, "NONE", 1, 3 },
                    { 10, 1, "Artwork", 1, 3 },
                    { 11, 1, "Doodle", 1, 3 },
                    { 12, 1, "Graphic Design", 1, 3 },
                    { 13, 1, "Resource", 1, 3 },
                    { 14, 1, "Portfolio", 1, 3 },
                    { 15, 2, "NONE", 1, 3 },
                    { 16, 2, "Film", 1, 3 },
                    { 17, 2, "Doodle", 1, 3 },
                    { 18, 2, "Assignment", 1, 3 },
                    { 19, 3, "Music", 1, 3 },
                    { 20, 3, "Poetry", 1, 3 },
                    { 21, 3, "Mantra", 1, 3 },
                    { 22, 3, "Boardgame", 1, 3 },
                    { 23, 3, "Video game", 1, 3 },
                    { 24, 3, "Card game", 1, 3 },
                    { 25, 3, "Game - Design ONLY", 1, 3 },
                    { 26, 3, "Document", 1, 3 },
                    { 27, 3, "Invitation", 1, 3 },
                    { 28, 3, "Mail", 1, 3 },
                    { 29, 3, "Report", 1, 3 },
                    { 30, 3, "Doodle", 1, 3 },
                    { 31, 3, "Novel", 1, 3 },
                    { 32, 3, "Questionnaire", 1, 3 },
                    { 33, 3, "CV", 1, 3 },
                    { 34, 3, "Web page", 1, 3 },
                    { 35, 3, "Presentation", 1, 3 },
                    { 36, 3, "Spreadsheet", 1, 3 },
                    { 37, 3, "Notes", 1, 3 },
                    { 38, 3, "Access Database", 1, 3 },
                    { 39, 3, "Note", 1, 3 },
                    { 40, 3, "Check List", 1, 3 },
                    { 41, 0, "Exception", 1, 3 }
                });

            migrationBuilder.InsertData(
                table: "Subject",
                columns: new[] { "subjectID", "Category", "Subject", "accountID", "appID", "projectID" },
                values: new object[] { 1, 0, "No Subject", 1, 3, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_APPLICATION_accountID",
                table: "APPLICATION",
                column: "accountID");

            migrationBuilder.CreateIndex(
                name: "IX_CHECKLIST_accountID",
                table: "CHECKLIST",
                column: "accountID");

            migrationBuilder.CreateIndex(
                name: "IX_FEEDBACK_accountID",
                table: "FEEDBACK",
                column: "accountID");

            migrationBuilder.CreateIndex(
                name: "IX_FlexiNotesLOG_formatID",
                table: "FlexiNotesLOG",
                column: "formatID");

            migrationBuilder.CreateIndex(
                name: "IX_FlexiNotesLOG_mediumID",
                table: "FlexiNotesLOG",
                column: "mediumID");

            migrationBuilder.CreateIndex(
                name: "IX_FORMAT_accountID",
                table: "FORMAT",
                column: "accountID");

            migrationBuilder.CreateIndex(
                name: "IX_GraphicsLOG_formatID",
                table: "GraphicsLOG",
                column: "formatID");

            migrationBuilder.CreateIndex(
                name: "IX_GraphicsLOG_mediumID",
                table: "GraphicsLOG",
                column: "mediumID");

            migrationBuilder.CreateIndex(
                name: "IX_GraphicsLOG_unitID",
                table: "GraphicsLOG",
                column: "unitID");

            migrationBuilder.CreateIndex(
                name: "IX_Item_accountID",
                table: "Item",
                column: "accountID");

            migrationBuilder.CreateIndex(
                name: "IX_Item_ChecklistlogID",
                table: "Item",
                column: "ChecklistlogID");

            migrationBuilder.CreateIndex(
                name: "IX_LOG_accountID",
                table: "LOG",
                column: "accountID");

            migrationBuilder.CreateIndex(
                name: "IX_LOG_appID",
                table: "LOG",
                column: "appID");

            migrationBuilder.CreateIndex(
                name: "IX_LOG_outputID",
                table: "LOG",
                column: "outputID");

            migrationBuilder.CreateIndex(
                name: "IX_LOG_projectID",
                table: "LOG",
                column: "projectID");

            migrationBuilder.CreateIndex(
                name: "IX_LOG_typeID",
                table: "LOG",
                column: "typeID");

            migrationBuilder.CreateIndex(
                name: "IX_MEDIUM_accountID",
                table: "MEDIUM",
                column: "accountID");

            migrationBuilder.CreateIndex(
                name: "IX_NoteItem_CheckListID",
                table: "NoteItem",
                column: "CheckListID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OUTPUT_accountID",
                table: "OUTPUT",
                column: "accountID");

            migrationBuilder.CreateIndex(
                name: "IX_OUTPUT_appID",
                table: "OUTPUT",
                column: "appID");

            migrationBuilder.CreateIndex(
                name: "IX_PostIt_accountID",
                table: "PostIt",
                column: "accountID");

            migrationBuilder.CreateIndex(
                name: "IX_PostIt_logID",
                table: "PostIt",
                column: "logID");

            migrationBuilder.CreateIndex(
                name: "IX_PostIt_subjectID",
                table: "PostIt",
                column: "subjectID");

            migrationBuilder.CreateIndex(
                name: "IX_PROJECT_accountID",
                table: "PROJECT",
                column: "accountID");

            migrationBuilder.CreateIndex(
                name: "IX_PROJECT_appID",
                table: "PROJECT",
                column: "appID");

            migrationBuilder.CreateIndex(
                name: "IX_Subject_accountID",
                table: "Subject",
                column: "accountID");

            migrationBuilder.CreateIndex(
                name: "IX_Subject_appID",
                table: "Subject",
                column: "appID");

            migrationBuilder.CreateIndex(
                name: "IX_Subject_projectID",
                table: "Subject",
                column: "projectID");

            migrationBuilder.CreateIndex(
                name: "IX_TYPE_accountID",
                table: "TYPE",
                column: "accountID");

            migrationBuilder.CreateIndex(
                name: "IX_TYPE_appID",
                table: "TYPE",
                column: "appID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AndroidCodingLOG");

            migrationBuilder.DropTable(
                name: "FEEDBACK");

            migrationBuilder.DropTable(
                name: "FilmLOG");

            migrationBuilder.DropTable(
                name: "FlexiNotesLOG");

            migrationBuilder.DropTable(
                name: "GraphicsLOG");

            migrationBuilder.DropTable(
                name: "Item");

            migrationBuilder.DropTable(
                name: "NoteItem");

            migrationBuilder.DropTable(
                name: "PostIt");

            migrationBuilder.DropTable(
                name: "CodingLOG");

            migrationBuilder.DropTable(
                name: "FORMAT");

            migrationBuilder.DropTable(
                name: "MEDIUM");

            migrationBuilder.DropTable(
                name: "UNIT");

            migrationBuilder.DropTable(
                name: "CHECKLIST");

            migrationBuilder.DropTable(
                name: "NotesLOG");

            migrationBuilder.DropTable(
                name: "Subject");

            migrationBuilder.DropTable(
                name: "LOG");

            migrationBuilder.DropTable(
                name: "OUTPUT");

            migrationBuilder.DropTable(
                name: "PROJECT");

            migrationBuilder.DropTable(
                name: "TYPE");

            migrationBuilder.DropTable(
                name: "APPLICATION");

            migrationBuilder.DropTable(
                name: "ACCOUNT");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CESAPSCOREWEBAPP.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "appEtcs",
                columns: table => new
                {
                    AppEtcId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppEtcName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_appEtcs", x => x.AppEtcId);
                });

            migrationBuilder.CreateTable(
                name: "appResults",
                columns: table => new
                {
                    AppResultId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppResultName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_appResults", x => x.AppResultId);
                });

            migrationBuilder.CreateTable(
                name: "appRooms",
                columns: table => new
                {
                    AppRoomId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppRoomName = table.Column<string>(nullable: true),
                    AppRoomColor = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_appRooms", x => x.AppRoomId);
                });

            migrationBuilder.CreateTable(
                name: "appStatuses",
                columns: table => new
                {
                    AppStatusId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppStatusName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_appStatuses", x => x.AppStatusId);
                });

            migrationBuilder.CreateTable(
                name: "appSuccesses",
                columns: table => new
                {
                    AppSuccessId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppSuccessName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_appSuccesses", x => x.AppSuccessId);
                });

            migrationBuilder.CreateTable(
                name: "appTelTypes",
                columns: table => new
                {
                    AppTelTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppTelTypeName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_appTelTypes", x => x.AppTelTypeId);
                });

            migrationBuilder.CreateTable(
                name: "BankRatios",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankName = table.Column<string>(nullable: true),
                    BankCode = table.Column<string>(nullable: true),
                    IssueQty = table.Column<int>(nullable: false),
                    CreateBy = table.Column<string>(nullable: true),
                    EditBy = table.Column<string>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    EditDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankRatios", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "BlogCats",
                columns: table => new
                {
                    BlogCatId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BlogCatName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogCats", x => x.BlogCatId);
                });

            migrationBuilder.CreateTable(
                name: "BlogFiles",
                columns: table => new
                {
                    BlogFileId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BlogFileName = table.Column<string>(nullable: true),
                    BlogFileType = table.Column<string>(nullable: true),
                    BlogId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogFiles", x => x.BlogFileId);
                });

            migrationBuilder.CreateTable(
                name: "BlogPics",
                columns: table => new
                {
                    BlogPicId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BlogPicName = table.Column<string>(nullable: true),
                    BlogId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogPics", x => x.BlogPicId);
                });

            migrationBuilder.CreateTable(
                name: "Bloods",
                columns: table => new
                {
                    BloodId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BloodName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bloods", x => x.BloodId);
                });

            migrationBuilder.CreateTable(
                name: "Branchs",
                columns: table => new
                {
                    BranchId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branchs", x => x.BranchId);
                });

            migrationBuilder.CreateTable(
                name: "caseBPCs",
                columns: table => new
                {
                    CaseBPCId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CaseBPCDate = table.Column<DateTime>(nullable: false),
                    CaseMA = table.Column<string>(nullable: true),
                    CaseBPCSubject = table.Column<string>(nullable: true),
                    CaseBPCDetail = table.Column<string>(nullable: true),
                    caseBPCStatus = table.Column<int>(nullable: false),
                    caseBPCPLevel = table.Column<int>(nullable: false),
                    CaseBPCPDateFix = table.Column<DateTime>(nullable: true),
                    EditBy = table.Column<string>(nullable: true),
                    openCaseBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_caseBPCs", x => x.CaseBPCId);
                });

            migrationBuilder.CreateTable(
                name: "CheckInOuts",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Data1 = table.Column<string>(nullable: true),
                    Data2 = table.Column<string>(nullable: true),
                    Site = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckInOuts", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CheckRowInOuts",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Site = table.Column<string>(nullable: true),
                    LastRow = table.Column<int>(nullable: false),
                    UpdateDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckRowInOuts", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CheckUsers",
                columns: table => new
                {
                    CheckUserId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CheckUserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckUsers", x => x.CheckUserId);
                });

            migrationBuilder.CreateTable(
                name: "CHQloses",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankCode = table.Column<string>(nullable: true),
                    BankName = table.Column<string>(nullable: true),
                    PostingDate = table.Column<DateTime>(nullable: false),
                    CHQNo = table.Column<string>(nullable: true),
                    Etc = table.Column<string>(nullable: true),
                    CreateBy = table.Column<string>(nullable: true),
                    EditBy = table.Column<string>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    EditDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CHQloses", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ConditionReports",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportName = table.Column<string>(nullable: true),
                    ReportDimension = table.Column<string>(nullable: true),
                    ReportValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConditionReports", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "DataXXYs",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: false),
                    CountData = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataXXYs", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "dataXY",
                columns: table => new
                {
                    X = table.Column<string>(nullable: false),
                    Y = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dataXY", x => x.X);
                });

            migrationBuilder.CreateTable(
                name: "Department1s",
                columns: table => new
                {
                    Department1Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Department1Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department1s", x => x.Department1Id);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    DepartmentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.DepartmentId);
                });

            migrationBuilder.CreateTable(
                name: "DetailEmpHouseReports",
                columns: table => new
                {
                    Code = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    PositionName = table.Column<string>(nullable: true),
                    Site = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetailEmpHouseReports", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "DetailTableERPs",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TableID = table.Column<int>(nullable: false),
                    TableName = table.Column<string>(nullable: true),
                    FieldID = table.Column<int>(nullable: false),
                    FieldName = table.Column<string>(nullable: true),
                    Detail = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetailTableERPs", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "DocumentBillings",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostingDate = table.Column<DateTime>(nullable: false),
                    PONo = table.Column<string>(nullable: true),
                    Site = table.Column<string>(nullable: true),
                    VendorName = table.Column<string>(nullable: true),
                    InvoiceNo = table.Column<string>(nullable: true),
                    DeliveryOrder = table.Column<string>(nullable: true),
                    Etc = table.Column<string>(nullable: true),
                    CreateBy = table.Column<string>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    UpdateBy = table.Column<string>(nullable: true),
                    UpdateDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentBillings", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "DocumentReceivables",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostingDate = table.Column<DateTime>(nullable: false),
                    DocNo = table.Column<string>(nullable: true),
                    JobNo = table.Column<string>(nullable: true),
                    Detail = table.Column<string>(nullable: true),
                    Outstanding = table.Column<decimal>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    TotalNAV = table.Column<decimal>(nullable: false),
                    Statuss = table.Column<int>(nullable: false),
                    CreateBy = table.Column<string>(nullable: true),
                    EditBy = table.Column<string>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    EditDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentReceivables", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Estimates",
                columns: table => new
                {
                    EstimateId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EstimateJob = table.Column<string>(nullable: true),
                    EstimateStart = table.Column<string>(nullable: true),
                    EstimateEnd = table.Column<string>(nullable: true),
                    DateCreate = table.Column<DateTime>(nullable: false),
                    EstimateBy = table.Column<string>(nullable: true),
                    Contractwork = table.Column<decimal>(nullable: false),
                    ExtraWork = table.Column<decimal>(nullable: false),
                    OtherIncome = table.Column<decimal>(nullable: false),
                    AECT = table.Column<decimal>(nullable: false),
                    UECT = table.Column<decimal>(nullable: false),
                    EC = table.Column<decimal>(nullable: false),
                    AICT = table.Column<decimal>(nullable: false),
                    NetProfit = table.Column<decimal>(nullable: false),
                    defalse = table.Column<int>(nullable: false),
                    logcation = table.Column<string>(nullable: true),
                    a1 = table.Column<string>(nullable: true),
                    a2 = table.Column<string>(nullable: true),
                    a3 = table.Column<string>(nullable: true),
                    a4 = table.Column<string>(nullable: true),
                    a5 = table.Column<string>(nullable: true),
                    a6 = table.Column<string>(nullable: true),
                    a7 = table.Column<string>(nullable: true),
                    a8 = table.Column<string>(nullable: true),
                    a9 = table.Column<string>(nullable: true),
                    a10 = table.Column<string>(nullable: true),
                    a11 = table.Column<string>(nullable: true),
                    a12 = table.Column<string>(nullable: true),
                    a13 = table.Column<string>(nullable: true),
                    a14 = table.Column<string>(nullable: true),
                    a15 = table.Column<string>(nullable: true),
                    a16 = table.Column<string>(nullable: true),
                    a17 = table.Column<string>(nullable: true),
                    b1 = table.Column<string>(nullable: true),
                    b2 = table.Column<string>(nullable: true),
                    b3 = table.Column<string>(nullable: true),
                    b4 = table.Column<string>(nullable: true),
                    c1 = table.Column<string>(nullable: true),
                    c2 = table.Column<string>(nullable: true),
                    c3 = table.Column<string>(nullable: true),
                    c4 = table.Column<string>(nullable: true),
                    c5 = table.Column<string>(nullable: true),
                    c6 = table.Column<string>(nullable: true),
                    c7 = table.Column<string>(nullable: true),
                    c8 = table.Column<string>(nullable: true),
                    c9 = table.Column<string>(nullable: true),
                    c10 = table.Column<string>(nullable: true),
                    c11 = table.Column<string>(nullable: true),
                    c12 = table.Column<string>(nullable: true),
                    c13 = table.Column<string>(nullable: true),
                    c14 = table.Column<string>(nullable: true),
                    c15 = table.Column<string>(nullable: true),
                    c16 = table.Column<string>(nullable: true),
                    c17 = table.Column<string>(nullable: true),
                    c18 = table.Column<string>(nullable: true),
                    c19 = table.Column<string>(nullable: true),
                    c20 = table.Column<string>(nullable: true),
                    c21 = table.Column<string>(nullable: true),
                    c22 = table.Column<string>(nullable: true),
                    c23 = table.Column<string>(nullable: true),
                    c24 = table.Column<string>(nullable: true),
                    c25 = table.Column<string>(nullable: true),
                    c26 = table.Column<string>(nullable: true),
                    c27 = table.Column<string>(nullable: true),
                    c28 = table.Column<string>(nullable: true),
                    c29 = table.Column<string>(nullable: true),
                    c30 = table.Column<string>(nullable: true),
                    c31 = table.Column<string>(nullable: true),
                    c32 = table.Column<string>(nullable: true),
                    c33 = table.Column<string>(nullable: true),
                    c34 = table.Column<string>(nullable: true),
                    c35 = table.Column<string>(nullable: true),
                    c36 = table.Column<string>(nullable: true),
                    c37 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estimates", x => x.EstimateId);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Subject = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Start = table.Column<DateTime>(nullable: false),
                    End = table.Column<DateTime>(nullable: false),
                    ThemeColor = table.Column<string>(nullable: true),
                    IsFullDay = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Faculties",
                columns: table => new
                {
                    FacultyId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FacultyName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faculties", x => x.FacultyId);
                });

            migrationBuilder.CreateTable(
                name: "FileManals",
                columns: table => new
                {
                    FileManalId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileManalName = table.Column<string>(nullable: true),
                    FileManalType = table.Column<string>(nullable: true),
                    ManualId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileManals", x => x.FileManalId);
                });

            migrationBuilder.CreateTable(
                name: "fixAccessDataTmps",
                columns: table => new
                {
                    FixAssId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FixAccNo = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Description2 = table.Column<string>(nullable: true),
                    FALocation = table.Column<string>(nullable: true),
                    RefPC = table.Column<string>(nullable: true),
                    RefPCDetail = table.Column<string>(nullable: true),
                    FAQty = table.Column<decimal>(nullable: false),
                    FATransfer = table.Column<decimal>(nullable: false),
                    LastModifi = table.Column<DateTime>(nullable: true),
                    ActionData = table.Column<string>(nullable: true),
                    FixAssetType = table.Column<string>(nullable: true),
                    FixAssetDetail = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fixAccessDataTmps", x => x.FixAssId);
                });

            migrationBuilder.CreateTable(
                name: "FixAssetStraightAlls",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FANO = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    Quantity = table.Column<decimal>(nullable: false),
                    PriceEnd = table.Column<decimal>(nullable: false),
                    Sale = table.Column<int>(nullable: false),
                    FAPostingGroup = table.Column<string>(nullable: true),
                    SaleDate = table.Column<DateTime>(nullable: true),
                    Percen = table.Column<decimal>(nullable: false),
                    Life = table.Column<decimal>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    DateInMount = table.Column<DateTime>(nullable: false),
                    StraightLine = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FixAssetStraightAlls", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "FixAssetStraightLines",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FANO = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    Quantity = table.Column<decimal>(nullable: false),
                    PriceEnd = table.Column<decimal>(nullable: false),
                    Sale = table.Column<int>(nullable: false),
                    FAPostingGroup = table.Column<string>(nullable: true),
                    SaleDate = table.Column<DateTime>(nullable: true),
                    Percen = table.Column<decimal>(nullable: false),
                    Life = table.Column<decimal>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    StraightLine = table.Column<decimal>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FixAssetStraightLines", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "GroupJobs",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentNo = table.Column<string>(nullable: true),
                    JobNo = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    PostingDate = table.Column<string>(nullable: true),
                    Amount = table.Column<decimal>(nullable: false),
                    GroupNo = table.Column<string>(nullable: true),
                    GroupName = table.Column<string>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    CreateBy = table.Column<string>(nullable: true),
                    EditDate = table.Column<DateTime>(nullable: true),
                    EditBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupJobs", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HouseRentalFileMonths",
                columns: table => new
                {
                    FileMonthID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileMonthName = table.Column<string>(nullable: true),
                    FileMonthDate = table.Column<DateTime>(nullable: false),
                    FileMonthType = table.Column<string>(nullable: true),
                    Period = table.Column<string>(nullable: true),
                    Job = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HouseRentalFileMonths", x => x.FileMonthID);
                });

            migrationBuilder.CreateTable(
                name: "HouseRentalFiles",
                columns: table => new
                {
                    HouseRentalFileID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HouseRentalFileName = table.Column<string>(nullable: true),
                    HouseRentalFileType = table.Column<string>(nullable: true),
                    ID = table.Column<int>(nullable: false),
                    username = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HouseRentalFiles", x => x.HouseRentalFileID);
                });

            migrationBuilder.CreateTable(
                name: "HouseRentals",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmpId = table.Column<string>(nullable: true),
                    EmpName = table.Column<string>(nullable: true),
                    EmpPosition = table.Column<string>(nullable: true),
                    PostingDate = table.Column<DateTime>(nullable: false),
                    Site = table.Column<string>(nullable: true),
                    Deposit = table.Column<decimal>(nullable: false),
                    DepositText = table.Column<string>(nullable: true),
                    Advanced = table.Column<decimal>(nullable: false),
                    AdvancedText = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(nullable: false),
                    Thaibath = table.Column<string>(nullable: true),
                    Etc = table.Column<string>(nullable: true),
                    HouseName = table.Column<string>(nullable: true),
                    RoomNumber = table.Column<string>(nullable: true),
                    CreateBy = table.Column<string>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    UpdateBy = table.Column<string>(nullable: true),
                    UpdateDate = table.Column<DateTime>(nullable: true),
                    Statuss = table.Column<int>(nullable: false),
                    TypeRooms = table.Column<int>(nullable: false),
                    Period = table.Column<string>(nullable: true),
                    Paymentdate = table.Column<string>(nullable: true),
                    RoomPrice = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HouseRentals", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HRRecruiteGroups",
                columns: table => new
                {
                    HRRecruiteGroupId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HRRecruiteGroupDetail = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HRRecruiteGroups", x => x.HRRecruiteGroupId);
                });

            migrationBuilder.CreateTable(
                name: "HRRecruiteStatuses",
                columns: table => new
                {
                    HRRecruiteStatusId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HRRecruiteStatusName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HRRecruiteStatuses", x => x.HRRecruiteStatusId);
                });

            migrationBuilder.CreateTable(
                name: "InventoryCHQs",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankCode = table.Column<string>(nullable: true),
                    BankName = table.Column<string>(nullable: true),
                    PostingDate = table.Column<DateTime>(nullable: false),
                    Qty = table.Column<int>(nullable: false),
                    Etc = table.Column<string>(nullable: true),
                    CreateBy = table.Column<string>(nullable: true),
                    StartNo = table.Column<string>(nullable: true),
                    EndNo = table.Column<string>(nullable: true),
                    EditBy = table.Column<string>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    EditDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryCHQs", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceExcels",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EtcTax = table.Column<string>(nullable: true),
                    CusNo = table.Column<string>(nullable: true),
                    Site = table.Column<string>(nullable: true),
                    PostingDate = table.Column<DateTime>(nullable: false),
                    CusName = table.Column<string>(nullable: true),
                    InvoiceId = table.Column<string>(nullable: true),
                    Detail = table.Column<string>(nullable: true),
                    Period = table.Column<string>(nullable: true),
                    TotalExcVat = table.Column<decimal>(nullable: false),
                    TotalCut = table.Column<decimal>(nullable: false),
                    RetentionTotal = table.Column<decimal>(nullable: false),
                    AdvanceTotal = table.Column<decimal>(nullable: false),
                    TotalConstruction = table.Column<decimal>(nullable: false),
                    IncomeExtra = table.Column<decimal>(nullable: false),
                    IncomeMaterial = table.Column<decimal>(nullable: false),
                    IncomeEtc = table.Column<decimal>(nullable: false),
                    IncomeAdvanceExtra = table.Column<decimal>(nullable: false),
                    Vat7 = table.Column<decimal>(nullable: false),
                    CustInVat = table.Column<decimal>(nullable: false),
                    Vat3 = table.Column<decimal>(nullable: false),
                    TotalIncome = table.Column<decimal>(nullable: false),
                    DueDate = table.Column<DateTime>(nullable: true),
                    Ref = table.Column<string>(nullable: true),
                    IncomeTotalBank = table.Column<decimal>(nullable: false),
                    Bankname = table.Column<string>(nullable: true),
                    ChqNo = table.Column<string>(nullable: true),
                    InvoiceTotal = table.Column<decimal>(nullable: false),
                    PayTotal = table.Column<decimal>(nullable: false),
                    BankTotal = table.Column<decimal>(nullable: false),
                    TypeOfPay = table.Column<string>(nullable: true),
                    ImportDate = table.Column<DateTime>(nullable: false),
                    ImportBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceExcels", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceExcelTMPs",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EtcTax = table.Column<string>(nullable: true),
                    CusNo = table.Column<string>(nullable: true),
                    Site = table.Column<string>(nullable: true),
                    PostingDate = table.Column<DateTime>(nullable: false),
                    CusName = table.Column<string>(nullable: true),
                    InvoiceId = table.Column<string>(nullable: true),
                    Detail = table.Column<string>(nullable: true),
                    Period = table.Column<string>(nullable: true),
                    TotalExcVat = table.Column<decimal>(nullable: false),
                    TotalCut = table.Column<decimal>(nullable: false),
                    RetentionTotal = table.Column<decimal>(nullable: false),
                    AdvanceTotal = table.Column<decimal>(nullable: false),
                    TotalConstruction = table.Column<decimal>(nullable: false),
                    IncomeExtra = table.Column<decimal>(nullable: false),
                    IncomeMaterial = table.Column<decimal>(nullable: false),
                    IncomeEtc = table.Column<decimal>(nullable: false),
                    IncomeAdvanceExtra = table.Column<decimal>(nullable: false),
                    Vat7 = table.Column<decimal>(nullable: false),
                    CustInVat = table.Column<decimal>(nullable: false),
                    Vat3 = table.Column<decimal>(nullable: false),
                    TotalIncome = table.Column<decimal>(nullable: false),
                    DueDate = table.Column<DateTime>(nullable: true),
                    Ref = table.Column<string>(nullable: true),
                    IncomeTotalBank = table.Column<decimal>(nullable: false),
                    Bankname = table.Column<string>(nullable: true),
                    ChqNo = table.Column<string>(nullable: true),
                    InvoiceTotal = table.Column<decimal>(nullable: false),
                    PayTotal = table.Column<decimal>(nullable: false),
                    BankTotal = table.Column<decimal>(nullable: false),
                    TypeOfPay = table.Column<string>(nullable: true),
                    ImportDate = table.Column<DateTime>(nullable: false),
                    ImportBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceExcelTMPs", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Levels",
                columns: table => new
                {
                    LevelId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LevelName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Levels", x => x.LevelId);
                });

            migrationBuilder.CreateTable(
                name: "LineAPIs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LineToken = table.Column<string>(nullable: true),
                    Detail = table.Column<string>(nullable: true),
                    onOff = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LineAPIs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ListMannualCats",
                columns: table => new
                {
                    Namelist = table.Column<string>(nullable: false),
                    Detail = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListMannualCats", x => x.Namelist);
                });

            migrationBuilder.CreateTable(
                name: "Majors",
                columns: table => new
                {
                    MajorId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MajorName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Majors", x => x.MajorId);
                });

            migrationBuilder.CreateTable(
                name: "ManualCats",
                columns: table => new
                {
                    ManualCatId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ManualCatName = table.Column<string>(nullable: true),
                    ManualCatDate = table.Column<DateTime>(nullable: false),
                    ManualCatEditDate = table.Column<DateTime>(nullable: true),
                    ManualCatUser = table.Column<string>(nullable: true),
                    ManualCatUserEdit = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManualCats", x => x.ManualCatId);
                });

            migrationBuilder.CreateTable(
                name: "MapSites",
                columns: table => new
                {
                    MapSiteId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SiteName = table.Column<string>(nullable: true),
                    SiteAddress = table.Column<string>(nullable: true),
                    Latitude = table.Column<double>(nullable: false),
                    Longitude = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapSites", x => x.MapSiteId);
                });

            migrationBuilder.CreateTable(
                name: "Monitors",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(nullable: false),
                    IP = table.Column<string>(nullable: true),
                    Mac = table.Column<string>(nullable: true),
                    OS = table.Column<string>(nullable: true),
                    OSVersion = table.Column<string>(nullable: true),
                    Browser = table.Column<string>(nullable: true),
                    BrowserVersion = table.Column<string>(nullable: true),
                    Latitude = table.Column<string>(nullable: true),
                    Longitude = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Monitors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "nationalities",
                columns: table => new
                {
                    NationalityId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NationalityName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_nationalities", x => x.NationalityId);
                });

            migrationBuilder.CreateTable(
                name: "Permisions",
                columns: table => new
                {
                    PermisionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PermisionName = table.Column<string>(nullable: true),
                    PermisionAction = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permisions", x => x.PermisionId);
                });

            migrationBuilder.CreateTable(
                name: "PictureManuals",
                columns: table => new
                {
                    PictureManualId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PictureManualName = table.Column<string>(nullable: true),
                    ManualId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PictureManuals", x => x.PictureManualId);
                });

            migrationBuilder.CreateTable(
                name: "Positions",
                columns: table => new
                {
                    PositionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PositionName = table.Column<string>(nullable: true),
                    PositionPower = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Positions", x => x.PositionId);
                });

            migrationBuilder.CreateTable(
                name: "povinces",
                columns: table => new
                {
                    PovinceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PovinceName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_povinces", x => x.PovinceId);
                });

            migrationBuilder.CreateTable(
                name: "Rates",
                columns: table => new
                {
                    RateId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RateScore = table.Column<decimal>(nullable: false),
                    RateDate = table.Column<DateTime>(nullable: false),
                    RateUser = table.Column<string>(nullable: true),
                    ManualId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rates", x => x.RateId);
                });

            migrationBuilder.CreateTable(
                name: "RegisGroupJobs",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupCode = table.Column<string>(nullable: true),
                    GroupName = table.Column<string>(nullable: true),
                    GroupDes = table.Column<string>(nullable: true),
                    RegisBy = table.Column<string>(nullable: true),
                    RegisDate = table.Column<DateTime>(nullable: false),
                    GroupSite = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegisGroupJobs", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "religions",
                columns: table => new
                {
                    ReligionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReligionName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_religions", x => x.ReligionId);
                });

            migrationBuilder.CreateTable(
                name: "ReportAges",
                columns: table => new
                {
                    Range = table.Column<string>(nullable: false),
                    CountAge = table.Column<int>(nullable: false),
                    progressbarAge = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportAges", x => x.Range);
                });

            migrationBuilder.CreateTable(
                name: "ReportDeparment1s",
                columns: table => new
                {
                    HeadDepartment1 = table.Column<string>(nullable: false),
                    CountDepartment1 = table.Column<int>(nullable: false),
                    TotalEmployee1 = table.Column<int>(nullable: false),
                    ProgressbarDepartment1 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportDeparment1s", x => x.HeadDepartment1);
                });

            migrationBuilder.CreateTable(
                name: "ReportDeparments",
                columns: table => new
                {
                    HeadDepartment = table.Column<string>(nullable: false),
                    CountDepartment = table.Column<int>(nullable: false),
                    TotalEmployee = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportDeparments", x => x.HeadDepartment);
                });

            migrationBuilder.CreateTable(
                name: "ReportGens",
                columns: table => new
                {
                    Head = table.Column<string>(nullable: false),
                    Etc = table.Column<string>(nullable: true),
                    CountGen = table.Column<int>(nullable: false),
                    Progressbar = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportGens", x => x.Head);
                });

            migrationBuilder.CreateTable(
                name: "ReportLevels",
                columns: table => new
                {
                    HeadLevel = table.Column<string>(nullable: false),
                    CountLevels = table.Column<int>(nullable: false),
                    progressbarLevel = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportLevels", x => x.HeadLevel);
                });

            migrationBuilder.CreateTable(
                name: "ReportManagements",
                columns: table => new
                {
                    LevelHead = table.Column<string>(nullable: false),
                    CountMnagement = table.Column<int>(nullable: false),
                    ProgressbarManagement = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportManagements", x => x.LevelHead);
                });

            migrationBuilder.CreateTable(
                name: "reportManPowers",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Department = table.Column<string>(nullable: true),
                    Department1 = table.Column<string>(nullable: true),
                    PositionData = table.Column<string>(nullable: true),
                    CountData = table.Column<int>(nullable: false),
                    Power = table.Column<int>(nullable: false),
                    Diff = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reportManPowers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Retentions",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostingDate = table.Column<DateTime>(nullable: false),
                    DocNo = table.Column<string>(nullable: true),
                    JobNo = table.Column<string>(nullable: true),
                    Detail = table.Column<string>(nullable: true),
                    Amount = table.Column<decimal>(nullable: false),
                    Group = table.Column<string>(nullable: true),
                    Statuss = table.Column<int>(nullable: false),
                    CreateBy = table.Column<string>(nullable: true),
                    EditBy = table.Column<string>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    EditDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Retentions", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SourceAutoCompletes",
                columns: table => new
                {
                    name = table.Column<string>(nullable: false),
                    code = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SourceAutoCompletes", x => x.name);
                });

            migrationBuilder.CreateTable(
                name: "statusUsers",
                columns: table => new
                {
                    StatusUserId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusUserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_statusUsers", x => x.StatusUserId);
                });

            migrationBuilder.CreateTable(
                name: "TitleOfUsers",
                columns: table => new
                {
                    TitleOfUserId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TitleOfUserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TitleOfUsers", x => x.TitleOfUserId);
                });

            migrationBuilder.CreateTable(
                name: "tranSectionFixAsset",
                columns: table => new
                {
                    TranSectionFixAssetId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FixAccNo = table.Column<string>(nullable: true),
                    FixAssetQty = table.Column<decimal>(nullable: false),
                    site = table.Column<string>(nullable: true),
                    TransectionDate = table.Column<DateTime>(nullable: false),
                    ActionData = table.Column<string>(nullable: true),
                    FixAssetItem = table.Column<string>(nullable: true),
                    FixAssetItem2 = table.Column<string>(nullable: true),
                    TransectionType = table.Column<string>(nullable: true),
                    TransectionBy = table.Column<string>(nullable: true),
                    TransectionEtc = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tranSectionFixAsset", x => x.TranSectionFixAssetId);
                });

            migrationBuilder.CreateTable(
                name: "TypeCongrates",
                columns: table => new
                {
                    TypeCongrateId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeCongrateName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeCongrates", x => x.TypeCongrateId);
                });

            migrationBuilder.CreateTable(
                name: "typeOfEmployee",
                columns: table => new
                {
                    TypeOfEmployeeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeOfEmployeeName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_typeOfEmployee", x => x.TypeOfEmployeeId);
                });

            migrationBuilder.CreateTable(
                name: "TypeOfResigns",
                columns: table => new
                {
                    TypeOfResignId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeOfResignName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeOfResigns", x => x.TypeOfResignId);
                });

            migrationBuilder.CreateTable(
                name: "TypeOfSalaries",
                columns: table => new
                {
                    TypeOfSalaryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeOfSalaryName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeOfSalaries", x => x.TypeOfSalaryId);
                });

            migrationBuilder.CreateTable(
                name: "TypeOfUsers",
                columns: table => new
                {
                    TypeOfUserId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeOfUserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeOfUsers", x => x.TypeOfUserId);
                });

            migrationBuilder.CreateTable(
                name: "Universities",
                columns: table => new
                {
                    UniversityId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UniversiryName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Universities", x => x.UniversityId);
                });

            migrationBuilder.CreateTable(
                name: "UserJobs",
                columns: table => new
                {
                    UserJobId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    UserJobDetail = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserJobs", x => x.UserJobId);
                });

            migrationBuilder.CreateTable(
                name: "v_RecruiteReports",
                columns: table => new
                {
                    RowNumber = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentName = table.Column<string>(nullable: true),
                    Department1Name = table.Column<string>(nullable: true),
                    PositionName = table.Column<string>(nullable: true),
                    RecuireNow = table.Column<int>(nullable: false),
                    Apptotal = table.Column<int>(nullable: false),
                    AppTel = table.Column<int>(nullable: false),
                    PerAppTel = table.Column<string>(nullable: true),
                    AppCome = table.Column<int>(nullable: false),
                    PerAppCome = table.Column<string>(nullable: true),
                    AppWait = table.Column<int>(nullable: false),
                    PerAppWait = table.Column<string>(nullable: true),
                    AppSucc = table.Column<int>(nullable: false),
                    PerAppSucc = table.Column<string>(nullable: true),
                    AppStart = table.Column<int>(nullable: false),
                    PerAppStart = table.Column<string>(nullable: true),
                    AppEtc2 = table.Column<int>(nullable: false),
                    AppEtc3 = table.Column<int>(nullable: false),
                    AppEtc4 = table.Column<int>(nullable: false),
                    AppEtc5 = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_v_RecruiteReports", x => x.RowNumber);
                });

            migrationBuilder.CreateTable(
                name: "Viewers",
                columns: table => new
                {
                    ViewerId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ViewerDate = table.Column<DateTime>(nullable: false),
                    ViewerUser = table.Column<string>(nullable: true),
                    ManualId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Viewers", x => x.ViewerId);
                });

            migrationBuilder.CreateTable(
                name: "WebModuls",
                columns: table => new
                {
                    WebModulId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WebModulName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebModuls", x => x.WebModulId);
                });

            migrationBuilder.CreateTable(
                name: "Blogs",
                columns: table => new
                {
                    BlogId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BlogTitle = table.Column<string>(nullable: true),
                    BlogDate = table.Column<DateTime>(nullable: false),
                    BlogEndDate = table.Column<DateTime>(nullable: false),
                    BlogDetail = table.Column<string>(nullable: true),
                    BlogPicTitle = table.Column<string>(nullable: true),
                    BlogStatus = table.Column<int>(nullable: false),
                    BlogCatId = table.Column<int>(nullable: false),
                    BlogCreateDate = table.Column<DateTime>(nullable: false),
                    BlogCreateBy = table.Column<string>(nullable: true),
                    BlogUpdateBy = table.Column<string>(nullable: true),
                    BlogUpdateDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blogs", x => x.BlogId);
                    table.ForeignKey(
                        name: "FK_Blogs_BlogCats_BlogCatId",
                        column: x => x.BlogCatId,
                        principalTable: "BlogCats",
                        principalColumn: "BlogCatId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Manuals",
                columns: table => new
                {
                    ManualId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ManualName = table.Column<string>(nullable: true),
                    ManualLink = table.Column<string>(nullable: true),
                    ManuaDetail = table.Column<string>(nullable: true),
                    ManualDate = table.Column<DateTime>(nullable: false),
                    ManuaEditLastDate = table.Column<DateTime>(nullable: true),
                    ManualHits = table.Column<int>(nullable: false),
                    ManualEnables = table.Column<int>(nullable: false),
                    ManualUser = table.Column<string>(nullable: true),
                    ManualUserEdit = table.Column<string>(nullable: true),
                    ManualCatId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manuals", x => x.ManualId);
                    table.ForeignKey(
                        name: "FK_Manuals_ManualCats_ManualCatId",
                        column: x => x.ManualCatId,
                        principalTable: "ManualCats",
                        principalColumn: "ManualCatId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Organizs",
                columns: table => new
                {
                    organizId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentId = table.Column<int>(nullable: false),
                    Department1Id = table.Column<int>(nullable: false),
                    PositionId = table.Column<int>(nullable: false),
                    Power = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizs", x => x.organizId);
                    table.ForeignKey(
                        name: "FK_Organizs_Department1s_Department1Id",
                        column: x => x.Department1Id,
                        principalTable: "Department1s",
                        principalColumn: "Department1Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Organizs_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "DepartmentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Organizs_Positions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "Positions",
                        principalColumn: "PositionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HRRecruites",
                columns: table => new
                {
                    HRRecruiteID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HRRecruiteCardId = table.Column<string>(nullable: true),
                    TitleOfUserId = table.Column<int>(nullable: false),
                    HRRecruiteFName = table.Column<string>(nullable: true),
                    HRRecruiteLName = table.Column<string>(nullable: true),
                    HRRecruiteNickname = table.Column<string>(nullable: true),
                    HRRecruitBirth = table.Column<DateTime>(nullable: false),
                    HRRecruiteTel = table.Column<string>(nullable: true),
                    HRRecruiteEmail = table.Column<string>(nullable: true),
                    HRRecruiteLineId = table.Column<string>(nullable: true),
                    organizId = table.Column<int>(nullable: false),
                    LevelId = table.Column<int>(nullable: false),
                    HRRecruitDate = table.Column<DateTime>(nullable: false),
                    HRRecruiteGroupId = table.Column<int>(nullable: false),
                    UniversityId = table.Column<int>(nullable: false),
                    GPA = table.Column<string>(nullable: true),
                    YearCongrate = table.Column<int>(nullable: false),
                    TypeCongrateId = table.Column<int>(nullable: false),
                    FacultyId = table.Column<int>(nullable: false),
                    MajorId = table.Column<int>(nullable: false),
                    LastWorkYear = table.Column<string>(nullable: true),
                    ExWorkYear = table.Column<string>(nullable: true),
                    LastPosition = table.Column<string>(nullable: true),
                    TypeOfResignId = table.Column<int>(nullable: false),
                    TypeOfSalaryId = table.Column<int>(nullable: false),
                    BloodId = table.Column<int>(nullable: false),
                    StartWork = table.Column<DateTime>(nullable: true),
                    HRRecruiteStatusId = table.Column<int>(nullable: false),
                    SignDate = table.Column<DateTime>(nullable: true),
                    HRRecruiteBy = table.Column<string>(nullable: true),
                    HRRecruiteCreateDate = table.Column<DateTime>(nullable: false),
                    HRRecruiteUpdateBy = table.Column<string>(nullable: true),
                    HRRecruiteUpdateDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HRRecruites", x => x.HRRecruiteID);
                    table.ForeignKey(
                        name: "FK_HRRecruites_Bloods_BloodId",
                        column: x => x.BloodId,
                        principalTable: "Bloods",
                        principalColumn: "BloodId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HRRecruites_Faculties_FacultyId",
                        column: x => x.FacultyId,
                        principalTable: "Faculties",
                        principalColumn: "FacultyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HRRecruites_HRRecruiteGroups_HRRecruiteGroupId",
                        column: x => x.HRRecruiteGroupId,
                        principalTable: "HRRecruiteGroups",
                        principalColumn: "HRRecruiteGroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HRRecruites_HRRecruiteStatuses_HRRecruiteStatusId",
                        column: x => x.HRRecruiteStatusId,
                        principalTable: "HRRecruiteStatuses",
                        principalColumn: "HRRecruiteStatusId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HRRecruites_Levels_LevelId",
                        column: x => x.LevelId,
                        principalTable: "Levels",
                        principalColumn: "LevelId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HRRecruites_Majors_MajorId",
                        column: x => x.MajorId,
                        principalTable: "Majors",
                        principalColumn: "MajorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HRRecruites_TitleOfUsers_TitleOfUserId",
                        column: x => x.TitleOfUserId,
                        principalTable: "TitleOfUsers",
                        principalColumn: "TitleOfUserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HRRecruites_TypeCongrates_TypeCongrateId",
                        column: x => x.TypeCongrateId,
                        principalTable: "TypeCongrates",
                        principalColumn: "TypeCongrateId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HRRecruites_TypeOfResigns_TypeOfResignId",
                        column: x => x.TypeOfResignId,
                        principalTable: "TypeOfResigns",
                        principalColumn: "TypeOfResignId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HRRecruites_TypeOfSalaries_TypeOfSalaryId",
                        column: x => x.TypeOfSalaryId,
                        principalTable: "TypeOfSalaries",
                        principalColumn: "TypeOfSalaryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HRRecruites_Universities_UniversityId",
                        column: x => x.UniversityId,
                        principalTable: "Universities",
                        principalColumn: "UniversityId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HRRecruites_Organizs_organizId",
                        column: x => x.organizId,
                        principalTable: "Organizs",
                        principalColumn: "organizId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TitleOfUserId = table.Column<int>(nullable: false),
                    Firstname = table.Column<string>(nullable: true),
                    Lastname = table.Column<string>(nullable: true),
                    EFirstName = table.Column<string>(nullable: true),
                    ELastname = table.Column<string>(nullable: true),
                    Nickname = table.Column<string>(nullable: true),
                    BirthName = table.Column<DateTime>(nullable: true),
                    LevelId = table.Column<int>(nullable: false),
                    organizId = table.Column<int>(nullable: false),
                    Pic = table.Column<string>(nullable: true),
                    EmailContact = table.Column<string>(nullable: true),
                    ExtTel = table.Column<string>(nullable: true),
                    MobileTel = table.Column<string>(nullable: true),
                    BranchId = table.Column<int>(nullable: false),
                    StatusUserId = table.Column<int>(nullable: false),
                    EmpId = table.Column<string>(nullable: true),
                    UserCreateDate = table.Column<DateTime>(nullable: false),
                    BloodId = table.Column<int>(nullable: false),
                    TypeCongrateId = table.Column<int>(nullable: false),
                    CongrateDetail = table.Column<string>(nullable: true),
                    NationalityId = table.Column<int>(nullable: false),
                    ReligionId = table.Column<int>(nullable: false),
                    PovinceId = table.Column<int>(nullable: false),
                    Weight = table.Column<string>(nullable: true),
                    Height = table.Column<string>(nullable: true),
                    Waistline = table.Column<string>(nullable: true),
                    Certificate = table.Column<string>(nullable: true),
                    Reference = table.Column<string>(nullable: true),
                    ReferenceTel = table.Column<string>(nullable: true),
                    ResignationDate = table.Column<DateTime>(nullable: true),
                    TypeOfEmployeeId = table.Column<int>(nullable: false),
                    Reletion = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_Bloods_BloodId",
                        column: x => x.BloodId,
                        principalTable: "Bloods",
                        principalColumn: "BloodId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_Branchs_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branchs",
                        principalColumn: "BranchId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_Levels_LevelId",
                        column: x => x.LevelId,
                        principalTable: "Levels",
                        principalColumn: "LevelId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_nationalities_NationalityId",
                        column: x => x.NationalityId,
                        principalTable: "nationalities",
                        principalColumn: "NationalityId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_povinces_PovinceId",
                        column: x => x.PovinceId,
                        principalTable: "povinces",
                        principalColumn: "PovinceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_religions_ReligionId",
                        column: x => x.ReligionId,
                        principalTable: "religions",
                        principalColumn: "ReligionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_statusUsers_StatusUserId",
                        column: x => x.StatusUserId,
                        principalTable: "statusUsers",
                        principalColumn: "StatusUserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_TitleOfUsers_TitleOfUserId",
                        column: x => x.TitleOfUserId,
                        principalTable: "TitleOfUsers",
                        principalColumn: "TitleOfUserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_TypeCongrates_TypeCongrateId",
                        column: x => x.TypeCongrateId,
                        principalTable: "TypeCongrates",
                        principalColumn: "TypeCongrateId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_typeOfEmployee_TypeOfEmployeeId",
                        column: x => x.TypeOfEmployeeId,
                        principalTable: "typeOfEmployee",
                        principalColumn: "TypeOfEmployeeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_Organizs_organizId",
                        column: x => x.organizId,
                        principalTable: "Organizs",
                        principalColumn: "organizId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "appointments",
                columns: table => new
                {
                    AppId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HRRecruiteID = table.Column<int>(nullable: false),
                    AppTelTypeId = table.Column<int>(nullable: false),
                    AppTelDate = table.Column<DateTime>(nullable: true),
                    AppStatusId = table.Column<int>(nullable: false),
                    AppDate = table.Column<DateTime>(nullable: true),
                    AppResultId = table.Column<int>(nullable: false),
                    AppEtcId = table.Column<int>(nullable: false),
                    AppSuccessId = table.Column<int>(nullable: false),
                    AppRoomId = table.Column<int>(nullable: false),
                    AppCreateBy = table.Column<string>(nullable: true),
                    AppCreateDate = table.Column<DateTime>(nullable: false),
                    AppUpdateBy = table.Column<string>(nullable: true),
                    AppUpdateDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_appointments", x => x.AppId);
                    table.ForeignKey(
                        name: "FK_appointments_appEtcs_AppEtcId",
                        column: x => x.AppEtcId,
                        principalTable: "appEtcs",
                        principalColumn: "AppEtcId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_appointments_appResults_AppResultId",
                        column: x => x.AppResultId,
                        principalTable: "appResults",
                        principalColumn: "AppResultId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_appointments_appRooms_AppRoomId",
                        column: x => x.AppRoomId,
                        principalTable: "appRooms",
                        principalColumn: "AppRoomId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_appointments_appStatuses_AppStatusId",
                        column: x => x.AppStatusId,
                        principalTable: "appStatuses",
                        principalColumn: "AppStatusId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_appointments_appSuccesses_AppSuccessId",
                        column: x => x.AppSuccessId,
                        principalTable: "appSuccesses",
                        principalColumn: "AppSuccessId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_appointments_appTelTypes_AppTelTypeId",
                        column: x => x.AppTelTypeId,
                        principalTable: "appTelTypes",
                        principalColumn: "AppTelTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_appointments_HRRecruites_HRRecruiteID",
                        column: x => x.HRRecruiteID,
                        principalTable: "HRRecruites",
                        principalColumn: "HRRecruiteID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Logins",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: true),
                    CheckUserId = table.Column<int>(nullable: false),
                    PermisionId = table.Column<int>(nullable: false),
                    TypeOfUserId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logins", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Logins_CheckUsers_CheckUserId",
                        column: x => x.CheckUserId,
                        principalTable: "CheckUsers",
                        principalColumn: "CheckUserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Logins_Permisions_PermisionId",
                        column: x => x.PermisionId,
                        principalTable: "Permisions",
                        principalColumn: "PermisionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Logins_TypeOfUsers_TypeOfUserId",
                        column: x => x.TypeOfUserId,
                        principalTable: "TypeOfUsers",
                        principalColumn: "TypeOfUserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Logins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_appointments_AppEtcId",
                table: "appointments",
                column: "AppEtcId");

            migrationBuilder.CreateIndex(
                name: "IX_appointments_AppResultId",
                table: "appointments",
                column: "AppResultId");

            migrationBuilder.CreateIndex(
                name: "IX_appointments_AppRoomId",
                table: "appointments",
                column: "AppRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_appointments_AppStatusId",
                table: "appointments",
                column: "AppStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_appointments_AppSuccessId",
                table: "appointments",
                column: "AppSuccessId");

            migrationBuilder.CreateIndex(
                name: "IX_appointments_AppTelTypeId",
                table: "appointments",
                column: "AppTelTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_appointments_HRRecruiteID",
                table: "appointments",
                column: "HRRecruiteID");

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_BlogCatId",
                table: "Blogs",
                column: "BlogCatId");

            migrationBuilder.CreateIndex(
                name: "IX_HRRecruites_BloodId",
                table: "HRRecruites",
                column: "BloodId");

            migrationBuilder.CreateIndex(
                name: "IX_HRRecruites_FacultyId",
                table: "HRRecruites",
                column: "FacultyId");

            migrationBuilder.CreateIndex(
                name: "IX_HRRecruites_HRRecruiteGroupId",
                table: "HRRecruites",
                column: "HRRecruiteGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_HRRecruites_HRRecruiteStatusId",
                table: "HRRecruites",
                column: "HRRecruiteStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_HRRecruites_LevelId",
                table: "HRRecruites",
                column: "LevelId");

            migrationBuilder.CreateIndex(
                name: "IX_HRRecruites_MajorId",
                table: "HRRecruites",
                column: "MajorId");

            migrationBuilder.CreateIndex(
                name: "IX_HRRecruites_TitleOfUserId",
                table: "HRRecruites",
                column: "TitleOfUserId");

            migrationBuilder.CreateIndex(
                name: "IX_HRRecruites_TypeCongrateId",
                table: "HRRecruites",
                column: "TypeCongrateId");

            migrationBuilder.CreateIndex(
                name: "IX_HRRecruites_TypeOfResignId",
                table: "HRRecruites",
                column: "TypeOfResignId");

            migrationBuilder.CreateIndex(
                name: "IX_HRRecruites_TypeOfSalaryId",
                table: "HRRecruites",
                column: "TypeOfSalaryId");

            migrationBuilder.CreateIndex(
                name: "IX_HRRecruites_UniversityId",
                table: "HRRecruites",
                column: "UniversityId");

            migrationBuilder.CreateIndex(
                name: "IX_HRRecruites_organizId",
                table: "HRRecruites",
                column: "organizId");

            migrationBuilder.CreateIndex(
                name: "IX_Logins_CheckUserId",
                table: "Logins",
                column: "CheckUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Logins_PermisionId",
                table: "Logins",
                column: "PermisionId");

            migrationBuilder.CreateIndex(
                name: "IX_Logins_TypeOfUserId",
                table: "Logins",
                column: "TypeOfUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Logins_UserId",
                table: "Logins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Manuals_ManualCatId",
                table: "Manuals",
                column: "ManualCatId");

            migrationBuilder.CreateIndex(
                name: "IX_Organizs_Department1Id",
                table: "Organizs",
                column: "Department1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Organizs_DepartmentId",
                table: "Organizs",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Organizs_PositionId",
                table: "Organizs",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_BloodId",
                table: "Users",
                column: "BloodId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_BranchId",
                table: "Users",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_LevelId",
                table: "Users",
                column: "LevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_NationalityId",
                table: "Users",
                column: "NationalityId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PovinceId",
                table: "Users",
                column: "PovinceId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ReligionId",
                table: "Users",
                column: "ReligionId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_StatusUserId",
                table: "Users",
                column: "StatusUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_TitleOfUserId",
                table: "Users",
                column: "TitleOfUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_TypeCongrateId",
                table: "Users",
                column: "TypeCongrateId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_TypeOfEmployeeId",
                table: "Users",
                column: "TypeOfEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_organizId",
                table: "Users",
                column: "organizId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "appointments");

            migrationBuilder.DropTable(
                name: "BankRatios");

            migrationBuilder.DropTable(
                name: "BlogFiles");

            migrationBuilder.DropTable(
                name: "BlogPics");

            migrationBuilder.DropTable(
                name: "Blogs");

            migrationBuilder.DropTable(
                name: "caseBPCs");

            migrationBuilder.DropTable(
                name: "CheckInOuts");

            migrationBuilder.DropTable(
                name: "CheckRowInOuts");

            migrationBuilder.DropTable(
                name: "CHQloses");

            migrationBuilder.DropTable(
                name: "ConditionReports");

            migrationBuilder.DropTable(
                name: "DataXXYs");

            migrationBuilder.DropTable(
                name: "dataXY");

            migrationBuilder.DropTable(
                name: "DetailEmpHouseReports");

            migrationBuilder.DropTable(
                name: "DetailTableERPs");

            migrationBuilder.DropTable(
                name: "DocumentBillings");

            migrationBuilder.DropTable(
                name: "DocumentReceivables");

            migrationBuilder.DropTable(
                name: "Estimates");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "FileManals");

            migrationBuilder.DropTable(
                name: "fixAccessDataTmps");

            migrationBuilder.DropTable(
                name: "FixAssetStraightAlls");

            migrationBuilder.DropTable(
                name: "FixAssetStraightLines");

            migrationBuilder.DropTable(
                name: "GroupJobs");

            migrationBuilder.DropTable(
                name: "HouseRentalFileMonths");

            migrationBuilder.DropTable(
                name: "HouseRentalFiles");

            migrationBuilder.DropTable(
                name: "HouseRentals");

            migrationBuilder.DropTable(
                name: "InventoryCHQs");

            migrationBuilder.DropTable(
                name: "InvoiceExcels");

            migrationBuilder.DropTable(
                name: "InvoiceExcelTMPs");

            migrationBuilder.DropTable(
                name: "LineAPIs");

            migrationBuilder.DropTable(
                name: "ListMannualCats");

            migrationBuilder.DropTable(
                name: "Logins");

            migrationBuilder.DropTable(
                name: "Manuals");

            migrationBuilder.DropTable(
                name: "MapSites");

            migrationBuilder.DropTable(
                name: "Monitors");

            migrationBuilder.DropTable(
                name: "PictureManuals");

            migrationBuilder.DropTable(
                name: "Rates");

            migrationBuilder.DropTable(
                name: "RegisGroupJobs");

            migrationBuilder.DropTable(
                name: "ReportAges");

            migrationBuilder.DropTable(
                name: "ReportDeparment1s");

            migrationBuilder.DropTable(
                name: "ReportDeparments");

            migrationBuilder.DropTable(
                name: "ReportGens");

            migrationBuilder.DropTable(
                name: "ReportLevels");

            migrationBuilder.DropTable(
                name: "ReportManagements");

            migrationBuilder.DropTable(
                name: "reportManPowers");

            migrationBuilder.DropTable(
                name: "Retentions");

            migrationBuilder.DropTable(
                name: "SourceAutoCompletes");

            migrationBuilder.DropTable(
                name: "tranSectionFixAsset");

            migrationBuilder.DropTable(
                name: "UserJobs");

            migrationBuilder.DropTable(
                name: "v_RecruiteReports");

            migrationBuilder.DropTable(
                name: "Viewers");

            migrationBuilder.DropTable(
                name: "WebModuls");

            migrationBuilder.DropTable(
                name: "appEtcs");

            migrationBuilder.DropTable(
                name: "appResults");

            migrationBuilder.DropTable(
                name: "appRooms");

            migrationBuilder.DropTable(
                name: "appStatuses");

            migrationBuilder.DropTable(
                name: "appSuccesses");

            migrationBuilder.DropTable(
                name: "appTelTypes");

            migrationBuilder.DropTable(
                name: "HRRecruites");

            migrationBuilder.DropTable(
                name: "BlogCats");

            migrationBuilder.DropTable(
                name: "CheckUsers");

            migrationBuilder.DropTable(
                name: "Permisions");

            migrationBuilder.DropTable(
                name: "TypeOfUsers");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "ManualCats");

            migrationBuilder.DropTable(
                name: "Faculties");

            migrationBuilder.DropTable(
                name: "HRRecruiteGroups");

            migrationBuilder.DropTable(
                name: "HRRecruiteStatuses");

            migrationBuilder.DropTable(
                name: "Majors");

            migrationBuilder.DropTable(
                name: "TypeOfResigns");

            migrationBuilder.DropTable(
                name: "TypeOfSalaries");

            migrationBuilder.DropTable(
                name: "Universities");

            migrationBuilder.DropTable(
                name: "Bloods");

            migrationBuilder.DropTable(
                name: "Branchs");

            migrationBuilder.DropTable(
                name: "Levels");

            migrationBuilder.DropTable(
                name: "nationalities");

            migrationBuilder.DropTable(
                name: "povinces");

            migrationBuilder.DropTable(
                name: "religions");

            migrationBuilder.DropTable(
                name: "statusUsers");

            migrationBuilder.DropTable(
                name: "TitleOfUsers");

            migrationBuilder.DropTable(
                name: "TypeCongrates");

            migrationBuilder.DropTable(
                name: "typeOfEmployee");

            migrationBuilder.DropTable(
                name: "Organizs");

            migrationBuilder.DropTable(
                name: "Department1s");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Positions");
        }
    }
}

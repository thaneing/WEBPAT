using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CESAPSCOREWEBAPP.Models;

namespace CESAPSCOREWEBAPP.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ReportManPower> reportManPowers { get; set; }
        public DbSet<TranSectionFixAsset> tranSectionFixAsset { get; set; }
        public DbSet<FixAccessDataTmp> fixAccessDataTmps { get; set; }
        public DbSet<TypeOfEmployee> typeOfEmployee { get; set; }
        public DbSet<Povince> povinces { get; set; }
        public DbSet<Religion> religions { get; set; }
        public DbSet<Nationality> nationalities { get; set; }
        public DbSet<Appointment> appointments { get; set; }
        public DbSet<AppTelType> appTelTypes { get; set; }
        public DbSet<AppStatus> appStatuses { get; set; }
        public DbSet<AppResult> appResults { get; set; }
        public DbSet<AppEtc> appEtcs { get; set; }
        public DbSet<AppSuccess> appSuccesses { get; set; }
        public DbSet<AppRoom> appRooms { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Blood> Bloods { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Major> Majors { get; set; }
        public DbSet<HRRecruite> HRRecruites { get; set; }
        public DbSet<TypeOfSalary> TypeOfSalaries { get; set; }
        public DbSet<TypeOfResign> TypeOfResigns { get; set; }
        public DbSet<TypeCongrate> TypeCongrates { get; set; }
        public DbSet<HRRecruiteStatus> HRRecruiteStatuses { get; set; }
        public DbSet<HRRecruiteGroup> HRRecruiteGroups { get; set; }
        public DbSet<University> Universities { get; set; }



        public DbSet<Department1> Department1s { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<User> Users { get; set; }
       
        public DbSet<Estimate> Estimates { get; set; }
        public DbSet<WebModul> WebModuls { get; set; }
        public DbSet<UserJob> UserJobs { get; set; }
        public DbSet<BlogFile> BlogFiles { get; set; }
        public DbSet<BlogCat> BlogCats { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<BlogPic> BlogPics { get; set; }
        public DbSet<TitleOfUser> TitleOfUsers { get; set; }
        public DbSet<TypeOfUser> TypeOfUsers { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Permision> Permisions { get; set; }
        public DbSet<Level> Levels { get; set; }
        public DbSet<CheckUser> CheckUsers {get;set;}

        public DbSet<Login> Logins { get; set; }

        public DbSet<Organiz> Organizs { get; set; }
        public DbSet<Branch> Branchs { get; set; }
        public DbSet<StatusUser> statusUsers { get; set; }
        public virtual DbSet<DataXY> dataXY { get; set; }
        public virtual DbSet<V_RecruiteReport> v_RecruiteReports { get; set; }

        public virtual DbSet<ReportGen> ReportGens { get; set; }
        public virtual DbSet<ReportAge> ReportAges { get; set; }
        public virtual DbSet<ReportLevel> ReportLevels { get; set; }
        public virtual DbSet<ReportManagement> ReportManagements { get; set; }
        public virtual DbSet<ReportDeparment> ReportDeparments { get; set; }
        public virtual DbSet<ReportDepartment1> ReportDeparment1s { get; set; }

        public DbSet<CaseBPC> caseBPCs { get; set; }
        public DbSet<LineAPI> LineAPIs { get; set; }


        //คู่มือ
        public DbSet<ManualCat> ManualCats { get; set; }
        public DbSet<Manual> Manuals { get; set; }
        public DbSet<PictureManual> PictureManuals { get; set; }
        public DbSet<FileManal> FileManals { get; set; }
        public DbSet<Rate> Rates { get; set; }
        public DbSet<Viewer> Viewers { get; set; }

        public virtual DbSet<ListMannualCat> ListMannualCats { get; set; }

        public DbSet<Monitor>Monitors { get; set; }

        public virtual DbSet<DataXXY> DataXXYs { get; set; }

        public DbSet<DetailTableERP> DetailTableERPs { get; set; }
        public DbSet<CheckInOut> CheckInOuts { get; set; }
        public DbSet<CheckRowInOut>CheckRowInOuts { get; set; }

        public DbSet<HouseRental>HouseRentals { get; set; }
        public virtual DbSet<SourceAutoComplete> SourceAutoCompletes { get; set; }
        public virtual DbSet<DetailEmpHouseReport> DetailEmpHouseReports { get; set; }

        public DbSet<MapSite> MapSites { get; set; }


        public DbSet<HouseRentalFile> HouseRentalFiles { get; set; }
        public DbSet<HouseRentalFileMonth> HouseRentalFileMonths { get; set; }

        public DbSet<InvoiceExcel> InvoiceExcels { get; set; }
        public DbSet<InvoiceExcelTMP> InvoiceExcelTMPs { get; set; }


        public DbSet<FixAssetStraightLine> FixAssetStraightLines { get; set; }

        public DbSet<FixAssetStraightAll> FixAssetStraightAlls { get; set; }
        public DbSet<ConditionReport> ConditionReports { get; set; }

        public DbSet<DocumentBilling> DocumentBillings { get; set; }

        //Stock Bank
        public DbSet<BankRatio> BankRatios { get; set; }
        public DbSet<CHQlose> CHQloses { get; set; }
        public DbSet<InventoryCHQ> InventoryCHQs { get; set; }

        //AccountReciptable
        public DbSet<GroupJob> GroupJobs { get; set; }
        public DbSet<RegisGroupJob> RegisGroupJobs { get; set; }


        public DbSet<DocumentReceivable> DocumentReceivables { get; set; }

        public DbSet<Retention> Retentions { get; set; }
    }
}

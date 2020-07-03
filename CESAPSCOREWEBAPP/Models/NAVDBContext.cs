using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public partial class NAVContext : DbContext
    {
        public NAVContext(DbContextOptions<NAVContext> options)
            : base(options)

        {

        }

        //Account
        public virtual DbSet<V_WHT03> v_WHT03s { get; set; }
        public virtual DbSet<PurchaseVat> PurchaseVats { get; set; }
        public virtual DbSet<V_APRV> v_APRVs { get; set; }
        public virtual DbSet<V_APPV> v_APPVs { get; set; }
        public virtual DbSet<DetailVendor> DetailVendors { get; set; }
        public virtual DbSet<APDoc> APDocs { get; set; }
        public virtual DbSet<GRReceipt> GRReceipts { get; set; }

        public virtual DbSet<V_CheckPOPCJobtask> v_CheckPOPCJobtasks { get; set; }

        //FixAsset
        public virtual DbSet<SourceAutoComplete> sourceAutoCompletes { get; set; }
        public virtual DbSet<FixAccessData> fixAccessDatas { get; set; }
        public virtual DbSet<FixAssetList> fixAssetLists { get; set; }
        public virtual DbSet<FAChangeStatus> FAChangeStatuses { get; set; }


        //ERP
        public virtual DbSet<CheckGL> CheckGLs { get; set; }
        public virtual DbSet<V_CheckCostDiffGL> CheckCostDiffGLs { get; set; }
        public virtual DbSet<JobPlanningLine> jobPlanningLines { get; set; }
        public virtual DbSet<ItemLedgerEntry> ItemLedgerEntry { get; set; }
        public virtual DbSet<V_Job> v_Job { get; set; }
        public virtual DbSet<JobJournalLine> jobJournalLines { get; set; }
        public virtual DbSet<Rental> rentals { get; set; }
        public virtual DbSet<ListReturn> listReturns { get; set; }
        public virtual DbSet<JobLedgerNew> JobLedgerNews { get; set; }
        public virtual DbSet<JobTask> jobTasks { get; set; }
        public virtual DbSet<V_JobLedgerEntry> V_JobLedgerEntry { get; set; }
        public virtual DbSet<V_ReportJobCost> V_ReportJobCost { get; set; }


        //Estimate
        public virtual DbSet<RateOfRental> RateOfRentals { get; set; }

        //Purchase
        public virtual DbSet<V_ItemCode> v_ItemCodes { get; set; }
        public virtual DbSet<V_OrderPurchaseLine> v_OrderPurchaseLines { get; set; }
        public virtual DbSet<OrderPurchaseLines> orderPurchaseLines { get; set; }


        //AdminTool
        public virtual DbSet<MonitorPurchase> monitorPurchases { get; set; }
        public virtual DbSet<V_CheckLedger> V_CheckLedgers { get; set; }
        public virtual DbSet<CheckJOError> CheckJOErrors { get; set; }
        public virtual DbSet<V_ListJobTask> V_listJobTask { get; set; }     
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<ItemBySite> ItemBySites { get; set; }
        public virtual DbSet<ValueEntry> ValueEntries { get; set; }
        public virtual DbSet<CheckGRError> CheckGRErrors { get; set; }
        public virtual DbSet<DetailRental> DetailRentals { get; set; }
        public virtual DbSet<DataXY> dataXies { get; set; }
        public virtual DbSet<DataXYString> dataXYString { get; set; }
        public virtual DbSet<SessionNav> sessionNavs { get; set; }
        public virtual DbSet<JobOrderReport>JobOrderReports{get;set;}
        public virtual DbSet<ReportRental> ReportRentals { get; set; }
        public virtual DbSet<AccountsReceivable> AccountsReceivables { get; set; }

        public virtual DbSet<FixAssetStraightLine> FixAssetStraightLines { get; set; }

        public virtual DbSet<FixAssetMiss> FixAssetMisses { get; set; }

        public virtual DbSet<AccountReport> AccountReports { get; set; }

        public virtual DbSet<IssueReport> IssueReports { get; set; }

        public virtual DbSet<ChartOfAccount> ChartOfAccounts { get; set; }
        public virtual DbSet<CHQDirect> CHQDirects { get; set; }

        public virtual DbSet<CHQCurrent> CHQCurrents { get; set; }
        public virtual DbSet<F03> F03s { get; set; }
        public virtual DbSet<PurchaseOrderHeader> PurchaseOrderHeaders { get; set; }

        public virtual DbSet<Outstanding> Outstandings { get; set; }

        public virtual DbSet<PostedPurchaseInvoices> PostedPurchaseInvoices { get; set; }



    }
}

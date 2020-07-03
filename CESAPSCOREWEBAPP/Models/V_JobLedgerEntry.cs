using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class V_JobLedgerEntry
    {

        [Key]
        public int EntryID { get; set; }
        public string JobNo { get; set; }
        public DateTime PostingDate { get; set; }
        public string DocumentNo { get; set; }
        public int Types { get; set; }
        public string No { get; set; }
        public decimal Quantity { get; set; }
        public decimal DirectUnitCostLCY { get; set; }
        public decimal UnitCostLCY { get; set; }
        public decimal TotalCostLCY { get; set; }
        public decimal UnitPriceLCY { get; set; }
        public decimal TotalPriceLCY { get; set; }
        public string ResourceGroupNo { get; set; }
        public string UnitOfMeasureCode { get; set; }
        public string LocationCode { get; set; }
        public string JobPostingGroup { get; set; }
        public string GlobalDimension1Code { get; set; }
        public string GlobalDimension2Code { get; set; }
        public string WorkTypeCode { get; set; }
        public string CustomerPriceGroup { get; set; }
        public string UserID { get; set; }
        public string SourceCode { get; set; }
        public decimal AmtToPostToGL { get; set; }
        public decimal AmtPostedToGL { get; set; } 
        public int EntryType { get; set; }
        public string JournalBatchName { get; set; }
        public string ReasonCode { get; set; }
        public string TransactionType { get; set; }
        public string TransportMethod { get; set; }
        public string CuntryRegionCode { get; set; }
        public string GenBusPostingGroup { get; set; }
        public string GenProdPostingGroup { get; set; }
        public string EntryExitPoint { get; set; }
        public DateTime DocumentDate { get; set; }
        public string ExternalDocumentNo { get; set; }
        public string Area { get; set; }
        public string TransactionSpecification { get; set; }
        public string NoSeries { get; set; }
        public decimal AdditionalCurrencyTotalCost { get; set; }
        public decimal AddCurrencyTotalPrice { get; set; }
        public decimal AddCurrencyLineAmount { get; set; }
        public int DimensionSetID { get; set; }
        public string JobTaskNo { get; set; }
        public decimal LineAmountLCY { get; set; }
        public decimal UnitCost { get; set; }
        public decimal TotalCost { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal LineAmount { get; set; }
        public decimal LineDiscountAmount { get; set; }
        public decimal LineDiscountAmountLCY { get; set; }
        public string CurrencyCode { get; set; }
        public decimal CurrencyFactor { get; set; }
        public string Description2 { get; set; }
        public int LedgerEntryType { get; set; }
        public int LedgerEntryNo { get; set; }
        public string SerialNo { get; set; }
        public string LotNo { get; set; }
        public decimal LineDiscount { get; set; }
        public int LineType { get; set; }
        public decimal OriginalUnitCostLCY { get; set; }
        public decimal OriginalTotalCostLCY { get; set; }
        public decimal OriginalUnitCost { get; set; }
        public decimal OriginalTotalCost { get; set; }
        public decimal OriginalTotalCostACY { get; set; }
        public byte Adjusted { get; set; }
        public DateTime DateTimeAdjusted { get; set; }
        public string VariantCode { get; set; }
        public string BinCode { get; set; }
        public decimal QtyPerUnitOfMeasure { get; set; }
        public decimal QuantityBase { get; set; }
        public string ServiceOrderNo { get; set; }
        public string PostedServiceShipmentNo { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public byte Original { get; set; }
        public string RepairFor { get; set; }
        public decimal DiscountAmount { get; set; }
        public int PurchaseType { get; set; }
        public string ReferenceNo { get; set; }
        public int ReserveEntryNo { get; set; }
        public byte Applied { get; set; }
        public string ReferenceDocumentNo { get; set; }
        public string ToJobNo { get; set; }
        public string ToJobTask { get; set; }
        public string VinNo { get; set; }
        public DateTime Date1 { get; set; }
        public DateTime ToDate { get; set; }
        public decimal MileStart { get; set; }
        public decimal MiliCurrent { get; set; }
        public string FromJob { get; set; }
        public decimal Hours { get; set; }
        public decimal Trip { get; set; }
        public string ShipmentNo { get; set; }
        public string Remark { get; set; }
        public string TransferLocationCode { get; set; }
        public byte Transfer { get; set; }
        public decimal ConfirmQtyToReceive { get; set; }
        public decimal ConfirmQtyToReceiveB { get; set; }
        public DateTime ShipmentDate { get; set; }
        public string ShipmentMethod { get; set; }
        public string LicensePlate { get; set; }
        public string Driver { get; set; }
        public decimal RentQty { get; set; }
        public string RentUOM { get; set; }
        public decimal OTHrs { get; set; }
        public DateTime StartDate { get; set; }
        public int RentalType { get; set; }
        public string RentalItemNo { get; set; }
        public string FromJobTaskNo { get; set; }
        public string LicensePlateOutside { get; set; }
        public string DriverNameOutside { get; set; }
        public decimal DurationQty { get; set; }
        public string DurationUOM { get; set; }
        public decimal CarRentalAmount { get; set; }
        public int RentalJobLedgerEntryNo { get; set; }
        public int RentalAppliesFromEntryNo { get; set; }
        public int RecurringEntryNo { get; set; }
        public DateTime RecurringDate { get; set; }
        public int TypeOfTask { get; set; }
        public string ToLocation { get; set; }
        public string FromLocation { get; set; }

    }
}

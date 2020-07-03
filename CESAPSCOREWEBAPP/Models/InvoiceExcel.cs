using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class InvoiceExcel
    {
        [Key]
        public int ID { get; set; }
        public string EtcTax { get; set; } //ออก Tax INV แล้ว
        public string CusNo { get; set; } //รหัสลูกค้า
        public string Site { get; set; } // หน่วยงาน
        public DateTime PostingDate { get; set; } //วันที่ออกใบแจ้งหนี้
        public string CusName { get; set; } //ชื่อลูกหนี้
        public string InvoiceId { get; set; } //เลขที่ใบ INV
        public string Detail { get; set; } //รายละเอียด
        public string Period { get; set; } //งวดที่
        public decimal TotalExcVat { get; set; } // จำนวนเงินตามใบแจ้งหนี้ ก่อน vat 
        public decimal TotalCut { get; set; } // ลูกหนี้การค้า 
        public decimal RetentionTotal { get; set; } // เงินประกัน 
        public decimal AdvanceTotal { get; set; } // เงินล่วงหน้า 
        public decimal TotalConstruction { get; set; } //  รายได้กส.สัญญา 
        public decimal IncomeExtra { get; set; } //  รายได้กส.เพิ่มเติม 
        public decimal IncomeMaterial { get; set; } //  รายได้ขายวัสดุ-หน่วยงาน 
        public decimal IncomeEtc { get; set; } // รายได้อื่นๆหน่วยงาน 
        public decimal IncomeAdvanceExtra { get; set; } // รายได้ค่ากส.รับล่วงหน้า 
        public decimal Vat7 { get; set; } // VAT 7% 
        public decimal CustInVat { get; set; } // ลูกหนี้การค้าตามบัญชีรวมภาษีมูลค่าเพิ่ม 
        public decimal Vat3 { get; set; } // Tax 3% 
        public decimal TotalIncome { get; set; }  //รับสุทธิ 
        public DateTime? DueDate { get; set; } // ว.ด.ป.   รับชำระ 
        public string Ref { get; set; } // อ้างถึง 
        public decimal IncomeTotalBank { get; set; } // จำนวนเงินเข้า BANK 
        public string Bankname { get; set; } // ธนาคาร 
        public string ChqNo { get; set; } // เลขที่เช็ค 
        public decimal InvoiceTotal { get; set; } //ยอดใบเสร็จรับเงิน
        public decimal PayTotal { get; set; } //If Pay
        public decimal BankTotal { get; set; } // ยอดธนาคารที่ได้รับ 
        public string TypeOfPay { get; set; } //สถานะ
        public DateTime ImportDate { get; set; } //วันที่ Import
        public string ImportBy { get; set; } //Import โดย
      


    }
}

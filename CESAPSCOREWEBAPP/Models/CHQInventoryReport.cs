using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class CHQInventoryReport
    {
        [Key]
        public int ID { get; set; } //ลำดับ
        public string BankCode { get; set; } //ธนาคาร
        public string BankName { get; set; } //ชื่อธนาคาร
        public int QTYOfOne { get; set; } //จำนวน เล่ม
        public int BankIssueQty { get; set; } //อัตราต่อเล่ม
        public int CHQInventoryHead { get; set; } //จำนวนเล่มคงเหลือ
        public int CHQInventorySub { get; set; } //ฉบับ
        public int CHQInventoryTotal { get; set; }  //รวม(ฉบับ)
        public int BuyInPeriod { get; set; } //ซื้อใช้ระหว่างเดือน
        public int TotalUseInPeriod { get; set; } //ใช้ไป
        public int TotalInstock { get; set; } //รวมใน Stock
        public int TotalUsing { get; set; } //ใช้สะสม


    }
}

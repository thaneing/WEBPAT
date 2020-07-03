using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class Tmp_House
    {
        public int ID { get; set; }
        public string EmpId { get; set; }
        public string EmpName { get; set; }
        public string EmpPosition { get; set; }
        public DateTime PostingDate { get; set; }
        public string Site { get; set; }
        public decimal Deposit { get; set; }
        public string DepositText { get; set; }
        public decimal Advanced { get; set; }
        public string AdvancedText { get; set; }
        public decimal Price { get; set; }
        public string Thaibath { get; set; }
        public string Etc { get; set; }
        public string HouseName { get; set; }
        public string RoomNumber { get; set; }
        public string CreateBy { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}")]
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int Statuss { get; set; }
        public int TypeRooms { get; set; }

        public string Period { get; set; }
        public string Paymentdate { get; set; }
        public int Count { get; set; }
        public decimal RoomPrice { get; set; }


        //public enum TypeRoom
        //{
        //    พักเดี่ยว,
        //    พักคู่
        //}
        //public enum Status
        //{
        //    รอ,
        //    สำเร็จ,
        //    ไม่สำเร็จ
        //}


    }
}

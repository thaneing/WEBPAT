using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class UserTmp
    {
        public int UserId { get; set; }

        public int TitleOfUserId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string EFirstName { get; set; }
        public string ELastname { get; set; }


        public string Nickname { get; set; }
        [DataType(DataType.Date)]
        public DateTime? BirthName { get; set; }
        public int LevelId { get; set; }
        public int organizId { get; set; }


        public string Pic { get; set; }
        public string EmailContact { get; set; }
        public string ExtTel { get; set; }
        public string MobileTel { get; set; }

        public int BranchId { get; set; }
        public int StatusUserId { get; set; }
        public string EmpId { get; set; }



        public DateTime UserCreateDate { get; set; }  //start work date


        public int BloodId { get; set; } //กรุ๊ฟเลือก
        public int TypeCongrateId { get; set; } //การศึกษาสูดสุด
        public string CongrateDetail { get; set; } //การศึกษารายละเอียด
        public int NationalityId { get; set; } //สัญชาติ
        public int ReligionId { get; set; } //ศาสนา
        public int PovinceId { get; set; } //ภูมิลำเนา
        public string Weight { get; set; } //น้ำหนัก
        public string Height { get; set; } //ส่วนสูง
        public string Waistline { get; set; } //รอบเอว
        public string Certificate { get; set; } //ใบประกอบวิชาชีพ
        public string Reference { get; set; } //บุคคลอ้างอิง
        public string ReferenceTel { get; set; } //เบอร์โทรบุคคลอ้างอิง

        public DateTime? ResignationDate { get; set; } //วันที่ลาออก


        public int TypeOfEmployeeId { get; set; }
        public string Reletion { get; set; }
    }

}

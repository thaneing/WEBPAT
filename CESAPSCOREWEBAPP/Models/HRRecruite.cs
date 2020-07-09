using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class HRRecruite
    {
        [Key]
        public int HRRecruiteID { get; set; }
        public string HRRecruiteCardId { get; set; }
        public int TitleOfUserId { get; set; }
        public string HRRecruiteFName { get; set; }
        public string HRRecruiteLName { get; set; }
        public string HRRecruiteNickname { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime HRRecruitBirth { get; set; }
        public string HRRecruiteTel { get; set; }
        public string HRRecruiteEmail { get; set; }
        public string HRRecruiteLineId { get; set; }


        public int organizId { get; set; }

        //public int DepartmentId { get; set; } //แผนก
        //public int Department1Id { get; set; } //ฝ่าย
        //public int PositionId { get; set; } //ตำแหน่ง
        public int LevelId { get; set; }  //ระดับ



        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime HRRecruitDate { get; set; } //วันที่ Recute
        public int HRRecruiteGroupId { get; set; }  //มาจากไหน
        public int UniversityId { get; set; }
        public string GPA { get; set; }
        public int YearCongrate { get; set; } //ปีที่จบ
        public int TypeCongrateId { get; set; } //ระดับการศึกษา
        public int FacultyId { get; set; } //คณะ
        public int MajorId { get; set; }  //สาขา




        public string LastWorkYear { get; set; } //สถานที่งานล่าสุด
        public string ExWorkYear { get; set; }
        public string LastPosition { get; set; }
        public int TypeOfResignId { get; set; } //เหตุผลที่ลาออก
        public int TypeOfSalaryId { get; set; } //เงินเดือนล่าสุด



        public int BloodId { get; set; } //Group เลิอก

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? StartWork { get; set; } //วันที่เริ่มงาน

        public int HRRecruiteStatusId { get; set; } //สถานการ Recruite

        public DateTime? SignDate { get; set; }

        public string HRRecruiteBy { get; set; }
        public DateTime HRRecruiteCreateDate { get; set; }
        public string HRRecruiteUpdateBy { get; set; }
        public DateTime? HRRecruiteUpdateDate { get; set; }

        public TitleOfUser TitleOfUsers { get; set; }
        public Blood Bloods { get; set; }
        public Faculty Faculties { get; set; }
        public Major Majors { get; set; }
        public TypeOfSalary TypeOfSalaries { get; set; }
        public TypeOfResign TypeOfResigns { get; set; }
        public TypeCongrate TypeCongrates { get; set; }
        public HRRecruiteStatus HRRecruiteStatuses { get; set; }
        public HRRecruiteGroup HRRecruiteGroups { get; set; }
        public University Universities { get; set; }


        public ICollection<Appointment> Appointments { get; set; }
    }
}

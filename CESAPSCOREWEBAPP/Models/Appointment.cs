using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Models
{
    public class Appointment
    {
        [Key]
        public int AppId { get; set; }
        public int HRRecruiteID { get; set; }
        public int AppTelTypeId { get; set; }
        public DateTime? AppTelDate { get; set; }
        public int AppStatusId { get; set; }
        public DateTime? AppDate { get; set; }
        public int AppResultId {get;set;}
        public int AppEtcId { get; set; }
        public int AppSuccessId { get; set; }
        public int AppRoomId { get; set; }
        public string AppCreateBy { get; set; }
        public DateTime AppCreateDate { get; set; }
        public string AppUpdateBy { get; set; }
        public DateTime? AppUpdateDate { get; set; }

        public AppTelType appTelTypes { get; set; }
        public AppStatus appStatuses { get; set; }
        public AppResult appResults { get; set; }
        public AppEtc appEtcs { get; set; }
        public AppSuccess appSuccesses { get; set; }
        public AppRoom appRooms { get; set; }

        public HRRecruite HRRecruites { get; set; }




    }
}

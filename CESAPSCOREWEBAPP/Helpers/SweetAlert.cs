using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CESAPSCOREWEBAPP.Models.Enums;

namespace CESAPSCOREWEBAPP.Helpers
{
    public class SweetAlert
    {

        public static string Alert(string message,string notifyTypet)
        {
            var Typealert = "";


            switch (notifyTypet)
            {
                case "success" :
                    Typealert = "alert-box success";
                    break;
                case "error":
                    Typealert = "alert-box errors";
                    break;
                case "warning":
                    Typealert = "alert-box warning";
                    break;

                case "info" :
                    Typealert = "alert-box notice";
                    break;
            }


            var msg = "<script>swal('" + notifyTypet.ToUpper() + "', '" + message + "','" + Typealert + "')" + "</script>";
            
            ///Console.WriteLine(msg);
            return msg;

        }


    }
}

using CESAPSCOREWEBAPP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CESAPSCOREWEBAPP.Models.Enums;

namespace CESAPSCOREWEBAPP.Helpers
{
    public class PermisionHelper
    {



        public static Boolean CheckPermision(string usertype,string permision,string page)
        {


              
                if (usertype != "3") //เช็คว่าเป็น SupperAdmin ไหม หากเป็น ไม่ใช่ User SupperAdmin ให้เช็คตาม Page
                {
                    var data = permision;
                    string[] words = data.Split(',');
                    foreach (string word in words)
                    {
                        if(word == page)
                        { 
                            return true;
                        }
                    }
                    return false;
                }
                else
                {
                    return true;   //กรณี เป็น Suppoer Admin ให้ Pass Only
                }
       
        }
    }
}

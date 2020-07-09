using CESAPSCOREWEBAPP.Helpers;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Middlewares
{
    public class HardwareInfoMiddleware
    {

        public static string PutRegitry()
        {
            var partwwwroot = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            /*Create Path */
            string FILE_NAME = "KeyOne.DAT";
            string pathImage = @"\OEM\Key\";
            string pathSave = $"wwwroot{pathImage}";


            if (!Directory.Exists(pathSave))
            {
                Directory.CreateDirectory(pathSave);
            }
            /**/

            /*Create Key .dat */
            string KeyOne = "ZaniimzOxide";
            var param1 = Encryption.Encrypt(getUniqueID("C"), KeyOne);
            var param2 = Encryption.Encrypt(GetMachineGuid(), KeyOne);
            var dir = Encryption.Encrypt(partwwwroot + pathImage, KeyOne);


            if (File.Exists(pathSave + FILE_NAME))
            {
                Console.WriteLine($"{FILE_NAME} already exists!");
            }
            else
            {
                using (FileStream fs = new FileStream(pathSave + FILE_NAME, FileMode.CreateNew))
                {
                    using (BinaryWriter w = new BinaryWriter(fs))
                    {
                        w.Write(param1);
                        w.Write(param2);
                    }
                }
            }


            List<string> datalists = new List<string>();

            using (FileStream fs = new FileStream(pathSave + FILE_NAME, FileMode.Open, FileAccess.Read))
            {

                using (BinaryReader r = new BinaryReader(fs))
                {
                    for (int i = 0; i < 2; i++)
                    {
                        datalists.Add(r.ReadString());
                    }

                }
            }

            string location = @"SOFTWARE\Foglight\";
            RegistryKey key = Registry.CurrentUser.CreateSubKey(location);
            key.SetValue("param1", datalists[0]);
            key.SetValue("param2", datalists[1]);
            key.SetValue("dir", dir);

            return pathSave + FILE_NAME;
        }




        public static string CheckRegitry()
        {
            var partwwwroot = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            /*Create Path */
            string KeyOne = "ZaniimzOxide";
            string FILE_NAME = "KeyOne.DAT";
            string pathImage = @"\OEM\Key\";
            string pathSave = $"wwwroot{pathImage}";

            var param1 = Encryption.Encrypt(getUniqueID("C"), KeyOne);
            var param2 = Encryption.Encrypt(GetMachineGuid(), KeyOne);
            var dir = Encryption.Encrypt(partwwwroot + pathImage, KeyOne);

            string location = @"SOFTWARE\Foglight\";
            RegistryKey key = Registry.CurrentUser.CreateSubKey(location);
            key.SetValue("param1", param1);
            key.SetValue("param2", param2);
            key.SetValue("dir", dir);


            if (!Directory.Exists(pathSave))
            {
                Directory.CreateDirectory(pathSave);
            }
            /**/

            List<string> datalists = new List<string>();
            try
            {


                using (FileStream fs = new FileStream(pathSave + FILE_NAME, FileMode.Open, FileAccess.Read))
                {

                    using (BinaryReader r = new BinaryReader(fs))
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            datalists.Add(r.ReadString());
                        }


                    }
                }


                if (Encryption.Decrypt(datalists[0], KeyOne) == getUniqueID("C"))
                {

                }
                else
                {
                    return "Please Regiter Licen";
                }
                if (Encryption.Decrypt(datalists[1], KeyOne) == GetMachineGuid())
                {

                }
                else
                {
                    return "Please Regiter Licen";
                }
                //if(JavaTimeStamp.JavaTimeStampToDateTime(Convert.ToDouble(Encryption.Decrypt(datalists[2], KeyOne))) <= DateTime.Now)
                // {

                // }
                // else
                // {
                //     return "Licen Expired";
                // }
                var a = JavaTimeStamp.JavaTimeStampToDateTime(Convert.ToDouble(Encryption.Decrypt(datalists[3], KeyOne)));

                if (DateTime.Now>JavaTimeStamp.JavaTimeStampToDateTime(Convert.ToDouble(Encryption.Decrypt(datalists[3], KeyOne))))
                {
                    return "Licen Expired";
                }
                else
                {
                   
                }



            }
            catch
            {
                return "Please Regiter Licen";
            }











            return "true";
        }





        private static string GetMachineGuid()
        {
            string location = @"SOFTWARE\Microsoft\Cryptography";
            string name = "MachineGuid";

            using (RegistryKey localMachineX64View =
                RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
            {
                using (RegistryKey rk = localMachineX64View.OpenSubKey(location))
                {
                    if (rk == null)
                        throw new KeyNotFoundException(
                            string.Format("Key Not Found: {0}", location));

                    object machineGuid = rk.GetValue(name);
                    if (machineGuid == null)
                        throw new IndexOutOfRangeException(
                            string.Format("Index Not Found: {0}", name));

                    return machineGuid.ToString();
                }
            }
        }



        private static string getUniqueID(string drive)
        {
            if (drive == string.Empty)
            {
                foreach (DriveInfo compDrive in DriveInfo.GetDrives())
                {
                    if (compDrive.IsReady)
                    {
                        drive = compDrive.RootDirectory.ToString();
                        break;
                    }
                }
            }

            if (drive.EndsWith(":\\"))
            {
                drive = drive.Substring(0, drive.Length - 2);
            }

            string volumeSerial = getVolumeSerial(drive);
            string cpuID = getCPUID();

            return cpuID.Substring(13) + cpuID.Substring(1, 4) + volumeSerial + cpuID.Substring(4, 4);
        }



        private static string getVolumeSerial(string drive)
        {
            ManagementObject disk = new ManagementObject(@"win32_logicaldisk.deviceid=""" + drive + @":""");
            disk.Get();

            string volumeSerial = disk["VolumeSerialNumber"].ToString();
            disk.Dispose();

            return volumeSerial;
        }

        private static string getCPUID()
        {
            string cpuInfo = "";
            ManagementClass managClass = new ManagementClass("win32_processor");
            ManagementObjectCollection managCollec = managClass.GetInstances();

            foreach (ManagementObject managObj in managCollec)
            {
                if (cpuInfo == "")
                {
                    cpuInfo = managObj.Properties["processorID"].Value.ToString();
                    break;
                }
            }

            return cpuInfo;
        }

    }
}

using System;
using System.IO;
//using System.Web.HttpContext.Current;
namespace bakend.Tools
{

    public class ELog
   {
       
       
      
        public static void  Add(string sLog)
       {
           CreateDirectory();
           string nombre = GetNameFile();
           string cadena = "";
 
           cadena += DateTime.Now + " - " + sLog + Environment.NewLine;
 
           StreamWriter sw = new StreamWriter("./Tools/log"+"/"+nombre,true);
           sw.Write(cadena);
           sw.Close();
 
       }
 
       #region HELPER
       private static string GetNameFile()
       {
           string nombre = "";
 
           nombre = "log_" + DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + ".txt";
 
           return nombre;
       }
 
       private static void CreateDirectory()
       {
           try
           {
               if (!Directory.Exists("./Tools/log"))
                   Directory.CreateDirectory("./Tools/log");
 
               
           }
           catch (DirectoryNotFoundException ex) {
               throw new Exception(ex.Message);
                
           }
       }
       #endregion
   }


}


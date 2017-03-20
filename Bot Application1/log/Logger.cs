using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Bot_Application1.log
{
    public class Logger
    {

        /** 
          * Log 
          *  path 
          *  
          * @var string 
          */
        //private static string __log_file_path = Environment.CurrentDirectory + "/log/" ;

        /** 
         * __log_file_path get/set 
         */
     //   public static string filePath
    //    {
    //        get { return Logger.__log_file_path; }
    //        set { if (value.Length > 0) Logger.__log_file_path = value; }
     //   }

        /** 
         * Flush log file contents 
         *  
         * @return void 
         */
        public static void flush()
        {
      //      File.WriteAllText(Logger.filePath, string.Empty);
        }

        /** 
         * Log message 
         *  
         * @param string msg 
         * @return void 
         */
        public static void log(string className, string methodName, string msg)
        {
            if (msg.Length > 0)
            {
                /*      using (StreamWriter sw = File.AppendText(Logger.filePath))
                      {
                          sw.WriteLine("{0} {1}: {2} {3}: {4}", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString(),className,methodName, msg);
                          sw.Flush();
                      } */

            //    System.Diagnostics.Trace.WriteLine(DateTime.Now.ToShortDateString() + " " +  DateTime.Now.ToShortTimeString() + " " + className + " "+ methodName + " "+ msg);
            //    System.Diagnostics.Trace.Flush();

            }
        }


        
    }
}
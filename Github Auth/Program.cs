using static Github_Auth.func;
using System.Net;
using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Windows;

namespace Github_Auth
{
    internal class Program
    {
        static string username_input;
        static string loginASCII = @"
         ██████╗ ██╗████████╗██╗  ██╗██╗   ██╗██████╗      █████╗ ██╗   ██╗████████╗██╗  ██╗
        ██╔════╝ ██║╚══██╔══╝██║  ██║██║   ██║██╔══██╗    ██╔══██╗██║   ██║╚══██╔══╝██║  ██║
        ██║  ███╗██║   ██║   ███████║██║   ██║██████╔╝    ███████║██║   ██║   ██║   ███████║
        ██║   ██║██║   ██║   ██╔══██║██║   ██║██╔══██╗    ██╔══██║██║   ██║   ██║   ██╔══██║
        ╚██████╔╝██║   ██║   ██║  ██║╚██████╔╝██████╔╝    ██║  ██║╚██████╔╝   ██║   ██║  ██║
         ╚═════╝ ╚═╝   ╚═╝   ╚═╝  ╚═╝ ╚═════╝ ╚═════╝     ╚═╝  ╚═╝ ╚═════╝    ╚═╝   ╚═╝  ╚═╝
        ";
        static string auth_url = "https://raw.githubusercontent.com/rarksyy/gh_auth_test/main/username.auth";

        [STAThread]
        static void Main()
        {
            log(loginASCII, ConsoleColor.Green);
            spacer();
        InitLogin:
            username_input = input("Enter Username: ");

            using (WebClient client = new WebClient())
            {
                Random random = new Random();
                bool valid_username = client.DownloadString(auth_url+$"?random={random.Next()}").Contains(createmd5(username_input.ToLower()));
                if (valid_username) login();
                else {log("Invalid Username", ConsoleColor.Red); goto InitLogin; }
            }
        }

        static void login()
        {
        InitPassword:
            string password_input = String.Empty;
            password_input = input("Enter Password: ", true);

            string CSerial = gethwinfo("Win32_LogicalDisk", "VolumeSerialNumber"),
            CPUID = gethwinfo("win32_processor", "processorID"),
            MOBOID = gethwinfo("Win32_BaseBoard", "SerialNumber"),
            password = createmd5(password_input),
            reversed = reversestring($"{username_input}{CSerial}{CPUID}{MOBOID}"),
            md5ified = createmd5(reversed);
            
            using (WebClient client = new WebClient())
            {
                string usermd5 = createmd5(username_input.ToLower());
                Random random = new Random();
                string auth = client.DownloadString(auth_url+"?random="+DateTime.Now.Ticks);
                if (!auth.Contains(md5ified))
                {
                    log(auth);
                    InvalidAuthInfo(usermd5 + md5ified + password);
                }
                string cur;
                StringReader sr = new StringReader(auth);
                while ((cur = sr.ReadLine()) != null)
                {
                    if (cur.Contains(usermd5+md5ified))
                    {
                        if (cur.Length == md5ified.Length)
                        {
                            log(auth);
                            InvalidAuthInfo(usermd5+md5ified+password);
                            Thread.Sleep(10000);
                        }
                        else if (cur.Length > md5ified.Length)
                        {
                            if (cur != usermd5 + md5ified + password)
                            {
                                log("Invalid Password", ConsoleColor.Red);
                                goto InitPassword;
                            }
                            if (cur.Contains(md5ified + password))
                            {
                                Continue();
                            }
                            else
                            {
                                log("error");
                                Thread.Sleep(10000);
                            }
                        }
                    }
                }
            }
        }

        private static void Continue()
        {
            //successfully logged in, do whatever you want here
            log("success");
            Thread.Sleep(10000);
        }
    }
}

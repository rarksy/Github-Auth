using static Github_Auth.func;
using System;
using System.Threading;
using System.Net;
using System.Windows;

namespace Github_Auth
{
    internal class Program
    {
        static string loginASCII = @"
 ██████╗ ██╗████████╗██╗  ██╗██╗   ██╗██████╗      █████╗ ██╗   ██╗████████╗██╗  ██╗
██╔════╝ ██║╚══██╔══╝██║  ██║██║   ██║██╔══██╗    ██╔══██╗██║   ██║╚══██╔══╝██║  ██║
██║  ███╗██║   ██║   ███████║██║   ██║██████╔╝    ███████║██║   ██║   ██║   ███████║
██║   ██║██║   ██║   ██╔══██║██║   ██║██╔══██╗    ██╔══██║██║   ██║   ██║   ██╔══██║
╚██████╔╝██║   ██║   ██║  ██║╚██████╔╝██████╔╝    ██║  ██║╚██████╔╝   ██║   ██║  ██║
 ╚═════╝ ╚═╝   ╚═╝   ╚═╝  ╚═╝ ╚═════╝ ╚═════╝     ╚═╝  ╚═╝ ╚═════╝    ╚═╝   ╚═╝  ╚═╝
        ";

        [STAThread]
        static void Main() {
            log(loginASCII, ConsoleColor.Green);
            spacer();
            spacer();
            log("["); log(1, ConsoleColor.Magenta); log("] Login\n", ConsoleColor.Gray);
            log("["); log(2, ConsoleColor.Magenta); log("] Register\n", ConsoleColor.Gray);

            while (true)
            {
                var key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.D1) {
                    login();
                    break;
                }
                if (key.Key == ConsoleKey.D2) {
                    register();
                    break;
                }

                else continue;
            }   
        }

        static void register() {
            string user, pass;
        EnterUsername:
            user = input("Enter A Username: ");
            WebClient client = new WebClient();
            string user_list = client.DownloadString("https://raw.githubusercontent.com/rarksyy/gh_auth_test/main/cppauth.auth");
            if (user_list.Contains(user.ToLower()+createmd5(user.ToLower()))) {
                log("Username Already In Use. Contact Support To Reset HWID\n", ConsoleColor.Red);
                goto EnterUsername;
            }
            else {
                pass = input("Enter A Password: ", true);
                string CPUID = gethwinfo("win32_processor", "processorID"),
                 CSerial = int.Parse(gethwinfo("Win32_LogicalDisk", "VolumeSerialNumber"), System.Globalization.NumberStyles.HexNumber).ToString();
                string md5info = createmd5(user.ToLower()) + createmd5(reversestring(CPUID + CSerial + Environment.UserName)) + createmd5(pass);
                Clipboard.SetText(user.ToLower()+md5info.ToLower());

                log("\nAuth Info Copied To Clipboard, Contact Owner / Support", ConsoleColor.Green);
                log("\nExiting In 6");
                for (int i = 5; i >= 0; i--) {
                    Thread.Sleep(1000);
                    Console.Write("\x1B[1D");
                    Console.Write("\x1B[1P");
                    Console.Write(i);
                }
            }
        }

        static void login() {

            string user, pass;
        EnterUsername:
            user = input("Enter Username: ");
            WebClient client = new WebClient();
            string user_list = client.DownloadString("https://raw.githubusercontent.com/rarksyy/gh_auth_test/main/cppauth.auth").ToLower();

            if (user_list.Contains(user.ToLower() + createmd5(user.ToLower()).ToLower())) {
        EnterPassword:
                pass = input("Enter Password: ", true);
                string hwInfo = user.ToLower() + createmd5(user.ToLower()).ToLower() + createmd5(
                                 reversestring(
                                 gethwinfo("win32_processor", "processorID") +
                                 int.Parse(gethwinfo("Win32_LogicalDisk", "VolumeSerialNumber"),
                                 System.Globalization.NumberStyles.HexNumber).ToString() +
                                 Environment.UserName.ToLower()));
                if (user_list.Contains(hwInfo.ToLower())) {
                    if (user_list.Contains(hwInfo.ToLower() + createmd5(pass).ToLower())) {
                        Access();
                    }
                    else {
                        log("Incorrect Password\n", ConsoleColor.Red);
                        goto EnterPassword;
                    }
                }
                else {
                    log("Incorrect HWID\n", ConsoleColor.Red);
                    log(hwInfo);
                    goto EnterUsername;
                }
            }
            else {
                log("Invalid Username\n", ConsoleColor.Red);
                goto EnterUsername;
            }

        }

        static void Access() {
            log("success");
            Thread.Sleep(10000);
        }
    }
}

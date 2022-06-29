using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Windows;
using System.Threading;

namespace Github_Auth
{
    class func
    {
        public static void log<T>(T arg, ConsoleColor Col = ConsoleColor.Gray) { Console.ForegroundColor = Col; Console.Write($"{arg}"); Console.ForegroundColor = ConsoleColor.Gray; }
        public static void spacer() { Console.WriteLine(Environment.NewLine); }
        public static string input(string label = null, bool password = false)
        {
            if (!password)
                { Console.Write(label); return Console.ReadLine(); }
            else
            {
                string pw = string.Empty;
                Console.Write(label);
                while (true)
                {
                    var key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.Enter)                   
                        return pw;
                    
                    if (key.Key == ConsoleKey.Backspace && pw.Length > 0)
                    {
                        string _new = pw.Remove(pw.Length - 1);
                        Console.Write("\x1B[1D");
                        Console.Write("\x1B[1P");
                        pw = _new;
                        continue;
                    }
                    if (key.Key != ConsoleKey.Backspace)
                    {
                        pw += key.KeyChar;
                        Console.OutputEncoding = Encoding.Unicode;
                        Console.Write("*");
                    }
                }
            }

        }
        public static string createmd5(string input)
        {
            using (var md5Hash = System.Security.Cryptography.MD5.Create())
            {
                var sourceBytes = Encoding.UTF8.GetBytes(input);

                var hashBytes = md5Hash.ComputeHash(sourceBytes);

                var hash = BitConverter.ToString(hashBytes).Replace("-", string.Empty);

                return hash;
            }
        }
        public static string gethwinfo(string management, string label) 
        {
            ManagementClass mgmt = new ManagementClass(management);
            ManagementObjectCollection moc = mgmt.GetInstances(); 
            string result = string.Empty;
            foreach (ManagementObject str in moc) 
            {
                if (result == string.Empty)
                    result += Convert.ToString(str[label]);
            }
            return result; 
        }
        public static string reversestring(string text)
        {
            char[] cArray = text.ToCharArray();
            string reverse = String.Empty;
            for (int i = cArray.Length - 1; i > -1; i--)
            {
                reverse += cArray[i];
            }
            return reverse;
        }
        public static void InvalidAuthInfo(string cbdtxt)
        {

            Clipboard.SetText(cbdtxt);
            log("Invalid Auth Info, Contact Owner / Support To Validate Credentials.", ConsoleColor.Red);
            log(cbdtxt);
            Console.Write("Exiting In 5");
            for (int i = 5; i >= 0; i--)
            {
                Thread.Sleep(1000);
                Console.Write("\x1B[1D");
                Console.Write("\x1B[1P");
                Console.Write(i);
            }
            Environment.Exit(0);
        }
        public static bool ContainsWord(string s, string word)
        {
            string[] ar = s.Split(' ');

            foreach (string str in ar)
            {
                if (str.ToLower() == word.ToLower())
                    return true;
            }
            return false;
        }

    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SearchString
{
    public class Program
    {
        public static string readText, fileName, stringToCompare;
        public static List<string> stringList;
        public static StringBuilder myResults;
        public static int invalidStrings;
        public static void Main(string[] args)
        {
            stringList = new List<string>();
            myResults = new StringBuilder();
            stringToCompare = string.Empty;

            fileName = @"MyStrings.txt";

            Console.WriteLine("###############################");
            Console.WriteLine("Enter string to find: ");
            stringToCompare = Console.ReadLine();

            while (!ValidateString(stringToCompare))
            {
                Console.WriteLine();
                Console.WriteLine("Please enter string with: ");
                Console.WriteLine("1. 5 characters");
                Console.WriteLine("2. No repeating charaters:");
                Console.WriteLine("3. Alphanumeric .e.g JHSDL, HJAS3, 9J2TX");
                Console.WriteLine();
                stringToCompare = Console.ReadLine();
            }

            Console.WriteLine("###############################");

            ReadFile();           
            WriteResults();
            Console.ReadKey();
        }
        public static void ReadFile()
        {            
            try
            {                
                using (StreamReader sr = File.OpenText(fileName))
                {
                    readText = sr.ReadToEnd();                    
                }

                stringList = readText.Split(',').ToList();

                invalidStrings = stringList.Where(x => ValidateString(x) == false).Count();

                ProcessFile();
            }          
            catch (Exception ex)
            {                
                Console.WriteLine(ex.Message);                
            }
        }
        public static void ProcessFile()
        {
            Parallel.For(0, stringList.Count, x =>
            {
                FindEquivalent(stringList[x]);
            });
        }
        private static void FindEquivalent(string str)
        {
            int count = 0;
           
            foreach (var chr in stringToCompare.ToUpper().ToCharArray())
            {
                bool check = ValidateString(str);

                if (check)
                {
                    if (str.Contains(chr))
                    {
                        count++;
                    }
                }                
            }

            if (count == stringToCompare.Count())
            {
                myResults.AppendLine(str);
            }
        }
        private static bool ValidateString(string str)
        {            
            if (str.Length != 5 )
            {
                return false;
            }

            if (str.Distinct().Count() != 5)
            {
                return false;
            }

            Regex r = new Regex("^[a-zA-Z0-9]*$");
            if (!r.IsMatch(str))
            {
                return false;
            }

            return true;
        }

        public static void WriteResults()
        {
            int countResults = myResults.ToString().Split('\n').Length-1;

            if (countResults > 0)
            {
                Console.WriteLine();
                Console.WriteLine("###############################");
                Console.WriteLine(string.Format("Search String : {0}", stringToCompare.ToUpper()));
                Console.WriteLine(string.Format("Number of  String(s): {0}", stringList.Count()));
                Console.WriteLine(string.Format("Invalid String(s) : {0}", invalidStrings));
                Console.WriteLine(string.Format("Number of Equivalent String(s) : {0}", countResults));
                Console.WriteLine("Equivalent string(s) :");
                Console.WriteLine(myResults);
                Console.WriteLine();
                Console.WriteLine("###############################");
                Console.WriteLine("Done!");
            }
            else
            {
                Console.WriteLine("No Equivalent strings found.");
            }
        }
    }
}

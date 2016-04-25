using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace ConsoleApplication1
{
    class Program
    {
        public static string wildcardtoevoege = "";
        static void Main(string[] args)
        {
        
        Console.Title = "LeakedSource Lookup";

            Console.WriteLine("\n\r  ██╗     ███████╗\n\r  ██║     ██╔════╝\n\r  ██║     ███████╗\n\r  ██║     ╚════██║\n\r  ███████╗███████║ Leakedsource Lookup\n\r  ╚══════╝╚══════╝ By Kenny & Matthew!\n\r");

            doAgain:
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("  Search: ");
            Console.ForegroundColor = ConsoleColor.White;
            string searchString = Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n\r  [1] Username   [2] IP\n\r  [3] Email      [4] Name (First & Last)");
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("  Type: ");
            Console.ForegroundColor = ConsoleColor.White;
            string searchType = Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("  Use wildcard? [y/n]: ");
            Console.ForegroundColor = ConsoleColor.White;
            string wildCard = Console.ReadLine();


            if (searchType == "2")
            {
                searchType = "4";
            }
            else if (searchType == "3")
            {
                searchType = "5";
            }
            else if (searchType == "4")
            {
                searchType = "13";
            }

            var allTypes = new List<string>() { "1", "4", "5", "13" };
            if (!allTypes.Contains(searchType))
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("  Invalid input.");
                Console.WriteLine();
                goto doAgain;
            }

            var request = (HttpWebRequest)WebRequest.Create("http://www.leakedsource.com/");

            var postData = $"&search={searchString}";
            postData += $"&searchType={searchType}";
            postData += $"&submit=Search";
            
            if (wildCard == "y") 
            {
                postData += $"&wildCard=true";
            }
            
            var data = Encoding.ASCII.GetBytes(postData);
            
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            if (responseString.Contains("hidden result"))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine();

                string rektHtml = Regex.Replace(responseString.Replace("<br>", "\r\n"), @"<[^>]*?>", "");

                string RegMatch = "onds.(.*?)func";
                if (rektHtml.Contains("free of charge!"))
                {
                    RegMatch = "seconds.(.*?)The following";
                }
                
                foreach (Match watchTime in Regex.Matches(rektHtml, "Search completed in: (.*?) seconds.", RegexOptions.Singleline))
                {
                    var totalTime = watchTime.Groups[1];
                }

                foreach (Match allResults in Regex.Matches(rektHtml, RegMatch, RegexOptions.Singleline))
                {
                    String fuckHtml = allResults.Groups[1].Value.Replace("Subscribe today to unlock every result! As low as $0.76 a day!", "").Replace("hidden", "").Replace("This data was leaked on approximately ", "").Replace("has: ", ": ").Replace("More records found but ", "").Replace("00:00:00", "").Replace("0000-00-00", "").Replace(", 79+ databases in one", "").TrimEnd(Environment.NewLine.ToCharArray()).TrimStart(Environment.NewLine.ToCharArray());
                    Console.WriteLine(String.Join(Environment.NewLine, fuckHtml.Split(new String[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).Select(line => line.Split(':')).Select(items => String.Format("  {0,-30}| {1,3}", items[0].Trim(), Regex.Match(items[1], "[0-9]+").Value))));

                    if (RegMatch != "onds.(.*?)func")
                    {
                        Console.WriteLine("  Mate1.com                     |   ?");
                    }

                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;

                if (responseString.Contains("Anti spam triggered. Please wait a few seconds before trying again."))
                {
                    Console.WriteLine("  Anti spam triggered. Please wait a few seconds before trying again.");
                }
                else
                {
                    Console.WriteLine("  Account not found.");
                }
                Console.WriteLine();
            }

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"It took us only {totalTime} ms to look this up!");
            
            goto doAgain;
            
        }
    }
}

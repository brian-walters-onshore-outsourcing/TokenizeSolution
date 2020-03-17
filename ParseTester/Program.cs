using System;
using MyANTLRparser;

namespace ParseTester
{
    class Program
    {
        static void Main(string[] args)
        {
            double d = .1230;
            //Parser p = new Parser();
            // Parser p = new Parser("1 12 123 i++ ++i i-- --i a+b +a -b ");
            Parser p = new Parser(
               // ".1230 1e12 1.2e30 1.23e12"
               // " \t abc if _else then -1 +230 i++ --j \\ @ # $"  
               "@ # $"

              // ".1230"
                );
            p.Log = Console.Write;
            p.InitToCSharpStatemachine();

          

            p.ParseAll();
            Console.WriteLine("***************************");
            Console.WriteLine("*****Parsed Elements*******");
            foreach (var x in p.ParsedElements)
            {
                Console.WriteLine(x);
            }
            Console.WriteLine("***************************");
            Console.WriteLine("*********Tokens************");
            foreach (var x in p.ParsedTokens)
            {
                Console.WriteLine(x);
            }
            Console.WriteLine("***************************");
            //Console.ReadLine();
            //p.Reset(" \t abc if _else then -1 +230 i++ --j \\ @ # $");
            //p.ParseAll();
            //Console.WriteLine("***************************");
            //Console.WriteLine("*****Parsed Elements*******");
            //foreach (var x in p.ParsedElements)
            //{
            //    Console.WriteLine(x);
            //}
            //Console.WriteLine("***************************");
            //Console.WriteLine("*********Tokens************");
            //foreach (var x in p.ParsedTokens)
            //{
            //    Console.WriteLine(x);
            //}
            //Console.WriteLine("***************************");
            //Console.ReadLine();

        }
    }
}


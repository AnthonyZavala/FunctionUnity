using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FunctionUnity;

namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var x = new Class1();
            x.Run().Wait();

            Console.ReadKey();
        }
    }
}

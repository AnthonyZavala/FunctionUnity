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
            var x = new FuncUnityB();

            var widgetController = new WidgetController(x.Resolver);

            var one = widgetController.Get(1);

            widgetController.Delete(2);
            widgetController.Create(new Widget { Id = 5, Name = "five" });
            var created = widgetController.Get(5);
        }
    }
}

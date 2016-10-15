using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionUnity
{
    public static class AppFunctions
    {
        public static class WidgetRepository
        {
            public delegate Widget Get(int id);
            public delegate void Create(Widget widget);
            public delegate void Delete(int id);
        }

        public static class WidgetService
        {
            public delegate Widget Get(int id);
            public delegate void Create(Widget widget);
            public delegate void Delete(int id);
        }
    }
}

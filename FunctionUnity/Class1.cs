using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using System.Reflection;
using System.Linq.Expressions;

namespace FunctionUnity
{
    public class Class1
    {
        private readonly UnityContainer _container;

        public Class1()
        {
            _container = new UnityContainer();
            Map<Messenger, MessengerA>();
        }

        public void Map<TFrom, TTo>()
        {
            Type fromType = typeof(TFrom);
            IEnumerable<Type> delegateTypes = fromType.GetNestedTypes(BindingFlags.Public).Where(t => t.BaseType == typeof(MulticastDelegate));

            Type toType = typeof(TTo);
            var toStaticMethods = toType.GetMethods(BindingFlags.Public | BindingFlags.Static);

            foreach (Type del in delegateTypes)
            {
                var match = toStaticMethods.SingleOrDefault(x => x.Name == del.Name);
                if (match != null)
                {
                    _container.RegisterType(del, new InjectionFactory(c => Delegate.CreateDelegate(del, match)));
                }
            }
        }

        public async Task Run()
        {
            var get = _container.Resolve<Messenger.Get>();
            await get("Hello Del");
        }
    }

    public abstract class Messenger
    {
        public delegate Task Get(string message);
        public delegate Task Save(string message);
    }

    public class MessengerA
    {
        public async static Task Get(string message)
        {
            await Task.Run(() => Console.Write("A - " + message));
        }
    }

    public class MessengerB
    {
        public async static Task Get(string message)
        {
            await Task.Run(() => Console.Write("B - " + message));
        }
    }
}

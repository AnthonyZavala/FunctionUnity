using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using System.Reflection;
using System.Linq.Expressions;
using static FunctionUnity.AppFunctions;

namespace FunctionUnity
{
    public class FuncUnityB
    {
        private readonly UnityContainer _container;

        public FuncUnityB()
        {
            _container = new UnityContainer();
            Resolver = new ServiceResolver(_container);
            _container.RegisterInstance(new ConnectionStringProvider());
            _container.RegisterInstance<WidgetService.Get>(id => WidgetServiceImpl.Get(id, Resolver));
            _container.RegisterInstance<WidgetService.Create>(widget => WidgetServiceImpl.Create(widget, Resolver));
            _container.RegisterInstance<WidgetService.Delete>(id => WidgetServiceImpl.Delete(id, Resolver));
            _container.RegisterInstance<WidgetRepository.Get>(id => WidgetRepositoryImpl._Get(id, Resolver));
            _container.RegisterInstance<WidgetRepository.Create>(widget => WidgetRepositoryImpl._Create(widget, Resolver));
            _container.RegisterInstance<WidgetRepository.Delete>(id => WidgetRepositoryImpl._Delete(id, Resolver));
        }

        public readonly IServiceResolver Resolver;
        
        class ServiceResolver : IServiceResolver
        {
            private readonly UnityContainer _container;

            public ServiceResolver(UnityContainer container)
            {
                _container = container;
            }

            public T Get<T>()
            {
                return _container.Resolve<T>();
            }
        }
    }

    public interface IServiceResolver
    {
        T Get<T>();
    }

    public class WidgetController
    {
        private readonly IServiceResolver _resolver;

        public WidgetController(IServiceResolver resolver)
        {
            _resolver = resolver;
        }

        public Widget Get(int id)
        {
            return _resolver.Get<WidgetService.Get>()(id);
        }

        public void Delete(int id)
        {
            _resolver.Get<WidgetService.Get>()(id);
        }

        public void Create(Widget widget)
        {
            _resolver.Get<WidgetService.Create>()(widget);
        }
    }

    public static class WidgetServiceImpl
    {
        public static Widget Get(int id, IServiceResolver serviceResolver)
        {
            var getWidget = serviceResolver.Get<WidgetRepository.Get>();
            return getWidget(id);
        }

        public static void Create(Widget widget, IServiceResolver serviceResolver)
        {
            var createWidget = serviceResolver.Get<WidgetRepository.Create>();
            createWidget(widget);
        }

        public static void Delete(int id, IServiceResolver serviceResolver)
        {
            var deleteWidget = serviceResolver.Get<WidgetRepository.Delete>();
            deleteWidget(id);
        }
    }

    public static class WidgetRepositoryImpl
    {
        public static Widget _Get(int id, IServiceResolver serviceResolver)
        {
            Console.WriteLine($"Connecting using {serviceResolver.Get<ConnectionStringProvider>().Get}");
            return _widgets.First(x => x.Id == id);
        }

        public static void _Create(Widget widget, IServiceResolver serviceResolver)
        {
            Console.WriteLine($"Connecting using {serviceResolver.Get<ConnectionStringProvider>().Get}");
            _widgets.Add(widget);
        }

        public static void _Delete(int id, IServiceResolver serviceResolver)
        {
            Console.WriteLine($"Connecting using {serviceResolver.Get<ConnectionStringProvider>().Get}");
            _widgets.Remove(_widgets.First(x => x.Id == id));
        }

        private static List<Widget> _widgets = new List<Widget>()
        {
            new Widget {Id = 1, Name = "widgA" },
            new Widget {Id = 2, Name = "widgB" },
            new Widget {Id = 3, Name = "widgC" }
        };
    }

    public class Widget
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }

    public class ConnectionStringProvider
    {
        public string Get =>
#if DEBUG
            "connStr123";
#else
            "connStr456";
#endif

    }
}

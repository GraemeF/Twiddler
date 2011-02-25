namespace Twiddler
{
    #region Using Directives

    using System;
    using System.Collections.Generic;

    using Autofac;

    using Caliburn.Micro;

    #endregion

    public class TypedAutofacBootStrapper<TRootViewModel> : Bootstrapper<TRootViewModel>
    {
        private readonly ILog _logger = LogManager.GetLog(typeof(TypedAutofacBootStrapper<>));

        private IContainer _container;

        protected IContainer Container
        {
            get { return _container; }
        }

        protected override void BuildUp(object instance)
        {
            Container.InjectProperties(instance);
        }

        protected override void Configure()
        {
            var builder = new ContainerBuilder();

            builder.Register<IWindowManager>(c => new WindowManager()).InstancePerLifetimeScope();
            builder.Register<IEventAggregator>(c => new EventAggregator()).InstancePerLifetimeScope();

            ConfigureContainer(builder);

            _container = builder.Build();
        }

        protected virtual void ConfigureContainer(ContainerBuilder builder)
        {
        }

        protected override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return Container.Resolve(typeof(IEnumerable<>).MakeGenericType(serviceType)) as IEnumerable<object>;
        }

        protected override object GetInstance(Type serviceType, string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                if (Container.IsRegistered(serviceType))
                    return Container.Resolve(serviceType);
            }
            else if (Container.IsRegisteredWithName(key, serviceType))
                return Container.ResolveNamed(key, serviceType);
            throw new Exception(string.Format("Could not locate any instances of contract {0}.", 
                                              key ?? serviceType.Name));
        }
    }
}
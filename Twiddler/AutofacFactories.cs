﻿namespace Twiddler
{
    #region Using Directives

    using Autofac;

    #endregion

    public class AutofacFactories : Factories
    {
        private readonly IContainer _container;

        public AutofacFactories(IContainer container)
        {
            _container = container;
        }

        protected override TPart ComposePartWith<TPart, TImport>(TImport import)
        {
            using (ILifetimeScope lifetime = _container.BeginLifetimeScope(builder => builder.RegisterInstance(import)))
                return lifetime.Resolve<TPart>();
        }

        protected override void RegisterFactory<TFactory>(TFactory factory)
        {
            var builder = new ContainerBuilder();

            builder.RegisterInstance(factory);

            builder.Update(_container);
        }
    }
}
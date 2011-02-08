namespace Twiddler
{
    #region Using Directives

    using Microsoft.Practices.Unity;

    #endregion

    public class UnityFactories : Factories
    {
        private readonly IUnityContainer _container;

        public UnityFactories(IUnityContainer container)
        {
            _container = container;
        }

        protected override TPart ComposePartWith<TPart, TImport>(TImport import)
        {
            IUnityContainer childContainer = _container.CreateChildContainer();
            childContainer.RegisterInstance(import);

            return childContainer.Resolve<TPart>();
        }

        protected override void RegisterFactory<TFactory>(TFactory factory)
        {
            _container.RegisterInstance(factory);
        }
    }
}
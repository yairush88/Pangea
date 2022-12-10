using System;
using Unity;

namespace Pangea.Core.Helpers
{
    public class Factory
    {
        private static readonly Lazy<IUnityContainer> _lazyContainer = new Lazy<IUnityContainer>(() => new UnityContainer());

        public static IUnityContainer DIContainer => _lazyContainer.Value;
    }
}

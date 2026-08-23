using System.Runtime.CompilerServices;
using Zenject;

namespace Modules.Extensions
{
    public static class ZenjectExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DiContainer Install(this DiContainer container, Installer installer)
        {
            if (installer.IsEnabled)
            {
                container.Inject(installer);
                installer.InstallBindings();
            }

            return container;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DiContainer Install(this DiContainer container, Installer installer, params object[] extraArgs)
        {
            if (installer.IsEnabled)
            {
                container.Inject(installer, extraArgs);
                installer.InstallBindings();    
            }
            
            return container;
        }
    }
}
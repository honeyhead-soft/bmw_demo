using MauiReactor;

namespace MauiWanted.Components
{
    [Scaffold(typeof(CommunityToolkit.Maui.Behaviors.StatusBarBehavior))]
    partial class StatusBarBehavior
    {
        public CommunityToolkit.Maui.Behaviors.StatusBarBehavior GetNative() 
            => NativeControl ?? null;
    }
}

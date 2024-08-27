using MauiReactor;
using Microsoft.Maui.Platform;

namespace MauiWanted.Components.Native
{
    public class CustomEditor : MauiControls.Editor
    {
        public static void Configure()
        {
            Microsoft.Maui.Handlers.EditorHandler.Mapper.AppendToMapping(nameof(Editor), (handler, view) =>
            {
#if ANDROID
                handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);

                // 키보드가 화면을 가리지 않도록 설정
                var activity = Microsoft.Maui.ApplicationModel.Platform.CurrentActivity;
                activity?.Window?.SetSoftInputMode(Android.Views.SoftInput.AdjustResize);
#endif
            });
        }
    }
}

namespace MauiWanted.Components.Controls
{
    [Scaffold(typeof(Native.CustomEditor))]
    public partial class CustomEditor
    {
        public Native.CustomEditor GetNative() => NativeControl;
    }
}

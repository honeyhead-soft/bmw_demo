using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Behaviors;
using FFImageLoading.Maui;
using MauiReactor;
using MauiWanted.AppServices;
using MauiWanted.Pages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace MauiWanted
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiReactorApp<MainPage>(app =>
                {
                    app.AddResource("Resources/Styles/Colors.xaml");
                    app.AddResource("Resources/Styles/Styles.xaml");

                })
#if DEBUG
                .EnableMauiReactorHotReload()
                .OnMauiReactorUnhandledException(ex =>
                {
                   System.Diagnostics.Debug.WriteLine(ex.ExceptionObject);
                })
#endif
                .UseFFImageLoading()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("FontAwesome-Brands-400.otf", "FontAwsomeBrand");
                    fonts.AddFont("FontAwesome-Regular-400.otf", "FontAwsomeRegular");
                    fonts.AddFont("FontAwesome-Solid-400.otf", "FontAwsomeSolid");
                    fonts.AddFont("Pretendard-Black.otf", "PretendardBlack");
                    fonts.AddFont("Pretendard-Bold.otf", "PretendardBold");
                    fonts.AddFont("Pretendard-ExtraBold.otf", "PretendardExtraBold");
                    fonts.AddFont("Pretendard-ExtraLight.otf", "PretendardExtraLight");
                    fonts.AddFont("Pretendard-Light.otf", "PretendardLight");
                    fonts.AddFont("Pretendard-Medium.otf", "PretendardMedium");
                    fonts.AddFont("Pretendard-Regular.otf", "PretendardRegular");
                    fonts.AddFont("Pretendard-SemiBold.otf", "PretendardSemiBold");
                    fonts.AddFont("Pretendard-Thin.otf", "PretendardThin");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<DebugService>();
            builder.Services.AddSingleton<ComponentCacheService>();
            return builder.Build();
        }
    }
}
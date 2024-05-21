using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Supabase;

namespace SupabaseMauiDemo
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            Preferences.Set("SupabaseURL", "https://test.com"); //Supabase 주소 입력
            Preferences.Set("SupabaseAnonKey", "testkey");  //Supabase 키 입력
            var options = new SupabaseOptions
            {
                AutoRefreshToken = true,
            };
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Supabase;
using Supabase.Interfaces;
using Supabase.Realtime.PostgresChanges;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Supabase.Realtime.PostgresChanges.PostgresChangesOptions;

namespace SupabaseMauiDemo.PageModels
{
    public partial class MainPageViewModel : ObservableObject
    {
        [ObservableProperty]
        ObservableCollection<string> chatList;

        [ObservableProperty]
        string messageContent;

        Supabase.Client currentClient;

        public MainPageViewModel()
        {
            ChatList = new ObservableCollection<string>();
            MessageContent = String.Empty;

            InitService().ConfigureAwait(false);
        }
        private async Task InitService()
        {
            try
            {
                var options = new SupabaseOptions
                {
                    AutoRefreshToken = true,
                    AutoConnectRealtime = true,
                };
                currentClient = new Supabase.Client(Preferences.Get("SupabaseURL", ""), Preferences.Get("SupabaseAnonKey", ""),options);
                await currentClient.Realtime.ConnectAsync();



                var channel = currentClient.Realtime.Channel("realtime", "public", "testmsg");
                channel.Register(new PostgresChangesOptions("public", "testmsg"));

                //방식 1
                await currentClient.From<testmsg>().On(ListenType.All, (sender, change) =>
                {
                    var work = change.Model<testmsg>();
                    if (work !=null)
                    {
                        ChatList.Add($"방식 1 : {work.content}");
                    }
                    else
                    {
                        if(change.Payload.Data.Type == Supabase.Realtime.Constants.EventType.Delete)
                        {

                        }                        
                    }
                });

                //방식 2                
                channel.AddPostgresChangeHandler(Supabase.Realtime.PostgresChanges.PostgresChangesOptions.ListenType.Inserts, (sender, change) =>
                {
                    var work = change.Model<testmsg>();
                    if (work != null)
                    {
                        ChatList.Add($"방식 2 : {work.content}");
                    }
                });

                channel.AddPostgresChangeHandler(Supabase.Realtime.PostgresChanges.PostgresChangesOptions.ListenType.Updates, (sender, change) =>
                {
                    var work = change.Model<testmsg>();
                    ChatList.Add(work.content);
                });

                await channel.Subscribe();
            }
            catch (Exception ex)
            {
            }
        }

        [RelayCommand]
        async Task SendMessage()
        {
            if (!String.IsNullOrEmpty(MessageContent))
            {
                await currentClient.From<testmsg>().Insert(new testmsg
                {
                    content = MessageContent,
                });
                MessageContent = "";
            }
        }
    }
}

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Nakama;
using Nakama.TinyJson;
using System.Collections.ObjectModel;
using System.Threading.Channels;

namespace NakamaMauiDemo.PageModels
{
    public partial class MainPageViewModel : ObservableObject
    {
        [ObservableProperty]
        ObservableCollection<string> chatList;

        [ObservableProperty]
        string messageContent;

        [ObservableProperty]
        bool isSocketOpen;

        Nakama.Client client;
        Nakama.ISocket socket;
        Nakama.IChannel channelId;
        Nakama.ISession session;

        [ObservableProperty]
        string groupId;

        public MainPageViewModel()
        {
            ChatList = new ObservableCollection<string>();
            MessageContent = String.Empty;

            InitService().ConfigureAwait(false);
        }

        private async Task InitService()
        {
            string scheme = "http";
            string host = "127.0.0.1"; //이곳에 Nakama 주소를 넣어주세요
            int port = 7350; //이곳에 포트를 넣어주세요
            string serverKey = "defaultkey"; //서버의 키를 입력해주세요
            client = new Client(scheme, host, port, serverKey);
            socket = Socket.From(client);



            //GroupId = "afb7c7ce-2ffb-4b68-b2a8-5256e10d4950";

            socket.Connected += Socket_Connected;
            socket.Closed += Socket_Closed;
            socket.ReceivedChannelMessage += Socket_ReceivedChannelMessage;
            socket.ReceivedError += Socket_ReceivedError;
            //string result = await App.Current.MainPage.DisplayPromptAsync("Notice", "Enter Chat Group Id");
            //if (!String.IsNullOrEmpty(result))
            //{
            //    session = await GetSession(client, "honeyhead@naver.com", "test1234!");
            //    await socket.ConnectAsync(session);
            //}
        }

        private void Socket_ReceivedError(Exception obj)
        {
            ChatList.Add($"Socket Error : {obj.Message}");
        }

        private void Socket_ReceivedChannelMessage(IApiChannelMessage obj)
        {
            ChatList.Add(obj.Content);
        }

        private void Socket_Closed()
        {
            ChatList.Add("Socket closed");
        }

        private async void Socket_Connected()
        {
            try
            {
                ChatList.Add($"Socket Connected");
                IsSocketOpen = true;
                channelId = await socket.JoinChatAsync(GroupId, ChannelType.Group, true, false);

                var chatData = await client.ListChannelMessagesAsync(session, channelId, 30);
                foreach (var item in chatData.Messages)
                {
                    if (!item.Content.Contains("{}"))
                    {
                        ChatList.Add($"[{item.CreateTime}]{item.Username} : {item.Content}");
                    }
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Notice", ex.Message, "OK");
            }
        }

        private void DisplayMessage(string msg)
        {
            if (!String.IsNullOrEmpty(msg)) ChatList.Add(msg);
        }
        [RelayCommand]
        async Task PopupPromprt()
        {
            string result = await App.Current.MainPage.DisplayPromptAsync("Notice", "Enter Chat Group Id");
            if (!String.IsNullOrEmpty(result))
            {
                GroupId = result;
                var session = await GetSession(client, "test@email.com", "password");  //이 곳에 계정정보를 입력해주세요
                await socket.ConnectAsync(session);
            }
        }

        [RelayCommand]
        async Task SendMessage()
        {
            if (!String.IsNullOrEmpty(MessageContent))
            {

                var msgData = new Dictionary<string, string> { { "message", MessageContent } };
                var messageSendAck = await socket.WriteChatMessageAsync(channelId, JsonWriter.ToJson(msgData));
                MessageContent = "";
            }
        }

        private async Task<ISession> GetSession(Client _client, string email, string password)
        {
            var vars_custom = new Dictionary<string, string>();
            vars_custom["AppName"] = "TESTAPP";
            vars_custom["DeviceModel"] = "Android";
            var session = await _client.AuthenticateEmailAsync(email, password, vars: vars_custom);
            return session;
        }
    }
}

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppMauiDemo.PageModels
{
    public partial class MainPageViewModel : ObservableObject
    {
        [ObservableProperty]
        ObservableCollection<string> chatList;

        [ObservableProperty]
        string messageContent;

        ISubscriber subscriber;
        string _connectionAddress = "127.0.0.1"; //REDIS 주소
        string _connectionPassword = "password"; //REDIS 암호
        string _channelName = "test"; //채널 이름

        public MainPageViewModel()
        {
            ChatList = new ObservableCollection<string>();
            MessageContent = String.Empty;

            InitService().ConfigureAwait(false);
        }

        private async Task InitService()
        {

            ConfigurationOptions opt = new ConfigurationOptions
            {
                Ssl = false,
                EndPoints = new EndPointCollection { _connectionAddress },
                Password = _connectionPassword,

            };

            ConnectionMultiplexer _connectionMultiplexer = ConnectionMultiplexer.Connect(opt);
            subscriber = _connectionMultiplexer.GetSubscriber();

            await subscriber.SubscribeAsync(_channelName, (channel, message) => DisplayMessage(message));
            await subscriber.PublishAsync(_channelName, $"APP User has joined room {_channelName}");
        }

        private void DisplayMessage(string msg)
        {
            if(!String.IsNullOrEmpty(msg)) ChatList.Add(msg);
        }

        [RelayCommand]
        async Task SendMessage()
        {
            if (!String.IsNullOrEmpty(MessageContent))
            {
                await subscriber.PublishAsync(_channelName, $"[{DateTime.Now.Hour}:{DateTime.Now.Second}] APP : {MessageContent}");
                MessageContent = "";
            }
        }
    }
}

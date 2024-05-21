// See https://aka.ms/new-console-template for more information

using System.Runtime.CompilerServices;
using StackExchange.Redis;

string _connectionAddress = "127.0.0.1"; //REDIS 주소 입력
string _connectionPassword = "password"; //REDIS 암호 입력 (아이디가 있다면 별도 기입)
ConfigurationOptions opt = new ConfigurationOptions
{
    Ssl = false,
    EndPoints = new EndPointCollection { _connectionAddress },
    Password = _connectionPassword,
    
};
ConnectionMultiplexer _connectionMultiplexer = ConnectionMultiplexer.Connect(opt);
string? _userName, _channelName;




Console.WriteLine("Hello, BMW! Enter your name");
_userName = Console.ReadLine();

Console.WriteLine("Enter your chat room name");
_channelName = Console.ReadLine();

Console.Title = $"Hello {_userName}, Your are in chat room {_channelName}";

var pubSub = _connectionMultiplexer.GetSubscriber();
await pubSub.SubscribeAsync(_channelName, (channel, message) => DisplayMessage(message));

await pubSub.PublishAsync(_channelName, $"{_userName} has joined room {_channelName}");

while (true)
{
    var message = Console.ReadLine();

    await pubSub.PublishAsync(_channelName, $"[{DateTime.Now.Hour}:{DateTime.Now.Second}] {_userName} : {message}");
}



async void DisplayMessage(RedisValue msg)
{
    await Console.Out.WriteLineAsync(msg);
}
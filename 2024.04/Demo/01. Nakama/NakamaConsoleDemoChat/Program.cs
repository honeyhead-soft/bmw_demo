// See https://aka.ms/new-console-template for more information
using Nakama;
using Nakama.TinyJson;
const string scheme = "http"; 
const string host = "127.0.0.1"; //이곳에 Nakama 주소를 넣어주세요
const int port = 7350; //이곳에 포트를 넣어주세요
const string serverKey = "defaultkey"; //서버의 키를 입력해주세요
var client = new Client(scheme, host, port, serverKey);
bool isSocketOpen =false;
IChannel channelId =null;

Console.WriteLine("Hello, World!");
Console.Write("Insert Group Name : ");
var _groupId = Console.ReadLine();
var socket = Socket.From(client);

socket.Connected += Socket_Connected;
socket.Closed += Socket_Closed;
socket.ReceivedChannelMessage += Socket_ReceivedChannelMessage;
socket.ReceivedError += Socket_ReceivedError;

var _session = await GetSession(client, "test@email.com", "password"); //이 곳에 계정정보를 입력해주세요
await socket.ConnectAsync(_session);

while (true)
{
    if (isSocketOpen)
    {
        Console.Write("Enter Message : ");
        var msg = Console.ReadLine();
        var messageContent = new Dictionary<string, string> { { "message", msg } };
        var messageSendAck = await socket.WriteChatMessageAsync(channelId, JsonWriter.ToJson(messageContent));
    }
    
}

Console.ReadLine();


async void Socket_Connected()
{
    await Console.Out.WriteLineAsync("Socket Connected");
    isSocketOpen = true;
    channelId = await socket.JoinChatAsync(_groupId, ChannelType.Group, true, false);
}

async void Socket_Closed()
{
    await System.Console.Out.WriteLineAsync("Socket closed.");
}

async void Socket_ReceivedChannelMessage(IApiChannelMessage obj)
{
    await Console.Out.WriteLineAsync(obj.Content);
}
async void Socket_ReceivedError(Exception obj)
{
    await Console.Out.WriteLineAsync(obj.Message);
}

static async Task<ISession> GetSession(Client _client, string email, string password)
{
    var vars_custom = new Dictionary<string, string>();
    vars_custom["AppName"] = "TESTAPP";
    vars_custom["DeviceModel"] = "Android";
    var session = await _client.AuthenticateEmailAsync(email, password, vars: vars_custom);
    return session;
}


using Nakama;
using Nakama.TinyJson;

Console.WriteLine("Hello, World!");

const string scheme = "http";
const string host = "127.0.0.1"; //이곳에 Nakama 주소를 넣어주세요
const int port = 7350; //이곳에 포트를 넣어주세요
const string serverKey = "defaultkey"; //서버의 키를 입력해주세요
var client = new Client(scheme, host, port, serverKey);


//사용자 신규 가입 및 세션 가져오기
var session = await GetSession(client, "test@email.com", "password"); //이 곳에 계정정보를 입력해주세요

//사용자 정보 업데이트
await UpdateUserInfo(client, session);

//기존 사용자 정보에 디바이스 아이디 추가
await UpdateDeviceId(client, session);

//사용자 정보 가져오기
var userInfo = await GetUserInfo(client, session);

#region STEP 1
//그룹 생성
var groupInfo = await CreateGroup(client, session, "TEST");


//새로운 더미 유저 생성
var userList = await CreateMultipleUser(client);
await JoinGroupUsers(client, session, groupInfo.Id, userList);
Console.ReadLine();
#endregion

#region STEP 2
//await NewChatInGroup(client, session, (await client.ListGroupsAsync(session, name: "TEST")).Groups.First().Id, "테스트 내용은 여기에 작성합니다");
//Console.ReadLine();
#endregion




static async Task<ISession> GetSession(Client _client, string email, string password)
{
    var vars_custom = new Dictionary<string, string>();
    vars_custom["AppName"] = "TESTAPP";
    vars_custom["DeviceModel"] = "Android";
    var session = await _client.AuthenticateEmailAsync(email, password, vars: vars_custom);
    return session;
}

static async Task<bool> UpdateUserInfo(Client _client, ISession _session)
{
    try
    {
        await _client.UpdateAccountAsync(
        session: _session,
        username: "blackcowtest", //사용자 이름
        displayName: "blackcowmaster", //프로필 이름
        avatarUrl: "https://pds.joongang.co.kr/news/component/htmlphoto_mmdata/202402/10/18516afa-9d48-4a95-8aef-5a1883d32184.jpg", //프로필 사진
        location: "Seoul", //지역
        langTag: "KOR" //언어 태그
        );

        return true;
    }
    catch (Exception ex)
    {
        await Console.Out.WriteLineAsync(ex.Message);
    }

    return false;
}

static async Task<bool> UpdateDeviceId(Client _client, ISession _session)
{
    try
    {
        await _client.LinkDeviceAsync(_session, Guid.NewGuid().ToString());

        return true;
    }
    catch (Exception ex)
    {
        await Console.Out.WriteLineAsync(ex.Message);
    }
    return false;
}

static async Task<IApiAccount> GetUserInfo(Client _client, ISession _session)
{
    return await _client.GetAccountAsync(_session);
}

static async Task<IApiGroup> CreateGroup(Client _client, ISession _session, string groupName, string desc = "This is TEST Group")
{
    return await _client.CreateGroupAsync(_session, groupName, desc, langTag: "KO", open: true, maxCount: 200);
}

static async Task<List<string>> CreateMultipleUser(Client _client)
{
    var vars_custom = new Dictionary<string, string>();
    vars_custom["AppName"] = "TESTAPP";
    vars_custom["DeviceModel"] = "Android";
    List<string> idSet = new List<string>();
    for (int i = 0; i < 10; i++)
    {
        var newGuid = Guid.NewGuid().ToString();
        string customUserName = "User_" + Random.Shared.Next(1, 9999);
        var session = await _client.AuthenticateDeviceAsync(newGuid, username: customUserName, vars: vars_custom);
        idSet.Add(session.UserId);
        await _client.UpdateAccountAsync(session, customUserName, displayName: customUserName);
    }
    return idSet;
}

static async Task<bool> JoinGroupUsers(Client _client, ISession _session, string _groupId, List<string> _userIdSet)
{
    try
    {
        await _client.AddGroupUsersAsync(_session, _groupId, _userIdSet);
        return true;
    }
    catch (Exception ex)
    {
        await Console.Out.WriteLineAsync(ex.Message);
    }
    return false;
}

static async Task<bool> NewChatInGroup(Client _client, ISession _session, string _groupId, string msg="TEST")
{
    try
    {
        var persistence = true;
        var hidden = false;

        var socket = Socket.From(_client);
        socket.Connected += async () =>
        {
            System.Console.WriteLine("Socket connected.");
            var channelId = await socket.JoinChatAsync(_groupId, ChannelType.Group, persistence, hidden);
            var messageContent = new Dictionary<string, string> { { "message", msg } };
            var messageSendAck = await socket.WriteChatMessageAsync(channelId, JsonWriter.ToJson(messageContent));
        };
        socket.ReceivedChannelMessage += e =>
        {
            Console.WriteLine(e.Content);
        };
        socket.Closed += () =>
        {
            System.Console.WriteLine("Socket closed.");
        };
        socket.ReceivedError += e => System.Console.WriteLine(e);
        await socket.ConnectAsync(_session);

        return true;
    }
    catch (Exception ex)
    {
        return false;
    }
}
using Dpoch.SocketIO;
using Chat;
using Newtonsoft.Json;
using Signals;
using UnityEngine;
using Zenject;

namespace Chat
{
    public enum ChatType
    {
        global,
    }
    
    public class BackEndChatService : IInitializable
    {
        private string IP = "127.0.0.1";
        private string PORT = "3000";
        private string SERVICE_NAME = "/Chat";
        private string url = "ws://127.0.0.1:6000/socket.io/?EIO=4&transport=websocket";

        private SocketIO _socket;

        [Inject] private SignalBus _signalBus;

        public void Initialize()
        {
            _socket = new SocketIO(url);
            _socket.OnOpen += () => Debug.Log("SOCKET OPEN!");
            _socket.OnError += ex => Debug.Log("er: " + ex);
            _socket.On("connect", ev =>
            {
                Debug.Log("CONNECT!!");
            });
            
            _socket.On("receive history", message =>
            {
                var chatDataArray = JsonConvert.DeserializeObject<ChatData[]>(message.Data[0].ToString());

                foreach (var chatData in chatDataArray)
                    _signalBus.Fire(new MessageSignal {recieveData = chatData});
            });

            _socket.On("receive message", message =>
            {
                var chatData = JsonConvert.DeserializeObject<ChatData>(message.Data[0].ToString());
                _signalBus.Fire(new MessageSignal {recieveData = chatData});
            });

            _socket.Connect();
        }

        public void SendMessage(ChatData chatData)
        {
            if (!_socket.IsAlive)
                return;
            
            var json = JsonConvert.SerializeObject(chatData);

            _socket.Emit("chat", json);
        }
    }
}
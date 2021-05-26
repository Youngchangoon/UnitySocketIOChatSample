using System;
using UniRx;

namespace Chat
{
    [Serializable]
    public class ChatData
    {
        public ChatType chatType;
        public string nickname;
        public string message;
    }
    
    public class ChatModel
    {
        public ReactiveCollection<ChatData> chatList;

        public ChatModel()
        {
            chatList = new ReactiveCollection<ChatData>();
        }
    }
}
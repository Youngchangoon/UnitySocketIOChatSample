using Signals;
using UniRx;
using UnityEngine;
using Zenject;

namespace Chat
{
    public class ChatService : IInitializable
    {
        [Inject] private ChatServerService _backendChatServerService;
        [Inject] private ChatModel _chatModel;
        [Inject] private ChatView _chatView;
        [Inject] private SignalBus _signalBus;
        
        public string UserNickname { get; set; }

        private ReactiveCollection<ChatData> _chatList;
        private int _chatCountLimit = 10;

        public void Initialize()
        {
            _chatList = _chatModel.chatList;
            
            _chatList.ObserveAdd().Subscribe(chatObject => _chatView.AddChatObject(chatObject.Value));
            _chatList.ObserveRemove().Subscribe(chatObject => _chatView.RemoveObject(chatObject.Value));

            _signalBus.Subscribe<MessageSignal>(signal => ReceiveMessageFromServer(signal.recieveData));
        }

        public void Send(ChatData chatData)
        {
            chatData.nickname = UserNickname;
            
            _backendChatServerService.SendMessage(chatData);
        }

        private void ReceiveMessageFromServer(ChatData newChat)
        {
            _chatList.Add(newChat);
            
            for (var i = _chatList.Count - 1; i >= 0; --i)
            {
                if (i >= _chatCountLimit)
                    _chatList.RemoveAt(0);
            }
        }
    }
}
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Chat
{
    public class ChatView : MonoBehaviour
    {
        [SerializeField] private ChatObject chatPrefab;
        [SerializeField] private Transform chatContent;
        [SerializeField] private InputField inputField;

        [Inject] private ChatService _chatService;

        private List<ChatObject> _chatObjectList = new List<ChatObject>();

        public void AddChatObject(ChatData chatData)
        {
            var newChatObject = Instantiate(chatPrefab, chatContent, true);

            newChatObject.UpdateData(chatData);

            _chatObjectList.Add(newChatObject);
        }

        public void RemoveObject(ChatData chatData)
        {
            var removeChatObject = _chatObjectList.Find(obj => obj.MyChatData == chatData);

            if (removeChatObject == null)
                return;

            Destroy(removeChatObject.gameObject);

            _chatObjectList.Remove(removeChatObject);
        }
        
        public void OnPressedSendButton()
        {
            _chatService.Send(new ChatData {message = inputField.text});
            inputField.text = string.Empty;
        }
    }
}
using UnityEngine;
using UnityEngine.UI;

namespace Chat
{
    public class ChatObject : MonoBehaviour
    {
        [SerializeField] private Text nicknameText;
        [SerializeField] private Text msgText;
        
        public ChatData MyChatData { get; private set; }

        public void UpdateData(ChatData chatData)
        {
            MyChatData = chatData;
            
            nicknameText.text = chatData.nickname;
            msgText.text = chatData.message;
        }
    }
}
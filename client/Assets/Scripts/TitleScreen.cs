using System.Collections;
using System.Collections.Generic;
using Chat;
using Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class TitleScreen : MonoBehaviour, IInitializable
{
    [SerializeField] private InputField inputField;
    
    [Inject] private ChatServerService _chatServerService;
    [Inject] private ChatService _chatService;
    [Inject] private SignalBus _signalBus;
    [Inject] private ChatScreen _chatScreen;
    
    public void Initialize()
    {
        _signalBus.Subscribe<SocketSuccessSignal>(() =>
        {
            gameObject.SetActive(false);
            _chatScreen.gameObject.SetActive(true);
        });
    }
    
    public void OnPressedConnectButton()
    {
        _chatService.UserNickname = inputField.text;
        _chatServerService.Connect();
    }
}

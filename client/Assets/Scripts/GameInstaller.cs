using Chat;
using Signals;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<ChatModel>().AsSingle();

        Container.BindInterfacesAndSelfTo<ChatServerService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<ChatService>().AsSingle().NonLazy();

        // View
        Container.Bind<ChatView>().FromInstance(FindObjectOfType<ChatView>(true));
        
        // Objects
        Container.BindInterfacesAndSelfTo<TitleScreen>().FromInstance(FindObjectOfType<TitleScreen>(true));
        Container.BindInterfacesAndSelfTo<ChatScreen>().FromInstance(FindObjectOfType<ChatScreen>(true));
        
        // Signals
        Container.DeclareSignal<MessageSignal>();
        Container.DeclareSignal<SocketSuccessSignal>();
        Container.DeclareSignal<SocketFailSignal>();
        
        SignalBusInstaller.Install(Container);
    }
}
using Chat;
using Signals;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<ChatModel>().AsSingle();

        Container.BindInterfacesAndSelfTo<BackEndChatService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<ChatService>().AsSingle().NonLazy();

        // View
        Container.Bind<ChatView>().FromInstance(FindObjectOfType<ChatView>());
        
        // Signals
        Container.DeclareSignal<MessageSignal>();
        
        SignalBusInstaller.Install(Container);
    }
}
using System.Collections;
using UnityEngine;
using Zenject;

namespace Assets.Scripts
{
    public class GameInstaller : MonoInstaller
    {

        public override void InstallBindings()
        {
            // Bind the HighScoreManager to the IHighScoreManager interface
            Container.Bind<IHighScoreManager>().To<HighScoreManager>().AsSingle();

            // Bind the PlatformHandler to the correct platform-specific implementation
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                Container.Bind<IPlatformHandler>().To<WebGLPlatformHandler>().AsSingle();
            }
            else
            {
                Container.Bind<IPlatformHandler>().To<DefaultPlatformHandler>().AsSingle();
            }
        }
    }
}
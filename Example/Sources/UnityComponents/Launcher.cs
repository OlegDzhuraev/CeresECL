using UnityEngine;

namespace CeresECL.Example
{
    public class Launcher : MonoBehaviour
    {
        [SerializeField] GameData gameData;
        
        void Start()
        {
            var mainCamera = Camera.main;
            
            var playerEnt = Entity.Spawn<PlayerEntityBuilder>(gameData.PlayerPrefab);
            
            playerEnt.Logics.Inject(gameData);
            playerEnt.Logics.Inject(mainCamera);
        }
    }
}
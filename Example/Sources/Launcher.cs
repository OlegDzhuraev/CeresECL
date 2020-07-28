using UnityEngine;

namespace CeresECL.Example
{
    public class Launcher : MonoBehaviour
    {
        [SerializeField] GameData gameData;
        
        void Start()
        {
            Entity.Spawn<PlayerEntityBuilder>(gameData.PlayerPrefab);
        }
    }
}
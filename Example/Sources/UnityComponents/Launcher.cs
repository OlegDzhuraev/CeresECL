using UnityEngine;

namespace CeresECL.Example
{
    public class Launcher : MonoBehaviour
    {
        [SerializeField] GameData gameData;
        
        void Start()
        {
            CeresSettings.Instance.TagsEnum = typeof(Tag);
            
            var mainCamera = Camera.main;
            
            // it is better to spawn system, which will handle all game start process.

            var playerEnt = Entity.Spawn<PlayerEntityBuilder>(gameData.PlayerPrefab);
            
            playerEnt.Logics.Inject(gameData);
            playerEnt.Logics.Inject(mainCamera);

            for (int i = 0; i < 3; i++)
            {
                var enemyEnt = Entity.Spawn<EnemyEntityBuilder>(gameData.EnemyPrefab);

                enemyEnt.Logics.Inject(gameData);
                enemyEnt.Logics.Inject(mainCamera);
                
                enemyEnt.transform.position = new Vector2(Random.Range(-3f, 3f), Random.Range(-3f, 3f));
            }
        }
    }
}
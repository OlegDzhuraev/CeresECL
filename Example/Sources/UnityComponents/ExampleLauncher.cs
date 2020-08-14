using UnityEngine;

namespace CeresECL.Example
{
    public class ExampleLauncher : Launcher
    {
        [SerializeField] GameData gameData;
        
        protected override void StartAction()
        {
            CeresSettings.Instance.TagsEnum = typeof(Tag);
            
            var mainCamera = Camera.main;
            
            // it is better to spawn system, which will handle all game start process.

            var playerEnt = Entity.Spawn<PlayerEntity>(gameData.PlayerPrefab);
            
            playerEnt.Logics.Inject(gameData);
            playerEnt.Logics.Inject(mainCamera);

            for (int i = 0; i < 3; i++)
            {
                var enemyEnt = Entity.Spawn<EnemyEntity>(gameData.EnemyPrefab);

                enemyEnt.Logics.Inject(gameData);
                enemyEnt.Logics.Inject(mainCamera);
                
                enemyEnt.Transform.position = new Vector2(Random.Range(-3f, 3f), Random.Range(-3f, 3f));
            }
        }
    }
}
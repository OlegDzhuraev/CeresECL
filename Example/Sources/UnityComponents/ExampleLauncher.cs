using UnityEngine;

namespace CeresECL.Example
{
    public class ExampleLauncher : Launcher
    {
        [SerializeField] GameData gameData;
        
        protected override void StartAction()
        {
            CeresSettings.Instance.TagsEnum = typeof(Tag);
            
            // it is better to spawn system, which will handle all game start process.

            Entity.Spawn(gameData.PlayerPrefab);

            for (int i = 0; i < 3; i++)
            {
                var enemyEnt = Entity.Spawn(gameData.EnemyPrefab);

                enemyEnt.transform.position = new Vector2(Random.Range(-3f, 3f), Random.Range(-3f, 3f));
            }
        }
    }
}
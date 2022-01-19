using FrameworkDesign;
using UnityEngine;
namespace ShootingEditor2D.Command
{
    public class KillEnemyCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.GetSystem<IStatSystem>().KillCount.Value++;
            var randomIndex = Random.Range(0, 10);
            if (randomIndex < 8) this.GetSystem<IGunSystem>().CurrentGunInfo.BulletCountInGun.Value += Random.Range(1, 4);
        }
    }
}
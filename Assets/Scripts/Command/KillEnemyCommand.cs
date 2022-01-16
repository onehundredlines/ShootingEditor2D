using FrameworkDesign;
namespace ShootingEditor2D.Command
{
    public class KillEnemyCommand : AbstractCommand
    {
        protected override void OnExecute() => this.GetSystem<IStatSystem>().KillCount.Value++;
    }
}
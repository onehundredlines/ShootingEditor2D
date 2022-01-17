using FrameworkDesign;
namespace ShootingEditor2D.Command
{
    public class HurtPlayerCommand : AbstractCommand
    {
        private readonly int mHurt;
        public HurtPlayerCommand(int hurt = 1)
        {
            mHurt = hurt;
        }
        protected override void OnExecute() => this.GetModel<IPlayerModel>().Hp.Value-=mHurt;
    }
}
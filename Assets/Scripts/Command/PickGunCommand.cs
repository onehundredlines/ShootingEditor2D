using QFramework;
namespace ShootingEditor2D
{
    public class PickGunCommand : AbstractCommand
    {
        private readonly string mName;
        private readonly int mBulletInGun;
        private readonly int mBulletOutGun;
        public PickGunCommand(string name, int bulletInGun, int bulletOutGun)
        {
            mName = name;
            mBulletInGun = bulletInGun;
            mBulletOutGun = bulletOutGun;
        }
        protected override void OnExecute() => this.GetSystem<IGunSystem>().PickGun(mName, mBulletInGun, mBulletOutGun);
    }
}
using QFramework;
namespace ShootingEditor2D
{
    public class ShootCommand : AbstractCommand
    {
        //优化，添加单例缓存，减少new的次数，从而减少GC
        //因为没有参数，这样写也行
        //如果对命令系统做了一些记录,比如说记录了执行了那些命令的时候，这样写会有问题
        public static readonly ShootCommand Single = new ShootCommand();
        protected override void OnExecute()
        {
            var gunSystem = this.GetSystem<IGunSystem>();
            gunSystem.CurrentGunInfo.State.Value = GunState.Shooting;
            gunSystem.CurrentGunInfo.BulletCountInGun.Value--;
            var gunConfigItem = this.GetModel<IGunConfigModel>().GetItemByName(gunSystem.CurrentGunInfo.Name.Value);
            this.GetSystem<ITimeSystem>().AddTimeDelay(gunConfigItem.Frequency, () =>
            {
                if (gunSystem.CurrentGunInfo.BulletCountInGun.Value <= 0) this.SendCommand<ReloadCommand>();
                else gunSystem.CurrentGunInfo.State.Value = GunState.Idle;
            });
        }
    }
}
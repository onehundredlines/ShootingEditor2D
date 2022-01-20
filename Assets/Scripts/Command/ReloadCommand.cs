using FrameworkDesign;
namespace ShootingEditor2D
{
    public class ReloadCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            //当前枪的数据
            var currentGun = this.GetSystem<IGunSystem>().CurrentGunInfo;
            //当前枪的配置数据
            var gunConfigItem = this.GetModel<IGunConfigModel>().GetItemByName(currentGun.Name.Value);
            //需要的子弹数量
            var needBulletCount = gunConfigItem.BulletMaxCount - currentGun.BulletCountInGun.Value;
            if (needBulletCount > 0)
            {
                if (currentGun.BulletCountOutGun.Value > 0)
                {
                    //状态切换
                    currentGun.State.Value = GunState.Reload;
                    //状态切回
                    this.GetSystem<ITimeSystem>().AddTimeDelay(gunConfigItem.ReloadSeconds, () =>
                    {
                        currentGun.State.Value = GunState.Idle;
                        //如果枪外子弹充足
                        if (currentGun.BulletCountOutGun.Value >= needBulletCount)
                        {
                            currentGun.BulletCountInGun.Value += needBulletCount;
                            currentGun.BulletCountOutGun.Value -= needBulletCount;
            
                        }
                        //如果不充足，就全部填弹
                        else
                        {
                            currentGun.BulletCountInGun.Value += currentGun.BulletCountOutGun.Value;
                            currentGun.BulletCountOutGun.Value = 0;
                        }
                    });
                }
            }
        }
    }
}
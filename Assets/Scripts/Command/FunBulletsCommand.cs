using FrameworkDesign;
namespace ShootingEditor2D
{
    public class FunBulletsCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            var gunSystem = this.GetSystem<IGunSystem>();
            var gunConfigModel = this.GetModel<IGunConfigModel>();
            //填满当前枪的弹夹
            gunSystem.CurrentGunInfo.BulletCountInGun.Value = gunConfigModel.GetItemByName(gunSystem.CurrentGunInfo.Name.Value).BulletMaxCount;
            //填满缓存的所有枪的弹夹
            foreach(var info in gunSystem.GunInfos) info.BulletCountInGun.Value = gunConfigModel.GetItemByName(info.Name.Value).BulletMaxCount;
        }
    }
}
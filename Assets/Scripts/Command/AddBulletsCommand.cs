using FrameworkDesign;
namespace ShootingEditor2D
{
    public class AddBulletsCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            var gunSystem = this.GetSystem<IGunSystem>();
            var gunConfigModel = this.GetModel<IGunConfigModel>();
            //当前枪添加一个弹夹的弹药量
            AddBullets(gunSystem.CurrentGunInfo, gunConfigModel);
            //所有缓存的枪添加一个弹夹的弹药量
            foreach(var gunInfo in gunSystem.GunInfos) AddBullets(gunInfo, gunConfigModel);
        }
        private void AddBullets(GunInfo gunInfo, IGunConfigModel gunConfigModel)
        {
            var gunConfigItem = gunConfigModel.GetItemByName(gunInfo.Name.Value);
            if (gunConfigItem.NeedBullet) gunInfo.BulletCountOutGun.Value = gunConfigItem.BulletMaxCount;
        }
    }
}
using FrameworkDesign;
namespace ShootingEditor2D
{
    public class AddBulletsCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            var gunSystem = this.GetSystem<IGunSystem>();
            var gunConfigModel = this.GetModel<IGunConfigModel>();
            AddBullets(gunSystem.CurrentGunInfo, gunConfigModel);
            foreach(var gunInfo in gunSystem.GunInfos) AddBullets(gunInfo, gunConfigModel);
        }
        private void AddBullets(GunInfo gunInfo, IGunConfigModel gunConfigModel)
        {
            var gunConfigItem = gunConfigModel.GetItemByName(gunInfo.Name.Value);
            if (gunConfigItem.NeedBullet) gunInfo.BulletCountOutGun.Value = gunConfigItem.BulletMaxCount;
        }
    }
}
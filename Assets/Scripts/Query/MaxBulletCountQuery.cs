using FrameworkDesign;
namespace ShootingEditor2D
{
    public class MaxBulletCountQuery : ICanGetModel
    {
        private readonly string mGunName;
        private int mMaxBulletCount;
        public MaxBulletCountQuery(string gunName) => mGunName = gunName;
        public int Do()
        {
            //查询代码
            var gunConfigModel = this.GetModel<IGunConfigModel>();
            var gunConfigItem = gunConfigModel.GetItemByName(mGunName);
            return gunConfigItem.BulletMaxCount;
        }
        public IArchitecture GetArchitecture() => ShootingEditor2D.Interface;
    }
}
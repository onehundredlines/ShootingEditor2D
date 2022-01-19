using FrameworkDesign;
namespace ShootingEditor2D
{
    public class MaxBulletCountQuery : AbstractQuery<int>
    {
        private readonly string mGunName;
        private int mMaxBulletCount;
        public MaxBulletCountQuery(string gunName) => mGunName = gunName;
        protected override int OnDo()
        {
            //查询代码
            var gunConfigModel = this.GetModel<IGunConfigModel>();
            var gunConfigItem = gunConfigModel.GetItemByName(mGunName);
            return gunConfigItem.BulletMaxCount;
        }
    }
}
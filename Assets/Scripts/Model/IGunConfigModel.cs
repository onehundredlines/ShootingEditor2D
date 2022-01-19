using System.Collections.Generic;
using FrameworkDesign;
namespace ShootingEditor2D
{
    public interface IGunConfigModel : IModel
    {
        GunConfigItem GetItemByName(string gunName);
    }
    public class GunConfigItem
    {
        public string Name { get; set; }
        public int BulletMaxCount { get; set; }
        public float Attack { get; set; }
        public float Frequency { get; set; }
        public float ShootDistance { get; set; }
        public bool NeedBullet { get; set; }
        public float ReloadSeconds { get; set; }
        public string Description { get; set; }

        public GunConfigItem(string name,
            int bulletMaxCount,
            float attack,
            float frequency,
            float shootDistance,
            bool needBullet,
            float reloadSeconds,
            string description)
        {
            Name = name;
            BulletMaxCount = bulletMaxCount;
            Attack = attack;
            Frequency = frequency;
            ShootDistance = shootDistance;
            NeedBullet = needBullet;
            ReloadSeconds = reloadSeconds;
            Description = description;
        }
    }
    public class GunConfigModel : AbstractModel, IGunConfigModel
    {
        private Dictionary<string, GunConfigItem> gunConfigItems = new Dictionary<string, GunConfigItem>
        {
            {"手枪", new GunConfigItem("手枪", 7, 1, 0.4f, 2f, false, 1, "默认枪")},
            {"冲锋枪", new GunConfigItem("冲锋枪", 50, 1, 0.1f, 2f, true, 3, "凸凸凸")},
            {"步枪", new GunConfigItem("步枪", 30, 3,0.3f,3f,true,3,"有一定后坐力")},
            {"狙击枪",new GunConfigItem("狙击枪", 12,6,1,6f,true,5,"瞄准镜，后坐力巨大")},
            {"火箭筒",new GunConfigItem("火箭筒",1,7,3,2f,true,7,"爆炸范围伤")},
            {"散弹枪",new GunConfigItem("散弹枪", 8,8,0.7f,1f,true,2f,"一次散射6~12单片,距离近")}
        };
        /// <summary>
        /// 如果有配置表，初始化是进行解析
        /// </summary>
        protected override void OnInit() { }
        public GunConfigItem GetItemByName(string gunName)
        {
            //通常情况要考虑字典没有的情况
            return gunConfigItems[gunName];
        }
    }
}
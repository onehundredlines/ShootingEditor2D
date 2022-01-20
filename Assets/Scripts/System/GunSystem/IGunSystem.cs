using System.Collections.Generic;
using System.Linq;
using FrameworkDesign;
namespace ShootingEditor2D
{
    public interface IGunSystem : ISystem
    {
        GunInfo CurrentGunInfo { get; }
        Queue<GunInfo> GunInfos { get; }
        void PickGun(string name, int bulletInGun, int bulletOutGun);
        void ShiftGun();
    }
    public struct OnCurrentGunChange
    {
        public string Name { get; set; }
    }
    public class GunSystem : AbstractSystem, IGunSystem
    {
        protected override void OnInit() { }
        public GunInfo CurrentGunInfo { get; } = new GunInfo
        {
            BulletCountInGun = new BindableProperty<int>(value: 3),
            Name = new BindableProperty<string>(value: "手枪"),
            State = new BindableProperty<GunState>(GunState.Idle),
            BulletCountOutGun = new BindableProperty<int>(1)
        };
        public Queue<GunInfo> GunInfos => mGunInfos;
        private readonly Queue<GunInfo> mGunInfos = new Queue<GunInfo>();

        /// <summary>
        /// 捡枪逻辑
        /// </summary>
        public void PickGun(string name, int bulletInGun, int bulletOutGun)
        {
            //如果和当前枪一样
            if (CurrentGunInfo.Name.Value == name)
            {
                CurrentGunInfo.BulletCountOutGun.Value += bulletInGun;
                CurrentGunInfo.BulletCountOutGun.Value += bulletOutGun;
            } else if (mGunInfos.Any(info => info.Name.Value == name))
            {
                var gunInfo = mGunInfos.First(info => info.Name.Value == name);
                gunInfo.BulletCountOutGun.Value += bulletInGun;
                gunInfo.BulletCountOutGun.Value += bulletOutGun;
            } else EnqueueCurrentGun(name, bulletInGun, bulletOutGun);
        }
        public void ShiftGun()
        {
            if (mGunInfos.Count <= 0) return;
            var previous = mGunInfos.Dequeue();
            EnqueueCurrentGun(previous.Name.Value, previous.BulletCountInGun.Value, previous.BulletCountOutGun.Value);
        }
        private void EnqueueCurrentGun(string nextGunName, int nextBulletInGun, int nextBulletOutgun)
        {
            //复制当前枪械数据
            var currentGunInfo = new GunInfo
            {
                Name = new BindableProperty<string>()
                {
                    Value = CurrentGunInfo.Name.Value
                },
                BulletCountInGun = new BindableProperty<int>()
                {
                    Value = CurrentGunInfo.BulletCountInGun.Value
                },
                BulletCountOutGun = new BindableProperty<int>()
                {
                    Value = CurrentGunInfo.BulletCountOutGun.Value
                },
                State = new BindableProperty<GunState>()
                {
                    Value = CurrentGunInfo.State.Value
                }
            };
            //缓存
            mGunInfos.Enqueue(currentGunInfo);
            //新枪设置为当前枪
            CurrentGunInfo.Name.Value = nextGunName;
            CurrentGunInfo.BulletCountInGun.Value = nextBulletInGun;
            CurrentGunInfo.BulletCountOutGun.Value = nextBulletOutgun;
            CurrentGunInfo.State.Value = GunState.Idle;
            //发送换枪事件
            this.SendEvent(new OnCurrentGunChange {Name = nextGunName});
        }
    }
}
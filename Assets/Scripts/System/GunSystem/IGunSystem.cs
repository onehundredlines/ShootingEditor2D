using System.Collections.Generic;
using System.Linq;
using FrameworkDesign;
namespace ShootingEditor2D
{
    public interface IGunSystem : ISystem
    {
        GunInfo CurrentGunInfo { get; }
        void PickGun(string name, int bulletInGun, int bulletOutGun);
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

        private Queue<GunInfo> gunInfoQueue = new Queue<GunInfo>();
        public void PickGun(string name, int bulletInGun, int bulletOutGun)
        {
            //如果和当前枪一样
            if (CurrentGunInfo.Name.Value == name)
            {
                CurrentGunInfo.BulletCountOutGun.Value += bulletInGun;
                CurrentGunInfo.BulletCountOutGun.Value += bulletOutGun;
            } else if (gunInfoQueue.Any(info => info.Name.Value == name))
            {
                var gunInfo = gunInfoQueue.First(info => info.Name.Value == name);
                CurrentGunInfo.BulletCountOutGun.Value += bulletInGun;
                CurrentGunInfo.BulletCountOutGun.Value += bulletOutGun;
            } else
            {
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
                gunInfoQueue.Enqueue(currentGunInfo);
                CurrentGunInfo.Name.Value = name;
                CurrentGunInfo.BulletCountInGun.Value = bulletInGun;
                CurrentGunInfo.BulletCountOutGun.Value = bulletOutGun;
                this.SendEvent(new OnCurrentGunChange {Name = name});
            }
        }
    }
}
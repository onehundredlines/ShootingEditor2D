using System;
using QFramework;
namespace ShootingEditor2D
{
    public enum GunState
    {
        Idle,
        Shooting,
        Reload,
        EmptyBullet,
        CoolDown
    }
    public class GunInfo
    {
        //这里没有直接删掉BulletCount，而是把BulletCount变为属性的方式对BulletCountInGun进行了"代理"
        //使用Obsolete对其他协同开发者和使用者是平滑的更新API的方式，正规的步骤方式，体验比较好。
        //Obsolete中true是报错，false是不报错只提醒。
        [Obsolete("请使用BulletCountInGun替代BulletCount", true)]
        public BindableProperty<int> BulletCount { get => BulletCountInGun; set => BulletCountInGun = value; }
        public BindableProperty<int> BulletCountInGun;
        public BindableProperty<string> Name;
        public BindableProperty<GunState> State;
        public BindableProperty<int> BulletCountOutGun;
    }
}
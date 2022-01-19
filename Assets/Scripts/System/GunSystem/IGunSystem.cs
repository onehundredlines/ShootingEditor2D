using FrameworkDesign;
namespace ShootingEditor2D
{
    public interface IGunSystem : ISystem
    {
        GunInfo CurrentGunInfo { get; }
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
    }
}
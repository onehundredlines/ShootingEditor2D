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
            BulletCountInGun = new BindableProperty<int>
            {
                Value = 3
            }
        };
    }
}
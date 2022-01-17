using FrameworkDesign;
namespace ShootingEditor2D
{
    public class ShootingEditor2D : Architecture<ShootingEditor2D>
    {
        protected override void Init()
        {
            RegisterSystem<IStatSystem>(new StatSystem());
            RegisterModel<IPlayerModel>(new PlayerModel());
            RegisterSystem<IGunSystem>(new GunSystem());
        }
    }
}
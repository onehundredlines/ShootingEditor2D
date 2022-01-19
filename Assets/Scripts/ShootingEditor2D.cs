using FrameworkDesign;
namespace ShootingEditor2D
{
    public class ShootingEditor2D : Architecture<ShootingEditor2D>
    {
        protected override void Init()
        {
            RegisterModel<IGunConfigModel>(new GunConfigModel());
            RegisterModel<IPlayerModel>(new PlayerModel());
            RegisterSystem<ITimeSystem>(new TimeSystem());
            RegisterSystem<IStatSystem>(new StatSystem());
            RegisterSystem<IGunSystem>(new GunSystem());
        }
    }
}
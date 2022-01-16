using FrameworkDesign;
namespace ShootingEditor2D
{
    public interface IPlayerModel : IModel
    {
        BindableProperty<int> Hp { get; }
    }
    public class PlayerModel : AbstractModel, IPlayerModel
    {
        protected override void OnInit() { }
        public BindableProperty<int> Hp { get; } = new BindableProperty<int> {Value = 3};
    }           
}
using FrameworkDesign;
using UnityEngine.SceneManagement;
namespace ShootingEditor2D.Command
{
    public class HurtPlayerCommand : AbstractCommand
    {
        private readonly int mHurt;
        public HurtPlayerCommand(int hurt = 1) { mHurt = hurt; }
        protected override void OnExecute()
        {
            var playerMode = this.GetModel<IPlayerModel>();
            playerMode.Hp.Value -= mHurt;
            if (playerMode.Hp.Value <= 0) SceneManager.LoadScene("GameOver");
        }
    }
}
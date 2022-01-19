using System;
using FrameworkDesign;
using UnityEngine;
namespace ShootingEditor2D
{
    public class UIController : MonoBehaviour, IController
    {
        private IStatSystem mStatSystem;
        private IPlayerModel mPlayerModel;
        private IGunSystem mGunSystem;
        private void Awake()
        {
            mStatSystem = this.GetSystem<IStatSystem>();
            mPlayerModel = this.GetModel<IPlayerModel>();
            mGunSystem = this.GetSystem<IGunSystem>();
        }
        //Lazy无法在别的周期中初始化，只能在OnGUI中，这里用回调解决初始化的问题。
        private readonly Lazy<GUIStyle> mLabelStyle = new Lazy<GUIStyle>(() => new GUIStyle(GUI.skin.label)
        {
            fontSize = 40
        });
        private void OnGUI()
        {
            GUI.Label(new Rect(10, 10, 400, 100), $"生命: {mPlayerModel.Hp.Value}/3", mLabelStyle.Value);
            GUI.Label(new Rect(10, 60, 400, 100), $"子弹: {mGunSystem.CurrentGunInfo.BulletCountInGun.Value}", mLabelStyle.Value);
            GUI.Label(new Rect(10, 110, 400, 100), $"总弹量: {mGunSystem.CurrentGunInfo.BulletCountOutGun.Value}", mLabelStyle.Value);
            GUI.Label(new Rect(10, 160, 400, 100), $"枪械: {mGunSystem.CurrentGunInfo.Name.Value}", mLabelStyle.Value);
            GUI.Label(new Rect(10, 210, 600, 100), $"状态: {mGunSystem.CurrentGunInfo.State.Value}", mLabelStyle.Value);
            GUI.Label(new Rect(Screen.width - 10 - 300, 10, 300, 100), $"击杀: {mStatSystem.KillCount.Value}", mLabelStyle.Value);
        }
        private void OnDestroy()
        {
            mGunSystem = null;
            mStatSystem = null;
            mPlayerModel = null;
        }
        public IArchitecture GetArchitecture() => ShootingEditor2D.Interface;
    }
}
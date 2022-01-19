using FrameworkDesign;
using UnityEngine;
namespace ShootingEditor2D
{
    public class Gun : MonoBehaviour, IController
    {
        private IGunSystem mGunSystem;
        private GunInfo mGunInfo;
        [SerializeField]
        private Bullet mBullet;
        [SerializeField]
        private Transform mBulletPoint;
        private void Awake()
        {
            mGunSystem = this.GetSystem<IGunSystem>();
            //现在是一把枪，以后有不同枪的时候，这里需要查询
            mGunInfo = mGunSystem.CurrentGunInfo;
            mBulletPoint = transform.Find("BulletPoint").transform;
            mBullet = Resources.Load<Bullet>("Bullet");
        }
        public void Shoot()
        {
            if (mGunInfo.BulletCountInGun.Value > 0)
            {
                var bullet = Instantiate(mBullet, mBulletPoint.position, Quaternion.identity);
                if (bullet == null) return;
                var trans = mBulletPoint.transform;
                var bulletTrans = bullet.transform;
                bulletTrans.position = trans.position;
                bulletTrans.rotation = trans.rotation;
                bullet.Init(mBulletPoint);
                bullet.gameObject.SetActive(true);
                this.SendCommand<ShootCommand>(ShootCommand.Single);
            }
        }
        public IArchitecture GetArchitecture() => ShootingEditor2D.Interface;
    }
}
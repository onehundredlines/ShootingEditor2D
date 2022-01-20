using FrameworkDesign;
using UnityEngine;
namespace ShootingEditor2D
{
    public class Gun : MonoBehaviour, IController
    {
        private IGunSystem mGunSystem;
        private GunInfo mGun;
        [SerializeField]
        private Bullet mBullet;
        [SerializeField]
        private Transform mBulletPoint;
        private int mMaxBulletCount;
        private void Awake()
        {
            mBulletPoint = transform.Find("BulletPoint").transform;
            mGunSystem = this.GetSystem<IGunSystem>();
            //现在是一把枪，以后有不同枪的时候，这里需要查询
            mGun = mGunSystem.CurrentGunInfo;
            mBullet = Resources.Load<Bullet>("Bullet");
            mMaxBulletCount = this.SendQuery(new MaxBulletCountQuery(mGun.Name.Value));
            this.RegisterEvent<OnCurrentGunChange>(e => mMaxBulletCount = this.SendQuery(new MaxBulletCountQuery(mGunSystem.CurrentGunInfo.Name.Value))).CancelWhenGameObjectDestroy(gameObject);
        }
        public void Shoot()
        {
            if (mGun.BulletCountInGun.Value > 0 && mGun.State.Value == GunState.Idle)
            {
                var bullet = Instantiate(mBullet, mBulletPoint.position, Quaternion.identity);
                if (bullet == null) return;
                var trans = mBulletPoint.transform;
                var bulletTrans = bullet.transform;
                bulletTrans.position = trans.position;
                bulletTrans.rotation = trans.rotation;
                bullet.Init(mBulletPoint);
                bullet.gameObject.SetActive(true);
                this.SendCommand(ShootCommand.Single);
            }
        }
        public IArchitecture GetArchitecture() => ShootingEditor2D.Interface;
        public void Reload()
        {
            if (mGun.State.Value == GunState.Idle && mGun.BulletCountInGun.Value < mMaxBulletCount && mGun.BulletCountOutGun.Value > 0) this.SendCommand<ReloadCommand>();
        }
    }
}
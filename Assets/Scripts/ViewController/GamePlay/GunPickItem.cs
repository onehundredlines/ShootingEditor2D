using QFramework;
using UnityEngine;
namespace ShootingEditor2D
{
    public class GunPickItem : ShootingEditor2DBaseController
    {
        public string Name;
        public int BulletInGun;
        public int BulletOutGun;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                this.SendCommand(new PickGunCommand(Name, BulletInGun, BulletOutGun));
                Destroy(gameObject);
            }
        }
    }
}
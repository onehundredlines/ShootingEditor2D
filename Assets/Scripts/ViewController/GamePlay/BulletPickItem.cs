using FrameworkDesign;
using UnityEngine;
namespace ShootingEditor2D
{
    public class BulletPickItem : ShootingEditor2DBaseController
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                this.SendCommand<AddBulletsCommand>();
                Destroy(gameObject);
            }
        }
    }
}
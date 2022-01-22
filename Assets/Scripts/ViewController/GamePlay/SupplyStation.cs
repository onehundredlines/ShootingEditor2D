using QFramework;
using UnityEngine;
namespace ShootingEditor2D
{
    public class SupplyStation : ShootingEditor2DBaseController
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                this.SendCommand<FunBulletsCommand>();
                Destroy(gameObject);
            }
        }
    }
}
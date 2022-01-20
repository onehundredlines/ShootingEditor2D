using FrameworkDesign;
using UnityEngine;
namespace ShootingEditor2D
{
    public class SupplyStation : MonoBehaviour, IController
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                this.SendCommand<FunBulletsCommand>();
                Destroy(gameObject);
            }
        }
        public IArchitecture GetArchitecture() => ShootingEditor2D.Interface;
    }
}
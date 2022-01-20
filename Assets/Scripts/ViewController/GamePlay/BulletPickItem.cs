using System;
using FrameworkDesign;
using UnityEngine;
namespace ShootingEditor2D
{
    public class BulletPickItem : MonoBehaviour, IController
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                this.SendCommand<AddBulletsCommand>();
                Destroy(gameObject);
            }
        }
        public IArchitecture GetArchitecture() => ShootingEditor2D.Interface;
    }
}
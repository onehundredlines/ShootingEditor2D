using FrameworkDesign;
using ShootingEditor2D.Command;
using UnityEngine;
namespace ShootingEditor2D
{
    public class Bullet : MonoBehaviour, IController
    {
        [SerializeField]
        private Rigidbody2D mRigidbody2D;
        private float mBulletForce;

        private void Awake()
        {
            mRigidbody2D = GetComponent<Rigidbody2D>();
            mBulletForce = 80f;
            Destroy(gameObject, 2f);

        }
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                this.SendCommand<KillEnemyCommand>();
                Destroy(other.gameObject);
                Destroy(gameObject);
            }
        }
        public void Init(Transform dir) => mRigidbody2D.AddForce(dir.right * mBulletForce, ForceMode2D.Impulse);
        public IArchitecture GetArchitecture() => ShootingEditor2D.Interface;
    }
}
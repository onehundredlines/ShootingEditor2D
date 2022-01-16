using UnityEngine;
namespace ShootingEditor2D
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody2D mRigidbody2D;
        private float mBulletForce;

        private void Awake()
        {
            mRigidbody2D = GetComponent<Rigidbody2D>();
            mBulletForce = 80f;
        }
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                Destroy(other.gameObject);
                Destroy(gameObject);
            }
        }
        public void Init(Transform trans)
        {
            mRigidbody2D.AddForce(trans.right * mBulletForce, ForceMode2D.Impulse);
        }
    }
}
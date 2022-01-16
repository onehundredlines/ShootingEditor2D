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
        private void OnEnable()
        {
            mRigidbody2D.AddForce(new Vector2(transform.position.x, 0) * mBulletForce, ForceMode2D.Impulse);
        }
    }
}
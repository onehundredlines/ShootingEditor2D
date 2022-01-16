using System.Collections;
using UnityEngine;
namespace ShootingEditor2D
{
    public class Player : MonoBehaviour
    {
        [SerializeField]
        private BoxCollider2D mBoxCollider2D;
        [SerializeField]
        private Rigidbody2D mRigidbody2D;
        [SerializeField]
        private Gun mGun;
        private float mSpeed;
        private bool mJumped;
        private Vector2 mJumpForce;
        private float mFallForce;
        private bool mCanShoot;

        [SerializeField]
        private TriggerCheck mGroundCheck;
        private void Awake()
        {
            mGun = transform.Find("Gun").GetComponent<Gun>();
            mBoxCollider2D = GetComponent<BoxCollider2D>();
            mRigidbody2D = GetComponent<Rigidbody2D>();
            mGroundCheck = transform.Find("GroundCheck").GetComponent<TriggerCheck>();
            mSpeed = 6f;
            mJumpForce = new Vector2(0, 12f);
            mFallForce = -25f;
            mCanShoot = true;
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.K) && mGroundCheck.Triggered) mJumped = true;
            mBoxCollider2D.sharedMaterial.friction = mGroundCheck.Triggered ? 0.4f : 0;
            if (mCanShoot && Input.GetKey(KeyCode.J)) StartCoroutine(DoShoot());
            var horizontal = Input.GetAxis("Horizontal");
            if (horizontal > 0) transform.localEulerAngles = new Vector3(0, 0, 0);
            else if (horizontal < 0) transform.localEulerAngles = new Vector3(0, 180, 0);
        }
        private void FixedUpdate()
        {
            var horizontal = Input.GetAxis("Horizontal");
            mRigidbody2D.velocity = new Vector2(horizontal * mSpeed, mRigidbody2D.velocity.y);

            if (mJumped)
            {
                mJumped = false;
                mRigidbody2D.AddForce(mJumpForce, ForceMode2D.Impulse);
            }
            if (mRigidbody2D.velocity.y < 0 && !mGroundCheck.Triggered) mRigidbody2D.velocity += new Vector2(0, mFallForce * Time.deltaTime);
        }

        private IEnumerator DoShoot()
        {
            mCanShoot = false;
            mGun.Shoot();
            yield return new WaitForSeconds(0.05f);
            mCanShoot = true;
        }
    }
}
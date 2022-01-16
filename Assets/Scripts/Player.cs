using UnityEngine;

namespace ShootingEditor2D
{
    public class Player : MonoBehaviour
    {
        private BoxCollider2D mBoxCollider2D;
        private Rigidbody2D mRigidbody2D;
        private float mSpeed;
        private bool mJumped;
        private float mJumpForce;
        private float mFallForce;

        [SerializeField]
        private TriggerCheck mGroundCheck;
        private void Awake()
        {
            mBoxCollider2D = GetComponent<BoxCollider2D>();
            mRigidbody2D = GetComponent<Rigidbody2D>();
            mGroundCheck = transform.Find("GroundCheck").GetComponent<TriggerCheck>();
            mSpeed = 5f;
            mJumpForce = 9f;
            mFallForce = -25f;
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.K) && mGroundCheck.Triggered) mJumped = true;
            mBoxCollider2D.sharedMaterial.friction = mGroundCheck.Triggered ? 0.4f : 0;
        }
        private void FixedUpdate()
        {
            var horizontal = Input.GetAxis("Horizontal");
            mRigidbody2D.velocity = new Vector2(horizontal * mSpeed, mRigidbody2D.velocity.y);

            if (mJumped)
            {
                mJumped = false;
                mRigidbody2D.velocity = new Vector2(mRigidbody2D.velocity.x, mJumpForce);
            }
            if (mRigidbody2D.velocity.y < 0 && !mGroundCheck.Triggered) mRigidbody2D.velocity += new Vector2(0, mFallForce * Time.deltaTime);
        }
    }
}
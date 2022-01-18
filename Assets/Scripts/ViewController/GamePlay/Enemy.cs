using UnityEngine;
namespace ShootingEditor2D
{
    public class Enemy : MonoBehaviour
    {
        private Rigidbody2D mRigidbody2D;

        private TriggerCheck mWalkCheck;
        private TriggerCheck mFallCheck;
        private TriggerCheck mGroundCheck;

        private void Awake()
        {
            mRigidbody2D = GetComponent<Rigidbody2D>();
            mWalkCheck = transform.Find("WalkCheck").GetComponent<TriggerCheck>();
            mFallCheck = transform.Find("FallCheck").GetComponent<TriggerCheck>();
            mGroundCheck = transform.Find("GroundCheck").GetComponent<TriggerCheck>();
        }
        private void Update()
        {
            var scaleX = transform.localScale.x;
            if (mGroundCheck.Triggered && mFallCheck.Triggered && !mWalkCheck.Triggered) mRigidbody2D.velocity = new Vector2(scaleX * 5, mRigidbody2D.velocity.y);
            else
            {
                var trans = transform;
                var localScale = trans.localScale;
                localScale.x = -localScale.x;
                trans.localScale = localScale;
            }
        }
    }
}
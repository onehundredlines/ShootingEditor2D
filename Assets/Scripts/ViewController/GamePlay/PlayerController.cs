using UnityEngine;
namespace ShootingEditor2D
{
    public class PlayerController : MonoBehaviour
    {
        public float maxAngleSpeed = 400;
        public float minAngleSpeed = 100;
        public float acceleration = 40;
        public float maxMoveSpeed = 5f;
        public float jumpSpeed = 10f;
        public float gravity = 10f;
        public Transform cameraTransform;

        bool isOnGround;
        bool isSecondJump;
        float ySpeed;
        float moveSpeed;
        Vector3 input;
        Vector3 move;
        CharacterController cont;

        void Start() { cont = GetComponent<CharacterController>(); }

        void Update()
        {
            isOnGround = cont.isGrounded;
            Drop();
            Jump();
            Turn();
            Move();
        }

        void Move()
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            input.x = h;
            input.z = v;

            moveSpeed = Mathf.MoveTowards(moveSpeed, input.normalized.magnitude * maxMoveSpeed, acceleration * Time.deltaTime);
            move = input * (Time.deltaTime * moveSpeed);
            move = cameraTransform.TransformDirection(move);
            move += Vector3.up * (ySpeed * Time.deltaTime);
            
            cont.Move(move);
        }

        void Drop()
        {
            if (!isOnGround)
            {
                ySpeed -= gravity * Time.deltaTime;
            } else
            {
                if (ySpeed < -1)
                {
                    ySpeed += gravity * Time.deltaTime;
                }
                if (isSecondJump)
                {
                    isSecondJump = false;
                }
            }
        }
        void Jump()
        {
            if (isOnGround)
            {
                if (Input.GetButtonDown("Jump"))
                {
                    ySpeed = jumpSpeed;
                }
            } else
            {
                if (!isSecondJump)
                {
                    if (Input.GetButtonDown("Jump"))
                    {
                        ySpeed += jumpSpeed;
                        isSecondJump = true;
                    }
                }
            }
        }

        void Turn()
        {
            if (input.x != 0 || input.z != 0)
            {
                Vector3 targetDirection = cameraTransform.TransformDirection(input);
                targetDirection.y = 0;
                Quaternion lookQuaternion = Quaternion.LookRotation(targetDirection);
                
                float turnSpeed = Mathf.Lerp(minAngleSpeed, maxAngleSpeed, moveSpeed / maxMoveSpeed);

                transform.rotation = Quaternion.RotateTowards(transform.rotation, lookQuaternion, turnSpeed * Time.deltaTime);
            }
        }
    }
}
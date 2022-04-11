using UnityEngine;
namespace Platformer.Mechanics
{
    
    public class PlayerMovement : MonoBehaviour
    {
        public Vector2 ropeHook;
        float friction = 0.01f;
        public float swingForce = 4f;
        public float speed = 1f;
        public float jumpSpeed = 3f;
        public bool groundCheck;
        public bool isSwinging;
        private SpriteRenderer playerSprite;
        private Rigidbody2D rBody;
        private bool isJumping;
        private Animator animator;
        private float jumpInput;
        private float horizontalInput;
        
        public int maxJumps = 2;
        private int jumps;
        private float jumpForce = 6f;
        bool double_jump = true;
        bool grapple = false;
        bool dash = true;

        public float dashSpeed = 100;


        void Awake()
        {
            playerSprite = GetComponent<SpriteRenderer>();
            rBody = GetComponent<Rigidbody2D>();
            //animator = GetComponent<Animator>();
            this.GetComponent<RopeSystem>().enabled = grapple;


        }

        void Update()
        {
            jumpInput = Input.GetAxis("Jump");
            horizontalInput = Input.GetAxis("Horizontal");

            var halfHeight = transform.GetComponent<SpriteRenderer>().bounds.extents.y;

            // int layerMask = (LayerMask.GetMask("Ground"));
            groundCheck = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - halfHeight - 0.04f), Vector2.down, 0.025f);

           


            Vector2 antiVelocity;
            antiVelocity.x = rBody.velocity.x * -friction;
            antiVelocity.y = rBody.velocity.y * -friction;

            rBody.AddForce(antiVelocity, ForceMode2D.Force);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                
                if (groundCheck)
                {
                    //gameObject.GetComponent<Rigidbody2D>().velocity.y = 0;
                    rBody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                    jumps = maxJumps - 1;
                }
                else
                //Double Jump Capabilities
                {
                    if (double_jump && jumps > 0)
                    {
                        jumps = jumps - 1;
                        //Vector2 vel = new Vector2(rBody.velocity.x, 0);
                        //rBody.velocity = vel;
                        gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                    }
                }
            }

            if (dash && Input.GetKeyDown(KeyCode.LeftShift))
            {
                Debug.Log("WHHYYY");
                var worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
                var facingDirection = worldMousePosition - transform.position;
                Debug.Log(facingDirection);

                rBody.AddForce(new Vector2(facingDirection.x, facingDirection.y), ForceMode2D.Impulse);

            }


        }

        void FixedUpdate()
        {   //if (!isSwinging)
            //{


            if (horizontalInput < 0f || horizontalInput > 0f)
            {
                //animator.SetFloat("Speed", Mathf.Abs(horizontalInput));
                playerSprite.flipX = horizontalInput < 0f;
                if (isSwinging)
                {
                    //animator.SetBool("IsSwinging", true);

                    // 1 - Get a normalized direction vector from the player to the hook point
                    var playerToHookDirection = (ropeHook - (Vector2)transform.position).normalized;

                    // 2 - Inverse the direction to get a perpendicular direction
                    Vector2 perpendicularDirection;
                    if (horizontalInput < 0)
                    {
                        perpendicularDirection = new Vector2(-playerToHookDirection.y, playerToHookDirection.x);
                        var leftPerpPos = (Vector2)transform.position - perpendicularDirection * -2f;
                        Debug.DrawLine(transform.position, leftPerpPos, Color.green, 0f);
                    }
                    else
                    {
                        perpendicularDirection = new Vector2(playerToHookDirection.y, -playerToHookDirection.x);
                        var rightPerpPos = (Vector2)transform.position + perpendicularDirection * 2f;
                        Debug.DrawLine(transform.position, rightPerpPos, Color.green, 0f);
                    }

                    var force = perpendicularDirection * swingForce;
                    rBody.AddForce(force, ForceMode2D.Force);
                }
                else
                {
                    //animator.SetBool("IsSwinging", false);
                    if (!isSwinging)
                    {
                        var groundForce = speed * 2f;
                        rBody.AddForce(new Vector2((horizontalInput * groundForce - rBody.velocity.x) * groundForce, 0));
                        rBody.velocity = new Vector2(rBody.velocity.x, rBody.velocity.y);
                    }
                }
            }
            //else
            //{
            //animator.SetBool("IsSwinging", false);
            //animator.SetFloat("Speed", 0f);
            //}




        }





    }
    

}





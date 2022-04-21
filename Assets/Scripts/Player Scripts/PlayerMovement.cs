using UnityEngine;
namespace Platformer.Mechanics
{
    
    public class PlayerMovement : MonoBehaviour
    {
        public Vector2 ropeHook;
        float friction = 0.01f;
        public float swingForce = 4f;
        public float speed = 5f;
        public float jumpSpeed = 10f;
        public bool groundCheck;
        public bool isSwinging = false;
        private SpriteRenderer playerSprite;
        private Rigidbody2D rBody;
        private bool isJumping;
        private Animator animator;
        private float jumpInput;
        private float horizontalInput;
        
        public int maxJumps = 2;
        private int jumps;
        public float jumpForce = 13f;

        
        public float stunTimer = 0.25f;
        public float dashSpeed = 100f;


        bool double_jump;
        bool grapple;
        bool dash;

        public bool isTurned;
        ///float speed;

        void Awake()
        {
            playerSprite = GetComponent<SpriteRenderer>();
            rBody = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            

            PlayerSettings playerSettings = gameObject.GetComponent<PlayerSettings>();
            double_jump = playerSettings.double_jump;
            grapple = playerSettings.grapple;
            dash = playerSettings.dash;
            speed = playerSettings.speed;
            
            this.GetComponent<RopeSystem>().enabled = grapple;
        }

        public void SetAbility(int whichAbility)
        {
            switch(whichAbility)
            {
                case 0:
                    dash = true;

                    break;

                case 1:
                    double_jump = true;
                    break;

                case 2:
                    grapple = true;
                    break;
            }
        }

        void Update()
        {
            if(stunTimer > 0)
                stunTimer -= Time.deltaTime;
            if (stunTimer < 0)
                stunTimer = 0;
            jumpInput = Input.GetAxis("Jump");
            horizontalInput = Input.GetAxis("Horizontal");


            RaycastHit2D hit = Physics2D.Raycast(rBody.GetComponent<BoxCollider2D>().bounds.center, Vector2.down, rBody.GetComponent<BoxCollider2D>().bounds.extents.y + 0.05f);
            Color color1 = Color.red;
            if(hit) color1 = Color.green;
            // Debug.DrawRay(rBody.GetComponent<BoxCollider2D>().bounds.center, Vector2.down * (rBody.GetComponent<BoxCollider2D>().bounds.extents.y + 0.05f), color1);
        
            groundCheck = hit;


            Vector2 antiVelocity;
            antiVelocity.x = rBody.velocity.x * -friction;
            antiVelocity.y = rBody.velocity.y * -friction;

            rBody.AddForce(antiVelocity, ForceMode2D.Force);

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown("w") || Input.GetKeyDown(KeyCode.UpArrow))
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
                        Vector2 vel = new Vector2(rBody.velocity.x, 0);
                        rBody.velocity = vel;
                        gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);

                    }
                }
            }

            if (dash && (Input.GetKeyDown("z")))
            {
                var facingDirection = new Vector2(1, 0);
                // PlayerMovement playerMovement = gameObject.GetComponent<PlayerMovement>();
                   //bool isTurned = playerMovement.isTurned;

                if (!isTurned)
                {
                    facingDirection = new Vector2(-1, 0);
                }

                ///Debug.Log("WHHYYY");
                //var worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
                //var facingDirection = worldMousePosition - transform.position;
                //Debug.Log(facingDirection);

                rBody.AddForce(facingDirection*32, ForceMode2D.Impulse);

            }
            // Set animator parameters
            animator.SetFloat("Speed", Mathf.Abs(rBody.velocity.x));
            animator.SetBool("Grounded", groundCheck);
            animator.SetBool("Jumping", !groundCheck && rBody.velocity.y > 0f);
            if (stunTimer == 0)
            {
                animator.SetBool("Hurt", false);
            }
        }

        void FixedUpdate()
        {   //if (!isSwinging)
            //{
            if (stunTimer == 0)
            {
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
                            if (horizontalInput > 0f)
                            {
                                isTurned = true;
                            }
                            if (horizontalInput < 0f)
                            {
                                isTurned = false;
                            }

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
}
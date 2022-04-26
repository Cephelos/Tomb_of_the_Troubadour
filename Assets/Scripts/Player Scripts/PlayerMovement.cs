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
        public ParticleSystem saaanicParticles;
        private SpriteRenderer playerSprite;
        private Rigidbody2D rBody;
        private bool isJumping;
        private Animator animator;
        private float jumpInput;
        private float horizontalInput;
        
        public int maxJumps = 2;
        private int jumps;
        public float jumpForce = 13f;
        public bool onIce = false; // is the player walking on ice
        
        public float stunTimer = 0.25f;
        public float invincibleTimer = 0f;
        public float dashSpeed = 100f;
        public float dashTimer = 0f;
        public float dashCooldown = 0.7f;
        public float dashIframeDuration = 0.2f;


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

        public void ToggleIcePhysics(bool on)
        {
            if(on)
            {
                rBody.sharedMaterial = GameController.Instance.iceMat;
                onIce = true;
            }
            else
            {
                rBody.sharedMaterial = null;
                onIce = false;
            }
        }

        public void BoostSpeed(bool boost) // If boost = true, boosts speed by 1 level; otherwise, resets to base value
        {
            if (boost)
            {
                speed += GameController.Instance.basePlayerSpeed;
                if(saaanicParticles == null)
                    saaanicParticles = Instantiate(GameController.Instance.saaanicParticles, transform);
            }
            else
            {
                speed = GameController.Instance.basePlayerSpeed;
                if(saaanicParticles != null)
                    Destroy(saaanicParticles.gameObject);
                saaanicParticles = null;
            }

        }

        void TickTimers() // Subtracts time passed since the last frame from all timers (stun timer, invincibility timer and dash cooldown timer)
        {
            if (stunTimer > 0)
                stunTimer -= Time.deltaTime;
            if (stunTimer < 0)
                stunTimer = 0;

            if (invincibleTimer > 0)
                invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                invincibleTimer = 0;
                

            if (dashTimer > 0)
                dashTimer -= Time.deltaTime;
            if (dashTimer < 0)
                dashTimer = 0;
        }

        void Update()
        {
            this.GetComponent<RopeSystem>().enabled = grapple;

            TickTimers(); // Moved the code that ticks the timers into a method to make things neater, since we have a lot of them

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

            if (dash && (Input.GetKeyDown("z") || Input.GetKeyDown(KeyCode.LeftShift)) && dashTimer ==0)
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
                rBody.position = new Vector2(rBody.position.x, rBody.position.y+0.01f);
                rBody.AddForce(facingDirection*36, ForceMode2D.Impulse);

                // Set cooldown timer
                dashTimer = dashCooldown;

                // Set I-frames
                invincibleTimer = dashIframeDuration;

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
                            var rawHorizInput = Input.GetAxisRaw("Horizontal");
                            Vector2 force = new Vector2();
                            
                            if (groundCheck && onIce) // If we're on ice, use ice physics
                            {
                                if (rawHorizInput != 0f && horizontalInput * rBody.velocity.x > 0 && Mathf.Abs(rBody.velocity.x) < groundForce)
                                // if we're inputting in the same direction as our movement and moving at less than max speed
                                {
                                    force = new Vector2(rawHorizInput * Mathf.Max((Mathf.Abs(horizontalInput * groundForce) - Mathf.Abs(rBody.velocity.x)) * groundForce/5f, 0), 0); // don't slow down!
                                }
                                else
                                {
                                    force = new Vector2((horizontalInput * groundForce - rBody.velocity.x) * groundForce/5f, 0); // slow down (same as before)!
                                }
                            }
                            else
                                if (rawHorizInput != 0f && horizontalInput * rBody.velocity.x > 0 && Mathf.Abs(rBody.velocity.x) < groundForce)
                            // if we're inputting in the same direction as our movement and moving at less than max speed
                            {
                                force = new Vector2(rawHorizInput * Mathf.Max((Mathf.Abs(horizontalInput * groundForce) - Mathf.Abs(rBody.velocity.x)) * groundForce, 0), 0); // don't slow down!
                            }
                            else
                            {
                                force = new Vector2((horizontalInput * groundForce - rBody.velocity.x) * groundForce, 0); // slow down (same as before)!
                            }

                            rBody.AddForce(force);
                            rBody.velocity = new Vector2(rBody.velocity.x, rBody.velocity.y);
                        }
                    }
                }
                else
                {
                    var groundForce = speed * 2f;
                    if (rBody.velocity.x > 1f * groundForce)
                    {
                        rBody.AddForce(new Vector2((1f * groundForce - rBody.velocity.x) * groundForce, 0));

                    }
                    else if(rBody.velocity.x < -1f * groundForce)
                    {
                        rBody.AddForce(new Vector2((-(1f * groundForce + rBody.velocity.x)) * groundForce, 0));

                    }
                    rBody.velocity = new Vector2(rBody.velocity.x, rBody.velocity.y);
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
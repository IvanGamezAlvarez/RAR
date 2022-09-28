using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Variables
    

    [System.Serializable]

    public struct PD
    {
        public float speedWalk, dashPower, jump, speedRun, stamina;
        public int health, strongDamage;
    }
    
    public PD playerVariables;

    [Header("Referencias")]
    public Animator AnimaRef;
    public Rigidbody2D rbPlayer;
    public Transform jumpDetector;
    public Transform detectorL, detectorR;
    public LayerMask layerForDetector;
    public float sizeOverlapRadio;
    public int canJump, maxOfJump, maxOfDash, canDash;

    #endregion
    private void Awake()
    {
        AnimaRef = GetComponent<Animator>();
        rbPlayer = GetComponent<Rigidbody2D>();
    }

    
    void Start()
    {
        
    }


    void Update()
    {
        BasicControllers();
        Dashing();
    }
    private void LateUpdate()
    {
        AnimationPlayer();
    }
    void BasicControllers()
    {
 
        float horizontalM = Input.GetAxisRaw("Horizontal");
        if(Input.Ge)
        {
            rbPlayer.velocity = new Vector2(horizontalM * playerVariables.speedWalk * Time.deltaTime, rbPlayer.velocity.y);

        }


        if (Input.GetKeyDown(KeyCode.Space) && canJump >0)
        {
            Debug.Log("Salta");
            rbPlayer.AddForce(Vector2.up *playerVariables.jump, ForceMode2D.Impulse);
            canJump --;
        }
        
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            playerVariables.speedWalk *= playerVariables.speedRun; 
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            playerVariables.speedWalk /= playerVariables.speedRun;
        }
        WallJump();



    }
    void AnimationPlayer()
    {
        float ScaleVertical = Input.GetAxisRaw("Horizontal");
        if (ScaleVertical != 0)
        {
            transform.localScale = new Vector2(ScaleVertical, transform.localScale.y);

            AnimaRef.SetBool("Run", true);
        }
        if (rbPlayer.velocity == Vector2.zero)
        {
            AnimaRef.SetBool("Run", false);
         
        }
        if (rbPlayer.velocity.y < -0.1 )
        {
            AnimaRef.SetBool("Fall", true);
            AnimaRef.SetBool("Jump", false);
        } 
        if (rbPlayer.velocity.y > -0.1 && rbPlayer.velocity.y < 0.1)
        {
            AnimaRef.SetBool("Fall", false);
        }
        if (rbPlayer.velocity.y > 0.1 && canJump < maxOfJump)
        {
            AnimaRef.SetBool("Jump", true);
            AnimaRef.SetBool("Run", false);
            AnimaRef.SetBool("Fall", false);
        }




    }
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag != "Player")
        {
            canJump = maxOfJump ;
            canDash = maxOfDash;
        }
    }
    #region Better Jump
    void BetterJump()
    {
        float fallMultipler = 5f, lowJumpMultpler = 4f;
        if(rbPlayer.velocity.y < 0)
        {
            rbPlayer.velocity += Vector2.up * Physics2D.gravity.y * (fallMultipler - 1) * Time.deltaTime;
        }
        else if (rbPlayer.velocity.y > 0 && !Input.GetKeyDown(KeyCode.Space))
        {
            rbPlayer.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultpler - 1) * Time.deltaTime;
        }
    }
    #endregion
    public void WallJump()
    {
        bool detectorLeft = Physics2D.OverlapCircle(detectorL.position, sizeOverlapRadio, layerForDetector);
        if (detectorLeft)
            Debug.Log("holaI" );
        bool detectorRight = Physics2D.OverlapCircle(detectorR.position, sizeOverlapRadio, layerForDetector);
        if (detectorRight)
            Debug.Log("holaD");
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(detectorL.position, sizeOverlapRadio);
        Gizmos.DrawWireSphere(detectorR.position, sizeOverlapRadio);
    }
    public void Dashing()
    {
        float horizontalM = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.LeftControl) && canDash > 0)
        {
            Debug.Log("Dash");
            rbPlayer.velocity = new Vector2(rbPlayer.velocity.x, rbPlayer.velocity.y);
            rbPlayer.AddForce(Vector2.right * horizontalM * playerVariables.dashPower, ForceMode2D.Impulse);
            canDash--;
        }
    }
}

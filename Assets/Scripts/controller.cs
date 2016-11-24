using UnityEngine;
using System.Collections;

public class controller : MonoBehaviour {
    [SerializeField]
    private float jumpHeight = 7.5f;
    [SerializeField]
    private int baseJumpCount = 1;
    private int jumpCount = 0;
    [SerializeField]
    private float maxSpeedY = 10f;

    [SerializeField]
    private float deathZone = 0.05f;
    [SerializeField]
    private float baseSpeed = 20f;
    [SerializeField]
    private float gravity = 20f;
    private Vector3 moveVec;
    private float inputDir;

    public Animator sAnimator;
    public GameObject mesh;
    private Rigidbody b;
    private enum Direction
    {
        LEFT,
        RIGHT
    };
    private Direction oldDir;
    private Direction newDir = Direction.RIGHT;

    private bool onGround = false;

    // Use this for initialization
    void Start () {
        jumpCount = baseJumpCount;
        b = GetComponent<Rigidbody>();
    }
    void Update()
    {
        if (Input.GetButtonDown("Jump") && jumpCount != 0)
        {
            sAnimator.SetTrigger("jump");
            b.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
            jumpCount -= 1;
        }
        inputDir = Input.GetAxis("Horizontal");
        sAnimator.SetFloat("hspeed", Mathf.Abs(inputDir));
        sAnimator.SetFloat("idleTime", Mathf.Abs(Mathf.Sin(Time.time)));

        oldDir = newDir;
        newDir = (inputDir < 0) ? Direction.LEFT : Direction.RIGHT;
        if(newDir != oldDir)
        {
            switch(newDir)
            {
                case Direction.LEFT: mesh.transform.rotation = Quaternion.Euler(0,-90,0); break;
                case Direction.RIGHT: mesh.transform.rotation = Quaternion.Euler(0,90,0); break;
            }
            b.velocity = new Vector3(0, b.velocity.y, 0);
        }
        else if (Mathf.Abs(inputDir) < deathZone)
            b.velocity = new Vector3(0, b.velocity.y, 0);
        else
        {
            b.AddForce(new Vector3(inputDir * Time.deltaTime * baseSpeed, 0, 0), ForceMode.Impulse);
        }
        if (transform.eulerAngles.z <= 90 || transform.eulerAngles.z >= 270)
        {
            transform.eulerAngles = Vector3.up;
        }

        if(!onGround)
        {
            //falling
            sAnimator.SetFloat("vspeed", Mathf.Abs(b.velocity.normalized.y) + 0.1f);
            b.AddForce(Vector3.down * gravity * Time.deltaTime, ForceMode.Impulse);
        }
    }

   
    void FixedUpdate()
    {
        if (b.velocity.magnitude > maxSpeedY)
        {
            b.velocity = b.velocity.normalized * maxSpeedY;
        }
    }
    

    void OnCollisionEnter(Collision hit)
    {
        if (hit.collider.gameObject.tag == "Floor")
        {
            jumpCount = baseJumpCount;
            onGround = true;
            sAnimator.SetBool("land", true);
            b.AddForce(Vector3.up*b.velocity.y/2 , ForceMode.Impulse);
        }
    }

    void OnCollisionExit(Collision hit)
    {
        if (hit.collider.gameObject.tag == "Floor")
        {
            onGround = false;
            sAnimator.SetBool("land", false);
        }
    }
}

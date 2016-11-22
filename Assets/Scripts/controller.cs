using UnityEngine;
using System.Collections;

public class controller : MonoBehaviour {
    [SerializeField]
    private float jumpHeight = 7.5f;
    [SerializeField]
    private int baseJumpCount = 1;
    private int jumpCount = 0;

    [SerializeField]
    private float baseSpeed = 20f;
    [SerializeField]
    private float gravity = 20f;
    private Vector3 moveVec;
    private float inputDir;

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
            b.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
            jumpCount -= 1;
        }
        inputDir = Input.GetAxis("Horizontal");
        oldDir = newDir;
        newDir = (inputDir < 0) ? Direction.LEFT : Direction.RIGHT;
        if (newDir != oldDir) b.velocity = new Vector3(0,b.velocity.y,0);
        b.AddForce(new Vector3(inputDir*Time.deltaTime*baseSpeed,0,0), ForceMode.Impulse);
        if (transform.eulerAngles.z <= 90 || transform.eulerAngles.z >= 270)
        {
            transform.eulerAngles = Vector3.up;
        }

        if(!onGround)
        {
            //falling
            b.AddForce(Vector3.down * gravity * Time.deltaTime, ForceMode.Impulse);
        }

        Debug.Log(onGround);
    }
    void OnCollisionEnter(Collision hit)
    {
        if (hit.gameObject.tag == "Floor")
        {
            jumpCount = baseJumpCount;
            onGround = true;
            b.AddForce(Vector3.up, ForceMode.Impulse);
        }
    }

    void OnCollisionExit(Collision hit)
    {
        if (hit.gameObject.tag == "Floor")
        {
            onGround = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    
    public float movespeed = 10f;
    public float rotatespeed = 75f;
    public float jumpVelocity = 5f;
    public float distanceToGround = 0.1f;

    public LayerMask groundLayer;

    public GameObject bullet;
    public float bulletSpeed = 100f;

    private CapsuleCollider _col;

    private float vInput;
    private float hInput;

    private Rigidbody _rb;
    
    private GameBehavior _gameManager;

    public delegate void JumpingEvent();

    public event JumpingEvent playerJump;


    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<CapsuleCollider>();

        _gameManager = GameObject.Find("GameManager").GetComponent<GameBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        
        vInput = Input.GetAxis("Vertical") * movespeed;

        
        hInput = Input.GetAxis("Horizontal") * rotatespeed;


        // this.transform.Translate(Vector3.forward * vInput * Time.deltaTime); 


        // this.transform.Rotate(Vector3.up * hInput * Time.deltaTime);

    }

    private void FixedUpdate()
    {
        Vector3 rotation = Vector3.up * hInput;
        Quaternion anglerot = Quaternion.Euler(rotation * Time.fixedDeltaTime);

        _rb.MovePosition(this.transform.position + this.transform.forward * vInput * Time.fixedDeltaTime);
        _rb.MoveRotation(_rb.rotation * anglerot);

        if (IsGrounded() && Input.GetKeyDown(KeyCode.Space))
        {
            _rb.AddForce(Vector3.up * jumpVelocity, ForceMode.Impulse);
            playerJump();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            GameObject newBullet = Instantiate(bullet, this.transform.position + new Vector3(1, 0, 0), this.transform.rotation) as GameObject;
            Rigidbody bulletRB = newBullet.GetComponent<Rigidbody>();

            bulletRB.velocity = this.transform.forward * bulletSpeed;
        }

        
    }
    private bool IsGrounded()
    {
        Vector3 capsuleBottom = new Vector3(_col.bounds.center.x, _col.bounds.min.y, _col.bounds.center.z);
        bool grounded = Physics.CheckCapsule(_col.bounds.center, capsuleBottom, distanceToGround, groundLayer, QueryTriggerInteraction.Ignore);

        return grounded;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Enemy")
        {
            _gameManager.HP -= 1;
        }
    }
}

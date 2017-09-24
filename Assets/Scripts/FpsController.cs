
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FpsController : MonoBehaviour {
    /*
     * It is just the place where all the calculated values are being implemented on
     * the rigid body.
     * */
    public Camera cam;
    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private float CamRotateX = 0f;
    private float currentRotationX = 0f;
    private Vector3 jumpForce = Vector3.zero;
    private Rigidbody body;

    public float cameraRotationLimit = 80f;

    public void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    public void move(Vector3 v)
    {
        velocity = v;
    }

    public void rotate(Vector3 r)
    {
        rotation = r;
    }

    public void Camerarotate(float R_x)
    {
        CamRotateX = R_x;
    }

    private void FixedUpdate()
    {
        Movement();
        Rotation();
    }

    void Movement()
    {
        if(velocity != Vector3.zero)
        {
            body.MovePosition(body.position + velocity * Time.fixedDeltaTime);
        }
        if(jumpForce != Vector3.zero)
        {
            body.transform.Translate(jumpForce, Space.World);
        }

    }

    void Rotation()
    {
        if(rotation != Vector3.zero)
        {
            body.MoveRotation(body.rotation *Quaternion.Euler(rotation));
        }
        if (cam != null)
        {
            currentRotationX -= CamRotateX;
            currentRotationX = Mathf.Clamp(currentRotationX, -cameraRotationLimit, cameraRotationLimit);
            cam.transform.localEulerAngles = new Vector3(currentRotationX, 0f, 0f);
        }
    }

    public void ApplyJumpForce(Vector3 j)
    {
        jumpForce = j;
    }

   
}

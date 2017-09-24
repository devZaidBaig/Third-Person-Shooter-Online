using UnityEngine;

[RequireComponent(typeof(FpsController))]
public class FpsMovement : MonoBehaviour {
    /*
     * This Script is actually calculates all the values for rotation and movement.
     * Make sure you try serialized field.
     * All inputs are taken in this Script.
     * */

    public float speed = 5f;
    public float mouseSensitivity = 3f;
    public FpsController control;
    public float jumpForce = 1000;

	// Use this for initialization
	void Start () {
        Cursor.visible = false;
        control = GetComponent<FpsController>();
	}
	
	// Update is called once per frame
	void Update () {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 moveRight = transform.right * x;
        Vector3 moveForward = transform.forward * z;

        Vector3 velocity = (moveRight + moveForward).normalized * speed;

        control.move(velocity);

        float rotate_y = Input.GetAxisRaw("Mouse X");
        float rotate_x = Input.GetAxisRaw("Mouse Y");
 

        Vector3 rotationMove = new Vector3(0f, rotate_y, 0f) * mouseSensitivity;
        control.rotate(rotationMove);

        float cameraRotation = rotate_x * mouseSensitivity;
        control.Camerarotate(cameraRotation);

        //For jumping
        Vector3 jump = Vector3.zero;
        if (Input.GetKey(KeyCode.Space))
        {
            jump = Vector3.up * jumpForce * Time.deltaTime;
        }

        control.ApplyJumpForce(jump);
	}
}

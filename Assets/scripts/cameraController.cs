using UnityEngine;

public class cameraController : MonoBehaviour {

    public Transform trackpos;
    public float lookSensitivity;
    public float scrolSensitivity;
    public bool control;
    public Vector2 heightClamp;
    Vector3 camVelocity;
    public float smoothTime;

    float height;

    private void Start() {
        height = transform.position.y;
    }

    void Update() {
        if (control) controlCamera();
        else TrackObject(trackpos);

        if (Input.GetKeyDown(KeyCode.Space)) control = !control;
        if (new Vector3((Input.GetKey(KeyCode.D) ? 1 : 0) - (Input.GetKey(KeyCode.A) ? 1 : 0), 0, (Input.GetKey(KeyCode.W) ? 1 : 0) - (Input.GetKey(KeyCode.S) ? 1 : 0)).magnitude > .1f)
            control = true;

        height -= Input.mouseScrollDelta.y * scrolSensitivity;
        height = Mathf.Clamp(height,heightClamp.x,heightClamp.y);
    }

    void controlCamera() {
        Vector3 walkVector = new Vector3((Input.GetKey(KeyCode.D) ? 1 : 0) - (Input.GetKey(KeyCode.A) ? 1 : 0),0, (Input.GetKey(KeyCode.W) ? 1 : 0) - (Input.GetKey(KeyCode.S) ? 1 : 0)).normalized;
        Vector3 desiredPos = new Vector3(transform.position.x,height, transform.position.z) + walkVector * lookSensitivity;
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPos, ref camVelocity, smoothTime);
        transform.position = smoothedPosition;
    }

    void TrackObject(Transform trackObject) {
        Vector3 desiredPos = trackObject.position + Vector3.up * height;
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPos, ref camVelocity, smoothTime);
        transform.position = smoothedPosition;
    }
}
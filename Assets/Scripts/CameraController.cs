using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    private float cameraMoveSpeed = 12f;
    private float cameraRotationSpeed = 100f;
    private CinemachineTransposer cinemachineTransposer;

    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] private float zoomAmount = 2f;
    [SerializeField] private float zoomSpeed = 150f; // Speed of zooming in and out
    [SerializeField] private float minZoomDistance = 7f; // Minimum zoom distance
    [SerializeField] private float maxZoomDistance = 20f; // Maximum zoom distance



    // Start is called before the first frame update
    void Start()
    {
        cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        if (cinemachineTransposer == null)
        {
            Debug.LogError("Camera Controller unable to get Transposer!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleMove();
        HandleRotate();
        HandleZoom();
    }

    private void HandleMove()
    {
        Vector3 inputMoveDirection = Vector3.zero; // More concise way to create a zero vector.

        // Check for vertical movement.
        if (Input.GetKey(KeyCode.W))
        {
            inputMoveDirection.z += 1f; // Using '+=' allows multiple keys to influence the direction.
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputMoveDirection.z -= 1f;
        }

        // Check for horizontal movement.
        if (Input.GetKey(KeyCode.D))
        {
            inputMoveDirection.x += 1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputMoveDirection.x -= 1f;
        }

        // Normalize the input direction if necessary (prevents faster diagonal movement).
        if (inputMoveDirection.magnitude > 1f)
        {
            inputMoveDirection = inputMoveDirection.normalized;
        }

        // Calculate the actual movement direction and apply it to the transform.
        Vector3 moveDirection = transform.forward * inputMoveDirection.z + transform.right * inputMoveDirection.x;
        transform.position += moveDirection * cameraMoveSpeed * Time.deltaTime; // Using moveDirection instead of inputMoveDirection here.

    }

    private void HandleRotate()
    {
        Vector3 rotationVector = Vector3.zero;

        if (Input.GetKey(KeyCode.Q)) { rotationVector.y += 1f; }
        if (Input.GetKey(KeyCode.E)) { rotationVector.y -= 1f; }

        transform.eulerAngles += rotationVector * cameraRotationSpeed * Time.deltaTime;
    }

    private void HandleZoom()
    {
        float zoomValue = Input.mouseScrollDelta.y;
        if (cinemachineTransposer != null)
        {
            Vector3 followOffset = cinemachineTransposer.m_FollowOffset;

            if (zoomValue > 0f)
            {
                followOffset.y -= zoomAmount;
            }
            if (zoomValue < 0f)
            {
                followOffset.y += zoomAmount;
            }
            followOffset.y = Mathf.Clamp(followOffset.y, minZoomDistance, maxZoomDistance);

            cinemachineTransposer.m_FollowOffset =
                Vector3.Lerp(cinemachineTransposer.m_FollowOffset, followOffset, Time.deltaTime * zoomSpeed);

        }
    }
}

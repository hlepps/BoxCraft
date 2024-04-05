using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    static CameraManager instance;
    public static CameraManager GetInstance() { return instance; }
    [SerializeField]
    float movementSpeed = 1f;
    [SerializeField]
    float scrollSpeed = 1f;
    [SerializeField]
    float minCamBounds = -10f;
    [SerializeField]
    float maxCamBounds = 10f;
    [SerializeField]
    float mouseScreenOffset = 0.05f;

    [SerializeField]
    Transform cameraHolder;
    [SerializeField]
    LayerMask cameraGrabLayerMask;

    [SerializeField] AudioSource windAmbience;
    [SerializeField] AudioSource forestAmbience;


    float currentZoom = 0.5f;

    Camera cameraComponent;

    public Camera GetCameraComponent() { return cameraComponent; }

    private void Start()
    {
        instance = this;
        cameraComponent = GetComponent<Camera>();
    }

    public void MoveCamera(Vector3 direction)
    {
        cameraHolder.Translate(direction);
        cameraHolder.position = Useful.ClampVector3(cameraHolder.position, minCamBounds, maxCamBounds);
    }


    Vector2 lastMousePos = new Vector2(0,0);
    private void Update()
    {
        Vector2 mousePos = Input.mousePosition;
        // orthograpic
        //cameraComponent.orthographicSize = Mathf.Clamp(cameraComponent.orthographicSize - Input.mouseScrollDelta.y * scrollSpeed,
        //                                                minScrollSize, maxScrollSize);

        currentZoom -= Input.mouseScrollDelta.y * scrollSpeed * Time.deltaTime;
        currentZoom = Mathf.Clamp(currentZoom, 0.01f, 0.99f);
        GetComponent<Animator>().SetFloat("AnimationTime", currentZoom);
        windAmbience.volume = currentZoom;
        forestAmbience.volume = (1f-currentZoom)*0.5f;

        var mouseViewportPos = cameraComponent.ScreenToViewportPoint(mousePos);
        
        // orthographic
        //Vector3 right = (cameraHolder.right + cameraHolder.forward).normalized;
        //Vector3 forward = (-cameraHolder.right + cameraHolder.forward).normalized;
        Vector3 right = cameraHolder.right;
        Vector3 forward = cameraHolder.forward;
        float scaledMovementSpeed = movementSpeed * (Mathf.Clamp(currentZoom,0.15f,1f) * 2f);

        if (Input.GetMouseButton(2))
        {
            if(lastMousePos != Vector2.zero)
            {
                Ray a = cameraComponent.ScreenPointToRay(mousePos);
                Ray b = cameraComponent.ScreenPointToRay(lastMousePos);
                Physics.Raycast(a, out RaycastHit hitA, 100, cameraGrabLayerMask);
                Vector3 aV = hitA.point;
                aV.y = 0;
                Physics.Raycast(b, out RaycastHit hitB, 100, cameraGrabLayerMask);
                Vector3 bV = hitB.point;
                bV.y = 0;
                //Debug.Log($"bv{bV} av{aV} bv-av{bV - aV}");
                MoveCamera(bV - aV);
                // orthographic: MoveCamera(straightCamera.ScreenToWorldPoint(lastMousePos) - straightCamera.ScreenToWorldPoint(mousePos));
            }
        }
        else
        {
            if ((mouseViewportPos.x < mouseScreenOffset && mouseViewportPos.x > 0f) || Input.GetKey(KeyCode.LeftArrow))
            {
                MoveCamera(-right * scaledMovementSpeed * Time.deltaTime);
            }
            if ((mouseViewportPos.x > 1f - mouseScreenOffset && mouseViewportPos.x < 1f) || Input.GetKey(KeyCode.RightArrow))
            {
                MoveCamera(right * scaledMovementSpeed * Time.deltaTime);
            }
            if ((mouseViewportPos.y < mouseScreenOffset && mouseViewportPos.y > 0f) || Input.GetKey(KeyCode.DownArrow))
            {
                MoveCamera(-forward * scaledMovementSpeed * Time.deltaTime);
            }
            if ((mouseViewportPos.y > 1f - mouseScreenOffset && mouseViewportPos.y < 1f) || Input.GetKey(KeyCode.UpArrow))
            {
                MoveCamera(forward * scaledMovementSpeed * Time.deltaTime);
            }
        }



        lastMousePos = mousePos;
    }
}

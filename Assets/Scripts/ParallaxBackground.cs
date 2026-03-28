using UnityEngine;

public class ParallaxBackground : MonoBehaviour{
    private Vector3 lastCameraPosition;

    public Transform cameraTransform;

    public float parallaxEffectMultiplier = 0.5f;

    void Start() {
        lastCameraPosition = cameraTransform.position;
    }

    void Update() {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier, deltaMovement.y, 0f);
        lastCameraPosition = cameraTransform.position;
    }
}

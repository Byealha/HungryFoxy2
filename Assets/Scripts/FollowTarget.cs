using UnityEngine;

public class FollowTarget : MonoBehaviour {
    public Transform target;

    public float smoothTime = 0.15f;
    public float XMaxValue = 0;
    public float XMinValue = 0;
    public float YMaxValue = 0;
    public float YMinValue = 0;

    public bool XMaxEnabled = false;
    public bool XMinEnabled = false;
    public bool YMaxEnabled = false;
    public bool YMinEnabled = false;

    Vector3 velocity = Vector3.zero;

    void FixedUpdate() {
        Vector3 targetPos = target.position;

        if (YMinEnabled && YMaxEnabled)
            targetPos.y = Mathf.Clamp(target.position.y, YMinValue, YMaxValue);

        else if (YMinEnabled)
            targetPos.y = Mathf.Clamp(target.position.y, YMinValue, target.position.y);

        else if (YMaxEnabled)
            targetPos.y = Mathf.Clamp(target.position.y, target.position.y, YMaxValue);

        if (XMinEnabled && XMaxEnabled)
            targetPos.x= Mathf.Clamp(target.position.x, XMinValue, XMaxValue);

        else if (XMinEnabled)
            targetPos.x = Mathf.Clamp(target.position.x, XMinValue, target.position.x);

        else if (XMaxEnabled)
            targetPos.x = Mathf.Clamp(target.position.x, target.position.x, XMaxValue);

        targetPos.z = transform.position.z;

        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
    }
}

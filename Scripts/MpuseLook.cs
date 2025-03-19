using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MpuseLook : MonoBehaviour
{
    public enum RotationAxes
    {
        MouseXY = 0,
        MouseX = 1,
        MouseY = 2,
    }

    public RotationAxes axes = RotationAxes.MouseXY;
    public float sens = 9.0f;
    public float VertBounds = 45.0f;

    private float VR = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.freezeRotation = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (axes == RotationAxes.MouseX)
        {
            //Horozontal
            transform.Rotate(0, Input.GetAxis("Mouse X") * sens, 0);
        }
        else if (axes == RotationAxes.MouseY)
        {
            //Vertical
            VR -= Input.GetAxis("Mouse Y") * sens;
            VR = Mathf.Clamp (VR, -VertBounds, VertBounds);

            float horizontalRot = transform.localEulerAngles.y;

            transform.localEulerAngles = new(VR, horizontalRot, 0);
        }
        else
        {
            //Both
            VR -= Input.GetAxis("Mouse Y") * sens;
            VR = Mathf.Clamp(VR, -VertBounds, VertBounds);

            float delta = Input.GetAxis("Mouse X") * sens;
            float horizontalRot = transform.localEulerAngles.y + delta;

            transform.localEulerAngles = new(VR, horizontalRot, 0);
        }
    }
}

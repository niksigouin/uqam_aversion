using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public float speed = 10;
    public bool active = true;

    private void Start()
    {
        CheckActive();
    }

    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            active = !active;
            CheckActive();
        }

        if (!active)
            return;

        Vector3 newrot = new Vector3();
        newrot.y = Time.deltaTime * Input.GetAxis("Mouse X") * speed;
        newrot.x = Time.deltaTime * -Input.GetAxis("Mouse Y") * speed;
        newrot.z = 0;
        
        transform.Rotate(newrot);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
    }

    void CheckActive()
    {
        if (active)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_script : MonoBehaviour
{
    public Transform followTransform;
    public BoxCollider2D mapBounds;
    
    private float xMin, xMax;
    private float camY,camX;
    private Camera mainCam;
    // Start is called before the first frame update
    void Start()
    {
        xMin = mapBounds.bounds.min.x;
        xMax = mapBounds.bounds.max.x;
        mainCam = GetComponent<Camera>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        camX = Mathf.Clamp(followTransform.position.x, 5.0f, xMax);
        this.transform.position = new Vector3(camX, 7.5f, this.transform.position.z);
    }
}

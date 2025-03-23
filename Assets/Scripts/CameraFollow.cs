using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public BoxCollider2D bounds;

    private float minX, maxX, minY, maxY;
    private float camHalfWidth, camHalfHeight;

    private void Start()
    {
        if (bounds != null)
        {
            Bounds boxBounds = bounds.bounds;
            Camera cam = Camera.main;
            camHalfHeight = cam.orthographicSize;
            camHalfWidth = camHalfHeight * cam.aspect;

            minX = boxBounds.min.x + camHalfWidth;
            maxX = boxBounds.max.x - camHalfWidth;
            minY = boxBounds.min.y + camHalfHeight;
            maxY = boxBounds.max.y - camHalfHeight;
        }
    }

    private void LateUpdate()
    {
        if (target == null) return;

        float targetX = Mathf.Clamp(target.position.x, minX, maxX);
        float targetY = Mathf.Clamp(target.position.y, minY, maxY);

        transform.position = new Vector3(targetX, targetY, transform.position.z);
    }
}

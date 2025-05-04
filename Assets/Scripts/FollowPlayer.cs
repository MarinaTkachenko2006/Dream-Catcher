using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow_player : MonoBehaviour
{
    public Transform target;
    public float smoothing = 5f;
    private Vector3 offset;

    void Start()
    {
        offset = transform.position - target.position;
    }

    void LateUpdate()
    {
        Vector3 targetCamPos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
    }
}

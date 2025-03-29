using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeNPC : MonoBehaviour
{
    [SerializeField] private Transform pupil;
    [SerializeField] private float maxPupilOffset = 0.1f;
    [SerializeField] private float smoothSpeed = 5f;

    private Transform player;

    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    private void Update()
    {
        if (player == null || pupil == null)
            return;
        Vector3 direction = player.position - transform.position;
        Vector3 targetLocalPos = direction.normalized * maxPupilOffset;
        pupil.localPosition = Vector3.Lerp(pupil.localPosition, targetLocalPos, Time.deltaTime * smoothSpeed);
    }
}


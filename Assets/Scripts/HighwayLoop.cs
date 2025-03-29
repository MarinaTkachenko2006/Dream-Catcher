using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighwayLoop : MonoBehaviour
{
    public float topY = 10f; // Координата Y, на которой будет телепорт
    public float bottomY = -10f; // Координата Y, куда телепортируется игрок
    GameObject playerObj;

    private void Awake()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (playerObj.transform.position.y > topY)
        {
            Vector3 newPosition = new Vector3(playerObj.transform.position.x, bottomY, playerObj.transform.position.z);
            playerObj.transform.position = newPosition;

            PlayerController pc = playerObj.GetComponent<PlayerController>();
            if (pc != null)
            {
                pc.ResetMovement();
            }
        }
        else if (playerObj.transform.position.y < bottomY)
        {
            Vector3 newPosition = new Vector3(playerObj.transform.position.x, topY, playerObj.transform.position.z);
            playerObj.transform.position = newPosition;

            PlayerController pc = playerObj.GetComponent<PlayerController>();
            if (pc != null)
            {
                pc.ResetMovement();
            }
        }
    }
}

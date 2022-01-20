using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeDoubleJump : MonoBehaviour
{
    private void Start()
    {
        if (PlayerController.availableJumps > 1)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Upgrade triggered " + name);
            PlayerController.availableJumps = 2;
            Destroy(gameObject);
        }
    }
}

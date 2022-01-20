using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeWallJump : MonoBehaviour
{

    private void Start()
    {
        if (PlayerController.canWallJump == true)
            Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("Upgrade triggered " + name);
            PlayerController.canWallJump = true;
            Destroy(gameObject);
        }
    }


}

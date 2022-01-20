using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public static PlayerManager instance;
    public ParticleSystem upgradeEffect;
    public Transform playerPosition;

    // Start is called before the first frame update
    void Start()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Upgrade")
        {
            Debug.Log("Upgrade Triggered");
            Instantiate(upgradeEffect, playerPosition);
        }
    }


}

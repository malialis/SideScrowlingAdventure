using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float timeOffSet;
    [SerializeField] private Vector3 offSetPos;

    [SerializeField] private Vector3 boundsMin;
    [SerializeField] private Vector3 boundsMax;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(player != null)
        {
            Vector3 startPos = transform.position;
            Vector3 targetPos = player.position;

            targetPos.x += offSetPos.x;
            targetPos.y += offSetPos.y;
            targetPos.z = transform.position.z;

            targetPos.x = Mathf.Clamp(targetPos.x, boundsMin.x, boundsMax.x);
            targetPos.y = Mathf.Clamp(targetPos.y, boundsMin.y, boundsMax.y);

            float targetTime = Mathf.Pow(1f - timeOffSet, Time.deltaTime * 30);
            transform.position = Vector3.Lerp(startPos, targetPos, targetTime);
        }
    }


}

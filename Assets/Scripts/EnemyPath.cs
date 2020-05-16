using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPath : MonoBehaviour
{
    WaveConfig waveConfig;
    List<Transform> waypoints;
    int WayPointsIndex = 0;
   
    void Start()
    {
        
        waypoints = waveConfig.GetWayPoints();
        transform.position = waypoints[WayPointsIndex].transform.position;
    }

    public void SetWaveConfig(WaveConfig waveConfig)
    {
        this.waveConfig = waveConfig;
    }
    void Update()
    {
        if (WayPointsIndex <= waypoints.Count - 1)
        {
            var TargetPos = waypoints[WayPointsIndex].transform.position;
            var Speed = waveConfig.GetMoveSpeed()* Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, TargetPos, Speed);

            if (transform.position == TargetPos)
            {
                WayPointsIndex++;
            }

        }
        else
        {
            Destroy(gameObject);
        }


    }
        
    }


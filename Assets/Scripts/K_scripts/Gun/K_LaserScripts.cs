using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K_LaserScripts : MonoBehaviour
{
    private LineRenderer laserLine;

    void Start()
    {

        laserLine = GetComponent<LineRenderer>();

        laserLine.startWidth = 0.1f;
        laserLine.endWidth = 0.1f;

        laserLine.enabled = false;
    }

    public void FireLaser(Vector3 startPoint, Vector3 endPoint)
    {

        laserLine.SetPosition(0, startPoint);
        laserLine.SetPosition(1, endPoint);
        laserLine.enabled = true;
    }

    public void DisableLaser()
    {

        laserLine.enabled = false;
    }
}

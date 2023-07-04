using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectStat : MonoBehaviour
{
    public int healthPoint;
    public int agilityPoint;
    public int powerPoint;
    public int rangePoint;
    public int aspeedPoint;
    public int sumPoint;

    public bool overPoint(int point)
    {
        return (sumPoint + point) > 100;
    }
}

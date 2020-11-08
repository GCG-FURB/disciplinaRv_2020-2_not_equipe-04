using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MotionUtility
{
    public static Vector3 Normalize(GameObject obj)
    {
        float x = obj.transform.position.x;
        float xNormalized;

        if (x < -3f)
        {
            xNormalized = -5f;
        }
        else if (x > 3f)
        {
            xNormalized = 5f;
        }
        else
        {
            xNormalized = 0f;
        }

        return new Vector3(xNormalized, obj.transform.position.y, obj.transform.position.z);
    }
}

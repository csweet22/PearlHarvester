using System;
using UnityEngine;


public static class StaticHelpers
{
    public static Vector3 Change(this Vector3 srcVector, object x = null, object y = null, object z = null)
    {
        float newX = x == null ? srcVector.x : Convert.ToSingle(x);
        float newY = y == null ? srcVector.y : Convert.ToSingle(y);
        float newZ = z == null ? srcVector.z : Convert.ToSingle(z);

        return new Vector3(newX, newY, newZ);
    }

    public static Vector3 ChangeDelta(this Vector3 srcVector, object x = null, object y = null, object z = null)
    {
        float newX = x == null ? srcVector.x : Convert.ToSingle(x);
        float newY = y == null ? srcVector.y : Convert.ToSingle(y);
        float newZ = z == null ? srcVector.z : Convert.ToSingle(z);

        return new Vector3(srcVector.x + newX, srcVector.y + newY, srcVector.z + newZ);
    }

    public static int GetLayerIndex(LayerMask mask)
    {
        int layer = mask.value;
        if (layer == 0 || (layer & (layer - 1)) != 0){
            Debug.LogError("No layers or multiple layers selected.");
            return -1;
        }

        int index = 0;
        while (layer > 1){
            layer = layer >> 1;
            index++;
        }

        return index;
    }

    public static void DrawBounds(Bounds b, float delay = 0)
    {
        var p1 = new Vector3(b.min.x, b.min.y, b.min.z);
        var p2 = new Vector3(b.max.x, b.min.y, b.min.z);
        var p3 = new Vector3(b.max.x, b.min.y, b.max.z);
        var p4 = new Vector3(b.min.x, b.min.y, b.max.z);

        Debug.DrawLine(p1, p2, Color.blue, delay);
        Debug.DrawLine(p2, p3, Color.red, delay);
        Debug.DrawLine(p3, p4, Color.yellow, delay);
        Debug.DrawLine(p4, p1, Color.magenta, delay);

        var p5 = new Vector3(b.min.x, b.max.y, b.min.z);
        var p6 = new Vector3(b.max.x, b.max.y, b.min.z);
        var p7 = new Vector3(b.max.x, b.max.y, b.max.z);
        var p8 = new Vector3(b.min.x, b.max.y, b.max.z);

        Debug.DrawLine(p5, p6, Color.blue, delay);
        Debug.DrawLine(p6, p7, Color.red, delay);
        Debug.DrawLine(p7, p8, Color.yellow, delay);
        Debug.DrawLine(p8, p5, Color.magenta, delay);

        Debug.DrawLine(p1, p5, Color.white, delay);
        Debug.DrawLine(p2, p6, Color.gray, delay);
        Debug.DrawLine(p3, p7, Color.green, delay);
        Debug.DrawLine(p4, p8, Color.cyan, delay);
    }
}
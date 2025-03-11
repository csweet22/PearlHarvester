using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ACMenu : MonoBehaviour
{
    public virtual void Open()
    {
        transform.localScale = Vector3.one;
    }

    public virtual void Close()
    {
        transform.localScale = Vector3.zero;
    }
}

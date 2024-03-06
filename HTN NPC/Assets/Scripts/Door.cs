using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool open = false;
    public bool hasTimer = false;
    public float time = 3;

    private void Awake()
    {
        if (open)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
    }

    private void OpenDoor()
    {
        gameObject.SetActive(false);
        open = true;
    }

    private void CloseDoor()
    {
        gameObject.SetActive(true);
        open = false;
    }

    public void Open()
    {
        if (hasTimer)
        {
            Invoke("OpenDoor", time);
        }
        else
        {
            OpenDoor();
        }
    }

    public void Close()
    {
        if (hasTimer)
        {
            Invoke("CloseDoor", time);
        }
        else
        {
            CloseDoor();
        }
    }
}

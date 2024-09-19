using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    public float cooldown = 30f;

    public void ActivateBarrier()
    {
        this.gameObject.SetActive(true);
    }
    public void DisableBarrier()
    {
        this.gameObject.SetActive(false);
    }
}

using QuangDM.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(DropTimer());
        }
    }
    private IEnumerator DropTimer()
    {
        yield return new WaitForSeconds(1.5f);
        Observer.Instance.Notify("ReactivatePlatform", this.transform.parent.gameObject);
        this.transform.parent.gameObject.SetActive(false);
    }
}

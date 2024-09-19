using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    private GameObject iceSpikePrefab;
    private GameObject lightningBoltPrefab;
    private Barrier barrier;

    private bool isBarrier;
    private bool isIceSpikes;
    private bool isLightningBolts;

    public void Initialize()
    {
        iceSpikePrefab = ResourcesManager.Instance.Load<GameObject>("Prefabs/ActiveAbilities/IceSpike");
        lightningBoltPrefab = ResourcesManager.Instance.Load<GameObject>("Prefabs/ActiveAbilities/LightningBolt");
        barrier = Player.Instance.gameObject.GetComponentInChildren<Barrier>();
    }
    private void Update()
    {
        if (isBarrier)
        {

        }
        if (isIceSpikes)
        {

        }
        if (isLightningBolts)
        {

        }
    }
}

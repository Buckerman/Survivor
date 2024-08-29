using QuangDM.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallet : MonoBehaviour
{
    private int _coinAmount = 0;

    public void UpdateWallet(int amount)
    {
        _coinAmount += amount;
        Observer.Instance.Notify(EventName.UpdateWalletUI, _coinAmount);
    }
}

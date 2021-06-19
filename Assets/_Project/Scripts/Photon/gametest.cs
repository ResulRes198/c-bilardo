using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gametest : MonoBehaviour
{
    MetamaskConnection metamask = null;

    // Start is called before the first frame update
    void Start()
    {
        GameObject temp = GameObject.Find("Metamask Controller");
        metamask = temp.GetComponent<MetamaskConnection>();
        Debug.Log($"this user is attending this game with this wallet address: {metamask.walletAddr}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

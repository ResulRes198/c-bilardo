using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PopUpBidScreen : MonoBehaviour
{
    public GameObject Panel;
    public GameObject Button1;
    public GameObject Button2;
    public GameObject Button3;
    public MetamaskConnection metamask;
    public double balance { get; private set; }
    private void Start()
    {
        getBalance();
    }
    public void getBalance()
    {
        metamask.getBallanceRequest();
        balance = metamask.Balance;
        Debug.Log("get balane request");
    }
    public void OpenPanel()
    {
        if (Panel != null)
        {
            Panel.SetActive(true);
        }
    }
    public void showAvailableBets()
    {
        checkBalanece1();
        checkBalanece2();
        checkBalanece3();
    }
    public void checkBalanece1()
    {
        if (metamask.Balance > 0.1)
        {
            if (Button1 != null)
            {
                Button1.SetActive(true);
            }
        }
    }
    public void checkBalanece2()
    {
        if (metamask.Balance > 0.3)
        {
            if (Button2 != null)
            {
                Button2.SetActive(true);
            }
        }
    }
    public void checkBalanece3()
    {
        if (metamask.Balance > 0.9)
        {
            if (Button2 != null)
            {
                Button3.SetActive(true);
            }
        }
    }
    public void closeBetPanel()
    {
        Panel.SetActive(false);
    }
}

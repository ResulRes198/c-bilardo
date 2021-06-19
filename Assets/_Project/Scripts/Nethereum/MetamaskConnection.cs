using System.Collections;
using Nethereum.JsonRpc.UnityClient;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Util;
using UnityEngine;
using UnityEngine.UI;
using System;
using Firebase.Database;
using Firebase;

public class MetamaskConnection : MonoBehaviour
{
    // Start is called before the first frame update
    //[SerializeField] private string walletAddr;
    [SerializeField] public double Balance { get; private set; }
    [SerializeField] public string PrivateKey;
    [SerializeField] public string walletAddr;
    [SerializeField] public GameObject dont_destroy;
    [SerializeField] public Text balanceText;
    public string url = "https://ropsten.infura.io/v3/6bc36bee51e8447690190d4878b7ac2e";

    //database
    FirebaseFunctions firebase;

    void Start()
    {
        firebase = FindObjectOfType<FirebaseFunctions>();
        walletAddr = firebase.walletAddr;

        Debug.Log("wallet address is : "+ walletAddr);
        getBallanceRequest();
        
    }
    public void getBallanceRequest()
    {
        StartCoroutine(getBallance());
    }
    // Update is called once per frame
    void Update()
    {
        balanceText.text = "Balance of this user is: "+Balance.ToString();
    }
    #region Func
    public void setWalletAddr(string Addr)
    {
        walletAddr = Addr;
    }
    public IEnumerator getBallance()
    {
        var balanceRequest = new EthGetBalanceUnityRequest(url);
        yield return balanceRequest.SendRequest(walletAddr, BlockParameter.CreateLatest());
        var BalanceAddressTo = Convert.ToDouble(UnitConversion.Convert.FromWei(balanceRequest.Result.Value));
        yield return BalanceAddressTo;
        //Debug.Log("Balance of account:" + BalanceAddressTo);
        Balance = Convert.ToDouble(BalanceAddressTo);
    }
    public void TransferEther(string receiver_address, string private_key_of_sender, decimal amount)
    {
        StartCoroutine(TransferEtherFunc(receiver_address, private_key_of_sender, amount));
    }
    public IEnumerator TransferEtherFunc(string receiver_address, string private_key_of_sender, decimal amount)
    {
        var ethTransfer = new EthTransferUnityRequest(url, private_key_of_sender);
        yield return ethTransfer.TransferEther(receiver_address, amount, 2);

    }
    void Awake()
    {
        DontDestroyOnLoad(dont_destroy);    
    }
    #endregion
}

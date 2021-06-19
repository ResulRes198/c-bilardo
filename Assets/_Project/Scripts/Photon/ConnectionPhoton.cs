using UnityEngine;
using System;
using Photon.Pun;
using Photon.Realtime;

public class ConnectionPhoton : MonoBehaviourPunCallbacks
{
    FirebaseFunctions firebase;

    void Start()
    {
        Connect();
    }
    public void Connect()
    {
        firebase = FindObjectOfType<FirebaseFunctions>();
        string nickname = firebase.auth.CurrentUser.DisplayName.ToString();
        PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "eu";
        PhotonNetwork.AuthValues = new AuthenticationValues(nickname);
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.NickName = nickname;
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log($"Connect to Photon as {nickname}");
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Photon Connection is established on: " + PhotonNetwork.CloudRegion);
    }
}

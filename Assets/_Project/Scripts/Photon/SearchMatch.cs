using UnityEngine;
using System;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using ExitGames.Client.Photon;

public class SearchMatch : MonoBehaviourPunCallbacks
{
    MetamaskConnection metamask = null;
    public float BET;
    // Start is called before the first frame update
    private ExitGames.Client.Photon.Hashtable _userprops = new ExitGames.Client.Photon.Hashtable(); // we are going to add more properties about user. Check 19th line for learning how to add
    void Start()
    {
        GameObject temp = GameObject.Find("Metamask Controller");
        metamask = temp.GetComponent<MetamaskConnection>();
        //Debug.Log($"player2 is attending this game with this wallet address: {metamask.walletAddr}");
        _userprops["address"] = metamask.walletAddr;
        _userprops["secret"] = metamask.PrivateKey;
        PhotonNetwork.LocalPlayer.CustomProperties = _userprops;
    }
    public void FindMatch(float betAmount)
    {
        BET = betAmount;
        Hashtable expectedRoomProps = new Hashtable { { "B", BET } };
        PhotonNetwork.JoinRandomRoom(expectedRoomProps, 2);
    }
    
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("We couldn't find a proper room");
        MakeRoom();
    }
    public override void OnJoinedRoom()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.Log($"You joint a room that is called as {PhotonNetwork.CurrentRoom.Name}");
            Debug.Log($"Game is ready to start, your opponent is {PhotonNetwork.MasterClient.NickName}");
            Debug.Log($"your opponent's wallet address is {PhotonNetwork.MasterClient.CustomProperties["address"]}");
        }
    }
    public void MakeRoom()
    {
        int RandomRoomId = UnityEngine.Random.Range(1, 5000);
        string[] roomPropsInLobby = { "B" };   
        RoomOptions roomOptions = new RoomOptions()
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = 2,
            CustomRoomProperties = new Hashtable { { "B", BET } },
            CustomRoomPropertiesForLobby = roomPropsInLobby
        };
        Debug.Log($"room created for a Game with {BET} betsize");
        PhotonNetwork.CreateRoom("RoomName_"+ RandomRoomId,roomOptions);
    }
    public override void OnCreatedRoom()
    {
        Debug.Log("Room is created");
        
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log($"we cannot create room because of this: {message}");
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if(PhotonNetwork.CurrentRoom.PlayerCount == 2 && PhotonNetwork.IsMasterClient)
        {
            Debug.Log($"Game is ready to start, your opponent is {newPlayer.NickName}");
            Debug.Log($"your opponent's wallet address is {newPlayer.CustomProperties["address"]}");
            SceneController.LoadScene("GameScene");
        }
    }
    public override void OnJoinedLobby()
    {
        PhotonNetwork.JoinLobby();
        FindMatch(BET);
    }
}

using UnityEngine;
using System;
using Photon.Pun;
using Photon.Realtime;
public class PhotonConnector : MonoBehaviourPunCallbacks
{
    FirebaseFunctions firebase;

    #region Unity Method
    private void Start()
    {
        firebase = FindObjectOfType<FirebaseFunctions>();
        string username = firebase.auth.CurrentUser.Email.ToString();
        Debug.Log("welcome "+username);
        ConnectToPhoton(username);
    }
    #endregion
    #region Private Methods
    private void ConnectToPhoton(string nickname)
    {
        Debug.Log($"Connect to Photon as {nickname}");
        PhotonNetwork.AuthValues = new AuthenticationValues(nickname);
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.NickName = nickname;
        PhotonNetwork.ConnectUsingSettings();
    }
    private void CreatePhotonRoom(string RoomName)
    {
        RoomOptions ro = new RoomOptions();
        ro.IsOpen = true;
        ro.IsVisible = true;
        ro.MaxPlayers = 2;
        PhotonNetwork.JoinOrCreateRoom(RoomName, ro, TypedLobby.Default);
    }
    #endregion
    #region Public Methods
    #endregion
    #region Photon Callback
    public override void OnConnectedToMaster()
    {
        Debug.Log("you have connected to the photon master server");
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
    }
    public override void OnJoinedLobby()
    {
        Debug.Log("you have connected to photon lobby");
        CreatePhotonRoom("Test Room");
    }
    public override void OnCreatedRoom()
    {
        Debug.Log($"You created and named a room as {PhotonNetwork.CurrentRoom.Name}");

    }
    public override void OnJoinedRoom()
    {
        Debug.Log($"You joint a room that is called as {PhotonNetwork.CurrentRoom.Name}");
    }
    public override void OnLeftRoom()
    {
        Debug.Log("You have left the room");
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log($"you have failed to join room: {message}");
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"A new user has joined the room {newPlayer.UserId}");
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"Player has left the room {otherPlayer.UserId}");
    }
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.Log($"new master client is {newMasterClient.UserId}");
    }
    #endregion
}

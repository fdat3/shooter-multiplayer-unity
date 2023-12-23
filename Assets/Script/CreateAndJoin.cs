using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class CreateAndJoin : MonoBehaviourPunCallbacks
{
    public TMP_InputField input_create;
    public TMP_InputField input_join;
    
    public void CreateRoom()
    {
        print("I am create room");
        print(input_create.text);
        PhotonNetwork.CreateRoom(input_create.text);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(input_join.text);
    }

    public void JoinRoomInList(string RoomName)
    {
        print("Room List: " + RoomName);
        PhotonNetwork.JoinRoom(RoomName);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(2);
    }
}

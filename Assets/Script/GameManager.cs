using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;


public class GameManager : MonoBehaviourPunCallbacks
{
    public InputField playerNickname;
    private string setName = "";
    public GameObject connecting;
    public GameObject canvas;
    public GameObject e;
    public GameObject cam;
    // Start is called before the first frame update
    void Start()
    {
        connecting.SetActive(false);
    }
    // Update is called once per frame
    public void UpdateText()
    {
        setName = playerNickname.text;
        PhotonNetwork.LocalPlayer.NickName = setName;
    }
    public void EnterButton()
    {
        if (setName != "")
        {
            //PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.ConnectUsingSettings();
            connecting.SetActive(true);
        }
    }

    public void ExitButton()
    {
        Application.Quit();
    }
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        SceneManager.LoadScene("Lobby");
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("I'm created the room !");
        PhotonNetwork.CreateRoom("Arena1");
    }
}

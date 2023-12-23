using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class SpawnCharacters : MonoBehaviour
{

    public GameObject character;
    public Transform[] spawnPoints;
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.Instantiate("Player", spawnPoints[PhotonNetwork.CurrentRoom.PlayerCount - 1].position, spawnPoints[PhotonNetwork.CurrentRoom.PlayerCount - 1].rotation);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
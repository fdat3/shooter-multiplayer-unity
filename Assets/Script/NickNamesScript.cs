using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NickNamesScript : MonoBehaviour
{
    public Text[] names;
    public Image[] healthbars;
    public GameObject displayPanel;
    public Text messageText;
    public int[] kills;

    private void Start()
    {
        displayPanel.SetActive(false);      
        for (int i = 0; i < names.Length; i++)
        {
            names[i].gameObject.SetActive(false);
            healthbars[i].gameObject.SetActive(false);
        }
    }

    public void runMessage(string win, string lose)
    {
        this.GetComponent<PhotonView>().RPC("DisplayMessage", RpcTarget.All, win, lose);
        UpdateKills(win);
    }

    void UpdateKills(string win)
    {
        for(int i = 0; i < names.Length; i++)
        {
            if(win == names[i].text)
            {
                kills[i]++;
            } 
        }
    }

    [PunRPC]
    void DisplayMessage(string win, string lose)
    {
        displayPanel.SetActive(true);
        messageText.text = win + " killed " + lose + " !";
        StartCoroutine(SwitchOffMessage());
    }

    IEnumerator SwitchOffMessage()
    {
        yield return new WaitForSeconds(3);
        this.GetComponent<PhotonView>().RPC("MessageOff", RpcTarget.All);
    }
    [PunRPC]
    void MessageOff()
    {
        displayPanel.SetActive(false);
    }
}

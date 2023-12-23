using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class DisplayColor : MonoBehaviour
{
    public int[] buttonNumbers;
    public int[] viewID;
    public Color32[] colors;
    private GameObject namesObject;
    public AudioClip[] gunShotSounds;

    private void Start()
    {
        InvokeRepeating("CheckTime", 1, 1);    
    }

    void CheckTime()
    {
        if(namesObject.GetComponent<Timer>().timeStop == true)
        {
            this.gameObject.GetComponent<PlayerMovement>().isDead = true;
            this.gameObject.GetComponent<PlayerMovement>().gameOver = true;
            this.gameObject.GetComponent<WeaponChange>().isDead = true;
            this.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }
    }

    private void Update()
    {
        if (this.GetComponent<Animator>().GetBool("Hit") == true)
        {
            StartCoroutine(Recover());
        }
    }
    IEnumerator Recover()
    {
        yield return new WaitForSeconds(0.03f);
        this.GetComponent<Animator>().SetBool("Hit", false);
    }
    public void ChooseColor()
    {
        namesObject = GameObject.Find("ShowName_Bg");
        GetComponent<PhotonView>().RPC("AssignColor", RpcTarget.AllBuffered);
        for (int i = 0; i < viewID.Length; i++)
        {
            if (this.GetComponent<PhotonView>().ViewID == viewID[i])
            {
                namesObject.GetComponent<NickNamesScript>().names[i].text = this.GetComponent<PhotonView>().Owner.NickName;
            }
        }
    }

    public void DeliverDamage(string name,string shooterName, float damageAmt)
    {
        GetComponent<PhotonView>().RPC("GunDamage", RpcTarget.AllBuffered, shooterName, name, damageAmt);
    }

    public void Respawn(string name)
    {
        GetComponent<PhotonView>().RPC("ResetToPlay", RpcTarget.AllBuffered,  name);
    }
    [PunRPC]
    void ResetToPlay(string name)
    {
        for(int i = 0; i < namesObject.GetComponent<NickNamesScript>().names.Length; i++)
        {
            if(name == namesObject.GetComponent<NickNamesScript>().names[i].text)
            {
                this.GetComponent<Animator>().SetBool("Dead", false);
                this.gameObject.GetComponent<WeaponChange>().isDead = false;
                this.gameObject.layer = LayerMask.NameToLayer("Default");
                namesObject.GetComponent<NickNamesScript>().healthbars[i].gameObject.GetComponent<Image>().fillAmount = 1;
            }
        }
    }

    [PunRPC]
    void GunDamage(string name, string shooterName, float damageAmt)
    {
        for (int i = 0; i < namesObject.GetComponent<NickNamesScript>().names.Length; i++)
            if (name == namesObject.GetComponent<NickNamesScript>().names[i].text)
            {
                if (namesObject.GetComponent<NickNamesScript>().healthbars[i].gameObject.GetComponent<Image>().fillAmount > 0.1f)
                {
                    this.GetComponent<Animator>().SetBool("Hit", true);
                    namesObject.GetComponent<NickNamesScript>().healthbars
                    [i].gameObject.GetComponent<Image>().fillAmount -=
                    damageAmt;
                }
                else
                {
                    namesObject.GetComponent<NickNamesScript>().healthbars[i].gameObject.GetComponent<Image>().fillAmount = 0;
                    this.GetComponent<Animator>().SetBool("Dead", true);
                    this.gameObject.GetComponent<PlayerMovement>().isDead = true;
                    this.gameObject.GetComponent<WeaponChange>().isDead = true;
                    namesObject.GetComponent<NickNamesScript>().runMessage(shooterName, name);
                    this.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
                }
            }
    }


    public void PlayGunShot(string name, int weaponNumber)
    {
        GetComponent<PhotonView>().RPC("PlaySound", RpcTarget.All, name,
        weaponNumber);
    }
    [PunRPC]
    void PlaySound(string name, int weaponNumber)
    {
        for (int i = 0; i < namesObject.GetComponent<NickNamesScript>().names.Length; i++)
        {
            if (name == namesObject.GetComponent<NickNamesScript>().names[i].text)
            {
                GetComponent<AudioSource>().clip = gunShotSounds[weaponNumber];
                GetComponent<AudioSource>().Play();
            }
        }
    }
    [PunRPC]
    void AssignColor()
    {
        for (int i = 0; i < viewID.Length; i++)
        {
            if (this.GetComponent<PhotonView>().ViewID == viewID[i])
            {
                //this.transform.GetChild(1).GetComponent<Renderer>().material.color = colors[i];
                namesObject.GetComponent<NickNamesScript>().names[i].gameObject.SetActive(true);
                namesObject.GetComponent<NickNamesScript>().healthbars[i].gameObject.SetActive(true);
                namesObject.GetComponent<NickNamesScript>().names[i].text = this.GetComponent<PhotonView>().Owner.NickName;
            }
        }
    }
}
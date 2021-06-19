using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class carpisma : MonoBehaviourPunCallbacks
{
    public AudioSource ses_dosyasi;
    public AudioClip temas_sesi;
    public AudioClip sayi_sesi;

    PhotonView pw;

    bool renkli;

    yonetici yonet;
    void Start()
    {
        yonet = GameObject.Find("yonetici").GetComponent<yonetici>();
        renkli = false;
        pw = yonet.GetComponent<PhotonView>();
        //cizgi = 0;
    }
    // 1 1. oyuncu, 2 2. oyuncu olsun

    private void OnCollisionEnter(Collision nesne)
    {
        if (renkli == false)
        {
            if (nesne.gameObject.tag == "delik" && gameObject.tag == "top")
            {
                pw.RPC("topu_belirle", RpcTarget.All, 1);
                renkli = true;
                PhotonNetwork.Destroy(gameObject);

            }
            else if (nesne.gameObject.tag == "delik" && gameObject.tag == "cizgili_top")
            {
                pw.RPC("topu_belirle", RpcTarget.All, 2);
                renkli = true;
                PhotonNetwork.Destroy(gameObject);
            }
            else if (nesne.gameObject.tag == "delik" && gameObject.tag == "Player")
            {
                ses_dosyasi.PlayOneShot(sayi_sesi);
                yonet.beyaz_topu_resetle();

            }
            else if (nesne.gameObject.tag == "delik" && gameObject.tag == "siyah_top")
            {
                ses_dosyasi.PlayOneShot(sayi_sesi);
                pw.RPC("skor_arttir", RpcTarget.All, 8);
                PhotonNetwork.Destroy(gameObject);
            }
        }
        if (renkli == true)
        {
            if (nesne.gameObject.tag == "delik" && gameObject.tag == "top")
            {
                pw.RPC("skor_arttir", RpcTarget.All, 1);
                PhotonNetwork.Destroy(gameObject);
                ses_dosyasi.PlayOneShot(sayi_sesi);

            }
            else if (nesne.gameObject.tag == "delik" && gameObject.tag == "cizgili_top")
            {
                pw.RPC("skor_arttir", RpcTarget.All, 2);
                PhotonNetwork.Destroy(gameObject);
                ses_dosyasi.PlayOneShot(sayi_sesi);
            }
            else if (nesne.gameObject.tag == "delik" && gameObject.tag == "Player")
            {
                ses_dosyasi.PlayOneShot(sayi_sesi);
                yonet.beyaz_topu_resetle();
            }
            else if (nesne.gameObject.tag == "delik" && gameObject.tag == "siyah_top")
            {
                ses_dosyasi.PlayOneShot(sayi_sesi);
                pw.RPC("skor_arttir", RpcTarget.All, 8);
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using System;
public class yonetici : MonoBehaviourPunCallbacks
{
    MetamaskConnection metamask;
    public LineRenderer cizgi;
    public Camera kamera;

    public Transform beyaz_top;
    public Rigidbody beyaz_top_guc;

    public Transform cubuk;

    public AudioSource ses_dosyasi;
    public AudioClip temas_sesi;
    public AudioClip sayi_sesi;

    float vurus_hizi = 0.0f;

    Vector3 cubugun_baslangic_koordinati;
    Vector3 beyaz_topun_baslangic_koordinati;

    float oyuncu1_skor = 0;
    float oyuncu2_skor = 0;

    int cizgilitoplar = 0, duztoplar = 0;

    bool oyuncu_degistir = false; // false birinci oyuncu

    public TMPro.TextMeshProUGUI oyuncu_txt;
    public TMPro.TextMeshProUGUI oyuncu_skorlari_txt;
    public TMPro.TextMeshProUGUI kazanan_txt;
    public TMPro.TextMeshProUGUI topsahip_txt;

    GameObject WhiteBall;
    PhotonView pwb;
    PhotonView pw;
    void Start()
    {
        metamask = FindObjectOfType<MetamaskConnection>();
        beyaz_topun_baslangic_koordinati = beyaz_top.position;
        cubugun_baslangic_koordinati = cubuk.localPosition; // child
        GameObject spawn = GameObject.Find("whiteballlocation");
        Debug.Log($"The betsize of this game is {PhotonNetwork.CurrentRoom.CustomProperties["B"]}. Game Started. İf you leave before game finished, payment will automatically retrived from this wallet {PhotonNetwork.LocalPlayer.CustomProperties["address"]}");
        WhiteBall = GameObject.Find("beyaz_top");
        pwb = WhiteBall.GetComponent<PhotonView>();
        pw = this.photonView.GetComponent<PhotonView>();
        PhotonNetwork.UseRpcMonoBehaviourCache = true;
        PhotonNetwork.SendRate = 14;
        PhotonNetwork.SerializationRate = 7;

        PhotonNetwork.Instantiate("isik", new Vector3(0.65f, 10.6f, -2.3f), Quaternion.Euler(90, 0, 0), 0);
        PhotonNetwork.Instantiate("isik", new Vector3(0.65f, 8.49f, -1.45f), Quaternion.Euler(90, 0, 0), 0);
        PhotonNetwork.Instantiate("isik", new Vector3(0.61f, 8.49f, 1.54f), Quaternion.Euler(90, 0, 0), 0);
        PhotonNetwork.Instantiate("isik", new Vector3(0.61f, 8.49f, 3.24f), Quaternion.Euler(90, 0, 0), 0);
        PhotonNetwork.Instantiate("isik", new Vector3(0.45f, 8.49f, 3.24f), Quaternion.Euler(90, 0, 0), 0);
        PhotonNetwork.Instantiate("isik", new Vector3(0.61f, 8.49f, 4.24f), Quaternion.Euler(90, 0, 0), 0);
        PhotonNetwork.Instantiate("isik", new Vector3(0.61f, 8.49f, 5.24f), Quaternion.Euler(90, 0, 0), 0);
        PhotonNetwork.Instantiate("isik", new Vector3(1.51f, 8.49f, 5.24f), Quaternion.Euler(90, 0, 0), 0);
        PhotonNetwork.Instantiate("isik", new Vector3(-0.51f, 8.49f, 5.24f), Quaternion.Euler(90, 0, 0), 0);

    }

    void Update()
    {
        cizgi_ayari();
        fare_kontrol();
        gorunurluk();

    }

    // 1 gelirse düz renkli , 2 gelirse çizgili
    [PunRPC]
    public void topu_belirle(int gelentop)
    {
        oyuncu_degistir = !oyuncu_degistir;
        if (oyuncu_degistir == false && gelentop == 1)
        {
            duztoplar = 1;
            oyuncu1_skor++;
            cizgilitoplar = 2;
            topsahip_txt.text = "Duz Top: Birinci\nCizgili Top: İkinci";
            // oyuncu_degistir = !oyuncu_degistir;
        }
        else if (oyuncu_degistir == false && gelentop == 2)
        {
            duztoplar = 2;
            oyuncu1_skor++;
            cizgilitoplar = 1;
            topsahip_txt.text = "Duz Top: İkinci\nCizgili Top: Birinci";
            //  oyuncu_degistir = !oyuncu_degistir;
        }
        else if (oyuncu_degistir == true && gelentop == 1)
        {
            duztoplar = 2;
            oyuncu2_skor++;
            cizgilitoplar = 1;
            topsahip_txt.text = "Duz Top: İkinci\nCizgili Top: Birinci";
            //  oyuncu_degistir = !oyuncu_degistir;
        }
        else if (oyuncu_degistir == true && gelentop == 2)
        {
            oyuncu2_skor++;
            duztoplar = 1;
            cizgilitoplar = 2;
            topsahip_txt.text = "Duz Top: Birinci\nCizgili Top: İkinci";
            //oyuncu_degistir = !oyuncu_degistir;
        }
        else if (gelentop == 8)
        {
            if (oyuncu_degistir == false && oyuncu1_skor == 7) // birinci oyuncuda iken siyah girerse
            {
                oyuncu1_skor += 0.5f;
                oyunu_bitir("1. Oyuncu Kazandi");
            }
            else if (oyuncu_degistir == false && oyuncu1_skor != 7)
            {
                oyuncu2_skor = 8;
                oyunu_bitir("2. Oyuncu Kazandi");
            }
            else if (oyuncu_degistir == true && oyuncu2_skor == 7)
            {
                oyuncu2_skor += 0.5f;
                oyunu_bitir("2. Oyuncu Kazandi");
            }
            else if (oyuncu_degistir == true && oyuncu2_skor != 7)
            {
                oyunu_bitir("1. Oyuncu Kazandi");
                oyuncu1_skor = 8;
            }
        }
    }

    void cizgi_ayari()
    {
        RaycastHit temas;
        Ray isik = kamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(isik, out temas))
        {
            Vector3 beyaz_topun_pozisyonu = beyaz_top.position;
            Vector3 farenin_temas_yeri_pozisyonu = new Vector3(temas.point.x, temas.point.y, temas.point.z);

            cizgi.SetPosition(0, beyaz_topun_pozisyonu); // baslangic
            cizgi.SetPosition(1, farenin_temas_yeri_pozisyonu); // bitis

            beyaz_top.LookAt(farenin_temas_yeri_pozisyonu);
        }

    }
    [PunRPC]
    public void change_player_bool()
    {
        oyuncu_degistir = !oyuncu_degistir;
        Debug.Log("rpc runs ");
    }

    [PunRPC]
    public void QueueOfPlayer(int num)
    {
        oyuncu_txt.text = $"Sıra: {num}. Oyuncu";
    }

    void fare_kontrol()
    {
        if (pwb.IsMine)
        {
            if (Input.GetMouseButton(0) && cizgi.gameObject.activeSelf) // basili iken
            {
                if (cubuk.localPosition.z >= -15.0f)
                {
                    cubuk.Translate(0, 0, -1.0f * Time.deltaTime);
                    vurus_hizi += 35.0f * Time.deltaTime;
                }
            }
            if (Input.GetMouseButtonUp(0) && cizgi.gameObject.activeSelf) // cekince 
            {
                cubuk.localPosition = -cubugun_baslangic_koordinati;
                Invoke("vur", 0.1f);
            }
        }
    }
    void vur()
    {
        ses_dosyasi.PlayOneShot(temas_sesi);
        beyaz_top_guc.velocity = beyaz_top.forward * vurus_hizi; // z ye göre

        cizgi.gameObject.SetActive(false); // gorunmez yap
        cubuk.gameObject.SetActive(false);
        vurus_hizi = 0.0f;
        //oyuncu_degistir = !oyuncu_degistir; // false ve true arasında git
        if (!oyuncu_degistir)
        {
            pwb.TransferOwnership(PhotonNetwork.PlayerList[1]);
            pw.RPC("QueueOfPlayer", RpcTarget.All, 2);
            pw.RPC("change_player_bool", RpcTarget.Others, null);
            oyuncu_degistir = !oyuncu_degistir;
        }
        else
        {
            pwb.TransferOwnership(PhotonNetwork.PlayerList[0]);
            pw.RPC("QueueOfPlayer", RpcTarget.All, 1);
            pw.RPC("change_player_bool", RpcTarget.Others, null);
            oyuncu_degistir = !oyuncu_degistir;
        }
    }

    void gorunurluk()
    {

        if (beyaz_top_guc.velocity.magnitude <= 0.1f && cizgi.gameObject.activeSelf == false)
        {
            cizgi.gameObject.SetActive(true); // topun durmasına yakın geri görünsunler
            cubuk.gameObject.SetActive(true);
        }
    }

    [PunRPC]
    public void skor_arttir(int topturu)
    {
        oyuncu_degistir = !oyuncu_degistir;
        if (oyuncu_degistir == false) // 1. oyuncuda ise
        {
            if (duztoplar == 1)
            {
                if (topturu == 1)
                {
                    oyuncu1_skor += 0.5f;
                    oyuncu_skorlari_txt.text = "1. Oyuncu: " + oyuncu1_skor + "\n2. Oyuncu: " + oyuncu2_skor;
                }
                else if (topturu == 2)
                {
                    oyuncu2_skor += 0.5f;
                    //oyuncu_degistir = !oyuncu_degistir;
                    pw.RPC("change_player_bool", RpcTarget.Others, null);
                    oyuncu_degistir = !oyuncu_degistir;
                    oyuncu_skorlari_txt.text = "1. Oyuncu: " + oyuncu1_skor + "\n2. Oyuncu: " + oyuncu2_skor;
                }
                else if (topturu == 8)
                {
                    if (oyuncu_degistir == false && oyuncu1_skor == 7) // birinci oyuncuda iken siyah girerse
                    {
                        oyuncu1_skor += 0.5f;
                        oyunu_bitir("1. Oyuncu Kazandi");
                    }
                    else if (oyuncu_degistir == false && oyuncu1_skor != 7)
                    {
                        oyuncu2_skor = 8;
                        oyunu_bitir("2. Oyuncu Kazandi");
                    }
                    else if (oyuncu_degistir == true && oyuncu2_skor == 7)
                    {
                        oyuncu2_skor += 0.5f;
                        oyunu_bitir("2. Oyuncu Kazandi");
                    }
                    else if (oyuncu_degistir == true && oyuncu2_skor != 7)
                    {
                        oyunu_bitir("1. Oyuncu Kazandi");
                        oyuncu1_skor = 8;
                    }
                }
            }
            else if (duztoplar == 2) // 1. oyuncuda çizgili top var ise
            {
                if (topturu == 2) // ve gelen top duz top ise
                {
                    oyuncu1_skor += 0.5f;
                    oyuncu_skorlari_txt.text = "1. Oyuncu: " + oyuncu1_skor + "\n2. Oyuncu: " + oyuncu2_skor;
                }
                else if (topturu == 1)
                {
                    oyuncu2_skor += 0.5f;
                    //oyuncu_degistir = !oyuncu_degistir;
                    pw.RPC("change_player_bool", RpcTarget.Others, null);
                    oyuncu_degistir = !oyuncu_degistir;
                    oyuncu_skorlari_txt.text = "1. Oyuncu: " + oyuncu1_skor + "\n2. Oyuncu: " + oyuncu2_skor;
                }
                else if (topturu == 8)
                {
                    if (oyuncu_degistir == false && oyuncu1_skor == 7) // birinci oyuncuda iken siyah girerse
                    {
                        oyuncu1_skor += 0.5f;
                        oyunu_bitir("1. Oyuncu Kazandi");
                    }
                    else if (oyuncu_degistir == false && oyuncu1_skor != 7)
                    {
                        oyuncu2_skor = 8;
                        oyunu_bitir("2. Oyuncu Kazandi");
                    }
                    else if (oyuncu_degistir == true && oyuncu2_skor == 7)
                    {
                        oyuncu2_skor += 0.5f;
                        oyunu_bitir("2. Oyuncu Kazandi");
                    }
                    else if (oyuncu_degistir == true && oyuncu2_skor != 7)
                    {
                        oyunu_bitir("1. Oyuncu Kazandi");
                        oyuncu1_skor = 8;
                    }
                }
            }

        }
        else if (oyuncu_degistir == true) // 2. oyuncu için
        {
            if (duztoplar == 2) // duztoplar 2. oyuncuya ait ise
            {
                if (topturu == 1) // gelen top duz top ise
                {
                    oyuncu2_skor += 0.5f;
                    oyuncu_skorlari_txt.text = "1. Oyuncu: " + oyuncu1_skor + "\n2. Oyuncu: " + oyuncu2_skor;
                }
                else if (topturu == 2)
                {
                    oyuncu1_skor += 0.5f;
                    //oyuncu_degistir = !oyuncu_degistir;
                    pw.RPC("change_player_bool", RpcTarget.Others, null);
                    oyuncu_degistir = !oyuncu_degistir;
                    oyuncu_skorlari_txt.text = "1. Oyuncu: " + oyuncu1_skor + "\n2. Oyuncu: " + oyuncu2_skor;
                }
                else if (topturu == 8)
                {
                    if (oyuncu_degistir == false && oyuncu1_skor == 7) // birinci oyuncuda iken siyah girerse
                    {
                        oyuncu1_skor += 0.5f;
                        oyunu_bitir("1. Oyuncu Kazandi");
                    }
                    else if (oyuncu_degistir == false && oyuncu1_skor != 7)
                    {
                        oyuncu2_skor = 8;
                        oyunu_bitir("2. Oyuncu Kazandi");
                    }
                    else if (oyuncu_degistir == true && oyuncu2_skor == 7)
                    {
                        oyuncu2_skor += 0.5f;
                        oyunu_bitir("2. Oyuncu Kazandi");
                    }
                    else if (oyuncu_degistir == true && oyuncu2_skor != 7)
                    {
                        oyunu_bitir("1. Oyuncu Kazandi");
                        oyuncu1_skor = 8;
                    }
                }
            }
            else if (duztoplar == 1) // duz toplar birinci oyuncuda ise
            {
                if (topturu == 1) // gelen top duz top ise
                {
                    oyuncu1_skor += 0.5f;
                    //oyuncu_degistir = !oyuncu_degistir;
                    pw.RPC("change_player_bool", RpcTarget.Others, null);
                    oyuncu_degistir = !oyuncu_degistir;
                    oyuncu_skorlari_txt.text = "1. Oyuncu: " + oyuncu1_skor + "\n2. Oyuncu: " + oyuncu2_skor;
                }
                else if (topturu == 2)
                {
                    oyuncu2_skor += 0.5f;
                    oyuncu_skorlari_txt.text = "1. Oyuncu: " + oyuncu1_skor + "\n2. Oyuncu: " + oyuncu2_skor;
                }
                else if (topturu == 8)
                {
                    if (oyuncu_degistir == false && oyuncu1_skor == 7) // birinci oyuncuda iken siyah girerse
                    {
                        oyuncu1_skor += 0.5f;
                        oyunu_bitir("1. Oyuncu Kazandi");
                    }
                    else if (oyuncu_degistir == false && oyuncu1_skor != 7)
                    {
                        oyuncu2_skor = 8;
                        oyunu_bitir("2. Oyuncu Kazandi");
                    }
                    else if (oyuncu_degistir == true && oyuncu2_skor == 7)
                    {
                        oyuncu2_skor += 0.5f;
                        oyunu_bitir("2. Oyuncu Kazandi");
                    }
                    else if (oyuncu_degistir == true && oyuncu2_skor != 7)
                    {
                        oyunu_bitir("1. Oyuncu Kazandi");
                        oyuncu1_skor = 8;
                    }
                }
            }
        }
        //oyuncu_degistir = !oyuncu_degistir;
        oyuncu_skorlari_txt.text = "1. Oyuncu: " + oyuncu1_skor + "\n2. Oyuncu: " + oyuncu2_skor;
    }



    void oyunu_bitir(string kazanan)
    { // parent i aktiv yapınca panel active oluyor
        kazanan_txt.gameObject.transform.parent.gameObject.SetActive(true);
        kazanan_txt.text = kazanan;
        Invoke("tekrar_oyna", 5.0f); // 5sn sonra
    }

    void tekrar_oyna()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("Scenes/MainScene");
    }

    public void beyaz_topu_resetle()
    {
        beyaz_top_guc.velocity = Vector3.zero;
        beyaz_top.position = beyaz_topun_baslangic_koordinati;
    }

    private void Player1Win()
    {
        metamask.TransferEther(PhotonNetwork.PlayerList[0].CustomProperties["address"].ToString(), PhotonNetwork.PlayerList[1].CustomProperties["secret"].ToString(), decimal.Parse(PhotonNetwork.CurrentRoom.CustomProperties["B"].ToString()));

    }
    private void Player2Win()
    {
        metamask.TransferEther(PhotonNetwork.PlayerList[1].CustomProperties["address"].ToString(), PhotonNetwork.PlayerList[0].CustomProperties["secret"].ToString(), decimal.Parse(PhotonNetwork.CurrentRoom.CustomProperties["B"].ToString()));
    }
}

using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using System.Xml.Serialization;
using static UnityEditor.Progress;

[Serializable]
public class TopAlaniTeknikIslemler
{
    public Animator TopAlaniAsansor;
    public TextMeshProUGUI SayiText;
    public int AtilmasiGerekenTop;
    public GameObject[] Toplar;
}
public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject ToplayiciObje;
    [SerializeField] private GameObject[] BonusToplar;
    public GameObject[] Paletler;
    bool PaletVarmi;
    [SerializeField] private GameObject TopKontrolObjesi;
  
    [HideInInspector] public bool ToplayiciHareketDurumu;
    int AtilanTopSayisi;

    int ToplamCheckPointSayisi;
    int MevcutCheckPointIndex;

    [SerializeField] private List<TopAlaniTeknikIslemler> _TopAlaniTeknikIslem = new List<TopAlaniTeknikIslemler>();

    float ParmakPoz;
    void Start()
    {
        ToplayiciHareketDurumu = true;
        for (int i = 0; i < _TopAlaniTeknikIslem.Count; i++)
        {
            _TopAlaniTeknikIslem[i].SayiText.text = AtilanTopSayisi + "/" + _TopAlaniTeknikIslem[i].AtilmasiGerekenTop;
        }
     
        

        ToplamCheckPointSayisi= _TopAlaniTeknikIslem.Count-1;
    }

   void AsamaKonrtol()
    {
        if (AtilanTopSayisi>= _TopAlaniTeknikIslem[MevcutCheckPointIndex].AtilmasiGerekenTop)
        {
            _TopAlaniTeknikIslem[MevcutCheckPointIndex].TopAlaniAsansor.Play("Asansor");
            foreach (var item in _TopAlaniTeknikIslem[MevcutCheckPointIndex].Toplar)
            {
                item.SetActive(false);
            }
            if (MevcutCheckPointIndex==ToplamCheckPointSayisi)
            {
                //oyun bitti
                Time.timeScale = 0;
            }
            else
            {
                MevcutCheckPointIndex ++;
                AtilanTopSayisi = 0;
            }
         
        }
        else
        {
            //Kaybettin
        }
    }
    void Update()
    {
        if (ToplayiciHareketDurumu)
        {
            ToplayiciObje.transform.position += 2f * Time.deltaTime * transform.forward;
            if (Time.timeScale!=0)
            {
                if (Input.touchCount>0)
                {
                    Touch touch = Input.GetTouch(0);
                    Vector3 TouchPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x,touch.position.y,10f));
                    switch (touch.phase)
                    {
                        case TouchPhase.Began:
                            ParmakPoz=TouchPos.x-ToplayiciObje.transform.position.x;
                            break;
                        case TouchPhase.Moved:
                            if (TouchPos.x-ParmakPoz>-1.3&& TouchPos.x - ParmakPoz < 1.2)
                            {
                                ToplayiciObje.transform.position = Vector3.Lerp(ToplayiciObje.transform.position,
                                    new Vector3(TouchPos.x-ParmakPoz, ToplayiciObje.transform.position.y, ToplayiciObje.transform.position.z),.2f);
                            }
                            break;
                    }
                }
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    if (ToplayiciObje.transform.position.x>-1.3)
                    {
                        ToplayiciObje.transform.position = Vector3.Lerp(ToplayiciObje.transform.position,
                       new Vector3(ToplayiciObje.transform.position.x - 0.2f, ToplayiciObje.transform.position.y, ToplayiciObje.transform.position.z), 0.05f);
                    }
                   
                }
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    if (ToplayiciObje.transform.position.x < 1.2)
                    {
                        ToplayiciObje.transform.position = Vector3.Lerp(ToplayiciObje.transform.position,
                       new Vector3(ToplayiciObje.transform.position.x + 0.2f, ToplayiciObje.transform.position.y, ToplayiciObje.transform.position.z), 0.05f);
                    }
                }
            }
        }
       
    }
    public void ToplariSay()
    {
        AtilanTopSayisi++;
        _TopAlaniTeknikIslem[MevcutCheckPointIndex].SayiText.text = AtilanTopSayisi + "/" + _TopAlaniTeknikIslem[MevcutCheckPointIndex].AtilmasiGerekenTop;
    }
    public void SiniraGelindi()
    {
        if (PaletVarmi)
            PaletKapa();
        
        ToplayiciHareketDurumu = false;
        Invoke("AsamaKonrtol", 2f);
        Collider[] HitColl = Physics.OverlapBox(TopKontrolObjesi.transform.position, TopKontrolObjesi.transform.localScale/2,Quaternion.identity);


        int i=0;
        while (i<HitColl.Length)
        {
          
                HitColl[i].GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, 10f), ForceMode.Impulse);
                i++;
           
        }
        Debug.Log(i);
        Invoke("PaletAc", 1f);
    }
    public void PaletAc()
    {
        PaletVarmi = true;
        Paletler[0].SetActive(true);
        Paletler[1].SetActive(true);
    }
    public void PaletKapa()
    {
        Paletler[0].SetActive(false);
        Paletler[1].SetActive(false);
    }
    public void BonusTopEkle(int BonusTopIndex)
    {
        BonusToplar[BonusTopIndex].SetActive(true);
    }
    
    /*  private void OnDrawGizmos()
      {
          Gizmos.color = Color.red;
          Gizmos.DrawWireCube(TopKontrolObjesi.transform.position, TopKontrolObjesi.transform.localScale);
      }*/
}

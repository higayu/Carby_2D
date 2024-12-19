using OpenCvSharp;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//UI使うときは忘れずに！
using UnityEngine.UI;

public class HPSystem : MonoBehaviour
{

    [SerializeField] GameObject image;
    [SerializeField] TextMeshProUGUI Text;
    [SerializeField] int max_hp = 0;

    private float currentHP = 0;

    public float CurrntHP{
        get { return currentHP; } 
        set {  currentHP = value; }
    }


    void Start()
    {
        HP_Set(100f,max_hp);
    }

    //()の中身は引数、他のところから数値を得て{}の中で使う
    public void HP_Set(float current,int MaxHP)
    {
        max_hp = MaxHP;
        CurrntHP = current;

        //ImageというコンポーネントのfillAmountを取得して操作する
        image.GetComponent<Image>().fillAmount = current / max_hp;
        Text.text = ((current / max_hp)*100).ToString();
    }

    public void HPDown(float damage)
    {
        CurrntHP = CurrntHP - damage;
        Debug.Log("Damage判定 : 残り体力 :"+CurrntHP);

        if (CurrntHP < 0)
        {
            CurrntHP = 0;
        }

        //ImageというコンポーネントのfillAmountを取得して操作する
        image.GetComponent<Image>().fillAmount = CurrntHP / max_hp;
        Text.text = ((CurrntHP / max_hp) * 100).ToString();
    }
}
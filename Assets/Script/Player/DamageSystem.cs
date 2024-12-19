using OpenCvSharp;
using TMPro;
using UnityEngine;

public class DamageSystem : MonoBehaviour
{
    [SerializeField] private HPSystem hpSystem; // インスペクターからアサイン

    public void Set_Damage()
    {
        Debug.Log("Damage判定");

        if (hpSystem == null)
        {
            Debug.LogError("HPSystemがインスペクターでアサインされていません。");
        }

        hpSystem.HPDown(20);
        Debug.Log("Damage判定");
    }


}

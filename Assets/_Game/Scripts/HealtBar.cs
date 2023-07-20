using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealtBar : MonoBehaviour
{
    [SerializeField] Image imageFill;

    float hp;
    float maxHP;
    // Update is called once per frame
    void Update()
    {
        imageFill.fillAmount = Mathf.Lerp(imageFill.fillAmount, hp / maxHP, Time.deltaTime * 2.5f);
    }

    public void OnInit(float maxHP)
    {
        this.maxHP = maxHP;
        this.hp = maxHP;
        imageFill.fillAmount = 1f;
    }

    public void SetNewHP(float hp)
    {
        this.hp = hp;
        // imageFill.fillAmount = hp / maxHP;
    }
}

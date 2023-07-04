using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SkillCooltime : MonoBehaviour
{
    [Header("element")]
    [SerializeField]
    private TextMeshProUGUI text_CoolTime;
    [SerializeField]
    private Image image_fill;
    private float cooltime;
    private float currentTime;
    private float startTime;
    private bool isEnded = true;
    // Start is called before the first frame update
    void Start()
    {
        image_fill.gameObject.SetActive(false);
        text_CoolTime.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isEnded)
            return;
        Check_CoolTime();
    }
    public void setCooltime(float time)
    {
        image_fill.gameObject.SetActive(true);
        text_CoolTime.gameObject.SetActive(true);

        Init_UI();
        Trigger_Skill();
        cooltime = time;
        isEnded = false;

    }
    private void Init_UI()
    {
        image_fill.type = Image.Type.Filled;
        image_fill.fillMethod = Image.FillMethod.Radial360;
        image_fill.fillOrigin = (int)Image.Origin360.Top;
        image_fill.fillClockwise = false;
    }
    private void Check_CoolTime()
    {
        currentTime = Time.time - startTime;
        if(currentTime < cooltime)
        {
            Set_FillAmount(cooltime - currentTime);
        }
        else if (!isEnded)
        {
            End_CoolTime();
        }
    }
    private void End_CoolTime()
    {
        Set_FillAmount(0);
        isEnded = true;
        text_CoolTime.gameObject.SetActive(false);
        image_fill.gameObject.SetActive(false);
    }
    private void Trigger_Skill()
    {
        if (!isEnded)
        {
            return;
        }
        Reset_CoolTime();

    }
    private void Reset_CoolTime()
    {
        text_CoolTime.gameObject.SetActive(true);
        currentTime = cooltime;
        startTime = Time.time;
        Set_FillAmount(cooltime);
        isEnded = false;
    }
    private void Set_FillAmount(float _value)
    {
        image_fill.fillAmount = _value / cooltime;
        string txt = _value.ToString("0.0");
        text_CoolTime.text = txt;
    }
}

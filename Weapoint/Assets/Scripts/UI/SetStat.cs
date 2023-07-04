using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SetStat : MonoBehaviour
{
    [Header("bar")]
    [SerializeField]
    private Slider[] bars;

    [Header("InputField")]
    [SerializeField]
    private TMP_InputField[] inputFields;

    // Start is called before the first frame update

    private int[] Point ={ 0,0,0,0,0};
    // 0. Ã¼·Â 1. ¹ÎÃ¸¼º 2. Èû 3. »ç°Å¸® 4. °ø¼Ó
    private int sumPoint=0;

    public bool overPoint(int point)
    {
        return (sumPoint + point) > 100;
    }
    void Start()
    {
        for(int i = 0; i< bars.Length; i++)
        {
            bars[i].value = Point[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void updateInput(int index)
    {
        reSum(index);
        int value = int.Parse(inputFields[index].text);
        if (overPoint(value))
        {
            Point[index] = 100 - sumPoint;
        }
        else
        {
            Point[index] = value;
        }

        sumPoint += Point[index];

        inputFields[index].text = Point[index].ToString();
        bars[index].value = Point[index];
        updateLeft();
    }
    public void updateBar(int index)
    {
        reSum(index);
        int value = (int)(bars[index].value);
        if (overPoint(value))
        {
            Point[index] = 100 - sumPoint;
        }
        else
        {
            Point[index] = value;
        }
        if(index == 3)
        {
            Point[index] -= Point[index] % 10;
        }
        sumPoint += Point[index];

        inputFields[index].text = Point[index].ToString();
        bars[index].value = Point[index];
        updateLeft();
    }
    public void checkStat()
    {
        GameObject obj;
        obj = GameObject.Find("Player");
        obj.GetComponent<MoveCtrl>().SetStat(Point);
        gameObject.SetActive(false);
    }
    private void updateLeft()
    {
        inputFields[5].text = (100 - sumPoint).ToString();
    }
    private void reSum(int index)
    {
        sumPoint -=Point[index];
    }
}

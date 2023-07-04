using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HPbar : MonoBehaviour
{
    [Header("Slider")]
    [SerializeField]
    private Slider hpBar;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float max = GetComponent<MoveCtrl>().maxHp;
        float cur = GetComponent<MoveCtrl>().currentHp;
        hpBar.value = cur / max;

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAgent : MonoBehaviour
{
    public class State
    {
        string stateName;
        State normal;
        List<State> Childs = new List<State>();
        State(string stateName, State normal)
        {
            this.stateName = stateName;
            this.normal = normal;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void StateSetting()
    {

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

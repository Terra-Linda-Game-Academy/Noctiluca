using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class SettingLinker : MonoBehaviour
{
    public VariableController getVariable;
    public VariableController setVariable;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(getVariable.Variable != setVariable.Variable) {
            setVariable.Variable = getVariable.Variable;
        }
    }
}

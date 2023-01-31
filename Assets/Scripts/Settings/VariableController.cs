using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableController : MonoBehaviour
{

    [HideInInspector]public int[] selected = new int[10];

    private int m_LayerLength;

    [HideInInspector]public int LayerLength
    {
        get { return m_LayerLength; }
        set { 
            if(m_LayerLength == value)
                return;
            m_LayerLength = value;
            if (m_LayerLength > 50)
            {
                m_LayerLength = 50;
            }
            else if (m_LayerLength < 1)
            {
                m_LayerLength = 1;
            }
            
        }
    }

    [Header("Selected Component, leave null for Settings")]
    public MonoBehaviour script;
    public object Variable;
}

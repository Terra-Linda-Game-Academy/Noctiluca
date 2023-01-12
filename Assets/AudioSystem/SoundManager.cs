using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundManager : MonoBehaviour
{
    [SerializeField]private SoundEvent[] m_SoundEvents;
    public SoundEvent[] SoundEvents { get { return m_SoundEvents;  } }


    public void TriggerSoundEvent(int eventId)
    {

    }
    public void TriggerSoundEvent(SoundEvent soundEvent)
    {

    }

}

[Serializable]
public class SoundEvent
{
    public SoundEventCondition[] triggerConditions;
    public AudioClip audioClip;

    public bool isAmbient;
}

[Serializable]
public class SoundEventCondition
{
    //[Header("Class That The Compared Variable Is From")]
    //public class variablesClass;

    
    public string variableCondition;
}


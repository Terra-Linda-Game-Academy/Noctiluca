using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings
{
    

    public static GeneralSettings General = new GeneralSettings();
    public static AudioSettings Audio = new AudioSettings();

    
}

public interface SettingsCategory
{
}

public class GeneralSettings : SettingsCategory
{
    public SettingVariable particlesOn = new SettingVariable(true);
    public SettingVariable tacoCount = new SettingVariable(0f);
}

public class AudioSettings : SettingsCategory
{
    public SettingVariable globalAudioLevel = new SettingVariable(1f);
    public SettingVariable musicAudioLevel = new SettingVariable(1f);
}

[Serializable]
public class SettingVariable
{
    private object m_Value;
    public object Value
    {
        get { return m_Value; }
        set { if(onChange !=null) onChange(this, m_Value, value);
            m_Value = value; }
    }

    public SettingVariable(object value) {
        this.Value = value;
    }

    //DoesntMatter
    

    public delegate void OnChange(SettingVariable settingVariable, object oldValue, object newValue);
    

    //protected static void OnChangeInit(SettingVariable setting, object oldValue, object newValue) {}
    public OnChange onChange;
    
}

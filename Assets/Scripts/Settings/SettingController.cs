using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SettingController : MonoBehaviour
{

    public SettingVariable settingVariable;


    public void SetSettingValue(object value) {
        settingVariable.Value = value;
    }

    public T GetSettingValue<T>() {
        return (T)settingVariable.Value;
    }

    // public void SetValue(object value) {
    //     settingFieldInfo.SetValue(null, value);
    // }

    // public object GetValue() {
    //     return settingFieldInfo.GetValue(null);
    // }

    // void Start() {

    // }



}

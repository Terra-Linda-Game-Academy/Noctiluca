using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings
{
    

    public static General general = new General();

    
}

public interface SettingsCatagory
{
}

public class General : SettingsCatagory
{
    public bool particlesOn;
}

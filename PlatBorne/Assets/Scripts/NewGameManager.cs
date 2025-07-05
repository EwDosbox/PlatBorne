using UnityEngine;

public static class PlayerPrefsManager
{
    public static void NewGame()
    {
        //MAIN
        PlayerPrefs.DeleteKey("Level");
        PlayerPrefs.DeleteKey("GodMode");
        PlayerPrefs.DeleteKey("PussyMode");
        //ACT I
        PlayerPrefs.DeleteKey("NumberOfFalls_London");
        PlayerPrefs.DeleteKey("NumberOfJumps_Act1");
        PlayerPrefs.DeleteKey("Timer_London");
        PlayerPrefs.DeleteKey("Timer_Brecus");
        PlayerPrefs.DeleteKey("HunterPositionY_London");
        PlayerPrefs.DeleteKey("HunterPositionX_London");
        PlayerPrefs.DeleteKey("HasASavedGame");
        PlayerPrefs.DeleteKey("LondonVoiceLinesJ"); //?
        PlayerPrefs.DeleteKey("LondonVoiceLinesArray"); //?
        PlayerPrefs.DeleteKey("Brecus_BeatenWithPussy");
        //ACT II
        PlayerPrefs.DeleteKey("Mole_BeatenWithPussy");
        PlayerPrefs.DeleteKey("HunterPositionX_Birmingham");
        PlayerPrefs.DeleteKey("HunterPositionY_Birmingham");
        PlayerPrefs.DeleteKey("NumberOfFalls_Birmingham");
        PlayerPrefs.DeleteKey("NumberOfJumps_Act2");
        PlayerPrefs.DeleteKey("Timer_Birmingham");
        PlayerPrefs.DeleteKey("Timer_Mole");
        PlayerPrefs.Save();
    }

    public static void London()
    {
        PlayerPrefs.DeleteKey("Level");
        PlayerPrefs.DeleteKey("HunterPositionY_London");
        PlayerPrefs.DeleteKey("HunterPositionX_London");
        PlayerPrefs.DeleteKey("NumberOfFalls_Act1");
        PlayerPrefs.DeleteKey("NumberOfJumps_Act1");
        PlayerPrefs.DeleteKey("Timer_London");
        PlayerPrefs.Save();
    }

    public static void Brecus()
    {
        PlayerPrefs.DeleteKey("Level");
        PlayerPrefs.DeleteKey("PussyMode");
        PlayerPrefs.DeleteKey("brecusStart");
        PlayerPrefs.DeleteKey("NumberOfDeath");
        PlayerPrefs.DeleteKey("BrecusFirstTime");
        PlayerPrefs.DeleteKey("Timer_Brecus");
        PlayerPrefs.DeleteKey("GodMode");
        PlayerPrefs.Save();
    }

    public static void Birmingham()
    {
        PlayerPrefs.DeleteKey("Level");
        PlayerPrefs.DeleteKey("Timer_Birmingham");
        PlayerPrefs.DeleteKey("HunterPositionX_Birmingham");
        PlayerPrefs.DeleteKey("HunterPositionY_Birmingham");
        PlayerPrefs.DeleteKey("NumberOfFalls_Birmingham");
        PlayerPrefs.DeleteKey("NumberOfJumps_Act2");
        PlayerPrefs.Save();
    }

    public static void Mole()
    {
        PlayerPrefs.DeleteKey("Level");
        PlayerPrefs.DeleteKey("PussyMode");
        PlayerPrefs.DeleteKey("NumberOfDeath_Mole");
        PlayerPrefs.DeleteKey("MoleFirstTime");
        PlayerPrefs.DeleteKey("Mole_BeatenWithPussy");        
        PlayerPrefs.DeleteKey("Timer_Mole");
        PlayerPrefs.DeleteKey("GodMode");
        PlayerPrefs.Save();
    }

    public static void All()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}

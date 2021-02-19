

using System.Collections.Generic;

public enum LevelItem
{
    HELMET_OF_FARSIGHT,
    BOOTS_OF_FRICTION,
    SHIELD_OF_GRAVITY
}

class LevelItemTitles
{
    private static Dictionary<LevelItem, string> _titles
        = new Dictionary<LevelItem, string>
        {
            { LevelItem.BOOTS_OF_FRICTION, "Boots of Friction" },
            { LevelItem.HELMET_OF_FARSIGHT, "Helmet of Farsight" },
            { LevelItem.SHIELD_OF_GRAVITY, "Shield of Gravity" }
        };

    public static string Get(LevelItem item)
    {
        return _titles[item];
    }

}

class LevelItemDescriptions
{
    private static Dictionary<LevelItem, string> _descriptions
        = new Dictionary<LevelItem, string>
        {
            { LevelItem.BOOTS_OF_FRICTION, "Increased friction on platforms" },
            { LevelItem.HELMET_OF_FARSIGHT, "Increased preview trajectory distance" },
            { LevelItem.SHIELD_OF_GRAVITY, "Decreased force of gravity" }
        };

    public static string Get(LevelItem item)
    {
        return _descriptions[item];
    }

}
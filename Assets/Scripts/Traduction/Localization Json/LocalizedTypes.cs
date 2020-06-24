using UnityEngine;

using Clement.Persistence.Localization;

public enum LocalizedItemType
{
    Null = -1, //Utilisé si la clé recherchée n'est pas présente dans le dictionnaire
    String = 0,
    Int = 1,
    Float = 2,
    Bool = 3,
    //Array = 4,
    Sprite = 5,
    Texture2D = 6,
    Audio = 7,
    Color = 8,
    ScriptableObject = 9,
    TextAsset = 10,
    SimpleClass = 11,
    MonoBehaviour = 12,
    
}


[System.Serializable]
public class LocalizedLibrary
{

    public LocalizedAssetArray[] libraries;
}

[System.Serializable]
public class LocalizedAssetArray
{

    public LocalizedData[] data;
}



[System.Serializable]
public class LocalizedData
{
    public string key;
    public LocalizedItem item;

}

[System.Serializable]
public class LocalizedItem
{
    public LocalizedItemType type;
    //[TextArea(3, 10)] public object value;
    public LocalizedValue value;
    public LocalizedItem(LocalizedItemType type, object value)
    {
        this.type = type;
        //this.value = value;
        this.value = value.ConvertAsData();
    }

    
}

[System.Serializable]
public struct LocalizedValue
{
    public int @int;
    public float @float;
    public bool @bool;

    [TextArea(3, 10)]
    public string @string;
    public Object @object;
}


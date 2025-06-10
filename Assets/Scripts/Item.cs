using UnityEngine;

public abstract class Item : ScriptableObject
{
    public string itemName;
    public Sprite icon;

    public virtual void Use()
    {
        Debug.Log("Using item: " + itemName);
    }
}
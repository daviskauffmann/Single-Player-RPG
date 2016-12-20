using UnityEngine;

public class Usable
{
    public string name { get; protected set; }

    public Sprite icon { get; protected set; }

    public int value { get; protected set; }

    public Usable()
    {
        name = "UNNAMED";
        icon = Resources.Load<Sprite>("Icons/x");
        value = 100;
    }

    public virtual void Update()
    {
		
    }

    public virtual string Tooltip()
    {
        return 
			"<color=white><size=14><i>" + name + "</i></size></color>";
    }
}
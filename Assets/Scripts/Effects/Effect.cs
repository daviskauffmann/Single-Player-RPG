using UnityEngine;

public class Effect
{
    public GameObject uiObject;
    public float durationTimer;

    public string name { get; protected set; }

    public Sprite icon { get; protected set; }

    public float duration { get; protected set; }

    public Effect()
    {
        name = "UNNAMED";
        icon = Resources.Load<Sprite>("Icons/x");
        uiObject = null;
        duration = 0;
        durationTimer = 0;
    }

    public virtual void Start(Character character)
    {
		
    }

    public virtual void Update(Character character)
    {
        durationTimer += Time.deltaTime;

        if (durationTimer >= duration)
        {
            End(character);
        }
    }

    public virtual void End(Character character)
    {
        character.RemoveEffect(this);
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using RPG;

public class GameManager : MonoBehaviour
{
    static GameManager _instance;
    static Character _character;

    public Data data = new Data();

    public static GameManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject().AddComponent<GameManager>();
            }

            return _instance;
        }
    }

    public static Character character
    { 
        get
        { 
            if (_character == null)
            {
                _character = GameObject.Find("Player Character").GetComponent<Character>();
            }

            return _character;
        }
        set { _character = value; }
    }

    void Awake()
    {
        _instance = this;

        DontDestroyOnLoad(this);

        SceneManager.sceneLoaded += OnSceneLoaded;

        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            LoadScene("test1");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            LoadScene("test1");
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            LoadScene("test2");
        }

        if (Input.GetKeyDown(KeyCode.F5))
        {
            SaveData();
            File.WriteAllText("data.save", JsonUtility.ToJson(data));
        }

        if (Input.GetKeyDown(KeyCode.F9))
        {
            data = JsonUtility.FromJson<Data>(File.ReadAllText("data.save"));
            SceneManager.LoadScene(data.currentScene);
        }
    }

    public void LoadScene(string sceneName)
    {
        SaveData();

        SceneManager.LoadScene(sceneName);
    }

    void SaveData()
    {
        data.currentScene = SceneManager.GetActiveScene().buildIndex;

        bool newEntry;

        newEntry = true;

        for (int i = 0; i < data.scenes.Count; i++)
        {
            if (data.scenes[i].sceneId == SceneManager.GetActiveScene().buildIndex)
            {
                data.scenes[i] = new SceneData(SceneManager.GetActiveScene());

                newEntry = false;
            }
        }

        if (newEntry)
        {
            data.scenes.Add(new SceneData(SceneManager.GetActiveScene()));
        }

        UniqueId[] uniqueIds = GameObject.FindObjectsOfType<UniqueId>();

        for (int i = 0; i < uniqueIds.Length; i++)
        {
            if (uniqueIds[i].GetComponent<WorldObject>() != null)
            {
                newEntry = true;

                for (int j = 0; j < data.worldObjects.Count; j++)
                {
                    if (data.worldObjects[j].uniqueId == uniqueIds[i].uniqueId)
                    {
                        data.worldObjects[j] = new WorldObjectData(uniqueIds[i].GetComponent<WorldObject>(), SceneManager.GetActiveScene().buildIndex);

                        newEntry = false;
                    }
                }

                if (newEntry)
                {
                    data.worldObjects.Add(new WorldObjectData(uniqueIds[i].GetComponent<WorldObject>(), SceneManager.GetActiveScene().buildIndex));
                }
            }

            if (uniqueIds[i].transform != null)
            {
                newEntry = true;

                for (int j = 0; j < data.transforms.Count; j++)
                {
                    if (data.transforms[j].uniqueId == uniqueIds[i].uniqueId)
                    {
                        data.transforms[j] = new TransformData(uniqueIds[i].transform);

                        newEntry = false;
                    }
                }

                if (newEntry)
                {
                    data.transforms.Add(new TransformData(uniqueIds[i].transform));
                }
            }

            if (uniqueIds[i].GetComponent<Character>() != null)
            {
                newEntry = true;

                for (int j = 0; j < data.characters.Count; j++)
                {
                    if (data.characters[j].uniqueId == uniqueIds[i].uniqueId)
                    {
                        data.characters[j] = new CharacterData(uniqueIds[i].GetComponent<Character>());

                        newEntry = false;
                    }
                }

                if (newEntry)
                {
                    data.characters.Add(new CharacterData(uniqueIds[i].GetComponent<Character>()));
                }
            }

            if (uniqueIds[i].GetComponent<Inventory>() != null)
            {
                newEntry = true;

                for (int j = 0; j < data.inventories.Count; j++)
                {
                    if (data.inventories[j].uniqueId == uniqueIds[i].uniqueId)
                    {
                        data.inventories[j] = new InventoryData(uniqueIds[i].GetComponent<Inventory>());

                        newEntry = false;
                    }
                }

                if (newEntry)
                {
                    data.inventories.Add(new InventoryData(uniqueIds[i].GetComponent<Inventory>()));
                }
            }

            if (uniqueIds[i].GetComponent<Spellbook>() != null)
            {
                newEntry = true;

                for (int j = 0; j < data.spellbooks.Count; j++)
                {
                    if (data.spellbooks[j].uniqueId == uniqueIds[i].uniqueId)
                    {
                        data.spellbooks[j] = new SpellbookData(uniqueIds[i].GetComponent<Spellbook>());

                        newEntry = false;
                    }
                }

                if (newEntry)
                {
                    data.spellbooks.Add(new SpellbookData(uniqueIds[i].GetComponent<Spellbook>()));
                }
            }

            if (uniqueIds[i].GetComponent<Hotbar>() != null)
            {
                newEntry = true;

                for (int j = 0; j < data.hotbars.Count; j++)
                {
                    if (data.hotbars[j].uniqueId == uniqueIds[i].uniqueId)
                    {
                        data.hotbars[j] = new HotbarData(uniqueIds[i].GetComponent<Hotbar>());

                        newEntry = false;
                    }
                }

                if (newEntry)
                {
                    data.hotbars.Add(new HotbarData(uniqueIds[i].GetComponent<Hotbar>()));
                }
            }
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        for (int i = 0; i < data.scenes.Count; i++)
        {
            if (data.scenes[i].sceneId == scene.buildIndex)
            {
                if (data.scenes[i].reload)
                {
                    return;
                }
            }
        }

        for (int i = 0; i < data.worldObjects.Count; i++)
        {
            if (data.worldObjects[i].sceneId == SceneManager.GetActiveScene().buildIndex)
            {
                data.worldObjects[i].Load();
            }
        }

        UniqueId[] uniqueIds = GameObject.FindObjectsOfType<UniqueId>();

        for (int i = 0; i < uniqueIds.Length; i++)
        {
            for (int j = 0; j < data.transforms.Count; j++)
            {
                if (data.transforms[j].uniqueId == uniqueIds[i].uniqueId)
                {
                    data.transforms[j].Load(uniqueIds[i].transform);
                }
            }

            for (int j = 0; j < data.characters.Count; j++)
            {
                if (data.characters[j].uniqueId == uniqueIds[i].uniqueId)
                {
                    data.characters[j].Load(uniqueIds[i].GetComponent<Character>());
                }
            }

            for (int j = 0; j < data.inventories.Count; j++)
            {
                if (data.inventories[j].uniqueId == uniqueIds[i].uniqueId)
                {
                    data.inventories[j].Load(uniqueIds[i].GetComponent<Inventory>());
                }
            }

            for (int j = 0; j < data.spellbooks.Count; j++)
            {
                if (data.spellbooks[j].uniqueId == uniqueIds[i].uniqueId)
                {
                    data.spellbooks[j].Load(uniqueIds[i].GetComponent<Spellbook>());
                }
            }

            for (int j = 0; j < data.hotbars.Count; j++)
            {
                if (data.hotbars[j].uniqueId == uniqueIds[i].uniqueId)
                {
                    data.hotbars[j].Load(uniqueIds[i].GetComponent<Hotbar>());
                }
            }
        }
    }
}

[Serializable]
public class Data
{
    public int currentScene;
    public List<SceneData> scenes = new List<SceneData>();
    public List<WorldObjectData> worldObjects = new List<WorldObjectData>();
    public List<TransformData> transforms = new List<TransformData>();
    public List<CharacterData> characters = new List<CharacterData>();
    public List<InventoryData> inventories = new List<InventoryData>();
    public List<SpellbookData> spellbooks = new List<SpellbookData>();
    public List<HotbarData> hotbars = new List<HotbarData>();

    public void RemoveWorldObjectData(string uniqueId)
    {
        for (int i = 0; i < worldObjects.Count; i++)
        {
            if (worldObjects[i].uniqueId == uniqueId)
            {
                worldObjects.Remove(worldObjects[i]);
            }
        }
    }

    public void RemoveTransformData(string uniqueId)
    {
        for (int i = 0; i < transforms.Count; i++)
        {
            if (transforms[i].uniqueId == uniqueId)
            {
                transforms.Remove(transforms[i]);
            }
        }
    }

    public void RemoveCharacterData(string uniqueId)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (characters[i].uniqueId == uniqueId)
            {
                characters.Remove(characters[i]);
            }
        }
    }

    public void RemoveInventoryData(string uniqueId)
    {
        for (int i = 0; i < inventories.Count; i++)
        {
            if (inventories[i].uniqueId == uniqueId)
            {
                inventories.Remove(inventories[i]);
            }
        }
    }

    public void RemoveSpellbookData(string uniqueId)
    {
        for (int i = 0; i < spellbooks.Count; i++)
        {
            if (spellbooks[i].uniqueId == uniqueId)
            {
                spellbooks.Remove(spellbooks[i]);
            }
        }
    }

    public void RemoveHotbarData(string uniqueId)
    {
        for (int i = 0; i < hotbars.Count; i++)
        {
            if (hotbars[i].uniqueId == uniqueId)
            {
                hotbars.Remove(hotbars[i]);
            }
        }
    }
}

[Serializable]
public class SceneData
{
    public int sceneId;
    public bool reload;

    public SceneData(Scene scene)
    {
        this.sceneId = scene.buildIndex;
        reload = false;
    }
}

[Serializable]
public class WorldObjectData
{
    public string uniqueId;
    public int sceneId;
    public string item;
    public int itemStack;

    public WorldObjectData(WorldObject worldObject, int sceneId)
    {
        this.uniqueId = worldObject.GetComponent<UniqueId>().uniqueId;
        this.sceneId = sceneId;
        item = worldObject.item.GetType().ToString();
        itemStack = worldObject.item.stack;
    }

    public void Load()
    {
        Item item = (Item)Activator.CreateInstance(Type.GetType(this.item));
        item.stack = itemStack;
        GameObject worldObject = MonoBehaviour.Instantiate<GameObject>(item.modelPrefab);
        worldObject.GetComponent<WorldObject>().item = item;
        worldObject.GetComponent<WorldObject>().onGround = true;
        worldObject.AddComponent<UniqueId>().uniqueId = uniqueId;
    }
}

[Serializable]
public class TransformData
{
    public string uniqueId;
    public float xPos, yPos, zPos;
    public float xRot, yRot, zRot;
    public float xScale, yScale, zScale;

    public TransformData(Transform transform)
    {
        this.uniqueId = transform.GetComponent<UniqueId>().uniqueId;
        xPos = transform.position.x;
        yPos = transform.position.y;
        zPos = transform.position.z;
        xRot = transform.eulerAngles.x;
        yRot = transform.eulerAngles.y;
        zRot = transform.eulerAngles.z;
        xScale = transform.localScale.x;
        yScale = transform.localScale.y;
        zScale = transform.localScale.z;
    }

    public void Load(Transform transform)
    {
        Vector3 position = new Vector3(xPos, yPos, zPos);
        Vector3 rotation = new Vector3(xRot, yRot, zRot);
        Vector3 scale = new Vector3(xScale, yScale, zScale);

        transform.position = position;
        transform.rotation = Quaternion.Euler(rotation);
        transform.localScale = scale;
    }
}

[Serializable]
public class CharacterData
{
    public string uniqueId;
    public CharacterInfo characterInfo;
    public CharacterClass characterClass;
    public float currentHealth;
    public float currentEnergy;
    public float currentMana;
    public string equippedHelmet;
    public string equippedArmor;
    public string equippedCloak;
    public string equippedMainHand;
    public string equippedOffHand;
    public string equippedRanged;
    public string[] effects;
    public float[] effectDurationTimers;

    public CharacterData(Character character)
    {
        this.uniqueId = character.GetComponent<UniqueId>().uniqueId;
        characterInfo = character.characterInfo;
        characterClass = character.characterClass;
        currentHealth = character.currentHealth;
        currentEnergy = character.currentEnergy;
        currentMana = character.currentMana;
        equippedHelmet = character.equippedHelmet != null ? character.equippedHelmet.GetType().ToString() : string.Empty;
        equippedArmor = character.equippedArmor != null ? character.equippedArmor.GetType().ToString() : string.Empty;
        equippedCloak = character.equippedCloak != null ? character.equippedCloak.GetType().ToString() : string.Empty;
        equippedMainHand = character.equippedMainHand != null ? character.equippedMainHand.GetType().ToString() : string.Empty;
        equippedOffHand = character.equippedOffHand != null ? character.equippedOffHand.GetType().ToString() : string.Empty;
        equippedRanged = character.equippedRanged != null ? character.equippedRanged.GetType().ToString() : string.Empty;
        effects = new string[character.effects.Count];
        effectDurationTimers = new float[character.effects.Count];
        for (int i = 0; i < character.effects.Count; i++)
        {
            effects[i] = character.effects[i].GetType().ToString();
            effectDurationTimers[i] = character.effects[i].durationTimer;
        }
    }

    public void Load(Character character)
    {
        character.characterClass = characterClass;
        character.characterInfo = characterInfo;
        character.currentHealth = currentHealth;
        character.currentEnergy = currentEnergy;
        character.currentMana = currentMana;
        if (equippedHelmet != string.Empty)
        {
            Item item = (Item)Activator.CreateInstance(Type.GetType(equippedHelmet));
            character.GetComponent<Inventory>().items.Add(item);
            item.Use(character, character.GetComponent<Inventory>());
        }
        if (equippedArmor != string.Empty)
        {
            Item item = (Item)Activator.CreateInstance(Type.GetType(equippedArmor));
            character.GetComponent<Inventory>().items.Add(item);
            item.Use(character, character.GetComponent<Inventory>());
        }
        if (equippedCloak != string.Empty)
        {
            Item item = (Item)Activator.CreateInstance(Type.GetType(equippedCloak));
            character.GetComponent<Inventory>().items.Add(item);
            item.Use(character, character.GetComponent<Inventory>());
        }
        if (equippedMainHand != string.Empty)
        {
            Item item = (Item)Activator.CreateInstance(Type.GetType(equippedMainHand));
            character.GetComponent<Inventory>().items.Add(item);
            item.Use(character, character.GetComponent<Inventory>());
        }
        if (equippedOffHand != string.Empty)
        {
            Item item = (Item)Activator.CreateInstance(Type.GetType(equippedOffHand));
            character.GetComponent<Inventory>().items.Add(item);
            item.Use(character, character.GetComponent<Inventory>());
        }
        if (equippedRanged != string.Empty)
        {
            Item item = (Item)Activator.CreateInstance(Type.GetType(equippedRanged));
            character.GetComponent<Inventory>().items.Add(item);
            item.Use(character, character.GetComponent<Inventory>());
        }

        for (int i = 0; i < effects.Length; i++)
        {
            Effect effect = (Effect)Activator.CreateInstance(Type.GetType(effects[i]));
            effect.durationTimer = effectDurationTimers[i];
            character.AddEffect(effect);
        }
    }
}

[Serializable]
public class InventoryData
{
    public string uniqueId;
    public string[] items;
    public int[] itemSlotIndexes;
    public int[] itemStacks;
    public int slotAmount;

    public InventoryData(Inventory inventory)
    {
        this.uniqueId = inventory.GetComponent<UniqueId>().uniqueId;
        items = new string[inventory.items.Count];
        itemSlotIndexes = new int[inventory.items.Count];
        itemStacks = new int[inventory.items.Count];
        for (int i = 0; i < inventory.items.Count; i++)
        {
            items[i] = inventory.items[i].GetType().ToString();
            itemSlotIndexes[i] = inventory.items[i].slotIndex;
            itemStacks[i] = inventory.items[i].stack;
        }
        slotAmount = inventory.slotAmount;
    }

    public void Load(Inventory inventory)
    {
        for (int i = 0; i < items.Length; i++)
        {
            Item item = (Item)Activator.CreateInstance(Type.GetType(items[i]));
            item.slotIndex = itemSlotIndexes[i];
            item.stack = itemStacks[i];
            inventory.items.Add(item);
        }
        inventory.slotAmount = slotAmount;
    }
}

[Serializable]
public class SpellbookData
{
    public string uniqueId;
    public string[] spells;
    public float[] spellCooldownTimers;

    public SpellbookData(Spellbook spellbook)
    {
        this.uniqueId = spellbook.GetComponent<UniqueId>().uniqueId;
        spells = new string[spellbook.spells.Count];
        spellCooldownTimers = new float[spellbook.spells.Count];
        for (int i = 0; i < spellbook.spells.Count; i++)
        {
            spells[i] = spellbook.spells[i].GetType().ToString();
            spellCooldownTimers[i] = spellbook.spells[i].cooldownTimer;
        }
    }

    public void Load(Spellbook spellbook)
    {
        for (int i = 0; i < spells.Length; i++)
        {
            Spell spell = (Spell)Activator.CreateInstance(Type.GetType(spells[i]));
            spell.cooldownTimer = spellCooldownTimers[i];
            spellbook.spells.Add(spell);
        }
    }
}

[Serializable]
public class HotbarData
{
    public string uniqueId;
    public string[] usables;
    public int[] usableInventorySlotIndexes;

    public HotbarData(Hotbar hotbar)
    {
        this.uniqueId = hotbar.GetComponent<UniqueId>().uniqueId;
        usables = new string[hotbar.usables.Length];
        usableInventorySlotIndexes = new int[hotbar.usables.Length];
        for (int i = 0; i < hotbar.usables.Length; i++)
        {
            usables[i] = string.Empty;

            if (hotbar.usables[i] != null)
            {
                usables[i] = hotbar.usables[i].GetType().ToString();

                if (hotbar.usables[i] is Item)
                {
                    Item item = (Item)hotbar.usables[i];
                    usableInventorySlotIndexes[i] = item.slotIndex;
                }
            }
        }
    }

    public void Load(Hotbar hotbar)
    {
        for (int i = 0; i < usables.Length; i++)
        {
            if (usables[i] != string.Empty)
            {
                Usable usable = (Usable)Activator.CreateInstance(Type.GetType(usables[i]));

                if (usable is Item)
                {
                    for (int j = 0; j < hotbar.GetComponent<Inventory>().items.Count; j++)
                    {
                        if (hotbar.GetComponent<Inventory>().items[j].slotIndex == usableInventorySlotIndexes[i])
                        {
                            hotbar.usables[i] = hotbar.GetComponent<Inventory>().items[j];
                        }
                    }
                }

                if (usable is Spell)
                {
                    for (int j = 0; j < hotbar.GetComponent<Spellbook>().spells.Count; j++)
                    {     
                        if (hotbar.GetComponent<Spellbook>().spells[j].GetType() == usable.GetType())
                        {
                            hotbar.usables[i] = hotbar.GetComponent<Spellbook>().spells[j];
                        }
                    }
                }
            }
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public enum BodyHolders
    {
        Head = 0,
        Back,
        LeftFoot,
        RightFoot,
        LeftHand,
        RightHand,
        LeftForeArm,
        RightForeArm,
        Spine,
        LeftUpLeg,
        RightUpLeg
    };

    public BodyHolder[] holders;
    public Transform itemsRoot;
    public GameObject door;
    public string[] itemsRequired;

    Hitbox m_hitbox;
    List<Item> m_listItems;

    public void Init (Player _player)
    {
        m_hitbox = GetComponent<Hitbox>();
        m_listItems = new List<Item>();
    }

    public void Think ()
    {
        for (int i = 0; i < m_listItems.Count; i++)
            m_listItems[i].Use();
    }

    public void AddItem (Item _item)
    {
        m_listItems.Add(_item);

        if (HasItemsRequired())
            door.SetActive(false);
    }

    public Item GetItem (string _name)
    {
        for (int i = 0; i < m_listItems.Count; i++)
        {
            if (m_listItems[i].itemName.Equals(_name))
                return m_listItems[i];
        }

        return null;
    }

    public Transform GetHolderRoot (BodyHolders _index)
    {
        for (int i = 0; i < holders.Length; i++)
        {
            if (holders[i].index == _index)
                return holders[i].origin;
        }

        return null;
    }

    void AttackEnd ()
    {
        ItemKnife knife = GetItem("Cuchillo") as ItemKnife;

        if (knife != null)
            knife.SetAttacking(false);
    }

    void AttackHit ()
    {
        ItemKnife knife = GetItem("Cuchillo") as ItemKnife;

        if (knife != null)
            m_hitbox.Detect(knife.damage);
    }

    bool HasItemsRequired ()
    {
        for (int i = 0; i < itemsRequired.Length; i++)
        {
            if (!GetItem(itemsRequired[i]))
                return false;
        }

        return true;
    }
}

[System.Serializable]
public class BodyHolder
{
    public PlayerInventory.BodyHolders index;
    public Transform origin;
}
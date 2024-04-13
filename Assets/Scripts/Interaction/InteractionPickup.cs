using UnityEngine;

public class InteractionPickup : MonoBehaviour, Interactable
{
    public Item item;

    public void Interact (Player _player)
    {
        PlayerInventory inventory = _player.playerInventory;

        if (inventory == null)
            return;

        Item newItem = Instantiate(item, inventory.itemsRoot);

        if (newItem != null)
        {
            newItem.Init(_player);
            inventory.AddItem(newItem);
        }

        Destroy(gameObject);
    }

    public bool IsAvailable ()
    {
        return true;
    }

    public string GetText ()
    {
        return item.itemName;
    }
}

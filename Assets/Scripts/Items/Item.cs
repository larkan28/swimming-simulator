using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [Header("Information")]
    public string itemName;

    [Header("3D Mesh")]
    public ItemMesh[] meshes;

    protected Player m_player;

    public virtual void Init (Player _player)
    {
        m_player = _player;

        for (int i = 0; i < meshes.Length; i++)
            meshes[i].CreateMesh(_player.playerInventory);
    }

    public virtual void Use ()
    {

    }
}

[System.Serializable]
public class ItemMesh
{
    public Transform mesh;
    public PlayerInventory.BodyHolders holder;

    public void CreateMesh (PlayerInventory _inventory)
    {
        Transform newMesh = Object.Instantiate(mesh, _inventory.GetHolderRoot(holder));
        
        if (newMesh != null)
        {
            newMesh.localPosition = Vector3.zero;
            newMesh.localRotation = Quaternion.Euler(Vector3.zero);
        }
    }
}
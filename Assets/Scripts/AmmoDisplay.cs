using UnityEngine;
using UnityEngine.UI;

public class AmmoDisplay : MonoBehaviour
{
    public GameObject ammo_in_gun;
    public GameObject ammo_in_inventory;
    public GameObject ammo;

    public void ShowSelectedWeapon(Raycasts raycasts, bool isKnife)
    {
        string inGun = isKnife ? "-" : (raycasts != null ? raycasts.current_ammo.ToString() : "-");
        string inInventory = isKnife ? "-" : (raycasts != null ? raycasts.all_ammo.ToString() : "-");
        string capacity = isKnife ? "-" : (raycasts != null ? raycasts.max_ammo.ToString() : "-");

        Write(ammo_in_gun, inGun);
        Write(ammo_in_inventory, inInventory);
        Write(ammo, capacity);
    }

    private static void Write(GameObject target, string value)
    {
        if (target == null)
            return;

        Text text = target.GetComponent<Text>();
        if (text != null)
            text.text = value;
    }
}

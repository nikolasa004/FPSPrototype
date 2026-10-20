using UnityEngine;

public class KontrolaOružja : MonoBehaviour
{
    [SerializeField] private OsobineOružja[] Oružja;

    private const int PlayableWeaponCount = 6;
    private int current_weapon_index = -1;

    private void Start()
    {
        if (Oružja == null || Oružja.Length == 0)
        {
            Debug.LogWarning("KontrolaOružja: Assign the weapons in the Inspector.", this);
            return;
        }

        // Zadnje oruzje nije implementirano
        int initialIndex = -1;
        int limit = Mathf.Min(PlayableWeaponCount, Oružja.Length);
        for (int i = 0; i < limit; i++)
        {
            if (Oružja[i] != null && Oružja[i].gameObject.activeSelf)
            {
                initialIndex = i;
                break;
            }
        }

        if (initialIndex < 0)
        {
            for (int i = 0; i < limit; i++)
            {
                if (Oružja[i] != null)
                {
                    initialIndex = i;
                    break;
                }
            }
        }

        for (int i = 0; i < Oružja.Length; i++)
        {
            if (Oružja[i] != null)
                Oružja[i].gameObject.SetActive(false);
        }

        current_weapon_index = -1;
        SelectWeapon(initialIndex);
    }

    private void Update()
    {
        for (int i = 0; i < PlayableWeaponCount; i++)
        {
            if (Input.GetKeyDown((KeyCode)((int)KeyCode.Alpha1 + i)) ||
                Input.GetKeyDown((KeyCode)((int)KeyCode.Keypad1 + i)))
            {
                SelectWeapon(i);
                return;
            }
        }
    }

    private void SelectWeapon(int index)
    {
        if (Oružja == null || index < 0 || index >= PlayableWeaponCount ||
            index >= Oružja.Length || Oružja[index] == null || index == current_weapon_index)
            return;

        for (int i = 0; i < Oružja.Length; i++)
        {
            if (Oružja[i] != null && i != index)
                Oružja[i].gameObject.SetActive(false);
        }

        current_weapon_index = index;
        Oružja[index].gameObject.SetActive(true);
    }

    public OsobineOružja GetCurrentSelectedWeapon()
    {
        if (Oružja == null || current_weapon_index < 0 || current_weapon_index >= Oružja.Length)
            return null;

        return Oružja[current_weapon_index];
    }

    public int GetWeaponIndex(OsobineOružja weapon)
    {
        if (Oružja == null || weapon == null)
            return -1;

        for (int i = 0; i < Oružja.Length; i++)
        {
            if (Oružja[i] == weapon)
                return i;
        }
        return -1;
    }
}

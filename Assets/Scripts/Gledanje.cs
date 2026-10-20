using UnityEngine;

public class Gledanje : MonoBehaviour
{
    [SerializeField] private Transform Igrac, Kamere;
    [SerializeField] private bool invert;
    [SerializeField] private float sensitivity = 5f;
    [SerializeField] private Vector2 look_limit = new Vector2(-70f, 80f);

    private Vector2 look_angles;
    private Vector2 current_mouse_look;
    private KontrolaOružja kontrola_oružja;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        kontrola_oružja = GetComponent<KontrolaOružja>();
    }

    private void Update()
    {
        LockAndUnlockCursor();

        Raycasts selectedRaycasts = null;
        if (kontrola_oružja != null)
        {
            OsobineOružja selected = kontrola_oružja.GetCurrentSelectedWeapon();
            if (selected != null)
                selectedRaycasts = selected.GetComponentInChildren<Raycasts>(true);
        }

        sensitivity = selectedRaycasts != null && selectedRaycasts.is_aiming ? 1f : 5f;

        if (Cursor.lockState == CursorLockMode.Locked)
            LookAround();
    }

    private void LockAndUnlockCursor()
    {
        if (!Input.GetKeyDown(KeyCode.Escape))
            return;

        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // Metoda koja upravlja kretanjem kamere i igraca na osnovu unosa misa
    private void LookAround()
    {
        current_mouse_look = new Vector2(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"));
        look_angles.x += current_mouse_look.x * sensitivity * (invert ? 1f : -1f);
        look_angles.y += current_mouse_look.y * sensitivity;
        look_angles.x = Mathf.Clamp(look_angles.x, look_limit.x, look_limit.y);
        Kamere.localRotation = Quaternion.Euler(look_angles.x, 0f, 0f);
        Igrac.localRotation = Quaternion.Euler(0f, look_angles.y, 0f);
    }
}

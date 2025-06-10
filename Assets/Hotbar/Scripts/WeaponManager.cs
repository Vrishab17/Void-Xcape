using UnityEngine;
using System.Collections.Generic;

public class WeaponManager : MonoBehaviour
{
    public Transform weaponHolder;
    public HotbarUIManager hotbarUI;
    public List<Sprite> weaponIcons;

    private List<GameObject> weapons = new List<GameObject>();
    private int currentWeaponIndex = 0;

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f && weapons.Count > 0)
        {
            currentWeaponIndex = (currentWeaponIndex + (scroll > 0 ? 1 : -1) + weapons.Count) % weapons.Count;
            SelectWeapon(currentWeaponIndex);
        }
    }

    public void AddWeapon(GameObject weapon)
    {
        weapon.SetActive(false);
        weapons.Add(weapon);

        if (hotbarUI != null && weaponIcons.Count >= weapons.Count)
            hotbarUI.SetIcon(weapons.Count - 1, weaponIcons[weapons.Count - 1]);

        if (weapons.Count == 1)
            SelectWeapon(0);
    }

    void SelectWeapon(int index)
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            if (weapons[i] != null)
            {
                weapons[i].SetActive(i == index);
            }
        }

        if (hotbarUI != null)
        {
            hotbarUI.UpdateHighlight(index);
        }

        currentWeaponIndex = index;
    }
}

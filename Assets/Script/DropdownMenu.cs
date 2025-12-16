using UnityEngine;
using UnityEngine.UI;

public class DropdownMenu : MonoBehaviour
{
    public GameObject dropdownPanel; // Panel yang berisi tombol Play, Pause, Kembali

    private bool isOpen = false; // Status dropdown

    void Start()
    {
        dropdownPanel.SetActive(false); // Pastikan dropdown tertutup di awal
    }

    public void ToggleDropdown()
    {
        isOpen = !isOpen; // Balik status
        dropdownPanel.SetActive(isOpen); // Aktifkan/nonaktifkan panel
    }

    // Fungsi ini dipanggil saat tombol Play/Pause/Kembali ditekan
    public void CloseDropdown()
    {
        isOpen = false;
        dropdownPanel.SetActive(false);
    }
}

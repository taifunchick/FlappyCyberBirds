using UnityEngine;

public class SpawnSelectedCharacter : MonoBehaviour
{
    public Transform spawnPoint; // Точка спавна (обязательно назначь в инспекторе!)
    public GameObject[] characterPrefabs; // Массив префабов персонажей (в том же порядке, что и имена в CharacterSelector)

    private void Start()
    {
        // Получаем имя выбранного персонажа
        string selectedCharName = PlayerPrefs.GetString("SelectedCharacter", "");

        // Находим подходящий префаб
        GameObject prefabToSpawn = null;
        foreach (GameObject prefab in characterPrefabs)
        {
            if (prefab != null && prefab.name == selectedCharName)
            {
                prefabToSpawn = prefab;
                break;
            }
        }

        if (prefabToSpawn != null && spawnPoint != null)
        {
            Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);
            Debug.Log($"Спавнен персонаж: {selectedCharName}");
        }
        else
        {
            Debug.LogWarning($"Персонаж '{selectedCharName}' не найден или точка спавна не задана!");
        }
    }
}
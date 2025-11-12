using UnityEngine;

public class CharacterModelSwitcher : MonoBehaviour
{
    [Header("Prefabs das diferentes peles")]
    public GameObject whiteSkinPrefab;   // 0
    public GameObject brownSkinPrefab;   // 1
    public GameObject blackSkinPrefab;   // 2

    [Header("Posição onde o personagem será instanciado")]
    public Transform spawnPoint;

    [Header("Ajustes de posição (aplicados APÓS instanciar)")]
    [Tooltip("Deslocamento vertical (Y) aplicado apenas ao modelo negro")]
    public float blackSkinYOffset = -0.05f;

    private GameObject currentCharacter;

    void Start()
    {
        int skinIndex = PlayerPrefs.GetInt("SkinIndex", 0);
        TrocarModelo(skinIndex);
    }

    public void TrocarModelo(int skinIndex)
    {
        if (currentCharacter != null)
            Destroy(currentCharacter);

        GameObject prefabToSpawn = null;

        switch (skinIndex)
        {
            case 0: prefabToSpawn = whiteSkinPrefab; break;
            case 1: prefabToSpawn = brownSkinPrefab; break;
            case 2: prefabToSpawn = blackSkinPrefab; break; // negra e parda estao trocadas (index)
            default: prefabToSpawn = whiteSkinPrefab; break;
        }

        if (prefabToSpawn == null || spawnPoint == null)
        {
            Debug.LogWarning("⚠️ Prefab ou spawnPoint não definidos!");
            return;
        }

        // Instancia no ponto padrão
        currentCharacter = Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);

        // ✅ Só depois de instanciar, aplica o deslocamento se for o modelo negro
        if (skinIndex == 1) // negro
        {
            var pos = currentCharacter.transform.position;
            pos.y += blackSkinYOffset;                
            currentCharacter.transform.position = pos;

        }
    }
}

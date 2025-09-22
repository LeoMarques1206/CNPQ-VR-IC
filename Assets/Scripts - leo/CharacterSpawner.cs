using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    public GameObject[] modelPrefabs; // mesmo array (ordem idêntica à da tela de seleção)
    public Transform spawnPoint;

    void Awake()
    {
        int idx = Mathf.Clamp(SelectedCharacterState.ModelIndex, 0, modelPrefabs.Length - 1);
        var go = Instantiate(modelPrefabs[idx], spawnPoint.position, spawnPoint.rotation);

        // Você pode anexar/metadados do perfil:
        var meta = go.AddComponent<CharacterMetadata>();
        meta.behavior = SelectedCharacterState.Behavior;
        meta.healthProfile = SelectedCharacterState.HealthProfile;

        // Aqui você pode aplicar efeitos práticos a partir das strings (placeholder):
        ApplyProfiles(meta);
    }

    void ApplyProfiles(CharacterMetadata meta)
    {
        // Exemplo: mapeie strings para variáveis
        // Comportamental: muda velocidade de fala, reação, etc.
        switch (meta.behavior)
        {
            case "Calma":      /* meta.reactionTime = 1.2f; */ break;
            case "Ansiosa":    /* meta.reactionTime = 0.8f; */ break;
            case "Confiante":  /* meta.reactionTime = 1.0f; */ break;
        }
        // Perfil de saúde: poderia ajustar tolerância a dor, stamina etc.
        switch (meta.healthProfile)
        {
            case "Padrão":   /* meta.stamina = 100; */ break;
            case "Atleta":   /* meta.stamina = 130; */ break;
            case "Sensível": /* meta.stamina = 80;  */ break;
        }
    }
}

public class CharacterMetadata : MonoBehaviour
{
    public string behavior;
    public string healthProfile;
    
}
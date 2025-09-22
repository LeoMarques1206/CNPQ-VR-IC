using UnityEngine;

public static class SelectedCharacterState
{
    // Chaves PlayerPrefs
    private const string KEY_MODEL_INDEX = "sel_model_index";
    private const string KEY_BEHAVIOR = "sel_behavior";
    private const string KEY_HEALTH = "sel_health";

    // Defaults
    public static int ModelIndex
    {
        get => PlayerPrefs.GetInt(KEY_MODEL_INDEX, 0);
        set { PlayerPrefs.SetInt(KEY_MODEL_INDEX, Mathf.Clamp(value, 0, 2)); PlayerPrefs.Save(); }
    }

    public static string Behavior
    {
        get => PlayerPrefs.GetString(KEY_BEHAVIOR, "Neutro");
        set { PlayerPrefs.SetString(KEY_BEHAVIOR, value); PlayerPrefs.Save(); }
    }

    public static string HealthProfile
    {
        get => PlayerPrefs.GetString(KEY_HEALTH, "Padrão");
        set { PlayerPrefs.SetString(KEY_HEALTH, value); PlayerPrefs.Save(); }
    }
}
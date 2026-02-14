using UnityEngine;

public class VolumeController : MonoBehaviour
{
    public void ChangeVolume(float value)
    {
        if (AudioManagerLeo.Instance != null)
        {
            AudioManagerLeo.Instance.SetVolume(value);
        }
    }
}

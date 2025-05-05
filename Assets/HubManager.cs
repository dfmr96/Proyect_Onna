using UnityEngine;

public class HubManager : MonoBehaviour
{
    [SerializeField] private LevelProgression levelProgression;
    private void Awake()
    {
        levelProgression.ResetProgress();
        RunData.Clear();
    }
}

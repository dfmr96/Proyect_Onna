using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;

    public GameObject SpawnPlayer()
    {
        GameObject player = Instantiate(playerPrefab, transform.position, Quaternion.identity);

        return player;
    }
}

using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;

    public GameObject SpawnPlayer()
    {
        GameObject player = Instantiate(playerPrefab, transform.position, Quaternion.identity);
        PlayerModel playerModel = player.GetComponent<PlayerModel>();
        playerModel.SetTime(RunData.CurrentStats.CurrentEnergyTime);
        playerModel.SetSpeed(RunData.CurrentStats.MoveSpeed);

        return player;
    }
}

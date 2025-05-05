using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public float TimeRemaining { get; private set; }
    [SerializeField] private GameObject playerSpawner;
    private GameObject player;
    private PlayerModel playerModel;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        Instance = this;
        player = playerSpawner.GetComponent<PlayerSpawner>().SpawnPlayer();
        PlayerHelper.SetPlayer(player);
        playerModel = player.GetComponent<PlayerModel>();
    }
    private void Update()
    {
        float damagePerFrame = playerModel.TimeDrainRate * Time.deltaTime;
        TimeRemaining -= damagePerFrame;
        playerModel.TakeDamage(damagePerFrame);
    }
}

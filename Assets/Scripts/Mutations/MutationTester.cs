using Mutations;
using UnityEngine;
using NaughtyAttributes;
using Player;
using Player.Stats;

public class MutationTester : MonoBehaviour
{
    [Header("Player Runtime Stats")] public RuntimeStats playerRuntimeStats;

    [Header("Mutations")] public UpgradeEffect oxigenoOnna;
    public UpgradeEffect blindajeOseo;
    public UpgradeEffect miradaDelUmbral;
    public UpgradeEffect segundaAlma;
    public UpgradeEffect tacticaDeGuerra;
    public UpgradeEffect sangreMaldita;

    private void Start()
    {
        var player = FindObjectOfType<PlayerModel>();
        if (player != null)
        {
            playerRuntimeStats = player.RuntimeStats;
            Debug.Log("✅ RuntimeStats obtenido desde PlayerModel.");
        }
        else
        {
            Debug.LogWarning("⛔ No se encontró PlayerModel en la escena.");
        }
    }


    [Button("🧪 Oxígeno ONИA")]
    private void TestOxigenoOnna()
    {
        ApplyMutation(oxigenoOnna);
    }

    [Button("🧪 Blindaje Óseo")]
    private void TestBlindajeOseo()
    {
        ApplyMutation(blindajeOseo);
    }

    [Button("🧪 Mirada del Umbral")]
    private void TestMiradaDelUmbral()
    {
        ApplyMutation(miradaDelUmbral);
    }

    [Button("🧪 Segunda Alma")]
    private void TestSegundaAlma()
    {
        ApplyMutation(segundaAlma);
    }

    [Button("🧪 Táctica de Guerra")]
    private void TestTacticaDeGuerra()
    {
        ApplyMutation(tacticaDeGuerra);
    }

    [Button("🧪 Sangre Maldita")]
    private void TestSangreMaldita()
    {
        ApplyMutation(sangreMaldita);
    }

    private void ApplyMutation(UpgradeEffect effect)
    {
        if (playerRuntimeStats == null)
        {
            Debug.LogWarning("⛔ PlayerRuntimeStats no está asignado.");
            return;
        }

        if (effect == null)
        {
            Debug.LogWarning("⛔ Mutation effect no asignado.");
            return;
        }

        Debug.Log($"✅ Aplicando mutación: {effect.GetType().Name}");
        effect.Apply(playerRuntimeStats);
    }
    
    [Button("🧪 Test Damage (con resistencia)")]
    private void TestDamageWithResistance()
    {
        var player = FindObjectOfType<PlayerModel>();
        if (player != null)
        {
            float testDamage = 10f;
            player.ApplyDamage(testDamage, applyResistance: true);
            Debug.Log($"🧪 Damage aplicado con resistencia: {testDamage}");
        }
        else
        {
            Debug.LogWarning("⛔ PlayerModel no encontrado en escena.");
        }
    }
    
    [Button("🧪 Test Damage (sin resistencia)")]
    private void TestDamageNoResistance()
    {
        var player = FindObjectOfType<PlayerModel>();
        if (player != null)
        {
            float testDamage = 10f;
            player.ApplyDamage(testDamage, applyResistance: false);
            Debug.Log($"🧪 Damage aplicado sin resistencia: {testDamage}");
        }
    }
}
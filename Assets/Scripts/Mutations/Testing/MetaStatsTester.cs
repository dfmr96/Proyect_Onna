using NaughtyAttributes;
using Player;
using Player.Stats;
using Player.Stats.Meta;
using UnityEngine;

namespace Mutations.Testing
{
    public class MetaStatsTester : MonoBehaviour
    {
        private PlayerModel player;
        private MetaStatBlock metaStats;
        private StatReferences statRefs;

        private void Start()
        {
            player = FindObjectOfType<PlayerModel>();
            if (player == null)
            {
                Debug.LogWarning("⛔ PlayerModel no encontrado en escena.");
                return;
            }

            if (player.StatContext.Source == null)
            {
                Debug.LogWarning("⛔ RuntimeStats no inicializado aún.");
                return;
            }

            metaStats = player.StatContext.Meta;
            statRefs = player.StatRefs;

            if (metaStats == null)
                Debug.LogWarning("⛔ MetaStats no encontrado en RuntimeStats.");
        }

        [Button("➕ +10 Tiempo Vital (Meta)")]
        private void AddMetaVitalTime()
        {
            AddMetaBonus(statRefs.maxVitalTime, 10f);
        }

        [Button("➕ +5% Velocidad Base (Meta)")]
        private void AddMetaSpeedPercent()
        {
            float baseSpeed = player.StatContext.Runtime.GetBaseValue(statRefs.movementSpeed);
            float increase = baseSpeed * 0.05f;
            AddMetaBonus(statRefs.movementSpeed, increase);
        }

        [Button("➕ +0.05 Drain Resistance (Meta)")]
        private void AddDrainResistance()
        {
            AddMetaBonus(statRefs.damageResistance, 0.05f);
        }

        private void AddMetaBonus(StatDefinition stat, float amount)
        {
            if (metaStats == null)
            {
                Debug.LogWarning("⛔ MetaStatBlock no está disponible.");
                return;
            }

            float current = metaStats.Get(stat);
            metaStats.Set(stat, current + amount);
            metaStats.RebuildLookup();

            Debug.Log($"🧬 MetaStat actualizado: {stat.name} = {current} + {amount} => {current + amount}");

            if (player != null)
            {
                player.ForceReinitStats();
                Debug.Log("🔁 RuntimeStats reinicializado con MetaStats actualizados.");
            }
        }
    }
}

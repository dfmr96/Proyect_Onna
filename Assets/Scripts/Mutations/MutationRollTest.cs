using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Mutations
{
    [ExecuteInEditMode]
    public class MutationRollTest : MonoBehaviour
    {
        [SerializeField] private MutationSelector selector;

        [Button("Roll Mutations")]
        private void RollMutationsEditor()
        {
            if (selector == null)
            {
                Debug.LogWarning("MutationSelector not assigned.");
                return;
            }

            List<MutationData> upgrades = selector.RollMutations(3);

            if (upgrades == null || upgrades.Count == 0)
            {
                Debug.LogWarning("No mutations returned from selector.");
                return;
            }

            foreach (var mutation in upgrades)
            {
                Debug.Log($"🧬 Rolled: {mutation.Organ} → {mutation.MutationName}");
            }
        }
    }
}
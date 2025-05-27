using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Mutations
{
    [ExecuteInEditMode]
    public class MutationRollTest : MonoBehaviour
    {
        [SerializeField] private MutationSelector selector;
        [SerializeField] private MutationOptionUI mutationUIPrefab;
        [SerializeField] private Transform uiContainer;

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
        
        [Button("Roll and Show UI")]
        private void RollMutationsInGame()
        {
            if (!Application.isPlaying)
            {
                Debug.LogWarning("UI display only works in Play Mode.");
                return;
            }

            if (selector == null || mutationUIPrefab == null || uiContainer == null)
            {
                Debug.LogError("Missing references: selector, UI prefab, or container.");
                return;
            }
            

            foreach (Transform child in uiContainer)
            {
                Destroy(child.gameObject);
            }

            var mutations = selector.RollMutations(3);

            if (mutations.Count == 0)
            {
                Debug.LogWarning("No mutations rolled.");
                return;
            }
            Debug.Log("Mutations rolled successfully. Displaying UI...");
            foreach (var mutation in mutations)
            {
                var ui = Instantiate(mutationUIPrefab, uiContainer);
                ui.SetData(mutation);
            }
            uiContainer.gameObject.SetActive(true);
        }
        
    }
}
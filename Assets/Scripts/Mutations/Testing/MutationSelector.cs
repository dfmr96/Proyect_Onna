using System.Collections.Generic;
using UnityEngine;

namespace Mutations.Testing
{
    public class MutationSelector : MonoBehaviour
    {
        [SerializeField] private MutationDatabase mutationDatabase;
        
        public List<MutationData> RollMutations(int amount)
        {
            List<OrganType> availableOrgans = new List<OrganType>(mutationDatabase.AvailableOrgans);

            if (availableOrgans.Count == 0)
            {
                Debug.LogWarning("No organs with available mutations.");
                return new List<MutationData>();
            }

            OrganType selectedOrgan = availableOrgans[Random.Range(0, availableOrgans.Count)];

            List<MutationData> pool = mutationDatabase.GetMutationsByOrgan(selectedOrgan);

            return GetRandomDistinct(pool, amount);
        }

        private List<MutationData> GetRandomDistinct(List<MutationData> source, int count)
        {
            if (source == null || source.Count == 0) return new List<MutationData>();

            List<MutationData> result = new List<MutationData>();
            List<MutationData> copy = new List<MutationData>(source);

            for (int i = 0; i < count && copy.Count > 0; i++)
            {
                int index = Random.Range(0, copy.Count);
                result.Add(copy[index]);
                copy.RemoveAt(index);
            }

            return result;
        }
    }
}
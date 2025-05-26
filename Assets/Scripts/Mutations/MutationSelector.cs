using System.Collections.Generic;
using UnityEngine;

namespace Mutations
{
    public class MutationSelector : MonoBehaviour
    {
        [SerializeField] private MutationDatabase mutationDatabase;
        
        public List<MutationData> RollMutations(int amount)
        {
            OrganType[] organTypes = (OrganType[])System.Enum.GetValues(typeof(OrganType));
            OrganType selectedOrgan = organTypes[Random.Range(0, organTypes.Length)];

            List<MutationData> pool = mutationDatabase.GetMutationsByOrgan(selectedOrgan);

            return GetRandomDistinct(pool, amount);
        }

        private List<MutationData> GetRandomDistinct(List<MutationData> source, int count)
        {
            List<MutationData> result = new List<MutationData>();
            List<MutationData> temp = new List<MutationData>(source);

            for (int i = 0; i < count && temp.Count > 0; i++)
            {
                int index = Random.Range(0, temp.Count);
                result.Add(temp[index]);
                temp.RemoveAt(index);
            }

            return result;
        }
    }
}
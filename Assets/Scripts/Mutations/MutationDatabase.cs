using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Mutations
{
    [CreateAssetMenu(fileName = "New Mutation Database", menuName = "Mutations/Database", order = 0)]
    public class MutationDatabase : ScriptableObject
    {
        [field:SerializeField] private List<MutationData> allMutations;

        public List<MutationData> AllMutations => allMutations;

        public List<MutationData> GetMutationsByOrgan(OrganType organ)
        {
            return AllMutations.Where(m => m.Organ == organ).ToList();
        }
    }
}
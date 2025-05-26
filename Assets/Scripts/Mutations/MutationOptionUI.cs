using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mutations
{
    public class MutationOptionUI : MonoBehaviour
    {
        [SerializeField] private Image mutationIcon;
        [SerializeField] private TextMeshProUGUI mutationName;
        [SerializeField] private TextMeshProUGUI mutationDescription;

        public void SetData(MutationData mutation)
        {
            if (mutationIcon != null)
                mutationIcon.sprite = mutation.Icon;

            if (mutationName != null)
                mutationName.text = mutation.MutationName;

            if (mutationDescription != null)
                mutationDescription.text = mutation.Description;
        }
    }
}
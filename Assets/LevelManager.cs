using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject loadScreenPrefab;
    void Start()
    {
        if (loadScreenPrefab != null)
        {
            GameObject loadScreen = Instantiate(loadScreenPrefab);
            loadScreen.GetComponent<Animator>().SetTrigger("FadeOut");
            StartCoroutine(DestroyAfterAnimation(loadScreen, loadScreen.GetComponent<Animator>()));
        }
    }
    IEnumerator DestroyAfterAnimation(GameObject obj, Animator animator)
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        while (stateInfo.length == 0)
        {
            yield return null;
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        }

        float animationLength = stateInfo.length;

        yield return new WaitForSeconds(animationLength);

        Destroy(obj);
    }
}

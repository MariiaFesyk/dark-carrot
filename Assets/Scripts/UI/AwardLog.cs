using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AwardLog : MonoBehaviour {
    [SerializeField] private GameObject prefab;
    [SerializeField] private float duration;

    //TODO use object pooling

    public void ShowReward(GameObject target, Resource resource, int amount){
        var reward = Instantiate(prefab, target.transform.position, Quaternion.identity, transform);

        var amountText = reward.transform.Find("Amount")?.GetComponent<TMP_Text>();
        if(amountText) amountText.text = $"+{amount}";

        reward.transform.localPosition += new Vector3(0f, 3f, 0);

        StartCoroutine(AnimateMessage(reward));
    }

    //TODO use animation later
    public IEnumerator AnimateMessage(GameObject reward){
        CanvasGroup group = reward.GetComponent<CanvasGroup>();

        Vector3 origin = reward.transform.localPosition;
        Vector3 target = origin + new Vector3(0f, 1f, 0);

        for(float elapsed = 0f; elapsed < duration; elapsed += Time.deltaTime){
            float percent = Mathf.Clamp01(elapsed / duration);
            group.alpha = 1f - Mathf.Clamp01(2f * percent - 1f);
            reward.transform.localPosition = Vector3.Lerp(origin, target, Mathf.Clamp01(2f * percent - 1f));
            
            yield return null;
        }

        Destroy(reward.gameObject);
    }
}

using UnityEngine;

public class CollectableUI : MonoBehaviour
{
    void OnEnable()
    {
        CollectableManager.Instance.OnCollectionProgressChanged += UpdateCollectableUI;
    }

    void OnDisable()
    {
        CollectableManager.Instance.OnCollectionProgressChanged -= UpdateCollectableUI;
    }

    private void UpdateCollectableUI(int collected, int total)
    {
        UIManager.Instance.UpdateCollectableTracker(collected, total);
    }
}

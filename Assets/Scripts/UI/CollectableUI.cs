using UnityEngine;

public class CollectableUI : MonoBehaviour
{
    void OnEnable()
    {
        var manager = CollectableManager.Instance;
        if (manager != null)
        {
            manager.OnCollectionProgressChanged += UpdateCollectableUI;
            // Force update in case initial event already happened before subscribing
            UpdateCollectableUI(manager.TotalCollected, manager.TotalCollectablesInScene);
        }

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

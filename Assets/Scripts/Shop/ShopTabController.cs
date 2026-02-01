using UnityEngine;

public class ShopTabController : MonoBehaviour
{
    [SerializeField] private GameObject[] birdItems;
    [SerializeField] private GameObject[] backgroundItems;
    [SerializeField] private GameObject[] pipeItems;

    private void Start()
    {
        ShowBirds();
    }

    public void ShowBirds()
    {
        SetGroupActive(birdItems, true);
        SetGroupActive(backgroundItems, false);
        SetGroupActive(pipeItems, false);
    }

    public void ShowBackgrounds()
    {
        SetGroupActive(birdItems, false);
        SetGroupActive(backgroundItems, true);
        SetGroupActive(pipeItems, false);
    }

    public void ShowPipes()
    {
        SetGroupActive(birdItems, false);
        SetGroupActive(backgroundItems, false);
        SetGroupActive(pipeItems, true);
    }

    private void SetGroupActive(GameObject[] items, bool isActive)
    {
        if (items == null)
        {
            return;
        }

        foreach (GameObject item in items)
        {
            if (item != null)
            {
                item.SetActive(isActive);
            }
        }
    }
}

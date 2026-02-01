using UnityEngine;

public class PipeSkinApplier : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] targetRenderers;
    [SerializeField] private Sprite[] pipeSprites;
    [SerializeField] private string playerPrefsKey = "SelectedPipeSkin";

    private void Awake()
    {
        ApplyPipeSkin();
    }

    public void ApplyPipeSkin()
    {
        if (pipeSprites == null || pipeSprites.Length == 0)
        {
            return;
        }

        int index = Mathf.Clamp(PlayerPrefs.GetInt(playerPrefsKey, 0), 0, pipeSprites.Length - 1);
        Sprite selectedSprite = pipeSprites[index];

        SpriteRenderer[] renderers = targetRenderers;
        if (renderers == null || renderers.Length == 0)
        {
            renderers = GetComponentsInChildren<SpriteRenderer>(true);
        }

        foreach (SpriteRenderer renderer in renderers)
        {
            if (renderer != null)
            {
                renderer.sprite = selectedSprite;
            }
        }
    }
}

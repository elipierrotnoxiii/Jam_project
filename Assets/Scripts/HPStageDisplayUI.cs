using UnityEngine;
using UnityEngine.UI;

public class HPStageDisplayUI : MonoBehaviour
{
    [Header("Hook these in Inspector")]
    [SerializeField] private Image targetImage;     // The UI Image
    [SerializeField] private Sprite[] stages;       // 0 = empty, last = full
    [SerializeField] private bool fullIsLast = true; // If true: stages[last] = full HP

    [Header("Health")]
    [SerializeField] private int maxHP = 5;
    [SerializeField] private int currentHP = 5;

    public void SetHP(int newHP)
    {
        currentHP = Mathf.Clamp(newHP, 0, maxHP);
        UpdateSprite();
    }

    public void Damage(int amount)
    {
        SetHP(currentHP - amount);
    }

    public void Heal(int amount)
    {
        SetHP(currentHP + amount);
    }

    private void Start()
    {
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        if (targetImage == null || stages == null || stages.Length == 0) return;

        float t = (maxHP <= 0) ? 0f : (float)currentHP / maxHP;
        int index = Mathf.Clamp(Mathf.RoundToInt(t * (stages.Length - 1)), 0, stages.Length - 1);

        if (!fullIsLast) index = (stages.Length - 1) - index;

        targetImage.sprite = stages[index];
    }
}

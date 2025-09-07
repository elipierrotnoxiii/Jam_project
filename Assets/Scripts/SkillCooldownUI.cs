using UnityEngine;
using UnityEngine.UI;

public class SkillCooldownUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image skillIcon;        // static icon sprite
    [SerializeField] private Image cooldownOverlay;  // black/grey overlay image (set type = Filled → Radial360)

    [Header("Cooldown Settings")]
    [SerializeField] private float cooldownTime = 5f;
    private float cooldownRemaining = 0f;

    public bool IsOnCooldown => cooldownRemaining > 0f;

    void Update()
    {
        if (IsOnCooldown)
        {
            cooldownRemaining -= Time.deltaTime;
            if (cooldownRemaining < 0) cooldownRemaining = 0;

            float t = cooldownRemaining / cooldownTime;
            cooldownOverlay.fillAmount = t; // 1 = full overlay, 0 = no overlay
        }
    }

    public void UseSkill()
    {
        if (!IsOnCooldown)
        {
            cooldownRemaining = cooldownTime;
            cooldownOverlay.fillAmount = 1f;
        }
    }
}


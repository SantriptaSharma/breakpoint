using System.Collections;
using UnityEngine.UI;
using UnityEngine;

namespace SantriptaSharma.Breakpoint.Game
{
    class UIController : MonoBehaviour
    {
        public static UIController instance;

        [SerializeField]
        private Image weaponImage, powerImage;
        [SerializeField]
        private RectTransform weaponCooldownIndicator, powerCooldownIndicator;

        private float weaponCooldownMaxHeight, powerCooldownMaxHeight;

        private void Awake()
        {
            if(instance != null)
            {
                DestroyImmediate(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            weaponCooldownMaxHeight = weaponCooldownIndicator.rect.height;
            powerCooldownMaxHeight = powerCooldownIndicator.rect.height;
            SetWeaponFraction(0);
            SetPowerFraction(0);
        }

        public void SetWeaponImage(Sprite weaponSprite)
        {
            weaponImage.sprite = weaponSprite;
            weaponImage.color = Color.white;
        }

        public void SetWeaponFraction(float f)
        {
            weaponCooldownIndicator.sizeDelta = new Vector2(weaponCooldownIndicator.sizeDelta.x, weaponCooldownMaxHeight * f);
        }

        public void SetPowerImage(Sprite powerSprite)
        {
            powerImage.sprite = powerSprite;
            powerImage.color = Color.white;
        }

        public void SetPowerFraction(float f)
        {
            powerCooldownIndicator.sizeDelta = new Vector2(powerCooldownIndicator.sizeDelta.x, powerCooldownMaxHeight * f);
        }
    }
}
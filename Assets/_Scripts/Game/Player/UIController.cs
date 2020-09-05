using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using SantriptaSharma.Breakpoint.Polygons;
using System.Security.AccessControl;
using TMPro;
using UnityEngine.SceneManagement;

namespace SantriptaSharma.Breakpoint.Game
{
    public class UIController : MonoBehaviour
    {
        public static UIController instance;
        [System.NonSerialized]
        public int polygonCount = 0;

        [SerializeField]
        private Image weaponImage, powerImage;
        [SerializeField]
        private RectTransform weaponCooldownIndicator, powerCooldownIndicator;

        private float weaponCooldownMaxHeight, powerCooldownMaxHeight;
        private bool isOver;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            weaponCooldownMaxHeight = weaponCooldownIndicator.rect.height;
            powerCooldownMaxHeight = powerCooldownIndicator.rect.height;
            SetWeaponFraction(0);
            SetPowerFraction(0);
            isOver = false;
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
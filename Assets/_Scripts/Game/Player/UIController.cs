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
        public GameObject overPanel;
        public TextMeshProUGUI gameOverText;


        [SerializeField]
        private Image weaponImage, powerImage;
        [SerializeField]
        private RectTransform weaponCooldownIndicator, powerCooldownIndicator;

        private float weaponCooldownMaxHeight, powerCooldownMaxHeight;
        private bool isOver;

        private void Awake()
        {
            if(instance != null)
            {
                DestroyImmediate(gameObject);
                return;
            }

            instance = this;
        }

        public void AddPolygon()
        {
            polygonCount++;
        }

        public void RemovePolygon()
        {
            if (--polygonCount == 0)
            {
                Player.instance.SetControlFactorForSeconds(Vector2.zero, 12389);
                ShowWin();
            }

            Debug.Log($"polygons left: {polygonCount}");
        }

        private void ShowWin()
        {
            isOver = true;
            overPanel.SetActive(true);
            gameOverText.text = "You Win.";
        }

        public void ShowLoss(Entity e)
        {
            isOver = true;
            overPanel.SetActive(true);
            gameOverText.text = "You Lose.";
        }

        private void Start()
        {
            weaponCooldownMaxHeight = weaponCooldownIndicator.rect.height;
            powerCooldownMaxHeight = powerCooldownIndicator.rect.height;
            Player.instance.GetComponent<Entity>().onEntityDied.AddListener(ShowLoss);
            SetWeaponFraction(0);
            SetPowerFraction(0);
            isOver = false;
            overPanel.SetActive(false);
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }

            if(isOver && Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(0);
            }
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
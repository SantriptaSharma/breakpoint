using SantriptaSharma.Breakpoint.Items;
using SantriptaSharma.Breakpoint.Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SantriptaSharma.Breakpoint.Game
{
    public class Player : MonoBehaviour
    {
        public static Player instance = null;

        public float topSpeed, timeToTopSpeed;
        public GameObject targetorAxis;
        public float pickupDelay;
        public float timeToResetSpeed;
        public Vector2 voluntaryMoveDirection;
        [Space]
        public float intensityStart;
        public float intensityZero;

        private Vector2 controlFactor;
        private Entity entity;
        private Vector3 targetDirection;
        private float accelerationMagnitude;
        private Rigidbody2D rb;
        private new SpriteRenderer renderer;
        private PlayerCamera cam;
        private float lastPickup;
        private Weapon weapon;
        private Item power;
        private bool constrainVelocity;
        private float currentIntensity, intensityPerDamage;
        
        private UIController ui;

        public Vector2 aimDirection { get { return targetorAxis.transform.right; } }
        public Vector2 moveDirection { get { return rb.velocity.normalized; } }

        private void Awake()
        {
            if (instance != null)
            {
                DestroyImmediate(gameObject);
                return;
            }

            instance = this;
        }

        void Start()
        {
            voluntaryMoveDirection = Vector2.right;
            lastPickup = -pickupDelay;
            power = weapon = null;
            cam = PlayerCamera.instance;
            rb = GetComponent<Rigidbody2D>();
            rb.mass = 1;
            targetDirection = new Vector3();
            accelerationMagnitude = Mathf.Abs(topSpeed / timeToTopSpeed);
            constrainVelocity = true;
            controlFactor = new Vector2(1, 1);

            renderer = GetComponent<SpriteRenderer>();

            entity = GetComponent<Entity>();
            entity.onEntityDied.AddListener(Die);
            entity.onTakeDamage.AddListener(Damaged);

            currentIntensity = intensityStart;
            intensityPerDamage = (intensityStart - intensityZero) / entity.maxHealth;
            float factor = currentIntensity;
            renderer.sharedMaterial.SetColor("_Color", new Color(0x16 * factor, 0xdf * factor, 0x05 * factor));

            ui = UIController.instance;
        }

        public void Die(Entity e)
        {
        }

        public void Damaged(float damage, float health)
        {
            if (health < 0) return;
            currentIntensity -= intensityPerDamage * damage;
            float factor = currentIntensity;
            renderer.material.SetColor("_Color", new Color(0x16 * factor, 0xdf * factor, 0x05 * factor));
        }

        void Update()
        {
            targetDirection.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
            targetDirection.Normalize();
            if(targetDirection.magnitude != 0)
            {
                voluntaryMoveDirection = targetDirection;
            }
            
            rb.AddForce(targetDirection * accelerationMagnitude * ((Vector2.up * controlFactor.y) + (Vector2.right * controlFactor.x)));

            if (rb.velocity.sqrMagnitude > topSpeed * topSpeed && constrainVelocity)
            {
                rb.velocity = Vector3.MoveTowards(rb.velocity,rb.velocity.normalized * topSpeed, (topSpeed/timeToResetSpeed)*Time.deltaTime);
            }

            ManageWeapon();
            ManagePower();

            Vector3 mouseDirection = (cam.MousePositionFromPlayerPOV() - transform.position).normalized;
            targetorAxis.transform.right = mouseDirection;
        }

        void ManageWeapon()
        {
            if (weapon == null) return;

            ui.SetWeaponFraction(weapon.GetFractionalTimeRemaining());

            if(Input.GetMouseButton(0))
            {
                weapon.Use();
            }
        }

        void ManagePower()
        {
            if (power == null) return;

            ui.SetPowerFraction(power.GetFractionalTimeRemaining());

            if (Input.GetMouseButton(1))
            {
                power.Use();
            }
        }

        private void EquipItem(GameObject obj)
        {
            Item item = obj.GetComponent<Item>();
            if (!item.isDropped)
                return;

            if(item.type == ItemType.Power)
            {
                if(power != null) power.Drop();
                ui.SetPowerImage(item.itemSprite);
                power = item;
            }

            if(item.type == ItemType.Weapon)
            {
                if(weapon != null) weapon.Drop();
                ui.SetWeaponImage(item.itemSprite);
                weapon = (Weapon)item;
            }

            item.Equip();

            lastPickup = Time.time;
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if(collision.CompareTag("Item"))
            {
                if(lastPickup + pickupDelay <= Time.time) EquipItem(collision.gameObject);
            }
        }

        private IEnumerator StopLimitingVelocityFor(float seconds)
        {
            constrainVelocity = false;
            yield return new WaitForSeconds(seconds);
            constrainVelocity = true;
        }

        private IEnumerator SetControlFactorFor(Vector2 newControlFactor, float seconds)
        {
            controlFactor = newControlFactor;
            yield return new WaitForSeconds(seconds);
            controlFactor = new Vector2(1, 1);
        }

        public void StopLimitingVelocityForSeconds(float seconds)
        {
            StartCoroutine(StopLimitingVelocityFor(seconds));
        }

        public void SetControlFactorForSeconds(Vector2 newControlFactor, float seconds)
        {
            StartCoroutine(SetControlFactorFor(newControlFactor, seconds));
        }

        public void AddForce(Vector2 force)
        {
            rb.AddForce(force);
        }
    }
}
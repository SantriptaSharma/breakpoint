using SantriptaSharma.Breakpoint.Items;
using SantriptaSharma.Breakpoint.Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace SantriptaSharma.Breakpoint.Game
{
    public class Player : MonoBehaviour
    {
        public static Player instance = null;

        public float topSpeed, timeToTopSpeed;
        public GameObject targetorAxis;
        public float pickupDelay;

        private Entity entity;
        private Vector3 targetDirection;
        private float accelerationMagnitude;
        private Rigidbody2D rb;
        private PlayerCamera cam;
        private float lastPickup;
        private Weapon weapon;
        private Item power;
        private bool constrainVelocity;

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
            DontDestroyOnLoad(gameObject);
        }

        void Start()
        {
            lastPickup = -pickupDelay;
            power = weapon = null;
            cam = PlayerCamera.instance;
            rb = GetComponent<Rigidbody2D>();
            rb.mass = 1;
            targetDirection = new Vector3();
            accelerationMagnitude = Mathf.Abs(topSpeed / timeToTopSpeed);
            constrainVelocity = true;

            entity = GetComponent<Entity>();
            entity.onEntityDied.AddListener(Die);
            entity.onTakeDamage.AddListener(Damaged);

            ui = UIController.instance;
        }

        public void Die(Entity e)
        {
            Debug.Log("I am literally dead");
        }

        public void Damaged(float damage, float health)
        {
            Debug.Log($"Took {damage} damage, still have {health} remaining");
        }

        void Update()
        {
            targetDirection.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
            targetDirection.Normalize();

            rb.AddForce(targetDirection * accelerationMagnitude);
            
            if(rb.velocity.sqrMagnitude > topSpeed * topSpeed && constrainVelocity)
            {
                rb.velocity = Vector3.Lerp(rb.velocity,rb.velocity.normalized * topSpeed, 0.3f);
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

        public void StopLimitingVelocity(float seconds)
        {
            StartCoroutine(StopLimitingVelocityFor(seconds));
        }

        public void AddForce(Vector2 force)
        {
            rb.AddForce(force);
        }
    }
}
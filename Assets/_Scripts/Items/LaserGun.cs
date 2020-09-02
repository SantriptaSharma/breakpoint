using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SantriptaSharma.Breakpoint.Game;
using UnityEngine.XR;
using UnityEditor;
using UnityEngine.UI;

namespace SantriptaSharma.Breakpoint.Items
{
    [RequireComponent(typeof(LineRenderer))]
    class LaserGun : Weapon
    {
        public float maxLength, startDistance, maskDistance, damagePerTick, knockbackPerTick, tickInterval, secondsDecayPerSecond, playerKnockbackPerSecond;
        public DamageTag damageTag;
        public LayerMask obstacles;
        public GameObject targetPoint;
        public LineRenderer laserRenderer;
        public ParticleSystem laserParticles;

        private GameObject damageObject;
        private Damage myDamage;
        private Entity lastTarget;
        private bool isUsing;
        private float trainedTime;
        private float emissionRate;

        public override void Use()
        {
            isUsing = true;
            laserRenderer.positionCount = 2;
            Vector3 aimDir = new Vector3(player.aimDirection.x, player.aimDirection.y);
            Vector3 startPosition = transform.position + aimDir * startDistance;
            laserRenderer.SetPosition(0, startPosition);
            RaycastHit2D hit = Physics2D.Raycast(startPosition, aimDir, maxLength, obstacles.value);
            player.AddForce(-aimDir * playerKnockbackPerSecond * Time.deltaTime);
            float distance = maxLength;

            laserParticles.transform.rotation = Quaternion.LookRotation(Vector3.forward, aimDir);

            if(hit.collider != null)
            {
                distance = hit.distance + maskDistance;
                player.AddForce(-aimDir * playerKnockbackPerSecond * Time.deltaTime * Mathf.Max(0,Mathf.Pow(1.1f, maxLength - hit.distance * 1.75f)));
                Entity entity = hit.collider.GetComponent<Entity>();
                
                // TODO: Replace with coroutines and refactor
                if (entity != null)
                {
                    if (lastTarget == entity)
                    {
                        trainedTime += Time.deltaTime;
                        int numberOfHits = Mathf.FloorToInt(trainedTime / tickInterval);
                        trainedTime = trainedTime % tickInterval;
                        for (int i = 0; i < numberOfHits; i++)
                        {
                            entity.ProcessHit(myDamage);
                        }
                    }
                    else if (entity.CheckValidity(myDamage))
                    {
                        lastTarget = entity;
                        trainedTime = 0;
                    }
                }
                else
                {
                    trainedTime = Mathf.MoveTowards(trainedTime, 0, secondsDecayPerSecond * Time.deltaTime);
                }
            }
            else
            {
                trainedTime = Mathf.MoveTowards(trainedTime, 0, secondsDecayPerSecond * Time.deltaTime);
            }

            laserParticles.transform.position = transform.position + aimDir * (distance / 2);
            var shape = laserParticles.shape;
            shape.radius = distance / 2;

            var emission = laserParticles.emission;
            emission.rateOverTime = emissionRate;
            laserRenderer.SetPosition(1, startPosition + aimDir * distance);
        }

        protected override void Start()
        {
            base.Start();
            laserRenderer.useWorldSpace = true;
            damageObject = new GameObject("laserdamage");
            damageObject.transform.position = transform.position;
            damageObject.transform.parent = transform;
            myDamage = damageObject.AddComponent<Damage>();
            myDamage.useLastPositionKnocking = false;
            myDamage.damage = damagePerTick;
            myDamage.knockbackAmount = knockbackPerTick;
            emissionRate = laserParticles.emission.rateOverTime.constant;
            laserParticles.Play();
        }

        protected override void Update()
        {
            base.Update();

            if (!isUsing)
            {
                laserRenderer.positionCount = 0;
                trainedTime = Mathf.MoveTowards(trainedTime, 0, secondsDecayPerSecond * Time.deltaTime);
                var emission = laserParticles.emission;
                emission.rateOverTime = 0;
            }

            if(Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Break();
            }

            isUsing = false;
        }

        public override void Equip()
        {
            base.Equip();
            Debug.Log("Equipped Laser");
        }

        public override void Drop()
        {
            base.Drop();
            Debug.Log("Dropped Laser");
        }
    }
}
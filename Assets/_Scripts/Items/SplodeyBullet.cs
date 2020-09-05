using SantriptaSharma.Breakpoint.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace SantriptaSharma.Breakpoint.Items
{
    class SplodeyBullet : MonoBehaviour
    {
        public GameObject bullet;
        public SpriteRenderer sprite;
        public int count;
        public float frequencyIncreasePerSecond;
        public float startingFrequency;
        public float lifeTime;
        public float size;
        public float maxSize;
        public float shardVelocity;
        public float shardLifeTime;

        private float sizeDifference;
        private float startTime;
        private float degreesPerBullet;

        private void Start()
        {
            sprite.transform.localScale = new Vector3(size, size, 1);
            degreesPerBullet = 360 / count;
            startTime = Time.time;
            sizeDifference = maxSize - size;
        }

        private void Update()
        {
            if (PauseController.isPaused) return;

            float elapsedTime = Time.time - startTime;
            float currentFrequency = startingFrequency + frequencyIncreasePerSecond * elapsedTime;
            float currentFactor = (Mathf.Sin(2 * Mathf.PI * elapsedTime * currentFrequency) + 1)/2;
            Assert.IsTrue(currentFactor >= 0 && currentFactor <= 1, "Current size factor is not bounded in [0,1]");
            float currentSize = size + currentFactor * sizeDifference;
            sprite.transform.localScale = new Vector3(currentSize, currentSize, 1);
            if (elapsedTime >= lifeTime)
                Burst();
        }

        public void Burst()
        {
            for(int i = 0; i < count; i++)
            {
                Quaternion rot = Quaternion.AngleAxis(degreesPerBullet * i, Vector3.back);
                Vector3 dir = (rot * Vector3.up).normalized;
                GameObject shard = Instantiate(bullet, transform.position + dir * 0.3f, rot, ProjectileHolder.instance);
                shard.GetComponent<Rigidbody2D>().velocity = dir * shardVelocity;
                Destroy(shard.gameObject, shardLifeTime);
            }

            Destroy(gameObject);
        }
    }
}
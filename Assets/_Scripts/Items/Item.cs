using SantriptaSharma.Breakpoint.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SantriptaSharma.Breakpoint.Items
{
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public abstract class Item : MonoBehaviour
    {
        public Sprite itemSprite;
        public ItemType type;
        public bool isDropped { get { return state == ItemState.Dropped; } }
        public float cooldown;
            
        protected float currentTime;
        protected new SpriteRenderer renderer;
        protected new BoxCollider2D collider;
        protected ItemState state = ItemState.Dropped;

        public abstract void Use();

        protected virtual void Start()
        {
            currentTime = 0;

            collider = GetComponent<BoxCollider2D>();
            collider.isTrigger = true;

            renderer = GetComponent<SpriteRenderer>();
            renderer.sprite = itemSprite;
            renderer.sortingOrder = 2;
        }

        protected virtual void Update()
        {
            if(state == ItemState.Dropped)
            {
                currentTime = cooldown;
                return;
            }

            currentTime -= Time.deltaTime;
        }

        public virtual void Drop()
        {
            state = ItemState.Dropped;
            renderer.enabled = true;
            transform.position = Player.instance.transform.position;
            transform.parent = null;
        }

        public virtual void Equip()
        {
            state = ItemState.Equipped;
            renderer.enabled = false;
            transform.position = Player.instance.transform.position;
            transform.parent = Player.instance.transform;
        }

        //Returns a fraction in [0,1] to show how much time is remaining before this can be used again. Used for rendering to the UI.
        public float GetFractionalTimeRemaining()
        {
            float cTime = Mathf.Clamp(currentTime, 0, cooldown);
            return (cTime / cooldown);
        }
    }

    public enum ItemType
    { 
        Weapon, Power
    }

    public enum ItemState
    { 
        Equipped, Dropped
    }
}
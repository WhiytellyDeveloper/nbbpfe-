﻿using nbppfe.PrefabSystem;
using PixelInternalAPI.Extensions;
using PixelInternalAPI.Classes;
using UnityEngine;
using System.Collections;
using nbppfe.FundamentalSystems;
using nbppfe.Extensions;
using nbppfe.CustomContent.CustomItems.ItemTypes;
using nbppfe.FundamentalsManager;

namespace nbppfe.CustomContent.CustomItems
{
    public class ITM_Soda : Item, DietItemVariation, IItemPrefab
    {
        public void Setup()
        {
            var holder = ObjectCreationExtensions.CreateSpriteBillboard(AssetsLoader.CreateSprite("SodaSprite", Paths.GetPath(PathsEnum.Items, "Soda"), 12)).AddSpriteHolder(out var renderer, 0, LayerStorage.ignoreRaycast);
            spr = holder.renderers[0].GetComponent<SpriteRenderer>();
            holder.transform.SetParent(transform);

            entity = gameObject.CreateEntity(0.1f, 0.1f, spr.transform).SetEntityCollisionLayerMask(0);
            gameObject.layer = LayerStorage.standardEntities;
            splashSound = AssetsLoader.CreateSound("Soda_open", Paths.GetPath(PathsEnum.Items, "Soda"), "", SoundType.Effect, Color.white, 1);
            trashSound = AssetsLoader.CreateSound("Soda_end", Paths.GetPath(PathsEnum.Items, "Soda"), "", SoundType.Effect, Color.white, 1);

            if (diet)
            {
                spr.sprite = AssetsLoader.CreateSprite("DietSodaSprite", Paths.GetPath(PathsEnum.Items, "DietSoda"), 12);
                speedModifier = new(Vector3.zero, 1.45f);
                lenghtMaxCount = 3;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        public override bool Use(PlayerManager pm)
        {
            ITM_Soda[] sodas = FindObjectsOfType<ITM_Soda>();
            if (sodas.Length > lenghtMaxCount)
            {
                Destroy(gameObject);
                return false;
            }

            this.pm = pm;
            cooldown.endAction = End;
            transform.position = pm.transform.position;
            entity.Initialize(pm.ec, transform.position);
            pm.Am.moveMods.Add(speedModifier);
            Singleton<CoreGameManager>.Instance.audMan.PlaySingle(splashSound);
            StartCoroutine(Fade(1.5f, 1f));
            return true;
        }

        private void Update()
        {
            cooldown.UpdateCooldown(Singleton<BaseGameManager>.Instance.Ec.EnvironmentTimeScale);
            entity.UpdateInternalMovement(pm.GetPlayerCamera().transform.forward * 30 * pm.ec.EnvironmentTimeScale);
        }

        private IEnumerator Fade(float speed, float duration)
        {
            float elapsedTime = 0f;
            Color color = spr.color;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                color.a = Mathf.Lerp(Color.white.a, 0, elapsedTime * speed / duration);
                spr.color = color;
                yield return null;
            }
        }

        public void End()
        {
            Singleton<CoreGameManager>.Instance.audMan.PlaySingle(trashSound);
            pm.Am.moveMods.Remove(speedModifier);
            Destroy(gameObject);
        }

        public Entity entity;
        public SpriteRenderer spr;
        public SoundObject splashSound;
        public SoundObject trashSound;
        public MovementModifier speedModifier = new(Vector3.zero, 3f);
        public int lenghtMaxCount = 1;
        public Cooldown cooldown = new(25, 0);

        public bool diet { get; set; }
    }
}
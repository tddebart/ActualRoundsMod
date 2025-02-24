﻿using System;
using System.Linq;
using BossSlothsCards.Extensions;
using BossSlothsCards.MonoBehaviours;
using BossSlothsCards.TempEffects;
using CardChoiceSpawnUniqueCardPatch.CustomCategories;
using UnboundLib;
using UnboundLib.Cards;
using UnityEngine;
using UnityEngine.Events;
using UnboundLib.Utils;

namespace BossSlothsCards.Cards
{
    public class Pong : CustomCard
    {

        protected override string GetTitle()
        {
            return "Pong";
        }

        protected override string GetDescription()
        {
            return "Max ammo will be locked at 1, Projectiles shot per bullet will be locked at pong projectiles and if you fire a bullet your last bullet will be destroyed";
        }
        
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            if (player.GetComponentsInChildren<ReloadTigger>().All(re => re.name != "Pong_A"))
            {
                var reloadTrigger = new GameObject("Pong_A");
                var trigger = reloadTrigger.AddComponent<ReloadTigger>();
                trigger.outOfAmmoEvent = new UnityEvent();
                trigger.outOfAmmoEvent.AddListener(() =>
                {
                });
                trigger.reloadDoneEvent = new UnityEvent();
                trigger.reloadDoneEvent.AddListener(() =>
                {
                    gunAmmo.GetAdditionalData().destroyAllPongOnNextShot = true;
                });
                
                reloadTrigger.transform.parent = player.transform;
            }

            var pong = player.gameObject.GetOrAddComponent<Pong_Mono>();
            pong.maxAmmo++;
            gun.reflects = int.MaxValue-150;
            gun.projectileColor = Color.yellow;
        }
        
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.allowMultiple = true;
            
            statModifiers.automaticReload = false;
            
            var mayhem = (GameObject)Resources.Load("0 cards/Mayhem");
            var A_ScreenEdge = mayhem.GetComponent<Gun>().objectsToSpawn[0];

            gun.spread = 0.1f;
            gun.gravity = 0.85f;
            gun.damage = 0.70f;

            var obj = new GameObject("pong", typeof(Sneeze_Mono), typeof(RemoveOnRoundEnd), typeof(Pong_Bullet));
            DontDestroyOnLoad(obj);
            
            var obj2 = new GameObject("pixel_ball");
            DontDestroyOnLoad(obj2);
            obj2.transform.position = new Vector3(1000, 0, 0);
            obj2.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
            var rendrer = obj2.AddComponent<SpriteRenderer>();
            rendrer.sprite = BossSlothCards.EffectAsset.LoadAsset<Sprite>("pixel_ball");
            obj2.AddComponent<EffectBulletRotate>();

            gun.objectsToSpawn = new[]
            {
                new ObjectsToSpawn()
                {
                    AddToProjectile = obj
                },
                new ObjectsToSpawn
                {
                    AddToProjectile = obj2
                },
                A_ScreenEdge
            };
        }

        protected override CardInfoStat[] GetStats()
        {
            return new[]
            {
                new CardInfoStat
                {
                    amount = "Infinite",
                    positive = true,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned,
                    stat = "Bounces"
                },
                new CardInfoStat
                {
                    amount = "+1",
                    positive = true,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned,
                    stat = "Max pong projectiles"
                },
                new CardInfoStat
                {
                    amount = "-15%",
                    positive = true,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned,
                    stat = "Bullet gravity"
                },
                new CardInfoStat
                {
                    amount = "-30%",
                    positive = false,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned,
                    stat = "Damage"
                }
            };
        }

        protected override CardInfo.Rarity GetRarity()
        {
            return CardInfo.Rarity.Rare;
        }

        protected override GameObject GetCardArt()
        {
            return BossSlothCards.ArtAsset.LoadAsset<GameObject>("C_Pong");
        }

        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.DestructiveRed;
        }

        public override string GetModName()
        {
            return "BSC";
        }

        public override void OnRemoveCard()
        {
        }
        
    }
}
﻿using BossSlothsCards.Extensions;
using UnboundLib.Cards;
using UnityEngine;

namespace BossSlothsCards.Cards
{
    public class CompactedShot : CustomCard
    {
        public AssetBundle Asset;
        
        protected override string GetTitle()
        {
            return "Compacted shot";
        }

        protected override string GetDescription()
        {
            return "Put it all in a little ball";
        }
        
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            characterStats.GetAdditionalData().recoil += 1;
            gun.spread -= 0.10f;
        }
        
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.allowMultiple = true;
            
            gun.numberOfProjectiles = 3;
            gun.reloadTimeAdd = 0.25f;
            gun.damage = 0.85f;

        }

        protected override CardInfoStat[] GetStats()
        {
            return new[]
            {
                new CardInfoStat
                {
                    amount = "+3",
                    positive = true,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned,
                    stat = "Projectiles"
                },
                new CardInfoStat
                {
                    amount = "smaller",
                    positive = true,
                    simepleAmount = CardInfoStat.SimpleAmount.smaller,
                    stat = "Spread"
                },
                new CardInfoStat
                {
                    amount = "+0.25s",
                    positive = true,
                    simepleAmount = CardInfoStat.SimpleAmount.smaller,
                    stat = "Reload time"
                },
                new CardInfoStat
                {
                    amount = "+100",
                    positive = false,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned,
                    stat = "Recoil"
                },
                new CardInfoStat
                {
                    amount = "-15%",
                    positive = false,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned,
                    stat = "Damage"
                }
            };
        }

        protected override CardInfo.Rarity GetRarity()
        {
            return CardInfo.Rarity.Uncommon;
        }

        protected override GameObject GetCardArt()
        {
            return null;
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
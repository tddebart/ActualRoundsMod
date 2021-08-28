﻿using UnboundLib.Cards;
using UnityEngine;

namespace BossSlothsCards.Cards
{
    public class SluggishRounds : CustomCard
    {

        protected override string GetTitle()
        {
            return "Sluggish rounds";
        }

        protected override string GetDescription()
        {
            return "";
        }
        
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {

        }
        
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.allowMultiple = false;

            gun.projectileSpeed = 0.05f;
            gun.reflects = 15;
            
            var explosiveBullet = (GameObject)Resources.Load("0 cards/Mayhem");
            var A_ScreenEdge = explosiveBullet.GetComponent<Gun>().objectsToSpawn[0];
            
            gun.objectsToSpawn = new[]
            {
                A_ScreenEdge
            };

        }

        protected override CardInfoStat[] GetStats()
        {
            return new[]
            {
                new CardInfoStat
                {
                    amount = "-95%",
                    positive = false,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned,
                    stat = "Bullet speed"
                },
                new CardInfoStat
                {
                    amount = "+15",
                    positive = false,
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned,
                    stat = "Bounces"
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
            return CardThemeColor.CardThemeColorType.ColdBlue;
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
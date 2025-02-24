﻿using BossSlothsCards.Utils.Text;
using CardChoiceSpawnUniqueCardPatch.CustomCategories;
using ModdingUtils.Extensions;
using ModdingUtils.Utils;
using UnboundLib;
using UnboundLib.Cards;
using UnityEngine;


namespace BossSlothsCards.Cards
{
    public class NoThanks : CustomCard
    {
        public AssetBundle Asset;

        public string cardRemovedName;

        public CardInfo.Rarity rarity;
        
        protected override string GetTitle()
        {
            return "No thanks";
        }

        protected override string GetDescription()
        {
            return "Replace your most recent card with 2 random cards of lower rarity if possible";
        }
        
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            BossSlothCards.instance.ExecuteAfterSeconds(0.1f, () =>
            {
                DoNoThanksThings(player,gun,gunAmmo,data,health,gravity,block,characterStats);
            });
        }

        private void DoNoThanksThings(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            if (player.data.currentCards.Count == 0)
            {
                return;
            }
            // get amount in currentCards
            var count = player.data.currentCards.Count - 1;
            var tries = 0;
            while (!(tries > 50))
            {
                tries++;
                if (player.data.currentCards.Count <= -1 || count <= -1)
                {
                    return;
                }
                
                // make sure the card is not
                if (!ModdingUtils.Utils.Cards.instance.CardIsNotBlacklisted(player.data.currentCards[count],
                    new[]
                    {
                        CustomCardCategories.instance.CardCategory("CardManipulation"),
                        CustomCardCategories.instance.CardCategory("NoRemove")
                    }))
                {
                    count--;
                    continue;
                }
                
                
                cardRemovedName = player.data.currentCards[count].cardName;
                rarity = player.data.currentCards[count].rarity;
                
                var cardToRemove = player.data.currentCards[count];
                var randomCard = ModdingUtils.Utils.Cards.instance.NORARITY_GetRandomCardWithCondition(player, gun, gunAmmo, data, health, gravity, block, characterStats, condition);
                var randomCard2 = ModdingUtils.Utils.Cards.instance.NORARITY_GetRandomCardWithCondition(player, gun, gunAmmo, data, health, gravity, block, characterStats, condition);

                ModdingUtils.Utils.Cards.instance.RemoveCardFromPlayer(player, cardToRemove, ModdingUtils.Utils.Cards.SelectionType.Newest);
                player.ExecuteAfterSeconds(0.2f, () =>
                {
                    ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, randomCard, false, "", 0, 0, true);
                    ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, randomCard2, false, "", 0, 0, true);
                    CardBarUtils.instance.ShowAtEndOfPhase(player, randomCard);
                    CardBarUtils.instance.ShowAtEndOfPhase(player, randomCard2);
                });
                break;
            }
        }

        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers)
        {
            cardInfo.allowMultiple = true;
            cardInfo.GetAdditionalData().canBeReassigned = false;
            cardInfo.categories = new[] { CustomCardCategories.instance.CardCategory("CardManipulation") };
            
            transform.Find("CardBase(Clone)(Clone)/Canvas/Front/Grid/EffectText")?.gameObject.GetOrAddComponent<RainbowText>();
        }

        protected override CardInfoStat[] GetStats()
        {
            return null;
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
            return CardThemeColor.CardThemeColorType.MagicPink;
        }


        public override void OnRemoveCard()
        {
        }
        
        public override string GetModName()
        {
            return "BSC";
        }
        
        //From PCE
        public bool condition(CardInfo card, Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            // do not allow duplicates of cards with allowMultiple == false
            // card rarity must be as desired
            // card cannot be another Gamble / Jackpot card
            // card cannot be from a blacklisted category of any other card

            var lowerRarity = rarity == CardInfo.Rarity.Rare ? CardInfo.Rarity.Uncommon : CardInfo.Rarity.Common;
            
            return card.cardName != cardRemovedName && card.cardName != "No thanks" && card.rarity == lowerRarity;
        }

    }
}

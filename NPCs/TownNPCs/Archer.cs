using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Steamworks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Zaranka.Utils.NPCUtils;
using Zaranka.Utils.ArcherQuestInfo;


namespace Zaranka.NPCs.TownNPCs
{

    [AutoloadHead]
    public class Archer : ModNPC
    {
        private ArcherQuestManager currentQuest = new ArcherQuestManager()
        {
            FinishedQuestCount = 0,
            IsCompletable = false,
            QuestStarted = false,
        };

        public override void SetDefaults()
        {
            NPC.townNPC = true;
            NPC.friendly = true;
            NPC.width = 20;
            NPC.height = 20;
            NPC.aiStyle = 7;
            NPC.defense = 35;
            NPC.lifeMax = 3000;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.5f;
            Main.npcFrameCount[NPC.type] = 25;
            NPCID.Sets.ExtraFramesCount[NPC.type] = 0;
            NPCID.Sets.AttackFrameCount[NPC.type] = 1;
            NPCID.Sets.DangerDetectRange[NPC.type] = 500;
            NPCID.Sets.AttackType[NPC.type] = 1;
            NPCID.Sets.AttackTime[NPC.type] = 30;
            NPCID.Sets.AttackAverageChance[NPC.type] = 10;
            NPCID.Sets.HatOffsetY[NPC.type] = 4;
            AnimationType = 22;
        }

        public override bool CanTownNPCSpawn(int numTownNPCs)
        {
            return true;
        }

        public override List<string> SetNPCNameList()
        {
            // TODO:
            return new List<string>()
            {
                "Orion",
                "Apollo",
                "Robin",
                "Nogn",
                "Sylick"
            };
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = "Shop";
            if (!currentQuest.QuestStarted)
            {
                button2 = "Quest";
            }
            else
            {
                button2 = "Finish Quest";
            }
        }

        public override void OnChatButtonClicked(bool firstButton, ref string shopName)
        {
            if (firstButton)
            {
                shopName = "Shop";
            }
            else
            {
                // If not started -> start quest
                if (!currentQuest.QuestStarted)
                {
                    string questDescription = currentQuest.GetQuestDescription(currentQuest.FinishedQuestCount);
                    Main.npcChatText = questDescription;
                    currentQuest.QuestStarted = true;
                }// If started -. check if you can finish
                else
                {
                    {
                        if (!currentQuest.CheckIfFinishable() && !currentQuest.IsCompletable)
                        {
                            Main.npcChatText = "You don't have enough items";
                        }
                        else
                        {
                            Main.npcChatText = GetChat();
                        }
                    }
                }
            }
        }

        public override void AddShops()
        {
            NPCShop shop = new(Type);
            // TODO: Add more items, quest items after quests are implemented, progression items.


            shop.AddWithCustomValue(ItemID.Minishark, Item.buyPrice(copper: 1))
                .AddWithCustomValue(ItemID.MusketBall, Item.buyPrice(copper: 1))
                .Register();
        }

        public override string GetChat()
        {
            NPC.FindFirstNPC(ModContent.NPCType<Archer>());
            switch (Main.rand.Next(3))
            {
                // TODO: Refine dialog
                case 0:
                    return "If you're a gunslinger, you can see yourself out.";
                case 1:
                    return "This is the second case";
                case 2:
                    return "This is the third case";
                default:
                    return "This is the default case.";
            }
        }

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 10 * currentQuest.FinishedQuestCount + 1;
            knockback = 2f;
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 5;
            randExtraCooldown = 10;
        }

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = ProjectileID.WoodenArrowFriendly;
            attackDelay = 1;
        }

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 7f;
        }

        public override void OnKill()
        {
            Item.NewItem(NPC.GetSource_Death(), NPC.getRect(), ItemID.GoldBow, 1, false, 0, false, false);
        }
    }
}


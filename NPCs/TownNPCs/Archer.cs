using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Steamworks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Zaranka.Utils.NPCUtils;
using Zaranka.Utils.QuestManager;
using Terraria.ModLoader.IO;
using Zaranka.Items.Weapons.Archer.PreHardmode;

namespace Zaranka.NPCs.TownNPCs
{

    [AutoloadHead]
    public class Archer : ModNPC
    {
        private QuestManager currentQuest = new QuestManager();

        public static List<string> QuestName { get; } = new List<string>
    {
        "Arrow Fletching",
        "Gel Finder",
        "Antlion Mandible Gatherer",
        "Eyes everywhere",
    };

        public static List<string> QuestDescription { get; } = new List<string>
        {
            "I need some arrows to defend this town! Bring me 20 of them so I can feel safe!",
            "Find slimes and kill them! Collect their gel afterwards and bring it to me! I need 15 gel for some torches",
            "Kill some desert pesks and bring their loot back to me. Gather 5 Antlion Mandibles.",
            "I feel like there are eyes everywhere.. Please kill some Demon Eyes and bring their lenses. I need 6 of them"
        };

        public int[] QuestItemID = {
            ItemID.WoodenArrow,
            ItemID.Gel,
            ItemID.AntlionMandible,
            ItemID.Lens,

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
            string questName = currentQuest.GetQuestDescription(questName: QuestName[currentQuest.FinishedQuestCount],
                questDescription: QuestDescription[currentQuest.FinishedQuestCount]);
            if (firstButton)
            {
                shopName = "Shop";
            }
            else
            {
                // If not started -> start quest
                if (!currentQuest.QuestStarted)
                {

                    string questDescription = currentQuest.GetQuestDescription(questName: QuestName[currentQuest.FinishedQuestCount],
                questDescription: QuestDescription[currentQuest.FinishedQuestCount]);
                    Main.npcChatText = questDescription;
                    currentQuest.QuestStarted = true;
                }
                else
                {
                    {


                        if (!currentQuest.CheckIfFinishable(itemID: QuestItemID[currentQuest.FinishedQuestCount]) && !currentQuest.IsCompletable)
                        {
                            Main.npcChatText = $"You don't have enough items for {questName}";
                        }
                        else
                        {
                            Main.npcChatText = "Congratulations! You have completed the quest";
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
                .Add<SylBow>()
                .Add<Medonski>()
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
                    return "I hope you brought something usefull";
                case 2:
                    return "Can I sniff your feet?";
                default:
                    return "Feeling cute, might talk with the nurse.";
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

        public override void LoadData(TagCompound tag)
        {
            currentQuest.FinishedQuestCount = tag.GetInt("finishedArcherQuestCount");
        }

        public override void SaveData(TagCompound tag)
        {
            tag["finishedArcherQuestCount"] = currentQuest.FinishedQuestCount;
        }

    }
}


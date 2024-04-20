using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Steamworks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Zaranka.Utils.NPCUtils;


namespace Zaranka.NPCs
{
    public class QuestData
    {
        public string QuestName { get; set; }
        public bool IsCompleted { get; set; }
        // Add other properties as needed, such as quest objectives and rewards
    }

    [AutoloadHead]
    public class Archer : ModNPC
    {
        private QuestData currentQuest;

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
            for(var i = 0;  i < 255; i++)
            {
                Player player = Main.player[i];
        if (player.active)
                {
                    return true;
                }
            }
            return false;
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
            button2 = "Quest";
        }

        public override void OnChatButtonClicked(bool firstButton, ref string shopName)
        {
            if(firstButton)
            {
                shopName = "Shop";
            }
            if(!firstButton)
            {
                if (currentQuest == null)
                {
                    // Assign a new quest to the player
                    currentQuest = new QuestData
                    {
                        QuestName = "Gel Collection",
                        IsCompleted = false
                        // Add other quest data such as objectives and rewards
                    };
                    Main.npcChatText = $"You have been assigned the quest: {currentQuest.QuestName}. Collect 10 gel and return to me.";
                }
                else
                {
                    // Inform the player that they already have an active quest
                    Main.npcChatText = "You already have an active quest.";
                }

            }
        }

        public override void AddShops()
        {
            NPCShop shop = new(Type);
            // TODO: Add more items, quest items after quests are implemented, progression items.


            shop.AddWithCustomValue(ItemID.Minishark, Item.buyPrice(copper: 1))
                .AddWithCustomValue(ItemID.MusketBall, Item.buyPrice(copper:1))
                .Register();
        }

        public override string GetChat()
        {
            NPC.FindFirstNPC(ModContent.NPCType<Archer>());
            switch (Main.rand.Next(3))
            {
                // TODO:
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
            damage = 15;
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

        public override void AI()
        {
            // Check if the current quest is active and not completed
            if (currentQuest != null && !currentQuest.IsCompleted)
            {
                // Count the number of gel in the player's inventory
                int gelCount = 0;
                for (int i = 0; i < Main.player[Main.myPlayer].inventory.Length; i++)
                {
                    if (Main.player[Main.myPlayer].inventory[i].type == ItemID.Gel)
                    {
                        gelCount += Main.player[Main.myPlayer].inventory[i].stack;
                    }
                }

                // Check if the player has collected enough items to complete the quest
                if (gelCount >= 10)
                {
                    currentQuest.IsCompleted = true;

                    // Remove items from the player's inventory (you may want to adjust this logic based on your game's requirements)
                    for (int i = 0; i < Main.player[Main.myPlayer].inventory.Length; i++)
                    {
                        if (Main.player[Main.myPlayer].inventory[i].type == ItemID.Gel)
                        {
                            int stackToRemove = Math.Min(Main.player[Main.myPlayer].inventory[i].stack, 10);
                            Main.player[Main.myPlayer].inventory[i].stack -= stackToRemove;
                            if (Main.player[Main.myPlayer].inventory[i].stack <= 0)
                            {
                                Main.player[Main.myPlayer].inventory[i].SetDefaults(0);
                            }
                            break; // Exit the loop after removing the required gel
                        }
                    }
                    // Provide the player with quest rewards
                    // You can implement this part based on your game's logic
                    // For example, you can give the player items, currency, or other rewards
                    Main.NewText($"Congratulations! You have completed the quest: {currentQuest.QuestName}");
                }
            }
        }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Achievements;
using Terraria.ID;
using Terraria.ModLoader.IO;

namespace Zaranka.Utils.ArcherQuestInfo
{
    public class ArcherQuestManager
    {
        public bool IsCompletable { get; set; }
        public bool QuestStarted { get; set; }

        public int FinishedQuestCount { get; set; }
        public static List<string> QuestName { get; } = new List<string>
    {
        "Gel 1",
        "Arrow 2",
        "Quest 3",
    };

        public static List<string> QuestDescription { get; } = new List<string>
        {
            "Gel 1",
            "Arrow 2",
            "Description 3"
        };

        public int[] QuestItemID = {
            ItemID.Gel,
            ItemID.WoodenArrow, 3, 4, 5,
        };

        public int[] QuestItemCount =
        {
            1,
            1,
            1,
            1,
        };
        public bool CheckIfFinishable()
        {
            int itemID = QuestItemID[FinishedQuestCount];
            int itemCount = 0;

            // Check for quest items in inventory
            for (int i = 0; i < Main.player[Main.myPlayer].inventory.Length; i++)
            {
                if (Main.player[Main.myPlayer].inventory[i].type == itemID)
                {
                    itemCount += Main.player[Main.myPlayer].inventory[i].stack;
                }
            }
            // If you have enough items
            if (itemCount >= QuestItemCount[FinishedQuestCount])
            {
                IsCompletable = true;
                FinishQuest();
                return true;
            }
            else { IsCompletable = false;
                return false;
            }
        }
        public void FinishQuest()
        {
            int itemID = QuestItemID[FinishedQuestCount];

            if (IsCompletable)
            {
                // Remove items from the player's inventory
                for (int i = 0; i < Main.player[Main.myPlayer].inventory.Length; i++)
                {
                    if (Main.player[Main.myPlayer].inventory[i].type == itemID)
                    {
                        int stackToRemove = Math.Min(Main.player[Main.myPlayer].inventory[i].stack, QuestItemCount[FinishedQuestCount]);
                        Main.player[Main.myPlayer].inventory[i].stack -= stackToRemove;
                        if (Main.player[Main.myPlayer].inventory[i].stack <= 0)
                        {
                            Main.player[Main.myPlayer].inventory[i].SetDefaults(0);
                        }
                        break;
                    }
                }

                // Provide the player with quest rewards
                Main.NewText("Congratulations! You have completed the quest");
                FinishedQuestCount++;
                IsCompletable = false;
                QuestStarted = false;
                if (FinishedQuestCount > 1)
                {
                    FinishedQuestCount = 0;
                }
            }
        }

        public string GetQuestDescription(int completedCount)
        {
            return $"Your quest is: {QuestName[completedCount]}. {QuestDescription[completedCount]}";
        }

    }
}

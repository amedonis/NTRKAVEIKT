using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Chat;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Zaranka.Utils.NPCUtils
{
    public static partial class ZarankaUtils
    {
        /// <summary>
        /// Efficiently counts the amount of existing enemies. May be used for multiple enemies.
        /// </summary>
        /// <param name="typesToCheck"></param>
        /// <returns></returns>
        public static int CountNPCsBetter(params int[] typesToCheck)
        {
            // Don't waste time if the type check list is empty for some reason.
            if (typesToCheck.Length <= 0)
                return 0;

            int count = 0;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (!typesToCheck.Contains(Main.npc[i].type) || !Main.npc[i].active)
                    continue;

                count++;
            }

            return count;
        }


        public static T ModNPC<T>(this NPC npc) where T : ModNPC => npc.ModNPC as T;

        public static NPCShop AddWithCustomValue(this NPCShop shop, int itemType, int customValue, params Condition[] conditions)
        {
            var item = new Item(itemType)
            {
                shopCustomPrice = customValue
            };
            return shop.Add(item, conditions);
        }

        public static NPCShop AddWithCustomValue<T>(this NPCShop shop, int customValue, params Condition[] conditions) where T : ModItem
        {
            return shop.AddWithCustomValue(ItemType<T>(), customValue, conditions);
        }

        public static void DrawBackglow(this NPC npc, Color backglowColor, float backglowArea, SpriteEffects spriteEffects, Rectangle frame, Vector2 screenPos, Texture2D overrideTexture = null)
        {
            Texture2D texture = overrideTexture is null ? TextureAssets.Npc[npc.type].Value : overrideTexture;
            Vector2 drawPosition = npc.Center - screenPos;
            Vector2 origin = frame.Size() * 0.5f;
            Color backAfterimageColor = backglowColor * npc.Opacity;
            for (int i = 0; i < 10; i++)
            {
                Vector2 drawOffset = (MathHelper.TwoPi * i / 10f).ToRotationVector2() * backglowArea;
                Main.spriteBatch.Draw(texture, drawPosition + drawOffset, frame, backAfterimageColor, npc.rotation, origin, npc.scale, spriteEffects, 0f);
            }
        }
    }
}
using CritRework.Content.Items.Equipable.Tokens;
using Mono.Cecil.Cil;
using Terraria.Map;
using Terraria.Utilities;
using Terraria.WorldBuilding;

namespace CritRework.Common.Systems
{
    class ChestLoot : ModSystem
    {
        public override void PostWorldGen()
        {
            WeightedRandom<int> tokens = new(WorldGen.genRand);
            tokens.Add(ModContent.ItemType<OrchestraToken>(), 0.5f);
            tokens.Add(ModContent.ItemType<RockToken>(), 0.8f);
            tokens.Add(ModContent.ItemType<BoxingToken>(), 1f);
            tokens.Add(ModContent.ItemType<DuckyToken>(), 1f);
            tokens.Add(ModContent.ItemType<BoneToken>(), 0.8f);
            tokens.Add(ModContent.ItemType<MetalPipeToken>(), 0.1f);

            for (int i = 0; i < 1000; i++)
            {
                Chest chest = Main.chest[i];


                if (chest != null)
                {
                    if (chest.y > GenVars.lavaLine) //Lava layer chests
                    {
                        if (WorldGen.genRand.NextBool(5))
                        {
                            Item lastItem = null;

                            if (chest.item[0].type == ItemID.CloudinaBottle)
                            {
                                chest.item[0].SetDefaults(ModContent.ItemType<Content.Items.Equipable.Accessories.FireInABottle>());
                                chest.item[0].Prefix(-1);
                                chest.item[0].stack = 1;
                            }
                            else
                            {

                                for (int inventoryIndex = 1; inventoryIndex < 40; inventoryIndex++)
                                {
                                    if (lastItem != null)
                                    {
                                        Item newLastItem = chest.item[inventoryIndex].Clone();
                                        chest.item[inventoryIndex] = lastItem.Clone();
                                        lastItem = newLastItem;
                                    }
                                    else
                                    {
                                        lastItem = chest.item[inventoryIndex].Clone();
                                        chest.item[inventoryIndex].SetDefaults(ItemID.None);
                                    }
                                }

                                chest.item[1].SetDefaults(ModContent.ItemType<Content.Items.Equipable.Accessories.FireInABottle>());
                                chest.item[1].Prefix(-1);
                                chest.item[1].stack = 1;
                            }
                        }
                    }

                    if (Main.tile[chest.x, chest.y].TileType == TileID.Containers && Main.tile[chest.x, chest.y].TileFrameX == 0 * 36) //Wooden chest
                    {
                        if (WorldGen.genRand.NextBool(7))
                        {
                            for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                            {
                                if (chest.item[inventoryIndex].type == ItemID.None)
                                {
                                    chest.item[inventoryIndex].SetDefaults(ModContent.ItemType<Content.Items.Equipable.Accessories.SharpenedWrench>());
                                    chest.item[inventoryIndex].Prefix(-1);
                                    chest.item[inventoryIndex].stack = 1;
                                    break;
                                }
                            }
                        }
                    }

                    if (Main.tile[chest.x, chest.y].TileType == TileID.Containers && Main.tile[chest.x, chest.y].TileFrameX == 12 * 36) //Living wood chest
                    {

                    }

                    if (Main.tile[chest.x, chest.y].TileType == TileID.Containers && Main.tile[chest.x, chest.y].TileFrameX == 1 * 36) //Golden chest
                    {
                        if (WorldGen.genRand.NextBool(4))
                        {
                            for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                            {
                                if (chest.item[inventoryIndex].type == ItemID.None)
                                {
                                    chest.item[inventoryIndex].SetDefaults(ModContent.ItemType<Content.Items.Equipable.Accessories.SharpenedWrench>());
                                    chest.item[inventoryIndex].Prefix(-1);
                                    chest.item[inventoryIndex].stack = 1;
                                    break;
                                }
                            }
                        }

                        if (WorldGen.genRand.NextBool(4))
                        {
                            for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                            {
                                if (chest.item[inventoryIndex].type == ItemID.None)
                                {
                                    chest.item[inventoryIndex].SetDefaults(WorldGen.SavedOreTiers.Iron == TileID.Iron ? ModContent.ItemType<Content.Items.Whetstones.IronWhetstone>() : ModContent.ItemType<Content.Items.Whetstones.LeadWhetstone>());
                                    chest.item[inventoryIndex].stack = 1;
                                    break;
                                }
                            }
                        }
                    }

                    int num = 0;
                    if (Main.tile[chest.x, chest.y].TileType == TileID.Containers && Main.tile[chest.x, chest.y].TileFrameX == 2 * 36) //Locked golden (dungeon) chest
                    {
                        num++;
                        if (num % 4 == 0)
                        {
                            for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                            {
                                if (chest.item[inventoryIndex].type == ItemID.None)
                                {
                                    chest.item[inventoryIndex].SetDefaults(ModContent.ItemType<Content.Items.Equipable.Accessories.EternalGuillotine>());
                                    chest.item[inventoryIndex].Prefix(-1);
                                    chest.item[inventoryIndex].stack = 1;
                                    break;
                                }
                            }
                        }

                        if (WorldGen.genRand.NextBool(4))
                        {
                            for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                            {
                                if (chest.item[inventoryIndex].type == ItemID.None)
                                {
                                    chest.item[inventoryIndex].SetDefaults(ModContent.ItemType<Content.Items.Whetstones.NecromanticWhetstone>());
                                    chest.item[inventoryIndex].stack = 1;
                                    break;
                                }
                            }
                        }
                    }

                    if (Main.tile[chest.x, chest.y].TileType == TileID.Containers && Main.tile[chest.x, chest.y].TileFrameX == 4 * 36) //Locked shadow chest
                    {
                        if (WorldGen.genRand.NextBool(3))
                        {
                            for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                            {
                                if (chest.item[inventoryIndex].type == ItemID.None)
                                {
                                    chest.item[inventoryIndex].SetDefaults(ModContent.ItemType<Content.Items.Equipable.Accessories.ShadowDonut>());
                                    chest.item[inventoryIndex].Prefix(-1);
                                    chest.item[inventoryIndex].stack = 1;
                                    break;
                                }
                            }
                        }
                    }

                    if (Main.tile[chest.x, chest.y].TileType == TileID.Containers && Main.tile[chest.x, chest.y].TileFrameX == 8 * 36) //Rich mahogany chest
                    {
                        if (WorldGen.genRand.NextBool(4))
                        {
                            for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                            {
                                if (chest.item[inventoryIndex].type == ItemID.None)
                                {
                                    chest.item[inventoryIndex].SetDefaults(ModContent.ItemType<Content.Items.Whetstones.AdaptiveWhetstone>());
                                    chest.item[inventoryIndex].stack = 1;
                                    break;
                                }
                            }
                        }
                    }

                    if (Main.tile[chest.x, chest.y].TileType == TileID.Containers && Main.tile[chest.x, chest.y].TileFrameX == 11 * 36) //Ice chest
                    {
                        if (WorldGen.genRand.NextBool(5))
                        {
                            for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                            {
                                if (chest.item[inventoryIndex].type == ItemID.None)
                                {
                                    chest.item[inventoryIndex].SetDefaults(ModContent.ItemType<Content.Items.Equipable.Accessories.ThawingCloth>());
                                    chest.item[inventoryIndex].Prefix(-1);
                                    chest.item[inventoryIndex].stack = 1;
                                    break;
                                }
                            }
                        }

                        if (WorldGen.genRand.NextBool(4))
                        {
                            for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                            {
                                if (chest.item[inventoryIndex].type == ItemID.None)
                                {
                                    chest.item[inventoryIndex].SetDefaults(ModContent.ItemType<Content.Items.Whetstones.FrozenWhetstone>());
                                    chest.item[inventoryIndex].stack = 1;
                                    break;
                                }
                            }
                        }
                    }
                    if (Main.tile[chest.x, chest.y].TileType == TileID.Containers2 && Main.tile[chest.x, chest.y].TileFrameX == 10 * 36) //Sandstone chest
                    {
                        if (WorldGen.genRand.NextBool(4))
                        {
                            for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                            {
                                if (chest.item[inventoryIndex].type == ItemID.None)
                                {
                                    chest.item[inventoryIndex].SetDefaults(ModContent.ItemType<Content.Items.Whetstones.AncientWhetstone>());
                                    chest.item[inventoryIndex].stack = 1;
                                    break;
                                }
                            }
                        }
                    }

                    if (Main.tile[chest.x, chest.y].TileType == TileID.Containers && Main.tile[chest.x, chest.y].TileFrameX == 15 * 36) //Spider chest
                    {
                        if (WorldGen.genRand.NextBool(2))
                        {
                            for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                            {
                                if (chest.item[inventoryIndex].type == ItemID.None)
                                {
                                    chest.item[inventoryIndex].SetDefaults(ModContent.ItemType<Content.Items.Whetstones.WebCoveredWhetstone>());
                                    chest.item[inventoryIndex].stack = 1;
                                    break;
                                }
                            }
                        }
                    }

                    if (Main.tile[chest.x, chest.y].TileType == TileID.Containers && Main.tile[chest.x, chest.y].TileFrameX == 32 * 36) //Mushroom Chest
                    {

                    }

                    if (Main.tile[chest.x, chest.y].TileType == TileID.Containers && Main.tile[chest.x, chest.y].TileFrameX == 13 * 36) //Skyware Chest
                    {
                        if (WorldGen.genRand.NextBool(3))
                        {
                            for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                            {
                                if (chest.item[inventoryIndex].type == ItemID.None)
                                {
                                    chest.item[inventoryIndex].SetDefaults(ModContent.ItemType<Content.Items.Whetstones.SoaringWhetstone>());
                                    chest.item[inventoryIndex].stack = 1;
                                    break;
                                }
                            }
                        }
                    }

                    if (WorldGen.genRand.NextBool(12) || (Main.drunkWorld && Main.rand.NextBool(5)))
                    {
                        for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                        {
                            if (chest.item[inventoryIndex].type == ItemID.None)
                            {
                                chest.item[inventoryIndex].SetDefaults(tokens.Get());
                                chest.item[inventoryIndex].Prefix(-1);
                                chest.item[inventoryIndex].stack = 1;
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}

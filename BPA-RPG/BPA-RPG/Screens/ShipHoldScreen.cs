﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using BPA_RPG.GameItems;
using BPA_RPG.GameObjects;
using Microsoft.Xna.Framework.Input;

namespace BPA_RPG.Screens
{
    public class ShipHoldScreen : Screen
    {
        private List<GameItem> inventory => PlayerData.inventory;
        private List<Weapon> weapons => PlayerData.weapons;
        private int holdSize => PlayerData.ship.holdSize;
        private List<GameObject> itemRects;
        private GameObject holdScrollArrowTop;
        private GameObject holdScrollArrowBot;

        private GameItem mouseItem;

        private int firstRender;

        private SpriteFont font;
        private Texture2D menu;
        private Texture2D holdScrollArrowReg;
        private Texture2D holdScrollArrowBlue;
        private Texture2D partInv;

        public ShipHoldScreen()
            : base("Hold")
        {
            itemRects = new List<GameObject>();
        }

        public override void LoadContent(ContentManager content)
        {
            font = content.Load<SpriteFont>("Fonts/ChoiceFont");
            menu = content.Load<Texture2D>("Images/ShipHoldMenu");
            holdScrollArrowReg = content.Load<Texture2D>("Images/HoldScrollArrow");
            holdScrollArrowBlue = content.Load<Texture2D>("Images/HoldScrollArrowBlue");
            partInv = content.Load<Texture2D>("Images/PartInv");

            holdScrollArrowTop = new GameObject(holdScrollArrowReg) { position = new Vector2(325, 125) };
            holdScrollArrowBot = new GameObject(holdScrollArrowReg) { position = new Vector2(325, 452), rotation = (float)Math.PI };

            Texture2D itemHoverRect = content.Load<Texture2D>("Images/ItemHoverRect");
            for (int i = 0; i < 10; i++)
            {
                itemRects.Add(new GameObject(itemHoverRect) { position = new Vector2(337, 155 + (i * 30)), visible = false } );
            }

            base.LoadContent(content);
        }

        public override void Deactivated()
        {
            if (mouseItem != null)
            {
                inventory.Add(mouseItem);
                mouseItem = null;
            }

            base.Deactivated();
        }

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < itemRects.Count; i++)
            {
                GameObject obj = itemRects[i];

                if (obj.boundingRectangle.Contains(MainGame.input.newMouseState.Position))
                {
                    obj.visible = true;

                    // If you click on one
                    if (MainGame.input.newMouseState.LeftButton == ButtonState.Pressed && MainGame.input.oldMouseState.LeftButton == ButtonState.Released)
                    {
                        if (i < inventory.Count - firstRender)
                        {
                            if (mouseItem == null)
                            {
                                mouseItem = inventory[i + firstRender];
                                inventory.RemoveAt(i + firstRender);
                            }
                            else
                            {
                                Rectangle rect = obj.boundingRectangle;
                                rect.Height /= 2;
                                if (rect.Contains(MainGame.input.newMouseState.Position))
                                    inventory.Insert(i + firstRender, mouseItem);
                                else inventory.Insert(i + firstRender + 1, mouseItem);

                                mouseItem = null;
                            }
                        }
                        else if (mouseItem != null)
                        {
                            inventory.Insert(inventory.Count, mouseItem);
                            mouseItem = null;
                        }
                    }
                }
                else obj.visible = false;
            }

            if (new Rectangle(520, 110, 60, 60).Contains(MainGame.input.newMouseState.Position) &&
                MainGame.input.newMouseState.LeftButton == ButtonState.Pressed && MainGame.input.oldMouseState.LeftButton == ButtonState.Released)
            {
                if (mouseItem == null)
                {
                    mouseItem = PlayerData.engine;
                    PlayerData.engine = null;
                }
                else if (mouseItem is Engine)
                {
                    Engine engine = PlayerData.engine;
                    PlayerData.engine = mouseItem as Engine;
                    mouseItem = engine;
                }
            }

            // Check if mouse is on up scroll arrow
            if (holdScrollArrowTop.boundingRectangle.Contains(MainGame.input.newMouseState.Position))
            {
                holdScrollArrowTop.texture = holdScrollArrowBlue;

                // If clicking
                if (MainGame.input.newMouseState.LeftButton == ButtonState.Pressed && MainGame.input.oldMouseState.LeftButton == ButtonState.Released)
                    firstRender--;
            }
            else holdScrollArrowTop.texture = holdScrollArrowReg;

            // Check if mouse is on down scroll arrow
            if (holdScrollArrowBot.boundingRectangle.Contains(MainGame.input.newMouseState.Position))
            {
                holdScrollArrowBot.texture = holdScrollArrowBlue;

                // If clicking
                if (MainGame.input.newMouseState.LeftButton == ButtonState.Pressed && MainGame.input.oldMouseState.LeftButton == ButtonState.Released)
                    firstRender++;
            }
            else holdScrollArrowBot.texture = holdScrollArrowReg;

            // Check Mouse scroll input
            if (MainGame.input.newMouseState.ScrollWheelValue > MainGame.input.oldMouseState.ScrollWheelValue) // scroll up
                firstRender--;
            else if (MainGame.input.newMouseState.ScrollWheelValue < MainGame.input.oldMouseState.ScrollWheelValue) //scroll down
                firstRender++;

            if (firstRender < 0)
                firstRender = 0;
            else if (firstRender > inventory.Count - 10)
                firstRender = inventory.Count - 10;

            if (firstRender < 0)
                firstRender = 0;
            
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spritebatch)
        {
            spritebatch.End();
            spritebatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);

            spritebatch.Draw(menu, MainGame.WindowCenter, new Rectangle(0, 0, menu.Width, menu.Height), Color.White,
                0, new Vector2(menu.Width / 2, menu.Height / 2), 1, SpriteEffects.None, 1);
            
            for (int i = 0; i < itemRects.Count; i++)
            {
                if (firstRender + i >= inventory.Count)
                    break;

                itemRects[i].Draw(gameTime, spritebatch);
            }

            for (int i = 0; i < 10; i++)
            {
                int k = i + firstRender;

                if (k >= inventory.Count)
                    break;

                spritebatch.Draw(inventory[k].texture, new Vector2(250, 145 + (i * 30)), Color.White);
                spritebatch.DrawString(font, inventory[k].name, new Vector2(290, 145 + (i * 30)), Color.White);

                holdScrollArrowTop.Draw(gameTime, spritebatch);
                holdScrollArrowBot.Draw(gameTime, spritebatch);
            }

            spritebatch.Draw(partInv, new Vector2(517, 107), new Rectangle(0, 0, partInv.Width, partInv.Height), Color.White);

            if (PlayerData.engine != null)
                spritebatch.Draw(PlayerData.engine.texture, new Vector2(550, 140), new Rectangle(0, 0, 20, 20), Color.White,
                    0, new Vector2(PlayerData.engine.Width / 2, PlayerData.engine.Height / 2), 3, SpriteEffects.None, 1);
            spritebatch.DrawString(font, "[Engine]", new Vector2(700, 125) - font.MeasureString("[Engine]") / 2, Color.White);

            string name = PlayerData.engine != null ? PlayerData.engine.name : "None!";
            spritebatch.DrawString(font, name, new Vector2(700, 155) - font.MeasureString(name) / 2, Color.White);


            if (mouseItem != null)
                spritebatch.Draw(mouseItem.texture, MainGame.input.oldMouseState.Position.ToVector2(), 
                    new Rectangle(0, 0, 20, 20), Color.White, 0, Vector2.Zero, 2, SpriteEffects.None, 1);

            spritebatch.End();
            spritebatch.Begin();

            base.Draw(gameTime, spritebatch);
        }
    }
}

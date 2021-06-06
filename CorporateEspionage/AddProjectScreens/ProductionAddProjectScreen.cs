/********************************************
 * Production Add Project Screen Class      *
 * by Mark McCarthy                         *
 *                                          *
 * Provides player controls for selecting   *
 * and queuing production projects          *
 * Displays selected project                *
 * based on MSDN Screen Manager suite       *
 ********************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CorporateEspionage
{
    class ProductionAddProjectScreen : ActiveMenuScreen
    {
        Vector2 pos = new Vector2(200, 200);   //center anchor

        List<Product> products;                //list storing products available for production
        int productIndex;

        MenuEntry productMenuEntry;
        MenuEntry confirmMenuEntry;
        MenuEntry backMenuEntry;

        public ProductionAddProjectScreen(Player player, Vector2 menuPos, Vector2 titlePos)
            :base("Add Project", player, menuPos, titlePos)
        {
            products = new List<Product>();

            foreach (Product p in player.Products)
            {
                if (p.IsResearched)
                    products.Add(p);
            }

            if(products.Count == 0)
                products.Add(new Product());

            productMenuEntry = new MenuEntry(string.Empty);
            confirmMenuEntry = new MenuEntry("Add This Project");
            backMenuEntry = new MenuEntry("Back");

            SetMenuEntryText();

            productMenuEntry.Selected += ProductMenuEntrySelected;
            confirmMenuEntry.Selected += ConfirmMenuEntrySelected;
            backMenuEntry.Selected += OnCancel;

            MenuEntries.Add(productMenuEntry);
            MenuEntries.Add(confirmMenuEntry);
            MenuEntries.Add(backMenuEntry);
        }

        #region Menu Functions
        void SetMenuEntryText()
        {
            productMenuEntry.Text = "Product: " + products[productIndex].Name;
        }

        void ProductMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            productIndex++;
            if (productIndex >= products.Count)
                productIndex = 0;
            SetMenuEntryText();
        }

        void ConfirmMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            if(!products[productIndex].IsResearched || products[productIndex].Cost > player.Money)
                PlayFailSound();
            else
            {
                Game1.soundBank.PlayCue("add_project");
                player.ProductionSegment.ProjectQueue.Enqueue(new Project(products[productIndex], 0));
            }
        }
        #endregion

        public override void Update(GameTime gameTime, bool focus, bool covered)
        {
            base.Update(gameTime, focus, covered);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            spriteBatch.Begin();

            Display(spriteBatch);

            spriteBatch.End();
        }
        
        //displays data on selected project
        public void Display(SpriteBatch spriteBatch)
        {
            Rectangle frame = new Rectangle((int)pos.X, (int)pos.Y, 110, 175);
            Rectangle itemPane = new Rectangle((int)pos.X + 10, (int)pos.Y + 10, frame.Width - 20, frame.Width - 20);
            Rectangle blurbPane = new Rectangle((int)pos.X + 10, (int)pos.Y + itemPane.Height + 15, frame.Width - 20,
                frame.Height - itemPane.Height - 25);
            
            spriteBatch.Draw(player.ProductionSegment.FrameImage, frame, Color.White);
            spriteBatch.Draw(blank, blurbPane, Color.Black);
            spriteBatch.Draw(blank, itemPane, Color.Black);

            if (products[productIndex].Name == "no product")
                spriteBatch.DrawString(tinyFont, products[productIndex].Name, new Vector2(blurbPane.X + 2, blurbPane.Y + 2), Color.White);
            else
            {
                Rectangle itemIcon = new Rectangle(itemPane.X + ((itemPane.Width - products[productIndex].ItemIcon.Width) / 2),
                        itemPane.Y + ((itemPane.Height - products[productIndex].ItemIcon.Height) / 2),
                        products[productIndex].ItemIcon.Width, products[productIndex].ItemIcon.Height);
                if (itemIcon.Width > itemPane.Width)
                {
                    itemIcon.Width = itemPane.Width;
                    itemIcon.X = itemPane.X;
                }
                if (itemIcon.Height > itemPane.Height)
                {
                    itemIcon.Height = itemPane.Height;
                    itemIcon.Y = itemPane.Y;
                }

                spriteBatch.Draw(products[productIndex].ItemIcon, itemIcon, Color.White);
                spriteBatch.DrawString(tinyFont, products[productIndex].Name + "\nTime:\n" + products[productIndex].ProductionTime
                    + "\nCost:\n$" + products[productIndex].Cost, new Vector2(blurbPane.X + 2, blurbPane.Y + 2), Color.White);
            }
        }
    }
}

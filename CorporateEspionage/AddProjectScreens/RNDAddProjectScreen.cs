/****************************************************
 * Research Add Project Screen Class                *
 * by Mark McCarthy                                 *
 *                                                  *
 * Provides player controls for selecting           *
 * and queuing research projects of different types *
 * Displays selected project                        *
 * based on MSDN Screen Manager suite               *
 ****************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CorporateEspionage
{
    class RNDAddProjectScreen : ActiveMenuScreen
    {
        Vector2 pos = new Vector2(200, 200);   //center anchor

        List<Product> products;             //researched products
        List<Product> newProducts;          //unresearched products

        List<Product> productsTest;         //used to test for changes in researched status while on this screen
        List<Product> newProductsTest;
        int productIndex;

        enum ResearchType
        {
            New = 1,
            Quality,
            Efficiency,
            Speed
        }

        ResearchType selectedResearch = ResearchType.New;

        MenuEntry researchMenuEntry;
        MenuEntry productMenuEntry;
        MenuEntry confirmMenuEntry;
        MenuEntry backMenuEntry;

        public RNDAddProjectScreen(Player player, Vector2 menuPos, Vector2 titlePos)
            :base("Add Project", player, menuPos, titlePos)
        {
            products = new List<Product>();
            newProducts = new List<Product>();

            foreach (Product p in player.Products)      //populates lists
            {
                if (p.IsResearched)
                    products.Add(p);
                else
                    newProducts.Add(p);
            }
            
            if(products.Count == 0)                     //if empty, empty product
                products.Add(new Product());

            if(newProducts.Count == 0)
                newProducts.Add(new Product());

            productsTest = new List<Product>(products);
            newProductsTest = new List<Product>(newProducts);

            researchMenuEntry = new MenuEntry(string.Empty);
            productMenuEntry = new MenuEntry(string.Empty);
            confirmMenuEntry = new MenuEntry("Add This Project");
            backMenuEntry = new MenuEntry("Back");

            SetMenuEntryText();

            researchMenuEntry.Selected += ResearchMenuEntrySelected;
            productMenuEntry.Selected += ProductMenuEntrySelected;
            confirmMenuEntry.Selected += ConfirmMenuEntrySelected;
            backMenuEntry.Selected += OnCancel;

            MenuEntries.Add(researchMenuEntry);
            MenuEntries.Add(productMenuEntry);
            MenuEntries.Add(confirmMenuEntry);
            MenuEntries.Add(backMenuEntry);
        }

        #region Menu Functions
        void SetMenuEntryText()
        {
            researchMenuEntry.Text = "Research: " + selectedResearch;
            if (selectedResearch == ResearchType.New)               //checks for changes in list
            {
                if (productIndex >= newProducts.Count)
                    productIndex = 0;
                productMenuEntry.Text = "Product: " + newProducts[productIndex].Name;
            }
            else
            {
                if (productIndex >= products.Count)
                    productIndex = 0;
                productMenuEntry.Text = "Product: " + products[productIndex].Name;
            }
        }

        void ResearchMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            selectedResearch++;
            if (selectedResearch > ResearchType.Speed)
                selectedResearch = ResearchType.New;

            SetMenuEntryText();
        }

        void ProductMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            productIndex++;
            if (((productIndex >= newProducts.Count) && (selectedResearch == ResearchType.New))
                || ((productIndex >= products.Count) && (selectedResearch != ResearchType.New)))
                productIndex = 0;
            SetMenuEntryText();
        }

        void ConfirmMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            Boolean fail = false;
            if (selectedResearch == ResearchType.New)               //if New research, makes sure research job is not already active/enqueued
            {
                if(newProducts[productIndex].Name == "no product")
                    fail = true;
                if(player.RNDSegment.ActiveProject.ThisProduct.Name == newProducts[productIndex].Name)
                    fail = true;
                foreach(Project p in player.RNDSegment.ProjectQueue)
                {
                    if(p.ThisProduct.Name == newProducts[productIndex].Name)
                        fail = true;
                }
            }
            else if((selectedResearch != ResearchType.New) && (products[productIndex].Name == "no product"))
                fail = true;

            if(fail)
                PlayFailSound();
            else
            {
                Product p;
                if (selectedResearch == ResearchType.New)
                    p = newProducts[productIndex];
                else
                    p = products[productIndex];
                Game1.soundBank.PlayCue("add_project");
                player.RNDSegment.ProjectQueue.Enqueue(new Project(p, (int)selectedResearch));
            }
        }
        #endregion

        public override void Update(GameTime gameTime, bool focus, bool covered)
        {
            if(!products.Equals(productsTest) || !newProducts.Equals(newProductsTest))      //checks for changes to lists
            {                                                                               //necessary to avoid IndexOutOfBounds
                products = new List<Product>();
                newProducts = new List<Product>();

                foreach (Product p in player.Products)
                {
                    if (p.IsResearched)
                        products.Add(p);
                    else
                        newProducts.Add(p);
                }

                if (products.Count == 0)
                    products.Add(new Product());

                if (newProducts.Count == 0)
                    newProducts.Add(new Product());

                productsTest = new List<Product>(products);
                newProductsTest = new List<Product>(newProducts);

                SetMenuEntryText();
            }

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
        
        //displays selected project
        public void Display(SpriteBatch spriteBatch)
        {
            Product p;
            int time;
            if (selectedResearch == ResearchType.New)
            {
                p = newProducts[productIndex];
                time = p.ResearchTime;
            }
            else
            {
                p = products[productIndex];
                if (selectedResearch == ResearchType.Efficiency)
                    time = p.ResearchEfficiencyTime;
                else if (selectedResearch == ResearchType.Quality)
                    time = p.ResearchQualityTime;
                else
                    time = p.ResearchSpeedTime;
            }

            Rectangle frame = new Rectangle((int)pos.X, (int)pos.Y, 110, 175);
            Rectangle itemPane = new Rectangle((int)pos.X + 10, (int)pos.Y + 10, frame.Width - 20, frame.Width - 20);
            Rectangle blurbPane = new Rectangle((int)pos.X + 10, (int)pos.Y + itemPane.Height + 15, frame.Width - 20,
                frame.Height - itemPane.Height - 25);
            
            spriteBatch.Draw(player.RNDSegment.FrameImage, frame, Color.White);
            spriteBatch.Draw(blank, blurbPane, Color.Black);
            spriteBatch.Draw(blank, itemPane, Color.Black);

            if (p.Name == "no product")
                spriteBatch.DrawString(tinyFont, p.Name, new Vector2(blurbPane.X + 2, blurbPane.Y + 2), Color.White);
            else
            {
                Rectangle itemIcon = new Rectangle(itemPane.X + ((itemPane.Width - p.ItemIcon.Width) / 2),
                        itemPane.Y + ((itemPane.Height - p.ItemIcon.Height) / 2),
                        p.ItemIcon.Width, p.ItemIcon.Height);
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

                spriteBatch.Draw(p.ItemIcon, itemIcon, Color.White);
                spriteBatch.DrawString(tinyFont, p.Name + "\nTime:\n" + time, new Vector2(blurbPane.X + 2, blurbPane.Y + 2), Color.White);
            }
        }
    }
}
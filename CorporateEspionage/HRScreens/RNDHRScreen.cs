/********************************************
 * Research HR Screen Class                 *
 * by Mark McCarthy                         *
 *                                          *
 * Provides player controls for managing    *
 * employees; displays employees available  *
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
    class RNDHRScreen : ActiveMenuScreen
    {
        Vector2 anchor = new Vector2(100, 200);

        Sprite rookie;      //employee sprites
        Sprite veteran;
        Sprite expert;

        enum Action { Hire, Fire }
        enum Employee { Rookie, Veteran, Expert }

        Action action = Action.Hire;
        Employee employee = Employee.Rookie;

        MenuEntry actionMenuEntry;
        MenuEntry employeeMenuEntry;
        MenuEntry confirmMenuEntry;
        MenuEntry backMenuEntry;

        public RNDHRScreen(Player player, Vector2 menuPos, Vector2 titlePos)
            : base("Human Resources", player, menuPos, titlePos)
        {
            actionMenuEntry = new MenuEntry(string.Empty);
            employeeMenuEntry = new MenuEntry(string.Empty);
            confirmMenuEntry = new MenuEntry(string.Empty);
            backMenuEntry = new MenuEntry("Back");

            SetMenuEntryText();

            actionMenuEntry.Selected += ActionMenuEntrySelected;
            employeeMenuEntry.Selected += EmployeeMenuEntrySelected;
            confirmMenuEntry.Selected += ConfirmMenuEntrySelected;
            backMenuEntry.Selected += OnCancel;

            MenuEntries.Add(actionMenuEntry);
            MenuEntries.Add(employeeMenuEntry);
            MenuEntries.Add(confirmMenuEntry);
            MenuEntries.Add(backMenuEntry);

            Vector2 pos = anchor;
            rookie = new Sprite(player.RNDSegment.TextureRookie, pos, new Point(16, 24), new Point(0, 0), new Point(2, 4), new Vector2(0, 1), 2f);
            pos.X += 100;
            veteran = new Sprite(player.RNDSegment.TextureVeteran, pos, new Point(16, 24), new Point(0, 0), new Point(2, 4), new Vector2(0, 1), 2f);
            pos.X += 100;
            expert = new Sprite(player.RNDSegment.TextureExpert, pos, new Point(16, 24), new Point(0, 0), new Point(2, 4), new Vector2(0, 1), 2f);
        }

        #region Menu Elements
        void SetMenuEntryText()
        {
            actionMenuEntry.Text = "Action: " + action;
            employeeMenuEntry.Text = "Class: " + employee;
            string str = "You're ";
            if (action == Action.Hire)
                str += "hired!";
            else
                str += "fired!";
            confirmMenuEntry.Text = str;
        }

        void ActionMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            action++;
            if (action > Action.Fire)
                action = Action.Hire;

            SetMenuEntryText();
        }

        void EmployeeMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            employee++;
            if (employee > Employee.Expert)
                employee = Employee.Rookie;

            SetMenuEntryText();
        }

        void ConfirmMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            CompanySegment seg = player.RNDSegment;
            Boolean fail = false;

            if (action == Action.Fire)
            {
                if (employee == Employee.Rookie)
                {
                    if (seg.Rookies < 1)
                        fail = true;
                    else
                    {
                        seg.Rookies--;
                        Game1.soundBank.PlayCue("fired");
                    }
                }
                else if (employee == Employee.Veteran)
                {
                    if (seg.Veterans < 1)
                        fail = true;
                    else
                    {
                        seg.Veterans--;
                        Game1.soundBank.PlayCue("fired");
                    }
                }
                else
                {
                    if (seg.Experts < 1)
                        fail = true;
                    else
                    {
                        seg.Experts--;
                        Game1.soundBank.PlayCue("fired");
                    }
                }
            }
            else
            {
                if (employee == Employee.Rookie)
                {
                    if (seg.CostRookie > player.Money)
                        fail = true;
                    else
                    {
                        player.Money -= seg.CostRookie;
                        seg.Rookies++;
                        Game1.soundBank.PlayCue("buy_asset");
                    }
                }
                else if (employee == Employee.Veteran)
                {
                    if (seg.CostVeteran > player.Money)
                        fail = true;
                    else
                    {
                        player.Money -= seg.CostVeteran;
                        seg.Veterans++;
                        Game1.soundBank.PlayCue("buy_asset");
                    }
                }
                else
                {
                    if (seg.CostExpert > player.Money)
                        fail = true;
                    else
                    {
                        player.Money -= seg.CostExpert;
                        seg.Experts++;
                        Game1.soundBank.PlayCue("buy_asset");
                    }
                }
            }

            if (fail)
                PlayFailSound();
        }
        #endregion

        public override void Update(GameTime gameTime, bool focus, bool covered)
        {
            base.Update(gameTime, focus, covered);
            Rectangle window = ScreenManager.Game.Window.ClientBounds;
            rookie.Update(gameTime, window);
            veteran.Update(gameTime, window);
            expert.Update(gameTime, window);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            CompanySegment seg = player.RNDSegment;
            base.Draw(gameTime);
            spriteBatch.Begin();
            
            
            //display current counts
            Vector2 pos = anchor;
            pos.Y += 24;
            pos.X += 34;
            spriteBatch.DrawString(screenFont, "x " + seg.Rookies, pos, Color.White);
            pos.X += 100;
            spriteBatch.DrawString(screenFont, "x " + seg.Veterans, pos, Color.White);
            pos.X += 100;
            spriteBatch.DrawString(screenFont, "x " + seg.Experts, pos, Color.White);

            //display costs
            pos = anchor;
            pos.Y += 52;
            spriteBatch.DrawString(screenFont, "$" + seg.CostRookie, pos, Color.White);
            pos.X += 100;
            spriteBatch.DrawString(screenFont, "$" + seg.CostVeteran, pos, Color.White);
            pos.X += 100;
            spriteBatch.DrawString(screenFont, "$" + seg.CostExpert, pos, Color.White);

            //display sprites
            rookie.Draw(gameTime, spriteBatch);
            veteran.Draw(gameTime, spriteBatch);
            expert.Draw(gameTime, spriteBatch);


            spriteBatch.End();
        }
    }
}

/********************************************
 * Player class                             *
 * Mark McCarthy                            *
 *                                          *
 * Tracks all data owned by player business *
 * (member segments, catalogue, money)      *
 ********************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace CorporateEspionage
{
    class Player : DrawableGameComponent
    {
        CompanySegment production;
        CompanySegment research;

        List<Product> products; //list of all products in game

        int money;              //player's money (score)

        #region Accessors
        public CompanySegment ProductionSegment
        {
            get { return production; }
        }

        public CompanySegment RNDSegment
        {
            get { return research; }
        }

        public int Money
        {
            get { return money; }
            set { money = value; }
        }

        public List<Product> Products
        {
            get { return products; }
        }

        public int QuarterlyCosts
        {
            get { return production.OperatingCost + research.OperatingCost; }
        }
        #endregion

        public Player(Game game)
            : base(game)
        {
            production = new CompanySegment(game, 300, 550, 700, 500, 800, 1100, "production");
            research = new CompanySegment(game, 350, 600, 750, 400, 750, 1000, "research");
            money = 20000;

            products = new List<Product>();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            
            //populates products list
            //more products can be added later, but will require changes to EndOfTurnScreen to display all at once
            products.Add(new Product("Chair", 60, 20, 60, Game.Content.Load<Texture2D>("images/chair2")));
            products.Add(new Product("Desk", 80, 40, 50, Game.Content.Load<Texture2D>("images/desk")));
            products.Add(new Product("TV", 600, 325, 200, Game.Content.Load<Texture2D>("images/tv")));
            products.Add(new Product("Camera", 1500, 900, 400, Game.Content.Load<Texture2D>("images/camera")));
            products.Add(new Product("Car", 15000, 8000, 1200, Game.Content.Load<Texture2D>("images/car")));
            products.Add(new Product("Helicopter", 1000000, 400000, 4000, Game.Content.Load<Texture2D>("images/helicopter")));

            production.LoadContent();
            research.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public void Load()
        {
            LoadContent();
        }

        //update performed at each tick
        public virtual void TickUpdate()
        {
            int q = production.ProjectQueue.Count;
            production.TickUpdate();                                    //tick production
            if (q > production.ProjectQueue.Count)                      //if new project has been undertaken
                money -= production.ActiveProject.ThisProduct.Cost;     //charge its cost
            research.TickUpdate();                                      //tick research
        }

        //reset production values
        public void NewQuarter()
        {
            foreach (Product p in products)
                p.ResetStock();
        }
    }
}

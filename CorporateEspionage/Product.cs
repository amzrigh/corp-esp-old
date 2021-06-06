/********************************************
 * Product class                            *
 * Mark McCarthy                            *
 *                                          *
 * Tracks traits of individual product.     *
 * Product members are lowest level of      *
 * player data; simple member manipulation. *
 ********************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace CorporateEspionage
{
    class Product// : Microsoft.Xna.Framework.GameComponent
    {
        String name;                //product name
        int stock;                  //number of items in stock (i.e., created this quarter)
        Boolean researched;         //is product available for production?
        //int[] researchDependency; //for Tech Tree -- list of prerequisite technologies (TBI)
        int baseSale;            //base sale value of item
        int baseCost;            //base cost for production
        int baseSpeed;              //base time to produce
        int plusSpeed;              //improvements to production speed; more improvements means greater cost for same return
        int plusEfficiency;         //improvements to production cost
        int quality;                //affects sale value; improved by researching quality+
        //int appeal;                 //affects sale value; improved by creating advertising campaign, harmed by opponent smear campaign (TBI)

        Texture2D itemIcon;         //icon displayed when job is processed/queued
        
        #region Accessors
        public String Name
        {
            get { return name; }
        }

        public Boolean IsResearched
        {
            get { return researched; }
        }

        public int Cost
        {
            get
            {
                double cost = (double)baseCost;
                for (int i = 0; i < plusEfficiency; i++)
                    cost *= .92;
                return (int)cost;
            }
        }

        public int SalePrice
        {
            get { return (int)((double)baseSale * (.1 * (double)quality)); }
        }

        public int BaseSale     //needed when saturation reached
        {
            get { return baseSale; }
        }

        public int ProductionTime
        {
            get
            {
                double time = (double)baseSpeed;
                for (int i = 0; i < plusSpeed; i++)
                    time *= .92;
                return (int)time;
            }
        }

        public int Stock
        {
            get { return stock; }
        }

        public Texture2D ItemIcon
        {
            get { return itemIcon; }
        }

        public int ResearchTime
        {
            get { return (int)((double)baseSpeed * 1.5); }
        }

        public int ResearchSpeedTime
        {
            get { return (int)((double)baseSpeed * (.9 + (.1 * (double)plusSpeed))); }
        }

        public int ResearchEfficiencyTime
        {
            get { return (int)((double)baseSpeed * (.9 + (.1 * (double)plusEfficiency))); }
        }

        public int ResearchQualityTime
        {
            get { return (int)((double)baseSpeed * (.9 + (.1 * (double)quality))); }
        }

        public int SaturationPoint
        {
            get { return (int)(20000000.0 / (double)baseSale); }
        }

        public int SaturatedSalePrice(int total)
        {
            return (int)((20000000.0 / (double)total) * (.1 * (double)quality));
        }
        #endregion
        #region Constructors
        public Product(String name, int baseSale, int baseCost, int baseSpeed, Texture2D itemIcon)
        {
            this.name = name;
            this.baseSale = baseSale;
            this.baseCost = baseCost;
            this.baseSpeed = baseSpeed;
            //appeal = 15;
            quality = 10;
            plusEfficiency = 0;
            plusSpeed = 0;
            stock = 0;
            researched = false;
            this.itemIcon = itemIcon;
        }

        public Product()
        {
            name = "no product";
            //itemIcon = no item image
        }

        //copy constructor
        public Product(Product p)
        {
            name = p.name;
            stock = p.stock;
            researched = p.researched;
            baseSale = p.baseSale;
            baseCost = p.baseCost;
            baseSpeed = p.baseSpeed;
            plusSpeed = p.plusSpeed;
            plusEfficiency = p.plusEfficiency;
            quality = p.quality;
            //appeal = p.appeal;
            itemIcon = p.itemIcon;
        }
        #endregion
        public void ResetStock()
        {
            stock = 0;
        }

        public void ResearchComplete()
        {
            researched = true;
        }

        public void ImproveSpeed()
        {
            plusSpeed++;
        }

        public void ImproveEfficiency()
        {
            plusEfficiency++;
        }

        public void ImproveQuality()
        {
            quality++;
        }

        /*public void Advertise()
        {
            appeal++;
        }*/

        public void Build()
        {
            stock++;
        }

        /*public void Smear()
        {
            appeal--;
        }*/
    }
}

/************************************
 * CPUOpponent class                *
 * Mark McCarthy                    *
 *                                  *
 * Extension of Player class        *
 * Adds automation for opponent to  *
 * take actions independently       *
 ************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CorporateEspionage
{
    class CPUOpponent : Player
    {
        int tickCount = 7;

        Random rnd = new Random();

        public CPUOpponent(Game game)
            : base(game)
        {
            RNDSegment.Rookies++;           //starts with one rookie researcher
            Money -= RNDSegment.CostRookie;
        }

        public override void TickUpdate()
        {
            tickCount++;
            int index;
            int proj;
            if (tickCount > 7)  //period of actions
            {
                tickCount = 0;
                index = rnd.Next(Products.Count);   //select product to act on

                bool researching = false;

                if (!Products[index].IsResearched)  //if product hasn't been researched...
                {
                    if (Products[index] == RNDSegment.ActiveProject.ThisProduct)  //and it's not enqueued for research...
                        researching = true;
                    else
                    {
                        foreach (Project p in RNDSegment.ProjectQueue)
                        {
                            if (Products[index] == p.ThisProduct)
                                researching = true;
                        }
                    }
                    if (!researching)
                        RNDSegment.ProjectQueue.Enqueue(new Project(Products[index], 1));   //research it!
                }
                else                                                                        //otherwise...
                {
                    proj = rnd.Next(8);
                    if(proj > 1 && proj < 5)                                                //queue different project for product (5/8 chance to produce)
                        RNDSegment.ProjectQueue.Enqueue(new Project(Products[index], proj));
                    else if(Money >= Products[index].Cost)                                  //only produces if affordable
                        ProductionSegment.ProjectQueue.Enqueue(new Project(Products[index], 0));
                }

                CompanySegment seg;
                index = rnd.Next(2);
                if(index == 0)                  //randomly select a segment
                    seg = ProductionSegment;
                else
                    seg = RNDSegment;

                index = rnd.Next(6);

                switch (index)                  //randomly select an asset
                {
                    case 0:
                        if(seg.CostRookie < Money - ((seg.CostRookie * 3) + QuarterlyCosts))        //if sufficient funds...
                        {
                            seg.Rookies++;                                                          //hire/buy!
                            Money -= seg.CostRookie;
                        }
                        break;
                    case 1:
                        if(seg.CostVeteran < Money - ((seg.CostVeteran * 3) + QuarterlyCosts))
                        {
                            seg.Veterans++;
                            Money -= seg.CostVeteran;
                        }
                        break;
                    case 2:
                        if(seg.CostExpert < Money - ((seg.CostExpert * 3) + QuarterlyCosts))
                        {
                            seg.Experts++;
                            Money -= seg.CostExpert;
                        }
                        break;
                    case 3:
                        if((seg.CostTools < Money - (seg.CostTools + QuarterlyCosts))
                            && (seg.Rookies + seg.Veterans + seg.Experts > seg.Tools + seg.Machinery + seg.HeavyMachinery))
                        {
                            seg.Tools++;
                            Money -= seg.CostTools;
                        }
                        break;
                    case 4:
                        if((seg.CostMach < Money - (seg.CostMach + QuarterlyCosts))
                            && (seg.Rookies + seg.Veterans + seg.Experts > seg.Tools + seg.Machinery + seg.HeavyMachinery))
                        {
                            seg.Machinery++;
                            Money -= seg.CostMach;
                        }
                        break;
                    case 5:
                        if((seg.CostHeavyMach < Money - (seg.CostHeavyMach + QuarterlyCosts))
                            && (seg.Rookies + seg.Veterans + seg.Experts > seg.Tools + seg.Machinery + seg.HeavyMachinery))
                        {
                            seg.HeavyMachinery++;
                            Money -= seg.CostHeavyMach;
                        }
                        break;
                    default:
                        break;
                }
            }

            base.TickUpdate();
        }
    }
}

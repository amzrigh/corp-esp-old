/****************************************
 * Project class                        *
 * Mark McCarthy                        *
 *                                      *
 * Holds project data                   *
 * Calls product methods on completion  *
 ****************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CorporateEspionage
{
    class Project// : DrawableGameComponent
    {
        Product product;            //product affected by Project
        int projectType;            //0 == create;  used by Production
                                    //1 == research; 2 == improve quality; 3 == improve efficiency; 4 == improve speed; used by R&D
                                    //5 == promote; 6 == smear;  used by Marketing
                                    //7 == infiltrate;  used by Security
                                    //-1== null project

        int progress;               //progress towards completion

        #region Accessors
        public Boolean Active       //is not null project
        {
            get
            {
                if (projectType == -1)
                    return false;
                else
                    return true;
            }
        }

        public Boolean Working      //is project active and complete
        {
            get { return (progress < TimeToComplete); }
        }

        public Product ThisProduct
        {
            get { return product; }
        }

        public int TimeToComplete
        {
            get
            {
                switch (projectType)
                {
                    case 0:
                        return product.ProductionTime;
                    case 1:
                        return product.ResearchTime;
                    case 2:
                        return product.ResearchQualityTime;
                    case 3:
                        return product.ResearchEfficiencyTime;
                    case 4:
                        return product.ResearchSpeedTime;
                    default:
                        return 1;
                }
            }
        }
        #endregion

        public Project(Product product, int projectType)
            
        {
            this.product = product;
            this.projectType = projectType;
            progress = 0;
        }

        public Project()            //creates null project
        {
            product = new Product();
            projectType = -1;
            progress = 0;
        }

        public void Process(int work)
        {
            progress += work;
            if (progress > TimeToComplete)
                progress = TimeToComplete;
        }

        public void Complete()
        {
            switch (projectType)
            {
                case 0:
                    product.Build(); break;
                case 1:
                    product.ResearchComplete(); break;
                case 2:
                    product.ImproveQuality(); break;
                case 3:
                    product.ImproveEfficiency(); break;
                case 4:
                    product.ImproveSpeed(); break;
                //case 5:
                //    product.Advertise(); break;
                //case 6:
                //    product.Smear(); break;
                //case 7:
                //  to be implemented
                default:
                    break;
            }
        }

        //displays project using given parameters
        public void Display(Vector2 pos, Texture2D border, Texture2D progressBar, Texture2D blank, SpriteFont font, SpriteBatch spriteBatch)
        {
            Rectangle frame = new Rectangle((int)pos.X, (int)pos.Y, 110, 200);
            Rectangle itemPane = new Rectangle((int)pos.X + 10, (int)pos.Y + 10, frame.Width - 20, frame.Width - 20);
            Rectangle barPane = new Rectangle((int)pos.X + 10, (int)pos.Y + frame.Height - 30, frame.Width - 20, 20);
            Rectangle blurbPane = new Rectangle((int)pos.X + 10, (int)pos.Y + itemPane.Height + 15, frame.Width - 20,
                frame.Height - itemPane.Height - barPane.Height - 30);
            float barRatio;
            Rectangle bar;

            spriteBatch.Draw(border, frame, Color.White);
            spriteBatch.Draw(blank, blurbPane, Color.Black);
            spriteBatch.Draw(blank, barPane, Color.Black);
            spriteBatch.Draw(blank, itemPane, Color.Black);
            if (projectType == -1)                          //if null project, don't try drawing everything else
            {
                spriteBatch.DrawString(font, "No active\nproject", new Vector2(blurbPane.X + 2, blurbPane.Y + 2), Color.White);
            }
            else
            {
                barRatio = (float)progress / (float)TimeToComplete;
                bar = new Rectangle(barPane.X, barPane.Y, (int)((float)barPane.Width * barRatio), barPane.Height);
                
                Rectangle itemIcon = new Rectangle(itemPane.X + ((itemPane.Width - product.ItemIcon.Width) / 2),        //center item icon in itemPane
                    itemPane.Y + ((itemPane.Height - product.ItemIcon.Height) / 2),
                    product.ItemIcon.Width, product.ItemIcon.Height);
                if (itemIcon.Width > itemPane.Width)                                                                    //if too large, fit
                {
                    itemIcon.Width = itemPane.Width;
                    itemIcon.X = itemPane.X;
                }
                if (itemIcon.Height > itemPane.Height)
                {
                    itemIcon.Height = itemPane.Height;
                    itemIcon.Y = itemPane.Y;
                }

                string type;
                switch (projectType)
                {
                    case 0:
                        type = "Produce"; break;
                    case 1:
                        type = "Research"; break;
                    case 2:
                        type = "Quality+"; break;
                    case 3:
                        type = "Efficiency+"; break;
                    case 4:
                        type = "Speed+"; break;
                    default:
                        type = string.Empty; break;
                }
                spriteBatch.Draw(product.ItemIcon, itemIcon, Color.White);
                spriteBatch.Draw(progressBar, bar, Color.White);
                spriteBatch.DrawString(font, type + "\n" + product.Name + "\nProgress:\n" + progress + "\n/" + TimeToComplete,
                    new Vector2(blurbPane.X + 2, blurbPane.Y + 2), Color.White);
            }
        }
    }
}

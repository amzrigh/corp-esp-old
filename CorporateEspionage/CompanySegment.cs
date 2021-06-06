/****************************************************
 * CompanySegment Class                             *
 * by Mark McCarthy                                 *
 *                                                  *
 * Stores and performs actions on data belonging    *
 * to a particular segment of the company           *
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
    class CompanySegment : GameComponent
    {
        string path;                 //segment resource path

        #region Employee Values
        int employeeRookie = 0;             //numbers of employees
        const double workRookie = 1;    //and their effectiveness
        int employeeVeteran = 0;
        const double workVeteran = 3;
        int employeeExpert = 0;
        const double workExpert = 4;

        int valueRookie;             //cost of employees
        int valueVeteran;
        int valueExpert;

        /*Sprite spriteFrameRookie;           //employee sprites
        Sprite spriteFrameVeteran;          //frame 16x24px
        Sprite spriteFrameExpert;           //sheet 2 x 4 frames
        Sprite spriteSideRookie;
        Sprite spriteSideVeteran;
        Sprite spriteSideExpert;*/
        Texture2D textureRookie;
        Texture2D textureVeteran;
        Texture2D textureExpert;
        #endregion
        #region Equipment Values
        int equipmentBasic = 0;             //numbers of equipment
        const double workBasic = 1.5;   //and their effectiveness
        int equipmentIntermediate = 0;
        const double workIntermediate = 2.25;
        int equipmentAdvanced = 0;
        const double workAdvanced = 3;

        int valueBasic;              //cost of equipment
        int valueIntermediate;
        int valueAdvanced;

        /*Sprite spriteBasic;             //equipment sprites
        Sprite spriteIntermediate;      //frame 32x48px
        Sprite spriteAdvanced;          //sheet 1x1 or 2x1 frames*/
        Texture2D textureBasic;
        Texture2D textureIntermediate;
        Texture2D textureAdvanced;
        #endregion
        #region Safety Values (not yet implemented)
        /*int safetyBasic;                //numbers of safety measures
        const double mitigationBasic = 1;   //and their effectiveness
        int safetyIntermediate;
        const double mitigationIntermediate = 2.5;
        int safetyAdvanced;
        const double mitigationAdvanced = 4;*/
        #endregion

        //Texture2D paneImage;
        Texture2D frameImage;         //frame used in segment display
        Texture2D progressBar;        //this segment's progress bar

        Project activeProject;        //project currently being worked on by employees
        Queue<Project> projectQueue;  //backlog of projects; first replaces activeProject

        #region Member Accessors
        public int Rookies
        {
            get { return employeeRookie; }
            set { employeeRookie = value; }
        }

        public int Veterans
        {
            get { return employeeVeteran; }
            set { employeeVeteran = value; }
        }

        public int Experts
        {
            get { return employeeExpert; }
            set { employeeExpert = value; }
        }

        public int Tools
        {
            get { return equipmentBasic; }
            set { equipmentBasic = value; }
        }

        public int Machinery
        {
            get { return equipmentIntermediate; }
            set { equipmentIntermediate = value; }
        }

        public int HeavyMachinery
        {
            get { return equipmentAdvanced; }
            set { equipmentAdvanced = value; }
        }

        public int CostRookie
        {
            get { return valueRookie; }
        }

        public int CostVeteran
        {
            get { return valueVeteran; }
        }

        public int CostExpert
        {
            get { return valueExpert; }
        }

        public int CostTools
        {
            get { return valueBasic; }
        }

        public int CostMach
        {
            get { return valueIntermediate; }
        }

        public int CostHeavyMach
        {
            get { return valueAdvanced; }
        }

        public Project ActiveProject
        {
            get { return activeProject; }
        }

        public Queue<Project> ProjectQueue
        {
            get { return projectQueue; }
        }

        public int OperatingCost
        {
            get
            {
                int cost = 0;
                cost += employeeRookie * valueRookie;
                cost += employeeVeteran * valueVeteran;
                cost += employeeExpert * valueExpert;
                cost += equipmentBasic * valueBasic;
                cost += equipmentIntermediate * valueIntermediate;
                cost += equipmentAdvanced * valueAdvanced;
                return cost;
            }
        }
        #endregion
        #region Graphics Accessors
        public Texture2D TextureRookie
        {
            get { return textureRookie; }
        }

        public Texture2D TextureVeteran
        {
            get { return textureVeteran; }
        }

        public Texture2D TextureExpert
        {
            get { return textureExpert; }
        }

        public Texture2D TextureBasic
        {
            get { return textureBasic; }
        }

        public Texture2D TextureIntermediate
        {
            get { return textureIntermediate; }
        }

        public Texture2D TextureAdvanced
        {
            get { return textureAdvanced; }
        }

        public Texture2D FrameImage
        {
            get { return frameImage; }
        }

        public Texture2D ProgressBar
        {
            get { return progressBar; }
        }
        #endregion

        public CompanySegment(Game game, int valueRookie, int valueVeteran, int valueExpert,
            int valueBasic, int valueIntermediate, int valueAdvanced, string title)
            :base(game)
        {
            this.valueRookie = valueRookie;
            this.valueVeteran = valueVeteran;
            this.valueExpert = valueExpert;
            this.valueBasic = valueBasic;
            this.valueIntermediate = valueIntermediate;
            this.valueAdvanced = valueAdvanced;
            path = "images/" + title;

            projectQueue = new Queue<Project>();
            activeProject = new Project();
        }

        public void LoadContent()
        {
            //paneImage = Game.Content.Load<Texture2D>("images/placeholder_splash");
            frameImage = Game.Content.Load<Texture2D>(path + "_border");
            progressBar = Game.Content.Load<Texture2D>(path + "_bar");

            #region Old Sprite Data
            //initializing each sprite in the proper location
            /*spriteFrameRookie = new Sprite(Game.Content.Load<Texture2D>(path + "_rookie"), new Vector2(204, 224),
                new Point(16, 24), new Point(0, 0), new Point(2, 4), new Vector2(0, -1), 2f);
            spriteFrameVeteran = new Sprite(Game.Content.Load<Texture2D>(path + "_veteran"), new Vector2(460, 208),
                new Point(16, 24), new Point(0, 0), new Point(2, 4), new Vector2(0, 1), 2f);
            spriteFrameExpert = new Sprite(Game.Content.Load<Texture2D>(path + "_expert"), new Vector2(175, 291),
                new Point(16, 24), new Point(0, 0), new Point(2, 4), new Vector2(-1, 0), 2f);

            Vector2 pos = sideAnchor;
            spriteSideRookie = new Sprite(Game.Content.Load<Texture2D>(path + "_rookie"), pos, new Point(16, 24),
                new Point(0, 0), new Point(2, 4), new Vector2(0, 1), 2f);
            pos.Y += 64;
            spriteSideVeteran = new Sprite(Game.Content.Load<Texture2D>(path + "_veteran"), pos, new Point(16, 24),
                new Point(0, 0), new Point(2, 4), new Vector2(0, 1), 2f);
            pos.Y += 64;
            spriteSideExpert = new Sprite(Game.Content.Load<Texture2D>(path + "_expert"), pos, new Point(16, 24),
                new Point(0, 0), new Point(2, 4), new Vector2(0, 1), 2f);
            pos.Y += 64;

            spriteBasic = new Sprite(Game.Content.Load<Texture2D>(path + "_basic"), pos, new Point(32, 48),
                new Point(0, 0), new Point(1, 1), new Vector2(0, 1), 2f);
            pos.Y += 96;
            spriteIntermediate = new Sprite(Game.Content.Load<Texture2D>(path + "_intermediate"), pos, new Point(32, 48),
                new Point(0, 0), new Point(1, 1), new Vector2(0, 1), 2f);
            pos.Y += 96;
            spriteAdvanced = new Sprite(Game.Content.Load<Texture2D>(path + "_advanced"), pos, new Point(32, 48),
                new Point(0, 0), new Point(1, 1), new Vector2(0, 1), 2f);*/
            #endregion
            //loading sprite textures to be used by screens
            textureRookie = Game.Content.Load<Texture2D>(path + "_rookie");
            textureVeteran = Game.Content.Load<Texture2D>(path + "_veteran");
            textureExpert = Game.Content.Load<Texture2D>(path + "_expert");
            textureBasic = Game.Content.Load<Texture2D>(path + "_basic");
            textureIntermediate = Game.Content.Load<Texture2D>(path + "_intermediate");
            textureAdvanced = Game.Content.Load<Texture2D>(path + "_advanced");
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        //TickUpdate passes above values to Project process() commands
        public void TickUpdate()
        {
            if(!activeProject.Working)                              //if project is active and finished
            {
                activeProject.Complete();                           //complete project
                activeProject = new Project();                      //free active slot
            }

            if (!activeProject.Active && projectQueue.Count > 0)    //if active slot free and queue not empty
                activeProject = projectQueue.Dequeue();             //first project in queue becomes active project
            

            if (activeProject.Active)
            {
                int work = (int)((employeeRookie * workRookie) + (employeeVeteran * workVeteran) + (employeeExpert * workExpert));
                //work measures the progress step on activeProject.  Each employee does a certain amount of work; equipment only adds to work if there
                //is an employee to operate it.
                if (equipmentAdvanced > employeeRookie + employeeVeteran + employeeExpert)
                    work += (int)(workAdvanced * (employeeRookie + employeeVeteran + employeeExpert));
                else
                    work += (int)(workAdvanced * equipmentAdvanced);

                if (employeeRookie + employeeVeteran + employeeExpert - equipmentAdvanced > 0)
                {
                    if (equipmentIntermediate > employeeRookie + employeeVeteran + employeeExpert - equipmentAdvanced)
                        work += (int)(workIntermediate * (employeeRookie + employeeVeteran + employeeExpert - equipmentAdvanced));
                    else
                        work += (int)(workIntermediate * equipmentIntermediate);
                }

                if (employeeRookie + employeeVeteran + employeeExpert - equipmentAdvanced - equipmentIntermediate > 0)
                {
                    if (equipmentBasic > employeeRookie + employeeVeteran + employeeExpert - equipmentAdvanced - equipmentIntermediate)
                        work += (int)(workBasic * (employeeRookie + employeeVeteran + employeeExpert - equipmentAdvanced - equipmentIntermediate));
                    else
                        work += (int)(workBasic * equipmentBasic);
                }

                activeProject.Process(work);
            }
        }

        
    }
}

using GooseShared;
using SamEngine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PetGoose
{
    class Bed : Item
    {
        private Image[] images;
        private float lastAnimateTime, animationGap;
        private int currentImage;
        bool asleep;
        public Bed() : base(new Point(20, Screen.PrimaryScreen.WorkingArea.Height - 70))
        {
            currentImage = 1;
            lastAnimateTime = Time.time;
            animationGap = 1f;
            asleep = false;
            images = new Image[4];

            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            images[0] = Image.FromFile(Path.Combine(assemblyFolder, "Images\\bed.png"));
            images[1] = Image.FromFile(Path.Combine(assemblyFolder, "Images\\z1.png"));
            images[2] = Image.FromFile(Path.Combine(assemblyFolder, "Images\\z2.png"));
            images[3] = Image.FromFile(Path.Combine(assemblyFolder, "Images\\z3.png"));
        }

        public override void tick(GooseEntity goose)
        {
            if (!isOn())
                return;
            if (!(goose.currentTask == API.TaskDatabase.getTaskIndexByID("RunToBed")) && !(goose.currentTask == API.TaskDatabase.getTaskIndexByID("Sleeping")))
            {
                API.Goose.setCurrentTaskByID(goose, "RunToBed", false);
            }
            if (goose.currentTask == API.TaskDatabase.getTaskIndexByID("RunToBed"))
            {
                goose.targetPos.x = position.X + 25;
                goose.targetPos.y = position.Y + 25;

                if (API.Goose.isGooseAtTarget(goose, 40))
                {
                    asleep = true;

                    API.Goose.setSpeed(goose, GooseEntity.SpeedTiers.Walk);
                    API.Goose.setCurrentTaskByID(goose, "Sleeping", false);
                }
            }
            else if(goose.currentTask == API.TaskDatabase.getTaskIndexByID("Sleeping"))
            {
                goose.direction = 0;

                if(Time.time - lastAnimateTime > animationGap)
                {
                    animate();
                    lastAnimateTime = Time.time;
                }
            }
        }

        private void animate()
        {
            currentImage++;
            if (currentImage > 3)
                currentImage = 1;
        }
        public override void activate()
        {
            setOn(!isOn());
            if (isOn())
                asleep = false;
        }

        public override void render(Graphics g)
        {
            if (!isOn())
                return;
            g.DrawImage(images[0], position.X, position.Y);

            if(asleep)
                g.DrawImage(images[currentImage], position.X + 50, position.Y - 60);
        }

        public override void render(Graphics g, int x, int y)
        {
            g.DrawImage(images[0], x, y);
        }
    }
}

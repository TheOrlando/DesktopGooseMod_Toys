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

namespace PetGoose
{
    class Laser : Item
    {
        private Image image;
        private SolidBrush redBrush;
        public Laser() : base(new Point(Input.mouseX - 15, Input.mouseY - 10))
        {
            redBrush = new SolidBrush(Color.Red);

            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            image = Image.FromFile(Path.Combine(assemblyFolder, "Images\\laser.png"));
        }
        public override void activate()
        {
            setOn(!isOn());
            if (isOn())
                updatePosition();
        }

        public override void render(Graphics g)
        {
            if (!isOn())
                return;
            g.DrawImage(image, position.X, position.Y);
            g.FillRectangle(redBrush, position.X + 3, position.Y - 80, 4, 4);
        }

        public override void render(Graphics g, int x, int y)
        {
            g.DrawImage(image, x + 15, y);
        }

        public override void tick(GooseEntity goose)
        {
            if (!isOn())
                return;
            updatePosition();
            if(!(goose.currentTask == API.TaskDatabase.getTaskIndexByID("RunToBed")) && !(goose.currentTask == API.TaskDatabase.getTaskIndexByID("Sleeping")) && !(goose.currentTask == API.TaskDatabase.getTaskIndexByID("ChaseLaser")))
            {
                API.Goose.setCurrentTaskByID(goose, "ChaseLaser");
            }
            if(goose.currentTask == API.TaskDatabase.getTaskIndexByID("ChaseLaser"))
            {
                goose.targetPos.x = position.X + 3;
                goose.targetPos.y = position.Y - 100;
            }
        }

        private void updatePosition()
        {
            position.X = Input.mouseX - 5;
            position.Y = Input.mouseY - 20;
        }
    }
}

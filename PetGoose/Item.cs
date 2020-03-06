using GooseShared;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetGoose
{
    abstract class Item
    {
        public Point position;
        bool on;
        public Item(Point position)
        {
            this.position = position;
            on = false;
        }

        public bool isOn()
        {
            return on;
        }

        public void setOn(bool on)
        {
            this.on = on;
        }

        abstract public void tick(GooseEntity goose);
        abstract public void activate();
        abstract public void render(Graphics g);
        abstract public void render(Graphics g, int x, int y);
    }
}

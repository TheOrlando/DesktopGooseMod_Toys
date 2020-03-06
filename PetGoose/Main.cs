using GooseShared;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetGoose
{
    public class ModEntryPoint : IMod
    {
        Item[] items;
        public void Init()
        {
            items = new Item[4];
            items[0] = new Ball();
            items[1] = new Stick();
            items[2] = new Bed();
            items[3] = new Laser();
            Menu.init(items);

            InjectionPoints.PreTickEvent += PreTick;
            InjectionPoints.PostTickEvent += PostTick;
            InjectionPoints.PreRenderEvent += PreRender;
            InjectionPoints.PostRenderEvent += PostRender;
        }

        public void PreTick(GooseEntity goose)
        {
            Menu.tick();

            if (goose.currentTask == API.TaskDatabase.getTaskIndexByID("ChargeToBall") && !items[0].isOn())
            {
                API.Goose.setSpeed(goose, GooseEntity.SpeedTiers.Walk);
                API.Goose.setTaskRoaming(goose);
            }
            else if ((goose.currentTask == API.TaskDatabase.getTaskIndexByID("ChargeToStick") || goose.currentTask == API.TaskDatabase.getTaskIndexByID("ReturnStick")) && !items[1].isOn())
            {
                API.Goose.setSpeed(goose, GooseEntity.SpeedTiers.Walk);
                API.Goose.setTaskRoaming(goose);
            }
            else if ((goose.currentTask == API.TaskDatabase.getTaskIndexByID("RunToBed") || goose.currentTask == API.TaskDatabase.getTaskIndexByID("Sleeping")) && !items[2].isOn())
            {
                API.Goose.setSpeed(goose, GooseEntity.SpeedTiers.Walk);
                API.Goose.setTaskRoaming(goose);
            }
            else if (goose.currentTask == API.TaskDatabase.getTaskIndexByID("ChaseLaser") && !items[3].isOn())
            {
                API.Goose.setSpeed(goose, GooseEntity.SpeedTiers.Walk);
                API.Goose.setTaskRoaming(goose);
            }
        }

        public void PostTick(GooseEntity goose)
        {
            for(int i = 0; i < items.Length; i++)
                items[i].tick(goose);
        }

        public void PreRender(GooseEntity goose, Graphics g)
        {
            for (int i = 0; i < items.Length; i++)
                items[i].render(g);
        }
        public void PostRender(GooseEntity goose, Graphics g)
        {
            Menu.render(g);
        }
    }
}

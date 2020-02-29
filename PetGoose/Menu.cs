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
    class Menu
    {
        public static readonly int REST_POSITION = -40, EXTENDED_POSITION = 0, FULL_EXTENDED_POSITION = 200, LOADING_BAR_FULL = 50;
        private static int x, y, state, speed, lastMouseX;
        private static float hoverTime, confirmTime, exitTime, loadingBar;
        private static Image tab, menu;
        private static Item[] items;
        private static SolidBrush brushGreen, brushWhite;
        private static Pen penBlack;

        public static void init(Item[] items)
        {
            x = REST_POSITION;
            y = Screen.PrimaryScreen.WorkingArea.Height - 200;
            speed = 4;
            hoverTime = 0;
            exitTime = 0;
            confirmTime = 1;
            Menu.items = items;
            brushGreen = new SolidBrush(Color.Green);
            brushWhite = new SolidBrush(Color.White);
            penBlack = new Pen(Color.Black);
            lastMouseX = Input.mouseX;

            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            tab = Image.FromFile(Path.Combine(assemblyFolder, "Images\\MenuTab.png"));
            menu = Image.FromFile(Path.Combine(assemblyFolder, "Images\\Menu.png"));
        }

        public static void tick()
        {
            if (state == 0)
            {
                if ((Input.mouseX < 70 && Input.mouseX >= 0) && (Input.mouseY < y + 70 && Input.mouseY > y - 20))
                    state = 1;
            }
            else if (state == 1)
            {
                if (x < EXTENDED_POSITION)
                    x += speed;
                if (x >= EXTENDED_POSITION)
                {
                    x = EXTENDED_POSITION;
                    state = 2;
                    exitTime = Time.time;
                }
            }
            else if (state == 2)
            {
                if ((Input.mouseX <= 50 && Input.mouseX >= 0) && (Input.mouseY <= y + 50 && Input.mouseY >= y))
                {
                    if (hoverTime == 0)
                        hoverTime = Time.time;

                    loadingBar = (Time.time - hoverTime) / confirmTime;

                    if (Time.time - hoverTime > confirmTime)
                    {
                        hoverTime = 0;
                        loadingBar = 0;
                        state = 3;
                    }
                    exitTime = 0;
                }
                else
                {
                    if (exitTime == 0)
                        exitTime = Time.time;
                    if (Time.time - exitTime > 3)
                    {
                        exitTime = 0;
                        state = 6;
                    }
                    hoverTime = 0;
                    loadingBar = 0;
                }
            }
            else if (state == 3)
            {
                if (x < FULL_EXTENDED_POSITION)
                    x += speed * 2;
                if (x >= FULL_EXTENDED_POSITION)
                {
                    x = FULL_EXTENDED_POSITION;
                    state = 4;
                    exitTime = Time.time;
                }
            }
            else if (state == 4)
            {
                if (Input.mouseY > y && Input.mouseY < y + 50)
                {
                    if (Input.mouseX >= x && Input.mouseX <= x + 50)
                        {
                        if ((lastMouseX < x || lastMouseX > x + 50 && hoverTime > 0))
                            hoverTime = 0;
                        if (hoverTime == 0)
                            hoverTime = Time.time;

                        loadingBar = (Time.time - hoverTime) / confirmTime;

                        if (Time.time - hoverTime > confirmTime)
                        {
                            hoverTime = 0;
                            loadingBar = 0;
                            state = 5;
                        }
                        exitTime = 0;
                    }
                    else if (Input.mouseX < x && Input.mouseX >= 0)
                    {
                        bool temp = false;
                        for(int i = 0; i < items.Length; i++)
                            if(Input.mouseX < x - 10 - (i*60) && Input.mouseX > x - 60 - (i * 60))
                            {
                                temp = true;

                                if ((lastMouseX > x - 10 - (i * 60) || lastMouseX < x - 60 - (i * 60) && hoverTime > 0))
                                    hoverTime = 0;
                                if (hoverTime == 0)
                                    hoverTime = Time.time;

                                loadingBar = (Time.time - hoverTime) / confirmTime;

                                if (Time.time - hoverTime > confirmTime)
                                {
                                    hoverTime = 0;
                                    loadingBar = 0;
                                    items[i].activate();
                                }
                            }
                        if (!temp)
                            loadingBar = 0;
                        exitTime = 0;
                    }
                    else
                    {
                        if (exitTime == 0)
                            exitTime = Time.time;
                        if (Time.time - exitTime > 3)
                        {
                            exitTime = 0;
                            state = 5;
                        }
                        hoverTime = 0;
                        loadingBar = 0;
                    }
                }
                else
                {
                    if (exitTime == 0)
                        exitTime = Time.time;
                    if(Time.time - exitTime > 3)
                    {
                        exitTime = 0;
                        state = 5;
                    }
                    hoverTime = 0;
                    loadingBar = 0;
                }
            }
            else if(state == 5){
                if (x > REST_POSITION)
                    x -= speed * 2;
                if (x <= REST_POSITION)
                {
                    x = REST_POSITION;
                    state = 0;
                }
            }
            else if(state == 6){
                if (x > REST_POSITION)
                    x -= speed;
                if(x <= REST_POSITION)
                {
                    x = REST_POSITION;
                    state = 0;
                }
            }

            lastMouseX = Input.mouseX;
        }
        public static void render(Graphics g)
        {
            if (state > 2 && state < 6)
            {
                g.DrawImage(menu, x - 300, y);
                for (int i = 0; i < items.Length; i++)
                    items[i].render(g, x - 60 - (i*60), y + 5);
            }
            g.DrawImage(tab, x, y);

            if (loadingBar > 0)
            {
                g.FillRectangle(brushWhite, Input.mouseX - 10, Input.mouseY - 20, 10, LOADING_BAR_FULL - (loadingBar * LOADING_BAR_FULL));
                g.FillRectangle(brushGreen, Input.mouseX - 10, Input.mouseY + 30 - (loadingBar * LOADING_BAR_FULL), 10, loadingBar * LOADING_BAR_FULL);
                g.DrawRectangle(penBlack, Input.mouseX - 10, Input.mouseY - 20, 10, LOADING_BAR_FULL);
            }
        }
    }
}

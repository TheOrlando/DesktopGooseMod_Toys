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
    class Ball : Item
    {
        Vector2 velocity;
        float speed, deceleration, lastKickTime, lastAnimateTime, animationGap;
        Image[] images;
        int currentImage;
        public Ball() : base(new Point(-100, 300))
        {
            speed = 20;
            velocity = new Vector2(speed, 0);
            lastKickTime = Time.time;
            deceleration = 0.25f;
            images = new Image[3];
            currentImage = 0;
            lastAnimateTime = Time.time;
            animationGap = 0;

            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            images[0] = Image.FromFile(Path.Combine(assemblyFolder, "Images\\ball.png"));
            images[1] = Image.FromFile(Path.Combine(assemblyFolder, "Images\\ball2.png"));
            images[2] = Image.FromFile(Path.Combine(assemblyFolder, "Images\\ball3.png"));
        }

        public override void tick(GooseEntity goose)
        {
            if (!isOn())
                return;
            if (speed > 0)
            {
                animationGap = 0.25f / speed;
                if (Time.time - lastAnimateTime > animationGap)
                {
                    animate();
                    lastAnimateTime = Time.time;
                }

                velocity.x = Vector2.Normalize(velocity).x * speed;
                velocity.y = Vector2.Normalize(velocity).y * speed;

                position.X += (int)velocity.x;
                if (position.X < 0)
                {
                    position.X = 0;
                    velocity.x *= -1;
                }
                if (position.X + 40 > Screen.PrimaryScreen.WorkingArea.Width)
                {
                    position.X = Screen.PrimaryScreen.WorkingArea.Width - 40;
                    velocity.x *= -1;
                }

                position.Y += (int)velocity.y;
                if (position.Y < 0)
                {
                    position.Y = 0;
                    velocity.y *= -1;
                }
                if (position.Y + 40 > Screen.PrimaryScreen.WorkingArea.Height)
                {
                    position.Y = Screen.PrimaryScreen.WorkingArea.Height - 40;
                    velocity.y *= -1;
                }

                speed -= deceleration;

                if (speed <= 0)
                    currentImage = 0;
            }

            if (Input.mouseX > position.X && Input.mouseX < position.X + 40)
                if (Input.mouseY > position.Y && Input.mouseY < position.Y + 40)
                {
                    speed = 20;
                    Vector2 temp = new Vector2(position.X + 20 - Input.mouseX, position.Y + 20 - Input.mouseY);
                    velocity.x = Vector2.Normalize(temp).x * speed;
                    velocity.y = Vector2.Normalize(temp).y * speed;
                    lastKickTime = Time.time;
                    
                    if(!(goose.currentTask == API.TaskDatabase.getTaskIndexByID("Sleeping")))
                        API.Goose.setCurrentTaskByID(goose, "ChargeToBall");
                }

            if (goose.currentTask == API.TaskDatabase.getTaskIndexByID("ChargeToBall"))
            {
                goose.targetPos.x = position.X + 20;
                goose.targetPos.y = position.Y + 20;

                if (API.Goose.isGooseAtTarget(goose, 40) && Time.time - lastKickTime > 1f)
                {
                    speed = 20;
                    Vector2 temp = new Vector2(position.X + 20 - goose.position.x, position.Y + 20 - goose.position.y);
                    velocity.x = Vector2.Normalize(temp).x * speed;
                    velocity.y = Vector2.Normalize(temp).y * speed;
                    lastKickTime = Time.time;

                    API.Goose.playHonckSound();
                    API.Goose.setSpeed(goose, GooseEntity.SpeedTiers.Walk);
                    API.Goose.setTaskRoaming(goose);
                }
            }
        }

        public override void activate()
        {
            setOn(!isOn());
            if (isOn())
            {
                position.X = -100;
                position.Y = 300;
                speed = 20;
                velocity = new Vector2(speed, 0);
                lastKickTime = Time.time;
                currentImage = 0;
                lastAnimateTime = Time.time;
                animationGap = 0;
            }
        }

        public void animate()
        {
            if (velocity.x < 0)
            {
                currentImage++;
            }
            else
            {
                currentImage--;
            }

            if (currentImage > 2)
            {
                currentImage = 0;
            }
            else if (currentImage < 0)
            {
                currentImage = 2;
            }
        }

        public override void render(Graphics g)
        {
            if (!isOn())
                return;
            g.DrawImage(images[currentImage], position);
        }

        public override void render(Graphics g, int x, int y)
        {
            g.DrawImage(images[0], x, y);
        }
    }
}

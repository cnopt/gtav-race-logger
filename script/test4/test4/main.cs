using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using GTA;
using GTA.Native;
using GTA.Math;
using GTA.UI;
using System.Windows.Forms;

using Screen = GTA.UI.Screen;
using UI = GTA.UI;


namespace test4
{
   public class Main : Script
    {
        bool vehicleStatsShow;
        string speed;
        string wheelSpeed;
        string rpm;
        string gear;
        string angle;
        string throttle;
        string braking;
        string clutch;
        string engineTemp;
        string fuelLevel;
        string worldPos;
        string posX;
        string posY;
        TextElement statsmsg;
        TextElement info_speed;
        TextElement info_wheelSpeed;
        TextElement info_rpm;
        TextElement info_gear;
        TextElement info_angle;
        TextElement info_throttle;
        TextElement info_braking;
        TextElement info_clutch;
        TextElement info_engineTemp;
        TextElement info_fuelLevel;
        TextElement info_worldPos;
        System.IO.StreamWriter file;
        int rNum;

        public Main()
        {
            Setup();

            Tick += OnTick;
            KeyDown += OnKeyDown;
        }

        void Setup()
        {
            vehicleStatsShow = false;
            info_speed = new UI.TextElement("", new PointF(0.3f, 0.5f), 0.5f, System.Drawing.Color.Red);
            info_wheelSpeed = new UI.TextElement("", new PointF(0.3f, 20f), 0.5f);
            info_rpm = new UI.TextElement("", new PointF(0.3f, 40f), 0.5f, System.Drawing.Color.Green);
            info_gear = new UI.TextElement("", new PointF(0.3f, 60f), 0.5f, System.Drawing.Color.Aqua);
            info_angle = new UI.TextElement("", new PointF(0.3f, 80f), 0.5f, System.Drawing.Color.Yellow);
            info_throttle = new UI.TextElement("", new PointF(0.3f, 100f), 0.5f, System.Drawing.Color.Orange);
            info_braking = new UI.TextElement("", new PointF(0.3f, 120f), 0.5f);
            info_clutch = new UI.TextElement("", new PointF(0.3f, 140f), 0.5f);
            info_engineTemp = new UI.TextElement("", new PointF(0.3f, 160f), 0.5f);
            info_fuelLevel = new UI.TextElement("", new PointF(0.3f, 180f), 0.5f);
            info_worldPos = new UI.TextElement("", new PointF(0.3f, 200f), 0.5f);
        }

        void OnTick(object sender, EventArgs e)
        {
            if (vehicleStatsShow)
            {
                speed = Game.Player.Character.CurrentVehicle.Speed.ToString();
                info_speed.Caption = "Speed: " + speed;
                wheelSpeed = Game.Player.Character.CurrentVehicle.WheelSpeed.ToString();
                info_wheelSpeed.Caption = "Wheel speed: " + wheelSpeed;
                rpm = Game.Player.Character.CurrentVehicle.CurrentRPM.ToString();
                info_rpm.Caption = "RPM: " + rpm;
                gear = Game.Player.Character.CurrentVehicle.CurrentGear.ToString();
                info_gear.Caption = "Gear: " + gear;
                angle = Game.Player.Character.CurrentVehicle.SteeringAngle.ToString();
                info_angle.Caption = "Steering angle: " + angle;
                throttle = Game.Player.Character.CurrentVehicle.Throttle.ToString();
                info_throttle.Caption = "Throttle: " + throttle;
                braking = Game.Player.Character.CurrentVehicle.BrakePower.ToString();
                info_braking.Caption = "Braking: " + braking;
                clutch = Game.Player.Character.CurrentVehicle.Clutch.ToString();
                info_clutch.Caption = "Clutch: " + clutch;
                engineTemp = Game.Player.Character.CurrentVehicle.EngineTemperature.ToString();
                info_engineTemp.Caption = "Engine temp: " + engineTemp;
                fuelLevel = Game.Player.Character.CurrentVehicle.FuelLevel.ToString();
                info_fuelLevel.Caption = "Fuel level: " + fuelLevel;
                worldPos = Game.Player.Character.Position.ToString();
                info_worldPos.Caption = "Pos: " + worldPos;
                posX = Game.Player.Character.Position.X.ToString();
                posY = Game.Player.Character.Position.Y.ToString();

                info_speed.Draw();
                info_wheelSpeed.Draw();
                info_rpm.Draw();
                info_gear.Draw();
                info_angle.Draw();
                info_throttle.Draw();
                info_braking.Draw();
                info_clutch.Draw();
                info_engineTemp.Draw();
                info_fuelLevel.Draw();
                info_worldPos.Draw();

                file.WriteLine(wheelSpeed + "," + rpm + "," + gear + "," + angle + "," +
                               throttle + "," + braking + "," + clutch + "," + engineTemp 
                               + "," + posX + "," + posY);

            }
            
        }

        void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F10)
            {
                info_speed.Enabled = true;
                info_wheelSpeed.Enabled = true;
                info_rpm.Enabled = true;
                info_gear.Enabled = true;
                info_angle.Enabled = true;
                info_throttle.Enabled = true;
                info_braking.Enabled = true;
                info_clutch.Enabled = true;
                info_engineTemp.Enabled = true;
                info_fuelLevel.Enabled = true;
                info_worldPos.Enabled = true;

                Random rnd = new System.Random();
                rNum = rnd.Next(1, 5000);
                file = new System.IO.StreamWriter(@"D:\Users\Charlie\Documents\gtav-race-logger\generated\" + rNum + ".txt", true);
                file.WriteLine("Wheel Speed,RPM,Gear,Angle,Throttle,Braking,Clutch,Temp,X,Y");
                vehicleStatsShow = true;
                Screen.ShowSubtitle("Logging vehicle signals..", 2000);
            }

            if (e.KeyCode == Keys.F11)
            {
                vehicleStatsShow = false;
                file.Flush();
                file.Close();
                Screen.ShowSubtitle("File write complete", 2000);
            }

        }
    }
}

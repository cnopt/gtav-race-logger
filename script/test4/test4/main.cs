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
        TextElement statsmsg;
        TextElement currentSpeed;
        TextElement currentRPM;
        TextElement currentGear;
        TextElement currentAngle;
        TextElement currentThrottle;
        TextElement currentBraking;
        TextElement currentClutch;
        TextElement currentEngineTemp;
        TextElement currentFuelLevel;
        TextElement currentPos;
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
            statsmsg = new UI.TextElement("Text", new PointF(0.3f, 0.5f), 0.7f);
            currentSpeed = new UI.TextElement("Text", new PointF(0.3f, 0.5f), 0.5f);
            currentRPM = new UI.TextElement("Text", new PointF(0.3f, 40f), 0.5f);
            currentGear = new UI.TextElement("Text", new PointF(0.3f, 60f), 0.5f);
            currentAngle = new UI.TextElement("Text", new PointF(0.3f, 80f), 0.5f);
            currentThrottle = new UI.TextElement("Text", new PointF(0.3f, 100f), 0.5f);
            currentBraking = new UI.TextElement("Text", new PointF(0.3f, 120f), 0.5f);
            currentClutch = new UI.TextElement("Text", new PointF(0.3f, 140f), 0.5f);
            currentEngineTemp = new UI.TextElement("Text", new PointF(0.3f, 160f), 0.5f);
            currentFuelLevel = new UI.TextElement("Text", new PointF(0.3f, 180f), 0.5f);
            currentPos = new UI.TextElement("Text", new PointF(0.3f, 200f), 0.5f);
        }

        void OnTick(object sender, EventArgs e)
        {
            if (vehicleStatsShow)
            {
                currentSpeed.Caption = Game.Player.Character.CurrentVehicle.Speed.ToString();
                currentRPM.Caption   = Game.Player.Character.CurrentVehicle.CurrentRPM.ToString();
                currentGear.Caption  = Game.Player.Character.CurrentVehicle.CurrentGear.ToString();
                currentAngle.Caption = Game.Player.Character.CurrentVehicle.SteeringAngle.ToString();
                currentThrottle.Caption = Game.Player.Character.CurrentVehicle.Throttle.ToString();
                currentBraking.Caption = Game.Player.Character.CurrentVehicle.BrakePower.ToString();
                currentClutch.Caption = Game.Player.Character.CurrentVehicle.Clutch.ToString();
                currentEngineTemp.Caption = Game.Player.Character.CurrentVehicle.EngineTemperature.ToString();
                currentFuelLevel.Caption = Game.Player.Character.CurrentVehicle.FuelLevel.ToString();
                currentPos.Caption = Game.Player.Character.Position.ToString();

                currentSpeed.Draw();
                currentRPM.Draw();
                currentGear.Draw();
                currentAngle.Draw();
                currentThrottle.Draw();
                currentBraking.Draw();
                currentClutch.Draw();
                currentEngineTemp.Draw();
                currentFuelLevel.Draw();
                currentPos.Draw();

                file.WriteLine(currentSpeed);

            }
            
        }

        void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F10)
            {
                currentSpeed.Enabled = true;
                currentRPM.Enabled = true;
                currentGear.Enabled = true;
                currentAngle.Enabled = true;
                currentThrottle.Enabled = true;
                currentBraking.Enabled = true;
                currentClutch.Enabled = true;
                currentEngineTemp.Enabled = true;
                currentFuelLevel.Enabled = true;
                currentPos.Enabled = true;

                Random rnd = new System.Random();
                rNum = rnd.Next(1, 5000);
                file = new System.IO.StreamWriter(@"D:\Users\Charlie\Documents\gtav-race-logger\generated\" + rNum + ".txt", true);

                vehicleStatsShow = true;
            }

        }
    }
}

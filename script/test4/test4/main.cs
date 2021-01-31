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
        }

        void OnTick(object sender, EventArgs e)
        {
            if (vehicleStatsShow)
            {
                currentSpeed.Caption = Game.Player.Character.CurrentVehicle.Speed.ToString();
                currentRPM.Caption   = Game.Player.Character.CurrentVehicle.CurrentRPM.ToString();
                currentGear.Caption  = Game.Player.Character.CurrentVehicle.CurrentGear.ToString();

                currentSpeed.Draw();
                currentRPM.Draw();
                currentGear.Draw();

            }
            
        }

        void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F10)
            {
                currentSpeed.Enabled = true;
                currentRPM.Enabled = true;
                currentGear.Enabled = true;

                vehicleStatsShow = true;
            }

        }
    }
}

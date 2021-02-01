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
using IniParser;
using IniParser.Model;


namespace test4
{
   public class Main : Script
    {
        bool vehicleStatsShow;
        bool isLoggingToFile;
        string beginKey;
        string endKey;
        bool debugDisplay;
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
        ContainerElement infoContainer;
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
            ReadConfig();
            SetupDisplay();

            Tick += OnTick;
            KeyDown += OnKeyDown;
        }

        void ReadConfig()
        {
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile(@"D:\Users\Charlie\Documents\gtav-race-logger\gtav-race-logger\config.ini");
            beginKey = data["General"]["BeginKey"];
            endKey = data["General"]["EndKey"];
            debugDisplay = bool.Parse(data["General"]["Debug"]);
        }
        void SetupOutputFile()
        {
            Random rnd = new System.Random();
            rNum = rnd.Next(1, 5000);
            file = new System.IO.StreamWriter(@"D:\Users\Charlie\Documents\gtav-race-logger\gtav-race-logger\generated\" + rNum + ".txt", true);
            file.WriteLine("Wheel Speed,RPM,Gear,Angle,Throttle,Braking,Clutch,Temp,X,Y");
            isLoggingToFile = true;
        }

        void SetupDisplay()
        {
            vehicleStatsShow    = false;
            isLoggingToFile     = false;
            infoContainer       = new UI.ContainerElement(new PointF(0.3f, 0.5f), new SizeF(200, 230), Color.Transparent);
            info_speed          = new UI.TextElement("", new PointF(0.3f, 0.5f), 0.3f);
            info_wheelSpeed     = new UI.TextElement("", new PointF(0.3f, 20f), 0.3f);
            info_rpm            = new UI.TextElement("", new PointF(0.3f, 40f), 0.3f);
            info_gear           = new UI.TextElement("", new PointF(0.3f, 60f), 0.3f);
            info_angle          = new UI.TextElement("", new PointF(0.3f, 80f), 0.3f);
            info_throttle       = new UI.TextElement("", new PointF(0.3f, 100f), 0.3f);
            info_braking        = new UI.TextElement("", new PointF(0.3f, 120f), 0.3f);
            info_clutch         = new UI.TextElement("", new PointF(0.3f, 140f), 0.3f);
            info_engineTemp     = new UI.TextElement("", new PointF(0.3f, 160f), 0.3f);
            info_fuelLevel      = new UI.TextElement("", new PointF(0.3f, 180f), 0.3f);
            info_worldPos       = new UI.TextElement("", new PointF(0.3f, 200f), 0.3f);
            infoContainer.Items.Add(info_speed);
            infoContainer.Items.Add(info_wheelSpeed);
            infoContainer.Items.Add(info_rpm);
            infoContainer.Items.Add(info_gear);
            infoContainer.Items.Add(info_angle);
            infoContainer.Items.Add(info_throttle);
            infoContainer.Items.Add(info_braking);
            infoContainer.Items.Add(info_clutch);
            infoContainer.Items.Add(info_engineTemp);
            infoContainer.Items.Add(info_fuelLevel);
            infoContainer.Items.Add(info_worldPos);
            infoContainer.Enabled = true;
        }

        void OnTick(object sender, EventArgs e)
        {
            if (vehicleStatsShow)
            {
                ShowVehicleSensors();
            }
            
            if (isLoggingToFile)
            {
                LogToFile();
            }
                        
        }

        void LogToFile()
        {
            speed       = Game.Player.Character.CurrentVehicle.Speed.ToString();
            wheelSpeed  = Game.Player.Character.CurrentVehicle.WheelSpeed.ToString();
            rpm         = Game.Player.Character.CurrentVehicle.CurrentRPM.ToString();
            gear        = Game.Player.Character.CurrentVehicle.CurrentGear.ToString();
            angle       = Game.Player.Character.CurrentVehicle.SteeringAngle.ToString();
            throttle    = Game.Player.Character.CurrentVehicle.Throttle.ToString();
            braking     = Game.Player.Character.CurrentVehicle.BrakePower.ToString();
            clutch      = Game.Player.Character.CurrentVehicle.Clutch.ToString();
            engineTemp  = Game.Player.Character.CurrentVehicle.EngineTemperature.ToString();
            fuelLevel   = Game.Player.Character.CurrentVehicle.FuelLevel.ToString();
            worldPos    = Game.Player.Character.Position.ToString();
            posX        = Game.Player.Character.Position.X.ToString();
            posY        = Game.Player.Character.Position.Y.ToString();

            file.WriteLine(wheelSpeed + "," + rpm + "," + gear + "," + angle + "," +
                           throttle + "," + braking + "," + clutch + "," + engineTemp
                           + "," + posX + "," + posY);
        }

        void ShowVehicleSensors()
        {
            info_speed.Caption      = "Speed: " + speed;
            info_wheelSpeed.Caption = "Wheel speed: " + wheelSpeed;
            info_rpm.Caption        = "RPM: " + rpm;
            info_gear.Caption       = "Gear: " + gear;
            info_angle.Caption      = "Steering angle: " + angle;
            info_throttle.Caption   = "Throttle: " + throttle;
            info_braking.Caption    = "Braking: " + braking;
            info_clutch.Caption     = "Clutch: " + clutch;
            info_engineTemp.Caption = "Engine temp: " + engineTemp;
            info_fuelLevel.Caption  = "Fuel level: " + fuelLevel;
            info_worldPos.Caption   = "Pos: " + worldPos;
            infoContainer.Draw();
        }

        void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == (Keys)Enum.Parse(typeof(Keys), beginKey))
            {
                SetupOutputFile();
                if (debugDisplay)
                {
                    vehicleStatsShow = true;
                }
                Screen.ShowSubtitle("Logging vehicle signals..", 2000);
            }

            if (e.KeyCode == (Keys)Enum.Parse(typeof(Keys), endKey))
            {
                isLoggingToFile = false;
                vehicleStatsShow = false;
                file.Flush();
                file.Close();
                Screen.ShowSubtitle("File write complete", 2000);
            }

        }
    }
}

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
        bool isDebugEnabled, isLoggingToFile = false;

        string cfgBeginKey, cfgEndKey; // config.ini options and hotkeys
        bool cfgDebug;

        string speed, wheelSpeed, rpm, gear, angle, throttle,
               braking, clutch, engineTemp, fuelLevel, worldPos,
               posX, posY; // sensors to be logged

        ContainerElement debugContainer; // debug screen (shows values)
        TextElement debug_speed, debug_wheelSpeed, debug_rpm, debug_gear,
                    debug_angle, debug_throttle, debug_braking, debug_clutch,
                    debug_engineTemp, debug_fuelLevel, debug_worldPos;

        System.IO.StreamWriter file;
        int rNum;

        public Main()
        {
            ReadConfig();
            SetupDisplay();

            Tick += OnTick;
            KeyDown += OnKeyDown;
        }

        void ReadConfig() // parse values inside the config file
        {
            var parser   = new FileIniDataParser();
            IniData data = parser.ReadFile(@"D:\Users\Charlie\Documents\gtav-race-logger\gtav-race-logger\config.ini");
            
            cfgBeginKey = data["General"]["BeginKey"];
            cfgEndKey   = data["General"]["EndKey"];
            cfgDebug    = bool.Parse(data["General"]["Debug"]);
        }
        void SetupOutputFile() // creates output file and writes headers
        {
            Random rnd = new System.Random();
            rNum = rnd.Next(1, 5000);
            file = new System.IO.StreamWriter(@"D:\Users\Charlie\Documents\gtav-race-logger\gtav-race-logger\generated\" + rNum + ".txt", true);
            file.WriteLine("Wheel Speed,RPM,Gear,Angle,Throttle,Braking,Clutch,Temp,X,Y");
            isLoggingToFile = true;
        }

        void SetupDisplay() // creates textelements for debug display
        {
            debugContainer       = new UI.ContainerElement(new PointF(0.3f, 0.5f), new SizeF(200, 230), Color.Transparent);
            debug_speed = new UI.TextElement("", new PointF(0.3f, 0.5f), 0.3f);
            debug_wheelSpeed = new UI.TextElement("", new PointF(0.3f, 20f), 0.3f);
            debug_rpm = new UI.TextElement("", new PointF(0.3f, 40f), 0.3f);
            debug_gear = new UI.TextElement("", new PointF(0.3f, 60f), 0.3f);
            debug_angle = new UI.TextElement("", new PointF(0.3f, 80f), 0.3f);
            debug_throttle = new UI.TextElement("", new PointF(0.3f, 100f), 0.3f);
            debug_braking = new UI.TextElement("", new PointF(0.3f, 120f), 0.3f);
            debug_clutch = new UI.TextElement("", new PointF(0.3f, 140f), 0.3f);
            debug_engineTemp = new UI.TextElement("", new PointF(0.3f, 160f), 0.3f);
            debug_fuelLevel = new UI.TextElement("", new PointF(0.3f, 180f), 0.3f);
            debug_worldPos = new UI.TextElement("", new PointF(0.3f, 200f), 0.3f);

            debugContainer.Items.Add(debug_speed);
            debugContainer.Items.Add(debug_wheelSpeed);
            debugContainer.Items.Add(debug_rpm);
            debugContainer.Items.Add(debug_gear);
            debugContainer.Items.Add(debug_angle);
            debugContainer.Items.Add(debug_throttle);
            debugContainer.Items.Add(debug_braking);
            debugContainer.Items.Add(debug_clutch);
            debugContainer.Items.Add(debug_engineTemp);
            debugContainer.Items.Add(debug_fuelLevel);
            debugContainer.Items.Add(debug_worldPos);
            debugContainer.Enabled = true;
        }

        void OnTick(object sender, EventArgs e)
        {
            if (isDebugEnabled)
            {
                ShowVehicleSensors();
            }
            
            if (isLoggingToFile)
            {
                LogToFile();
            }        
        }

        void LogToFile() // write logged sensors to output file every frame
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

            file.WriteLine(wheelSpeed + "," + rpm + "," + gear + "," + angle + "," 
                           + throttle + "," + braking + "," + clutch + "," 
                           + engineTemp + "," + posX + "," + posY);
        }

        void ShowVehicleSensors() // draws the debug display every frame (no flicker)
        {
            debug_speed.Caption      = "Speed: " + speed;
            debug_wheelSpeed.Caption = "Wheel speed: " + wheelSpeed;
            debug_rpm.Caption        = "RPM: " + rpm;
            debug_gear.Caption       = "Gear: " + gear;
            debug_angle.Caption      = "Steering angle: " + angle;
            debug_throttle.Caption   = "Throttle: " + throttle;
            debug_braking.Caption    = "Braking: " + braking;
            debug_clutch.Caption     = "Clutch: " + clutch;
            debug_engineTemp.Caption = "Engine temp: " + engineTemp;
            debug_fuelLevel.Caption  = "Fuel level: " + fuelLevel;
            debug_worldPos.Caption   = "Pos: " + worldPos;
            debugContainer.Draw();
        }

        void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == (Keys)Enum.Parse(typeof(Keys), cfgBeginKey))
            {
                SetupOutputFile();
                if (cfgDebug)
                {
                    isDebugEnabled = true;
                }
                Screen.ShowSubtitle("Logging vehicle signals..", 2000);
            }

            if (e.KeyCode == (Keys)Enum.Parse(typeof(Keys), cfgEndKey))
            {
                isLoggingToFile = false;
                isDebugEnabled = false;
                file.Flush();
                file.Close();
                Screen.ShowSubtitle("File write complete", 2000);
            }

        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using WindowsInput;
using WindowsInput.Native;

namespace ProclaimFieldsCreator
{
    public partial class Form1 : Form
    {
        private KeyHandler ghk; 

        private readonly List<Field> _fields = new List<Field>();
        private bool _settingNew = false;
        private bool _settingName = false;
        private bool _settingAlpha = false;
        private bool _emergencyStop = false;

        public Form1()
        {
            ghk = new KeyHandler(Keys.Escape, this);
            ghk.Register();

            InitializeComponent();
        }
        
        #region MOUSE CONTROL
        private new static void MouseClick()
        {
            var inputSimulator = new InputSimulator();
            inputSimulator.Mouse.LeftButtonClick();
        }

        private new static void MouseMove(Position position)
        {
            var inputSimulator = new InputSimulator();
            var x = (int)((65535.0f * (position.X / (float)Screen.PrimaryScreen.Bounds.Width)) + 0.5f);
            var y = (int)((65535.0f * (position.Y / (float)Screen.PrimaryScreen.Bounds.Height)) + 0.5f);
            inputSimulator.Mouse.MoveMouseTo(x,y);
        }

        #endregion

        #region KEYBOARD CONTROL
        private static void KeyboardPressTab()
        {
            var inputSimulator = new InputSimulator();
            inputSimulator.Keyboard.KeyPress(VirtualKeyCode.TAB);
        }

        private static void KeyboardTypeText(string text)
        {
            var inputSimulator = new InputSimulator();
            inputSimulator.Keyboard.TextEntry(text);
        }
        #endregion

        #region CSV INFO
        private void CvsInfoButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show(@"CVS file needs to have 5 columns:"
                            + Environment.NewLine
                            + Environment.NewLine
                            + @"[Type]: A - Alpha, N - Number, V - Value, D - Date"
                            + Environment.NewLine
                            + @"[Name]: Any proclaim allowed string (no commas!)"
                            + Environment.NewLine
                            + @"[Screen Label]: Any proclaim allowed string (no commas!)"
                            + Environment.NewLine
                            + @"[Initial Value]: String, Number or Decimal (date will default to None)"
                            + Environment.NewLine
                            + @"[Audited]: TRUE or FALSE (not case specific)"
                            + Environment.NewLine
                            + Environment.NewLine
                            + @"[EXAMPLE]: A,Name,ScreenLabel,Test,true"

                );
        }
        #endregion

        #region LOAD CSV
        private void LoadCsvFile_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog {Filter = @"CSV Files|*.csv"};
            var result = dialog.ShowDialog();
            if (result != DialogResult.OK) return;
            var content = File.ReadAllLines(dialog.FileName);
            Parse(content);
        }

        private void Parse(IEnumerable<string> content)
        {
            _fields.Clear();
            FieldsListBox.Items.Clear();
            foreach (var line in content)
            {
                if(string.IsNullOrWhiteSpace(line))continue;
                FieldsListBox.Items.Add(line);
                var parts = line.Split(',');
                var typeStr = parts[0];
                var type = ParseType(typeStr);
                var namne = parts[1];
                var label = parts[2];
                var value = parts[3];
                var auditStr = parts[4];
                var audit = ParseAudit(auditStr);
                _fields.Add(new Field()
                {
                    Type = type,
                    Name = namne,
                    ScreenLabel = label,
                    InitialValue = value,
                    Audited = audit
                });
            }
        }

        private static bool ParseAudit(string auditStr)
        {
            if(auditStr.ToLower() == "true") return true;
            if(auditStr.ToLower() == "false") return false;
            throw new Exception("Incorrect audited value: " + auditStr);
        }

        private static FieldType ParseType(string typeStr)
        {
            switch (typeStr.ToLower())
            {
                case "a": return FieldType.Alpha;
                case "n": return FieldType.Number;
                case "v": return FieldType.Value;
                case "d": return FieldType.Date;
            }
            throw new Exception("Incorrect field type: " + typeStr);
        }
        #endregion

        #region SETTINGS
        private void SetNewButton_Click(object sender, EventArgs e)
        {
            DisableGui();
            _settingNew = true;
        }
        
        private void SetNameButton_Click(object sender, EventArgs e)
        {
            DisableGui();
            _settingName = true;
        }

        private void SetAlphaButton_Click(object sender, EventArgs e)
        {
            DisableGui();
            _settingAlpha = true;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.F1) return;

            EnableGui();

            if (_settingNew)
            {
                NewPositionLabel.Text = SetPosition();
                _settingNew = false;
            }
            else if (_settingName)
            {
                NamePositionLabel.Text = SetPosition();
                _settingName = false;
            }
            else if (_settingAlpha)
            {
                AlphaPositionLabel.Text = SetPosition();
                _settingAlpha = false;
            }
        }

        private static string SetPosition()
        {
            var x = Cursor.Position.X;
            var y = Cursor.Position.Y;
            return $"{x},{y}";
        }

        private void DisableGui()
        {
            this.BackColor = Color.GreenYellow;
            SettingsGroupBox.Enabled = false;
            FieldsGroupBox.Enabled = false;
            CreateButton.Enabled = false;
        }

        private void EnableGui()
        {
            this.BackColor = DefaultBackColor;
            SettingsGroupBox.Enabled = true;
            FieldsGroupBox.Enabled = true;
            CreateButton.Enabled = true;
        }
        #endregion

        #region CREATE FIELDS
        private void CreateButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show(@"Press ESC at any time to stop the process.");
            _emergencyStop = false;
            Thread.Sleep(1000);

            var newPosition = new Position(NewPositionLabel.Text);
            var savePosition = new Position(NewPositionLabel.Text);
            savePosition.X -= 76;

            var namePosition = new Position(NamePositionLabel.Text);
            var auditPosition = new Position(NamePositionLabel.Text);
            auditPosition.Y += 142;

            var alphaPosition = new Position(AlphaPositionLabel.Text);
            var numberPosition = new Position(AlphaPositionLabel.Text);
            numberPosition.Y += 22;
            var valuePosition = new Position(AlphaPositionLabel.Text);
            valuePosition.Y += 22 + 22;
            var datePosition = new Position(AlphaPositionLabel.Text);
            datePosition.Y += 22 + 22 + 22;
            
            foreach (var field in _fields)
            {
                //Check for emergency exit
                Application.DoEvents();
                if (_emergencyStop) return;

                //New
                MouseMove(newPosition);
                MouseClick();
                Wait();

                //Type
                switch (field.Type)
                {
                    case FieldType.Alpha:
                        MouseMove(alphaPosition);
                        break;
                    case FieldType.Number:
                        MouseMove(numberPosition);
                        break;
                    case FieldType.Value:
                        MouseMove(valuePosition);
                        break;
                    case FieldType.Date:
                        MouseMove(datePosition);
                        break;
                }
                MouseClick();
                Wait();

                //Name
                MouseMove(namePosition);
                MouseClick();
                KeyboardTypeText(field.Name);
                KeyboardPressTab();

                //Screen label
                KeyboardTypeText(field.ScreenLabel);
                KeyboardPressTab();

                //Initial Value
                if (field.Type != FieldType.Date)
                {
                    KeyboardTypeText(field.InitialValue);
                }
                Wait();

                //Audit
                if (field.Audited)
                {
                    MouseMove(auditPosition);
                    MouseClick();
                }
                Wait();

                //Save
                MouseMove(savePosition);
                MouseClick();
                Wait();
            }

            MessageBox.Show(@"All done!");
        }

        private void Wait()
        {
            Thread.Sleep((int) DelayCounter.Value);
        }
        #endregion

        #region EMERGENCY STOP
        private void HandleHotkey()
        {
            _emergencyStop = true;
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == Constants.WM_HOTKEY_MSG_ID)
                HandleHotkey();
            base.WndProc(ref m);
        }
        #endregion
    }

    #region HELP CLASSES
    public static class Constants
    {
        //windows message id for hotkey
        public const int WM_HOTKEY_MSG_ID = 0x0312;
    }

    public class KeyHandler
    {
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private int key;
        private IntPtr hWnd;
        private int id;

        public KeyHandler(Keys key, Form form)
        {
            this.key = (int)key;
            this.hWnd = form.Handle;
            id = this.GetHashCode();
        }

        public override int GetHashCode()
        {
            return key ^ hWnd.ToInt32();
        }

        public bool Register()
        {
            return RegisterHotKey(hWnd, id, 0, key);
        }

        public bool Unregiser()
        {
            return UnregisterHotKey(hWnd, id);
        }
    }

    internal class Field
    {
        public FieldType Type { get; set; }
        public string Name { get; set; }
        public string ScreenLabel { get; set; }
        public string InitialValue { get; set; }
        public bool Audited { get; set; }
    }

    internal enum FieldType
    {
        Alpha,
        Number,
        Value,
        Date
    }

    internal class Position
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Position(string label)
        {
            var parts = label.Split(',');
            X = int.Parse(parts[0]);
            Y = int.Parse(parts[1]);
        }
    }

    #endregion
}

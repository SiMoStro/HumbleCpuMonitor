using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml.Serialization;

namespace HumbleCpuMonitor.Config
{
    /// <summary>
    /// The configuration of the application
    /// </summary>
    public class ConfigData
    {
        #region [private]

        private const int NUM = 10;

        #endregion

        #region [internals] Defaults

        [XmlIgnore]
        internal Color[] Defaults = {
            Color.Cyan,
            Color.Aquamarine,
            Color.MediumSpringGreen,
            Color.SpringGreen,
            Color.YellowGreen,
            Color.Yellow,
            Color.Gold,
            Color.Orange,
            Color.OrangeRed,
            Color.Red
        };

        #endregion

        /// <summary>
        /// Serialized value colors
        /// </summary>
        public string[] ValColor
        {
            get
            {
                string[] retVal = new string[NUM];
                for (int i = 0; i < NUM; i++) retVal[i] = Colors[i].ToHtmlColor();
                return retVal;
            }

            set
            {
                if (value.Length != NUM) return;
                for (int i = 0; i < NUM; i++) Colors[i] = value[i].FromHtmlColor();
                InitPensAndBrushes();
            }
        }

        /// <summary>
        /// Serialized Background
        /// </summary>
        public string ValBackground
        {
            get
            {
                return Background.ToHtmlColor();
            }

            set
            {
                Background = value.FromHtmlColor();
            }
        }

        /// <summary>
        /// Serialized foreground
        /// </summary>
        public string ValForeground
        {
            get
            {
                return Foreground.ToHtmlColor();
            }

            set
            {
                Foreground = value.FromHtmlColor();
            }
        }

        /// <summary>
        /// Serialized chart lines
        /// </summary>
        public string ValChartLines
        {
            get
            {
                return ChartLines.ToHtmlColor();
            }

            set
            {
                ChartLines = value.FromHtmlColor();
            }
        }

        #region [XML ignore] Ready-to-use properties derived from serialized values

        [XmlIgnore]
        internal Color Background { get; set; }

        [XmlIgnore]
        internal Color Foreground { get; set; }

        [XmlIgnore]
        internal Color ChartLines { get; set; }

        [XmlIgnore]
        internal Color[] Colors { get; set; }

        [XmlIgnore]
        internal Pen[] Pens { get; set; }

        [XmlIgnore]
        internal SolidBrush[] Brushes { get; set; }

        #endregion

        public ConfigData()
        {
            SetDefault();
            Background = Color.Black;
            Foreground = Color.White;
            ChartLines = Color.DarkGray;
        }

        #region Internal useful getters and setters

        /// <summary>
        /// Sets the colors to use for valued points
        /// </summary>
        /// <param name="colors">List of colors</param>
        internal void SetValueColors(List<Color> colors)
        {
            try
            {
                if (colors.Count != NUM) return;
                for (int i = 0; i < NUM; i++) Colors[i] = colors[i];
                InitPensAndBrushes();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Color associated to %
        /// </summary>
        /// <param name="val">Value in [0, 100]</param>
        /// <returns>Color</returns>
        internal Color GetColorForValue(double val)
        {
            return Colors[Idx(val)];
        }

        /// <summary>
        /// Pen associated to %
        /// </summary>
        /// <param name="val">Value in [0, 100]</param>
        /// <returns>Pen</returns>

        internal Pen GetPenForValue(double val)
        {
            return Pens[Idx(val)];
        }

        /// <summary>
        /// Brush associated to %
        /// </summary>
        /// <param name="val">Value in [0, 100]</param>
        /// <returns>Brush</returns>
        internal Brush GetBrushForValue(double val)
        {
            return Brushes[Idx(val)];
        }

        #endregion

        #region Load and Save

        internal void LoadData(string filename)
        {
            try
            {
                FileInfo file = new FileInfo(filename);
                if (!file.Exists) return;

                string serializedData = File.ReadAllText(filename);
                XmlSerializer xmlSerializer = new XmlSerializer(GetType());
                StringReader textWriter = new StringReader(serializedData);
                ConfigData cd = (ConfigData) xmlSerializer.Deserialize(textWriter);
                
                ValColor = cd.ValColor;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        internal void SaveData(string filename)
        {
            string saveData = null;
            XmlSerializer xmlSerializer = new XmlSerializer(GetType());
            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, this);
                saveData = textWriter.ToString();
            }
            if (saveData == null) return;
            FileInfo fi = new FileInfo(filename);
            if (!fi.Directory.Exists) fi.Directory.Create();
            fi.Directory.Refresh();
            if (!fi.Directory.Exists) return;
            if (fi.Exists) fi.Delete();
            File.WriteAllText(filename, saveData);
        }

        #endregion

        #region Utilities and initialization

        internal void SetDefault()
        {
            Colors = new Color[NUM];
            for (int i = 0; i < NUM; i++) Colors[i] = Defaults[i];
            InitPensAndBrushes();
        }

        private void InitPensAndBrushes()
        {
            Pens = new Pen[NUM];
            Brushes = new SolidBrush[NUM];
            for(int i = 0; i < NUM; i++)
            {
                Pens[i] = new Pen(Colors[i]);
                Brushes[i] = new SolidBrush(Colors[i]);
            }
        }

        private int Idx(double val)
        {
            if (val < 0) val = 0;
            if (val > 100) val = 100;
            int idx = (int)(val / NUM);

            if (idx >= NUM) return (NUM - 1);
            return idx;
        }

        #endregion
    }
}

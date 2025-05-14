using HumbleCpuMonitor.Charts;
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

        /// <summary>
        /// The X coordinate of the Main Chart window
        /// </summary>
        public int? MainWinX { get; set; }

        /// <summary>
        /// The Y coordinate of the Main Chart window
        /// </summary>
        public int? MainWinY { get; set; }

        /// <summary>
        /// The Width of the Main Chart window
        /// </summary>
        public int? MainWinWidth { get; set; }

        /// <summary>
        /// The Height of the Main Chart window
        /// </summary>
        public int? MainWinHeight { get; set; }

        /// <summary>
        /// True if the Main Chart window is caption-less, false otherwise
        /// </summary>
        public bool MainWinCaptionLess { get; set; }

        /// <summary>
        /// Chart type in use
        /// </summary>
        public ChartType ChartType { get; set; }

        /// <summary>
        /// Machine Info panel X position
        /// </summary>
        public int? MachineInfoX { get; set; }

        /// <summary>
        /// Machine Info panel Y position
        /// </summary>
        public int? MachineInfoY { get; set; }

        /// <summary>
        /// Top Processes Info panel X position
        /// </summary>
        public int? TopProcsInfoX { get; set; }

        /// <summary>
        /// Top Processes Info panel Y position
        /// </summary>
        public int? TopProcsInfoY { get; set; }

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

        [XmlIgnore]
        internal Point? MachineInfoLocation
        {
            get
            {
                if (MachineInfoX.HasValue && MachineInfoY.HasValue) return new Point(MachineInfoX.Value, MachineInfoY.Value);
                return null;
            }
        }

        [XmlIgnore]
        internal Point? TopProcsInfoLocation
        {
            get
            {
                if (TopProcsInfoX.HasValue && TopProcsInfoY.HasValue) return new Point(TopProcsInfoX.Value, TopProcsInfoY.Value);
                return null;
            }
        }

        #endregion

        public ConfigData()
        {
            SetDefault();
            Background = Color.Black;
            Foreground = Color.White;
            ChartLines = Color.DarkGray;
            ChartType = ChartType.Bar;
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
                ConfigData cd = SerializationExt.DeserializeFromFile<ConfigData>(filename);
                if (cd == null) return;
                InitFromInstance(cd);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        internal void SaveData(string filename)
        {
            this.SerializeToFile(filename);
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

        private void InitFromInstance(ConfigData cd)
        {
            ValColor = cd.ValColor;
            Foreground = cd.Foreground;
            Background = cd.Background;
            ChartLines = cd.ChartLines;
            MainWinX = cd.MainWinX;
            MainWinY = cd.MainWinY;
            MachineInfoX = cd.MachineInfoX;
            MachineInfoY = cd.MachineInfoY;
            TopProcsInfoX = cd.TopProcsInfoX;
            TopProcsInfoY = cd.TopProcsInfoY;
            MainWinHeight = cd.MainWinHeight;
            MainWinWidth = cd.MainWinWidth;
            MainWinCaptionLess = cd.MainWinCaptionLess;
            ChartType = cd.ChartType;
        }

        #endregion
    }
}

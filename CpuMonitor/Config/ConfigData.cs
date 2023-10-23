using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml.Serialization;

namespace HumbleCpuMonitor.Config
{
    public class ConfigData
    {
        private const int NUM = 10;
        
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

        [XmlIgnore]
        public Color[] Colors { get; set; }

        [XmlIgnore]
        internal Pen[] Pens { get; set; }

        [XmlIgnore]
        internal SolidBrush[] Brushes { get; set; }

        public ConfigData()
        {
            SetDefault();
        }

        #region Getters

        /// <summary>
        /// Color associated to %
        /// </summary>
        /// <param name="val">Value in [0, 100]</param>
        /// <returns>Color</returns>
        internal Color GetColor(double val)
        {
            return Colors[Idx(val)];
        }

        /// <summary>
        /// Pen associated to %
        /// </summary>
        /// <param name="val">Value in [0, 100]</param>
        /// <returns>Pen</returns>

        internal Pen GetPen(double val)
        {
            return Pens[Idx(val)];
        }

        internal void SetColors(List<Color> colors)
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
        /// Brush associated to %
        /// </summary>
        /// <param name="val">Value in [0, 100]</param>
        /// <returns>Brush</returns>
        internal Brush GetBrush(double val)
        {
            return Brushes[Idx(val)];
        }

        #endregion

        public void LoadData(string filename)
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

        public void SaveData(string filename)
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

        #region internals and private

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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Minesweeper.GUI.UserControls
{
    public partial class SectorUC : UserControl
    {
        #region types
        public enum SectorState
        {
            UNREVEALED = 0,
            REVEALED = 1,
            MARKED = 2
        }
        public enum SectorClickButton
        {
            LEFT_CLICK = 0,
            RIGHT_CLICK = 1,
            DOUBLE_CLICK = 2
        }
        public class SectorClickEventArgs : EventArgs
        {
            private SectorClickButton clickButton;

            public SectorClickButton ClickButton
            {
                get { return clickButton; }
                set { clickButton = value; }
            }

            public SectorClickEventArgs(SectorClickButton clickButton)
            {
                ClickButton = clickButton;
            }
        }
        #endregion

        #region events
        #region SectorClickedEvent
        public event EventHandler<SectorClickEventArgs> SectorClicked;

        protected void OnSectorClicked(SectorClickEventArgs e)
        {
            if (SectorClicked != null)
                SectorClicked(this, e);
        }

        private void button_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                SectorClickEventArgs args = new SectorClickEventArgs(SectorClickButton.RIGHT_CLICK);
                OnSectorClicked(args);
                ToggleMarked();
            }
        }

        private void button_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Middle)
            {
                SectorClickEventArgs args = new SectorClickEventArgs(SectorClickButton.DOUBLE_CLICK);
                OnSectorClicked(args);
                Reveal();
            }
        }

        private void button_MouseClick(object sender, MouseEventArgs e)
        {
            if (state == SectorState.MARKED)
                return;
            SectorClickEventArgs args = new SectorClickEventArgs(SectorClickButton.LEFT_CLICK);
            OnSectorClicked(args);
            Reveal();
        }
        private void button_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            SectorClickEventArgs ee = new SectorClickEventArgs(SectorClickButton.DOUBLE_CLICK);
            OnSectorClicked(ee);
            Reveal();
        }
        #endregion
        #endregion

        #region attributs and proprties
        private bool hasMine;
        private int number;
        private Image flagImage;
        private Image mineImage;
        private SectorState state;
        private int xIndex;
        private int yIndex;


        public int YIndex
        {
            get { return yIndex; }
            set { yIndex = value; }
        }

        public int XIndex
        {
            get { return xIndex; }
            set { xIndex = value; }
        }

        public SectorState State
        {
            get { return state; }
            set { state = value; }
        }

        public Color UnrevealedSectorColor
        {
            get { return button.BackColor; }
            set { button.BackColor = value; }
        }

        public Color TextColor
        {
            get
            {
                return label.ForeColor;
            }

            set
            {
                label.ForeColor = value;
            }
        }

        public Image MineImage
        {
            get { return mineImage; }
            set
            {
                mineImage = value;
            }
        }

        public Image FlagImage
        {
            get { return flagImage; }
            set { flagImage = value; }
        }

        public int Number
        {
            get { return number; }
            set
            {
                number = value;
                label.Text = value.ToString();
            }
        }

        public bool HasMine
        {
            get { return hasMine; }
            set { hasMine = value; }
        }
        #endregion

        #region constractors
        public SectorUC()
        {
            InitializeComponent();
            UnrevealedSectorColor = Color.SteelBlue;
            TextColor = Color.Black;
            button.MouseDoubleClick += new MouseEventHandler(button_MouseDoubleClick);
        }
        #endregion

        #region methods
        public void PerformLeftClick()
        {
            State = SectorState.UNREVEALED;
            BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            button.PerformClick();
        }

        public void ToggleMarked()
        {
            if (State == SectorState.MARKED)
                UnMark();
            else
                Mark();
        }

        public void Mark()
        {
            State = SectorState.MARKED;

            if (FlagImage != null)
                button.BackgroundImage = FlagImage;
        }

        public void UnMark()
        {
            State = SectorState.UNREVEALED;
            button.BackgroundImage = null;
        }

        public void Reveal()
        {
            if (HasMine && MineImage != null)
            {
                panel.BackgroundImage = MineImage;
            }
            else if (Number > 0)
            {
                label.Show();
            }
            else
            {
                label.Hide();
            }

            button.Hide();
        }
        #endregion

    }
}

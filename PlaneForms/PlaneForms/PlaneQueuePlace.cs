using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlaneForms
{
    public class PlaneQueuePlace
    {
        private System.Windows.Forms.Panel panel;
        private Row row;
        private bool isOccupied;
        private PlaneQueuePlace frontQueuePlace;
        private PlaneQueuePlace behindQueuePlace;

        public PlaneQueuePlace(Panel panel, Row row, PlaneQueuePlace frontQueuePlace, PlaneQueuePlace behindQueuePlace)
        {
            this.panel = panel;
            this.row = row;
            this.frontQueuePlace = frontQueuePlace;
            this.behindQueuePlace = behindQueuePlace;
            this.IsOccupied = false;
        }

        public Panel Panel { get => panel; set => panel = value; }
        internal Row Row { get => row; set => row = value; }
        internal PlaneQueuePlace FrontQueuePlace { get => frontQueuePlace; set => frontQueuePlace = value; }
        internal PlaneQueuePlace BehindQueuePlace { get => behindQueuePlace; set => behindQueuePlace = value; }

        public bool IsOccupied
        {
            get
            {
                return this.isOccupied;
            }
            set
            {
                if(this.isOccupied != value)
                {
                    this.isOccupied = value;
                    CheckOccupationStatus();
                }
            }
        }

        public void ResetPlaneQueue()
        {
            this.isOccupied = false;
        }

        public void CheckOccupationStatus()
        {
            if(isOccupied)
            {
                panel.BackColor = Color.Red;
            }
            else
            {
                panel.BackColor = SystemColors.InactiveCaption;
            }
        }

    }
}

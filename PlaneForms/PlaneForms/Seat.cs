using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlaneForms
{
    public class Seat
    {
        private int seatNumber;
        private bool isWindowSeat;
        private bool isOccupied;
        private bool isAssigned;
        private System.Windows.Forms.Panel panel;

        public Seat(int seatNumber, Panel panel)
        {
            this.seatNumber = seatNumber;
            this.panel = panel;
            this.isOccupied = false;
            this.isWindowSeat = false;
            this.isAssigned = false;
        }

        public int SeatNumber { get => seatNumber; set => seatNumber = value; }
        public bool IsWindowSeat { get => isWindowSeat; set => isWindowSeat = value; }
        public bool IsOccupied { get => isOccupied; set => isOccupied = value; }
        public Panel Panel { get => panel; set => panel = value; }
        public bool IsAssigned { get => isAssigned; set => isAssigned = value; }

        public void SetOccupied()
        {
            this.IsOccupied = true;
            this.Panel.BackColor = Color.Red;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaneForms
{
    public class Row
    {
        private List<Seat> upperSeats;
        private List<Seat> lowerSeats;
        private int rowNumber;

        public Row(int rowNumber, Seat upperWindowSeat, Seat upperAisleSeat, Seat lowerAisleSeat, Seat lowerWindowSeat)
        {
            this.rowNumber = rowNumber;
            upperSeats = new List<Seat>();
            upperWindowSeat.IsWindowSeat = true;
            upperSeats.Add(upperAisleSeat);
            upperSeats.Add(upperWindowSeat);
            lowerSeats = new List<Seat>();
            lowerWindowSeat.IsWindowSeat = true;
            LowerSeats.Add(lowerAisleSeat);
            lowerSeats.Add(lowerWindowSeat);

        }

        public int RowNumber { get => rowNumber; set => rowNumber = value; }
        internal List<Seat> UpperSeats { get => upperSeats; set => upperSeats = value; }
        internal List<Seat> LowerSeats { get => lowerSeats; set => lowerSeats = value; }

        public void ResetRow()
        {
            foreach (Seat item in upperSeats)
            {
                item.IsOccupied = false;
                item.Panel.BackColor = SystemColors.InactiveCaption;
            }
            foreach (Seat item in lowerSeats)
            {
                item.IsOccupied = false;
                item.Panel.BackColor = SystemColors.InactiveCaption;
            }
        }

        public bool Interference(int seatNumber)
        {
            bool interference = false;
            if(UpperSeats.Exists(a => a.SeatNumber == seatNumber))
            {
                interference = UpperSeats.Find(a => a.SeatNumber != seatNumber).IsOccupied ? true : false;
            }
            else
            {
                interference = UpperSeats.Find(a => a.SeatNumber != seatNumber).IsOccupied ? true : false;
            }
            return interference;
        }
    }
}

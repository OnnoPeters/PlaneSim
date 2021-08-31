using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaneForms
{
    public class Person
    {
        private int id;
        private Seat assignedSeat;
        private Row assignedRow;
        private PlaneQueuePlace currentPos;
        private int interferenceCounter = 0;
        bool interfered = false;
        bool luggaged = false;
        bool seated = false;
        private int luggageCounter = 0;

        public Person(int id, Seat assignedSeat, Row assignedRow)
        {
            this.id = id;
            this.assignedSeat = assignedSeat;
            this.assignedRow = assignedRow;
            this.currentPos = null;
        }

        public int Id { get => id; set => id = value; }
        internal Seat AssignedSeat { get => assignedSeat; set => assignedSeat = value; }
        internal Row AssignedRow { get => assignedRow; set => assignedRow = value; }
        internal PlaneQueuePlace CurrentPos { get => currentPos; set => currentPos = value; }
        public bool Seated { get => seated; set => seated = value; }

        public Row GetCurrentRow()
        {
            return CurrentPos.Row;
        }

        public void ResetPerson()
        {
            this.currentPos = null;
            this.interferenceCounter = 0;
            this.interfered = false;
            this.luggaged = false;
            this.seated = false;
            this.luggageCounter = 0;
        }

        public bool CheckIfRowIsMyRow()
        {
            bool isMyRow = false;
            if (GetCurrentRow() != null && GetCurrentRow().RowNumber == assignedRow.RowNumber)
            {
                isMyRow = true;
            }
            return isMyRow;
        }

        public void Step()
        {
            if (currentPos.FrontQueuePlace != null && !currentPos.FrontQueuePlace.IsOccupied)
            {
                CurrentPos.IsOccupied = false;
                CurrentPos = currentPos.FrontQueuePlace;
                CurrentPos.IsOccupied = true;
            }
            else if (currentPos.FrontQueuePlace == null)
            {
                Console.WriteLine(this.Id + " " + AssignedRow.RowNumber + AssignedSeat.SeatNumber + "tried to enter null");
            }

        }

        public void CheckAndEnterSeat()
        {
            if (currentPos.Row.RowNumber == AssignedRow.RowNumber)
            {
                if (luggaged)
                {
                    if (AssignedSeat.IsWindowSeat)
                    {
                        CheckForInterference();
                    }
                    else
                    {
                        if (assignedSeat.SeatNumber == 3)
                        {
                            CurrentPos.Row.LowerSeats[0].SetOccupied();
                        }
                        else
                        {
                            CurrentPos.Row.UpperSeats[0].SetOccupied();
                        }
                        CurrentPos.IsOccupied = false;
                        Seated = true;
                    }
                }
                else
                {
                    luggageCounter = 15;
                    luggaged = true;
                }
            }
        }

        public void CheckForInterference()
        {
            if (currentPos.Row.Interference(assignedSeat.SeatNumber))
            {
                if (interfered)
                {
                    if (assignedSeat.SeatNumber == 4)
                    {
                        CurrentPos.Row.LowerSeats[1].SetOccupied();
                    }
                    else
                    {
                        CurrentPos.Row.UpperSeats[1].SetOccupied();
                    }
                    CurrentPos.IsOccupied = false;
                    Seated = true;
                }
                else
                {
                    interferenceCounter = 7;
                    interfered = true;
                }
            }
            else
            {
                if (assignedSeat.SeatNumber == 4)
                {
                    CurrentPos.Row.LowerSeats[1].SetOccupied();
                }
                else
                {
                    CurrentPos.Row.UpperSeats[1].SetOccupied();
                }
                CurrentPos.IsOccupied = false;
                Seated = true;
            }
        }

        public void Action()
        {
            if (!Seated)
            {
                if (luggageCounter == 0)
                {
                    if (interferenceCounter == 0)
                    {
                        if (CheckIfRowIsMyRow())
                        {
                            CheckAndEnterSeat();
                        }
                        else
                        {
                            Step();
                        }
                    }
                    else
                    {
                        interferenceCounter--;
                    }

                }
                else
                {
                    luggageCounter--;
                }
            }
        }

    }
}

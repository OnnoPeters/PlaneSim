using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlaneForms
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// Counts the ticks in a simulation run.
        /// </summary>
        private int timeCounter = 0;

        /// <summary>
        /// Counts the number of people having entered the queue.
        /// </summary>
        private int peopleEntered = 0;

        /// <summary>
        /// 
        /// </summary>
        private List<Row> rows;
        private List<PlaneQueuePlace> queuePlaces;
        private List<PlaneQueuePlace> queue;
        private List<Person> people;
        private List<Person> activePeople;
        private bool randomSimulation = false;
        private List<Row> maxRows;
        private List<PlaneQueuePlace> maxQueuePlaces;
        private List<Person> maxPeople;

        public Form1()
        {
            InitializeComponent();
        }


        public List<Row> InitializeRows()
        {
            List<Row> rows = new List<Row>();
            for (int i = 0; i < 25; i++)
            {
                List<Panel> panels = this.Controls.OfType<Panel>().Where(a => a.Name.StartsWith("Seat" + (i + 1).ToString())).ToList();
                Panel[] orderedPanels = new Panel[4];
                foreach (Panel panel in panels)
                {
                    if (panel.Name == $"Seat{i + 1}1")
                    {
                        orderedPanels[0] = panel;
                    }
                    else if (panel.Name == $"Seat{i + 1}2")
                    {
                        orderedPanels[1] = panel;
                    }
                    else if (panel.Name == $"Seat{i + 1}3")
                    {
                        orderedPanels[2] = panel;
                    }
                    else if (panel.Name == $"Seat{i + 1}4")
                    {
                        orderedPanels[3] = panel;
                    }
                }
                Row row = new Row(i + 1, new Seat(1, orderedPanels[0]), new Seat(2, orderedPanels[1]), new Seat(3, orderedPanels[2]), new Seat(4, orderedPanels[3]));
                rows.Add(row);
            }
            return rows;
        }

        public List<PlaneQueuePlace> InitializePlaneQueuePlace(List<Row> rows)
        {
            List<PlaneQueuePlace> planeQueuePlaces = new List<PlaneQueuePlace>();
            PlaneQueuePlace prevPlaneQueuePlace = queue[49];
            for (int i = 0; i < 50; i++)
            {
                Panel panel = this.Controls.OfType<Panel>().Where(a => a.Name == ("panelQueue" + (i + 1).ToString())).First();
                Row row = i % 2 == 0 ? null : rows.ElementAt((int)(i / 2));
                PlaneQueuePlace queuePlace = new PlaneQueuePlace(panel, row, null, prevPlaneQueuePlace);
                if (prevPlaneQueuePlace != queue[49]) prevPlaneQueuePlace.FrontQueuePlace = queuePlace;
                planeQueuePlaces.Add(queuePlace);
                prevPlaneQueuePlace = queuePlace;
            }
            this.queue[49].FrontQueuePlace = planeQueuePlaces[0];
            return planeQueuePlaces;
        }

        public List<PlaneQueuePlace> InitializeQueue()
        {
            List<PlaneQueuePlace> queue = new List<PlaneQueuePlace>();
            PlaneQueuePlace prevPlaneQueuePlace = null;
            for (int i = 0; i < 50; i++)
            {
                Panel panel = this.Controls.OfType<Panel>().Where(a => a.Name == ("wait" + (i + 1).ToString())).First();
                PlaneQueuePlace queuePlace = new PlaneQueuePlace(panel, null, null, prevPlaneQueuePlace);
                if (prevPlaneQueuePlace != null) prevPlaneQueuePlace.FrontQueuePlace = queuePlace;
                queue.Add(queuePlace);
                prevPlaneQueuePlace = queuePlace;
            }
            return queue;
        }

        public List<Person> InitializePeople(List<Row> rows)
        {
            Random random = new Random();
            List<Person> people = new List<Person>();
            List<Person> drawPeople = new List<Person>();
            for (int i = 0; i < 100; i++)
            {
                drawPeople.Add(new Person(i, null, null));
            }
            for (int i = 0; i < 100; i++)
            {
                int rand = random.Next(0, drawPeople.Count);
                drawPeople[rand].AssignedRow = rows[(int)(i / 4)];
                drawPeople[rand].AssignedSeat = i % 4 > 1 ? rows[(int)(i / 4)].LowerSeats[(int)(i % 4) == 2 ? 0 : 1] : rows[(int)(i / 4)].UpperSeats[(int)(i % 4) == 0 ? 0 : 1];
                drawPeople[rand].AssignedSeat.IsAssigned = true;
                people.Add(drawPeople[rand]);
                drawPeople.Remove(drawPeople[rand]);
            }
            return people;
        }

        public void Initialize()
        {
            this.rows = InitializeRows();
            this.queue = InitializeQueue();
            this.queuePlaces = InitializePlaneQueuePlace(rows);
            this.people = InitializePeople(rows);
            this.activePeople = new List<Person>();
        }

        public void Action()
        {
            timeCounter++;
            lblPool.Text = this.people.Count.ToString();
            lblTime.Text = timeCounter.ToString();
            if (!queue[0].IsOccupied)
            {
                if (people.Count > 0)
                {
                    Person person = people.Where(a => a.Id == peopleEntered).Single();
                    person.CurrentPos = this.queue[0];
                    this.queue[0].IsOccupied = true;
                    this.activePeople.Add(person);
                    this.people.Remove(person);
                    peopleEntered++;
                }
            }
            foreach (Person activePerson in activePeople)
            {
                activePerson.Action();
            }

        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (this.randomSimulation)
            {
                lblNum.Visible = true;
                int timeNeededOpt = -1;
                int numberOfRuns;
                if (Int32.TryParse(textBoxML.Text, out numberOfRuns))
                {
                    for (int i = 0; i < numberOfRuns; i++)
                    {
                        DoAction();
                        if (timeCounter < timeNeededOpt || timeNeededOpt == -1)
                        {
                            timeNeededOpt = timeCounter;
                            Console.WriteLine("NewOpt:" + timeNeededOpt);
                            this.maxPeople = activePeople;
                            this.maxRows = rows;
                            this.maxQueuePlaces = queuePlaces;
                        }
                        Console.WriteLine(timeCounter);
                        Initialize();
                        timeCounter = 0;
                        peopleEntered = 0;
                        lblNum.Text = i.ToString();
                        Waiting.wait(1);
                    }
                    foreach (Person person in maxPeople)
                    {
                        person.ResetPerson();
                    }
                    foreach (Row row in maxRows)
                    {
                        row.ResetRow();
                    }
                    foreach (PlaneQueuePlace queuePlace in maxQueuePlaces)
                    {
                        queuePlace.ResetPlaneQueue();
                    }
                    this.people = maxPeople;
                    this.rows = maxRows;
                    this.queuePlaces = maxQueuePlaces;
                    this.btnOptRestart.Visible = true;
                    lblFinal.Text = "Opt.: " + timeNeededOpt.ToString() + " Sec.";
                }
                else
                {
                    MessageBox.Show("Value not valid Integer!");
                }
            }
            else
            {
                DoAction();
            }

        }

        public void DoAction()
        {
            while (this.people.Count > 0 || this.activePeople.Where(a => !a.Seated).Count() > 0)
            {
                DoActionOnce();
                if (!this.randomSimulation) Waiting.wait(1);
            }
            lblFinal.Visible = true;
            lblFinal.Text = lblTime.Text + " Seconds";
        }

        public void DoActionOnce()
        {
            Action();
        }


        private void Form1_Shown(object sender, EventArgs e)
        {
            Initialize();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                this.randomSimulation = true;
                textBoxML.Visible = true;
                lblNumberML.Visible = true;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                this.randomSimulation = false;
                textBoxML.Visible = false;
                lblNumberML.Visible = false;
            }
        }

        private void btnOptRestart_Click(object sender, EventArgs e)
        {
            this.randomSimulation = false;
            DoAction();
        }
    }
}

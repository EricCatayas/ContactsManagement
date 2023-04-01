using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ContactsManagement.Core.Domain.Entities.ContactsManager
{
    class Habit
    {
        public int HabitId;
        public string HabitName;
        public double MaxPoints;
        public double CurrentPoints;
        bool isMaxedOut;
    }

    class HabitTracker
    {
        double MaxedPoints;
        public int Level;
        public double PlayerPoints;
        public List<Habit> GoodHabits;
        public List<Habit> BadHabits;

        public HabitTracker()
        {
            GoodHabits = new List<Habit>();
            BadHabits = new List<Habit>();
        }

        public double GetPlayerPoints()
        {
            double totalPlayerPoints = 0;
            foreach (Habit habit in GoodHabits)
            {
                totalPlayerPoints += habit.CurrentPoints;
            }
            foreach (Habit habit in BadHabits)
            {
                totalPlayerPoints += habit.CurrentPoints;
            }
            PlayerPoints = totalPlayerPoints;
            return PlayerPoints;
        }

        public double GetMaxedPoints()
        {
            double totalMaxedPoints = 0;
            foreach (Habit habit in GoodHabits)
            {
                totalMaxedPoints += habit.MaxPoints;
            }
            foreach (Habit habit in BadHabits)
            {
                totalMaxedPoints += habit.MaxPoints;
            }
            MaxedPoints = totalMaxedPoints;
            return MaxedPoints;
        }

        public void EvaluateGoodHabitParticipation(int range, Habit habit)
        {
            double pointsToAdd = 0;
            double ratioDeduction;
            double progressRatio =  habit.CurrentPoints / habit.MaxPoints;

            // Assessing Level
            if (Level > 4)
            {
                ratioDeduction = 0.1; // 10% for 5
            }
            else if (Level > 3)
            {
                ratioDeduction = 0.2; // 20% for 4
            }
            else
            {
                ratioDeduction = 0.3; // 30% for below
            }

            double deviation = habit.CurrentPoints * ratioDeduction;  

            //Assessing Points
            if (range >= 6 && range <= 9)
            {
                double factor = ratioDeduction * progressRatio; 
                pointsToAdd = (range - 5) / factor;               // Lowest: (9-5) / (0.3 * 0.06) = 200 ||  Highest: (9-5) / (0.1 * 0.093) = 42
            }
            else if (range >= 1 && range <= 4)
            {
                double factor = ratioDeduction * progressRatio * -1; 
                pointsToAdd =  ((3+Level)-range) / factor;
            }

            habit.CurrentPoints = Math.Min(habit.MaxPoints, habit.CurrentPoints + pointsToAdd);

            GetPlayerPoints();
        }
        public void ColdTurkey(Habit habit)
        {
            double pointsToAdd = 0;

            double progressRatio = habit.MaxPoints / habit.CurrentPoints;


            double factor = 100 * progressRatio;
            pointsToAdd = (6 - Level) * factor;

            habit.CurrentPoints = Math.Min(habit.MaxPoints, habit.CurrentPoints + pointsToAdd);

            GetPlayerPoints();
        }
        public void Relapse(Habit habit)
        {
            double ratioDeduction;

            if (Level >= 4)
            {
                ratioDeduction = 4.0 / 5.0; // 80% deduction for level 4 and above
            }
            else
            {
                ratioDeduction = 3.0 / 4.0; // 75% deduction for level 3 and below
            }

            //double progressRatio = habit.MaxPoints / habit.CurrentPoints;
            //double levelMultiplier = Math.Pow(2, Level - progressRatio); // x^y
            //ratioDeduction *= levelMultiplier;

            habit.CurrentPoints -= (habit.CurrentPoints * ratioDeduction);

            GetPlayerPoints();
        }


        public void UpdateLevel()
        {
            double pointsRatio = PlayerPoints / MaxedPoints;
            if (pointsRatio >= 0.8 && Level < 5)
            {
                Level++;
            }
            else if (pointsRatio <= 0.5 && Level > 1)
            {
                Level--;
            }
        }
    }

}

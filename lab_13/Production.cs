using System;
using System.Collections;

namespace lab_13
{
    public class Production : ICloneable
    {
        private int _workersNumber;

        public int WorkersNumber
        {
            get => _workersNumber;
            set => _workersNumber = value;
        }

        public Production()
        {
            Random rnd = new Random();
            _workersNumber = rnd.Next(0, 100);
        }

        public virtual void ShowInfo()
        {
            Console.WriteLine($"Workers: {WorkersNumber}");
        }

        public object Clone()
        {
            return new Production {WorkersNumber = WorkersNumber};
        }

        public override string ToString()
        {
            return $"Workers: {WorkersNumber}";
        }
    }

    // sorting
    public class SortByWorkersNumber : IComparer
    {
        int IComparer.Compare(object ob1, object ob2)
        {
            Production s1 = (Production) ob1;
            Production s2 = (Production) ob2;
            if (s2 != null && s1 != null && s1.WorkersNumber == s2.WorkersNumber)
            {
                return 0;
            }

            return s2 != null && s1 != null && s1.WorkersNumber > s2.WorkersNumber ? 1 : -1;
        }
    }
}
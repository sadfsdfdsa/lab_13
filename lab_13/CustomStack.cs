using System;
using System.Collections;
using System.Collections.Generic;
using lab_13;


namespace lab_13
{
    public class Point
    {
        public Production data; //информационное поле
        public Point next, prev;

        public Point() //конструктор без параметров
        {
            data = null;
            next = null;
            prev = null;
        }

        public Point(Production d) //конструктор с параметрами
        {
            data = d;
            next = null;
            prev = null;
        }

        public override string ToString()
        {
            return data + " ";
        }
    }

    public class CustomStack<T> : IEnumerable, IEnumerator
    {
        public int Length;
        public Point Point;
        public int position = -1;

        public static Point CreateLinkedList(int length)
        {
            if (length <= 0)
            {
                Console.WriteLine("Invalid Length of the List, need to be > 0");
                return null;
            }

            Random rnd = new Random();
            Point beg = new Point(new Production {WorkersNumber = rnd.Next(0, 100)});

            Point nowPoint = beg;
            for (int i = 1; i < length; i++)
            {
                Point newPoint = new Point(new Production {WorkersNumber = rnd.Next(0, 100)});
                nowPoint.next = newPoint;
                nowPoint = newPoint;
            }

            return beg;
        }

        public CustomStack()
        {
        }

        public CustomStack(int length)
        {
            Length = length;
            Point = CreateLinkedList(length);
        }

        public void Add(Production item)
        {
            Point tmp = new Point(item);
            tmp.next = Point;
            Point = tmp;
            Length++;
        }

        public void Delete(int begin, int count = 1)
        {
            Point tmp = Point;
            for (int i = 0; i < begin - 1; i++)
            {
                tmp = tmp.next;
            }

            Point tmpDelete = tmp.next;
            for (int i = 0; i < count; i++)
            {
                tmpDelete = tmpDelete.next;
            }

            tmp.next = tmpDelete;

            if (begin + count <= Length)
            {
                Length -= count;
            }
            else
            {
                Length -= count - Length + begin;
            }
        }

        public Production FindByValue(int workersNumber)
        {
            int count = 1;
            foreach (Production item in this)
            {
                if (item.WorkersNumber == workersNumber)
                {
                    Console.WriteLine("Элемент под номером " + count);
                    return item;
                }

                count++;
            }

            Console.WriteLine("Элемента нет в стеке");
            return null;
        }

        public Production this[int index]
        {
            get
            {
                Point tmp = Point;
                for (int i = 0; i < Length; i++)
                {
                    if (i == index)
                    {
                        return tmp.data;
                    }

                    tmp = tmp.next;
                }

                return null;
            }
            set
            {
                Point tmp = Point;
                for (int i = 0; i < Length; i++)
                {
                    if (i == index)
                    {
                        tmp.data = value;
                        break;
                    }
                    tmp = tmp.next;
                }
            }
        }

        public override string ToString()
        {
            ShowList(Point);
            return "";
        }

        public static void ShowList(Point point)
        {
            if (point == null)
            {
                Console.WriteLine("The List is empty");
                return;
            }

            Point p = point;
            while (p != null)
            {
                p.data.ShowInfo();
                p = p.next;
            }

            Console.WriteLine();
        }

        public IEnumerator GetEnumerator()
        {
            return new CustomStackEnumerator(Length, Point);
        }

        public object Current
        {
            get
            {
                if (position == -1 || position >= Length)
                    throw new InvalidOperationException();
                Point tmp = Point;
                for (int i = 0; i < position; i++)
                {
                    tmp = tmp.next;
                }

                return tmp.data;
            }
        }

        public bool MoveNext()
        {
            if (position < Length - 1)
            {
                position++;
                return true;
            }

            return false;
        }

        public void Reset()
        {
            position = -1;
        }
    }

    public class CustomStackEnumerator : IEnumerator<Production>
    {
        public int Length;
        public Point Point;
        public int position = -1;
        private Production _current;

        public CustomStackEnumerator(int length, Point point)
        {
            Point = point;
            Length = length;
        }

        public object Current
        {
            get
            {
                if (position == -1 || position >= Length)
                    throw new InvalidOperationException();
                Point tmp = Point;
                for (int i = 0; i < position; i++)
                {
                    tmp = tmp.next;
                }

                _current = tmp.data;
                return tmp.data;
            }
        }

        public bool MoveNext()
        {
            if (position < Length - 1)
            {
                position++;
                return true;
            }

            return false;
        }

        public void Reset()
        {
            position = -1;
        }

        Production IEnumerator<Production>.Current => _current;

        public void Dispose()
        {
        }
    }
}
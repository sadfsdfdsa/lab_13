using System;
using System.Collections.Generic;

namespace lab_13
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            NewCustomStack stack1 = new NewCustomStack("First", 5);
            NewCustomStack stack2 = new NewCustomStack("Second", 5);
            
            //один объект Journal подписать на события CollectionCountChanged и CollectionReferenceChanged из первой коллекции
            Journal joun1 = new Journal();
            stack1.CollectionCountChanged += joun1.CollectionCountChanged;
            stack1.CollectionReferenceChanged += joun1.CollectionReferenceChanged;

            //второй объект Journal подписать на события CollectionReferenceChanged из обеих коллекций. 
            Journal joun2 = new Journal();
            stack2.CollectionCountChanged += joun2.CollectionCountChanged;
            stack2.CollectionReferenceChanged += joun2.CollectionReferenceChanged;

            // stack 1 - добавление происходит в начало, тк стек
            stack1.Add(new Production {WorkersNumber = 15});
            stack1.Add(new Production {WorkersNumber = 20});

            stack1.Remove(1);

            // stack 2
            stack2[1] = new Production {WorkersNumber = -10};

            // journals
            Console.WriteLine("________ЖУРНАЛ 1 КОЛЛЕКЦИИ__________");
            joun1.Show();
            Console.WriteLine("________ЖУРНАЛ 2 КОЛЛЕКЦИИ__________");
            joun2.Show();

            Console.ReadLine();
        }
    }

    //Записи для журнала
    public class JournalEntry
    {
        string NameCollection { get; set; }
        string ChangeCollection { get; set; }
        object Obj { get; set; }

        public JournalEntry()
        {
            NameCollection = null;
            ChangeCollection = null;
            Obj = default;
        }

        public JournalEntry(string colName, string changetype, object p)
        {
            NameCollection = colName;
            ChangeCollection = changetype;
            Obj = p;
        }

        public override string ToString()
        {
            return "Коллекция: " + NameCollection + ", " + ChangeCollection + " следующий элемент: " + Obj.ToString();
        }
    }

    public delegate void CollectionHandler(object source, CollectionHandlerEventArgs args); //   ДЕЛЕГАТ

    public class CollectionHandlerEventArgs : System.EventArgs
    {
        public string NameCollection { get; set; }
        public string ChangeCollection { get; set; }
        public object Obj { get; set; }

        public CollectionHandlerEventArgs()
        {
            NameCollection = null;
            ChangeCollection = null;
            Obj = default;
        }

        public CollectionHandlerEventArgs(string colName, string changetype, object p)
        {
            NameCollection = colName;
            ChangeCollection = changetype;
            Obj = p;
        }

        public override string ToString()
        {
            return "Коллекция: " + NameCollection + ", " + ChangeCollection + " следующий элемент: " + Obj.ToString();
        }
    }

    //Журнал в котором сохраняются все записи об изменениях в моей коллекции
    public class Journal
    {
        private List<JournalEntry> journal = new List<JournalEntry>();

        public void CollectionCountChanged(object sourse, CollectionHandlerEventArgs e)
        {
            JournalEntry je = new JournalEntry(e.NameCollection, e.ChangeCollection, e.Obj.ToString());
            journal.Add(je);
        }

        public void CollectionReferenceChanged(object sourse, CollectionHandlerEventArgs e)
        {
            JournalEntry je = new JournalEntry(e.NameCollection, e.ChangeCollection, e.Obj.ToString());
            journal.Add(je);
        }


        public void Show()
        {
            foreach (JournalEntry item in journal)
                Console.WriteLine(item + "\n");
        }
    }

    public class NewCustomStack
    {
        public CustomStack _stack = new CustomStack();
        string Name { get; set; }

        public override string ToString()
        {
            return _stack.ToString();
        }

        public NewCustomStack()
        {
            Name = null;
        }

        public NewCustomStack(string colName, int size)
        {
            Name = colName;
            _stack = new CustomStack(size);
        }

        public void Add(Production item)
        {
            _stack.Add(item);
            OnCollectionCountChanged(this, new CollectionHandlerEventArgs(Name, "ДОБАВЛЕН", item));
        }

        public bool Remove(int index)
        {
            if (index < _stack.Length)
            {
                OnCollectionCountChanged(this,
                    new CollectionHandlerEventArgs(Name, "УДАЛЕНИЕ", $"[{index}] - {_stack[index]}"));

                _stack.Delete(index, 1);
                return true;
            }

            return false;
        }

        public Production this[int index]
        {
            get => _stack[index];
            set
            {
                OnCollectionReferenceChanged(this,
                    new CollectionHandlerEventArgs(this.Name,
                        $"ПРИСВОЕНО НОВОЕ ЗНАЧЕНИЕ КОЛ-ВА РАБОТНИКОВ ({value.WorkersNumber})", $"[{index}] - {_stack[index]}"));
                _stack[index] = value;
            }
        }


        //происходит при добавлении нового элемента или при удалении элемента из коллекции
        public event CollectionHandler CollectionCountChanged;

        //объекту коллекции присваивается новое значение       
        public event CollectionHandler CollectionReferenceChanged;


        //обработчик события CollectionCountChanged
        public virtual void OnCollectionCountChanged(object source, CollectionHandlerEventArgs args)
        {
            if (CollectionCountChanged != null)
                CollectionCountChanged(source, args);
        }

        //обработчик события OnCollectionReferenceChanged
        public virtual void OnCollectionReferenceChanged(object source, CollectionHandlerEventArgs args)
        {
            if (CollectionReferenceChanged != null)
                CollectionReferenceChanged(source, args);
        }
    }
}
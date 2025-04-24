namespace Day2DOLID
{
    internal class Program
    {
        public interface IDeveloper
        {
            string Name { get; set; }
        }

        public interface ITask
        {
            string Name { get; set; }
            string Description { get; set; }
        }

        public class Developer : IDeveloper
        {

            private string name;
            public Developer(string n){ Name = n;}
            public string Name
            {
                get => name;
                set
                {
                    if (value.Trim() != null)
                        name = value;
                }
            }
        }

        public class TaskItem : ITask
        {
            public string Name { get; set; }
            public string Description { get; set; }

            public IDeveloper AssignedDeveloper { get; private set; }

            public void AssignTo(IDeveloper developer)
            {
                if (developer == null || string.IsNullOrWhiteSpace(developer.Name))
                    throw new ArgumentException("Developer must have a valid name.");

                AssignedDeveloper = developer;
                Console.WriteLine($"Task '{Name}' assigned to {developer.Name}");
            }
        }

        public interface ITaskCreator
        {
            void CreateSubTask();
        }

        public interface ITaskAssigner
        {
            void AssignTask(ITask task, IDeveloper developer);
        }

        public interface ITaskWorker
        {
            void WorkOnTask();
        }

        public class TeamLead : IDeveloper,ITaskCreator, ITaskAssigner, ITaskWorker
        {
            public string Name { get; set; }

            public void CreateSubTask()
            {
                Console.WriteLine("Sub-task created by TeamLead.");
            }

            public void AssignTask(ITask task, IDeveloper developer)
            {
                if (task is TaskItem t && developer != null)
                {
                    t.AssignTo(developer);
                }
            }

            public void WorkOnTask()
            {
                Console.WriteLine("TeamLead is working on a task.");
            }
        }


        public class Manager : ITaskAssigner
        {
            public void AssignTask(ITask task, IDeveloper developer)
            {
                if (task is TaskItem t && developer != null)
                {
                    t.AssignTo(developer);
                }
            }
        }


        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
        }
    }
}

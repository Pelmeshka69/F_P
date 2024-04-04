using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

public class Task
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
}

public class TaskManager
{
    public List<Task> Tasks { get; private set; } = new List<Task>();

    public void AddTask(Task task)
    {
        Tasks.Add(task);
    }

    public void RemoveTask(Task task)
    {
        Tasks.Remove(task);
    }

    public void EditTask(Task oldTask, Task newTask)
    {
        int index = Tasks.IndexOf(oldTask);
        if (index != -1)
        {
            Tasks[index] = newTask;
        }
    }

    public void SaveTasks(string filename)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(List<Task>));
        using (StreamWriter writer = new StreamWriter(filename))
        {
            serializer.Serialize(writer, Tasks);
        }
    }

    public void LoadTasks(string filename)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(List<Task>));
        using (StreamReader reader = new StreamReader(filename))
        {
            Tasks = (List<Task>)serializer.Deserialize(reader);
        }
    }
}

class Program
{
    static void Main()
    {
        TaskManager taskManager = new TaskManager();

        while (true)
        {
            Console.WriteLine("1. Add task");
            Console.WriteLine("2. Edit task");
            Console.WriteLine("3. Remove task");
            Console.WriteLine("4. Save tasks");
            Console.WriteLine("5. Load tasks");
            Console.WriteLine("6. View tasks");
            Console.WriteLine("7. Exit");
            Console.Write("Select an option: ");
            string option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    Task task = new Task();
                    Console.Write("Enter the title of the task: ");
                    task.Title = Console.ReadLine();
                    Console.Write("Enter the description of the task: ");
                    task.Description = Console.ReadLine();
                    task.DueDate = DateTime.Now.AddDays(7);
                    taskManager.AddTask(task);
                    Console.WriteLine("Task added: " + task.Title);
                    break;
                case "2":
                    Console.Write("Enter the title of the task to edit: ");
                    string oldTitle = Console.ReadLine();
                    Task oldTask = taskManager.Tasks.Find(t => t.Title == oldTitle);
                    if (oldTask != null)
                    {
                        Task newTask = new Task();
                        Console.Write("Enter the new title of the task: ");
                        newTask.Title = Console.ReadLine();
                        Console.Write("Enter the new description of the task: ");
                        newTask.Description = Console.ReadLine();
                        newTask.DueDate = DateTime.Now.AddDays(7);
                        taskManager.EditTask(oldTask, newTask);
                        Console.WriteLine("Task updated: " + newTask.Title);
                    }
                    else
                    {
                        Console.WriteLine("Task not found.");
                    }
                    break;
                case "3":
                    Console.Write("Enter the title of the task to remove: ");
                    string title = Console.ReadLine();
                    Task taskToRemove = taskManager.Tasks.Find(t => t.Title == title);
                    if (taskToRemove != null)
                    {
                        taskManager.RemoveTask(taskToRemove);
                        Console.WriteLine("Task removed: " + taskToRemove.Title);
                    }
                    else
                    {
                        Console.WriteLine("Task not found.");
                    }
                    break;
                case "4":
                    taskManager.SaveTasks("tasks.xml");
                    Console.WriteLine("Tasks saved to tasks.xml");
                    break;
                case "5":
                    taskManager.LoadTasks("tasks.xml");
                    Console.WriteLine("Tasks loaded from tasks.xml");
                    break;
                case "6":
                    foreach (Task t in taskManager.Tasks)
                    {
                        Console.WriteLine("Title: " + t.Title + ", Description: " + t.Description + ", DueDate: " + t.DueDate);
                    }
                    break;
                case "7":
                    return;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }
}

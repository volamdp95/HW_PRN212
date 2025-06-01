﻿// Build a task scheduler
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TaskScheduler
{
    // Simple task priority enum
    public enum TaskPriority
    {
        Low,
        Normal,
        High
    }
    
    // Interface for task definition
    public interface IScheduledTask
    {
        string Name { get; }
        TaskPriority Priority { get; }
        TimeSpan Interval { get; }
        DateTime LastRun { get; }
        Task ExecuteAsync();
    }
    
    // A basic implementation of a scheduled task
    public class SimpleTask : IScheduledTask
    {
        private readonly Func<Task> _action;
        private DateTime _lastRun = DateTime.MinValue;
        
        public string Name { get; }
        public TaskPriority Priority { get; }
        public TimeSpan Interval { get; }
        
        public DateTime LastRun => _lastRun;
        
        public SimpleTask(string name, TaskPriority priority, TimeSpan interval, Func<Task> action)
        {
            Name = name;
            Priority = priority;
            Interval = interval;
            _action = action;
        }
        
        public async Task ExecuteAsync()
        {
            await _action();
            _lastRun = DateTime.Now;
        }
    }
    
    // The scheduler that students need to implement
    public class TaskScheduler
    {
        // TODO: Implement task queue/storage mechanism

        private readonly List<IScheduledTask> _tasks;
        
        public TaskScheduler()
        {
             _tasks = new List<IScheduledTask>();
        }
        
        public void AddTask(IScheduledTask task)
        {
            _tasks.Add(task);
        }
        
        public void RemoveTask(string taskName)
        {
            _tasks.RemoveAll(t => t.Name == taskName);
        }
        
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            // TODO: Implement the scheduling logic
            // - Run higher priority tasks first
            // - Only run tasks when their interval has elapsed since LastRun
            // - Keep running until cancellation is requested

            while (!cancellationToken.IsCancellationRequested)
            {
                DateTime now = DateTime.Now;

                var runnableTasks = _tasks
                    .Where(t => (now - t.LastRun) >= t.Interval)
                    .OrderByDescending(t => t.Priority) // Ưu tiên cao trước
                    .ToList();

                foreach (var task in runnableTasks)
                {
                    if (cancellationToken.IsCancellationRequested)
                        break;

                    Console.WriteLine($"[{now:HH:mm:ss}] Executing: {task.Name} (Priority: {task.Priority})");
                    await task.ExecuteAsync();
                }

                await Task.Delay(1000, cancellationToken);
            }


            throw new NotImplementedException();
        }
        
        public List<IScheduledTask> GetScheduledTasks()
        {
            return _tasks.ToList();
            throw new NotImplementedException();
        }
    }
    
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Task Scheduler Demo");
            
            // Create the scheduler
            var scheduler = new TaskScheduler();
            
            // Add some tasks
            scheduler.AddTask(new SimpleTask(
                "High Priority Task", 
                TaskPriority.High,
                TimeSpan.FromSeconds(2),
                async () => {
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Running high priority task");
                    await Task.Delay(500); // Simulate some work
                }
            ));
            
            scheduler.AddTask(new SimpleTask(
                "Normal Priority Task", 
                TaskPriority.Normal,
                TimeSpan.FromSeconds(3),
                async () => {
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Running normal priority task");
                    await Task.Delay(300); // Simulate some work
                }
            ));
            
            scheduler.AddTask(new SimpleTask(
                "Low Priority Task", 
                TaskPriority.Low,
                TimeSpan.FromSeconds(4),
                async () => {
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Running low priority task");
                    await Task.Delay(200); // Simulate some work
                }
            ));
            
            // Create a cancellation token that will cancel after 20 seconds
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            
            // Or allow the user to cancel with a key press
            Console.WriteLine("Press any key to stop the scheduler...");
            
            // Run the scheduler in the background
            var schedulerTask = scheduler.StartAsync(cts.Token);
            
            // Wait for user input
            Console.ReadKey();
            cts.Cancel();
            
            try
            {
                await schedulerTask;
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Scheduler stopped by cancellation.");
            }
            
            Console.WriteLine("Scheduler demo finished!");
        }
    }
}
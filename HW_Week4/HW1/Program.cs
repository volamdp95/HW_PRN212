﻿using System;

namespace DelegatesLinQ.Homework
{
    // Event delegates for different operations
    public delegate void CalculationEventHandler(string operation, double operand1, double operand2, double result);
    public delegate void ErrorEventHandler(string operation, string errorMessage);

    /// <summary>
    /// Homework 1: Event Calculator
    /// Create a calculator class with events for each operation.
    /// 
    /// Requirements:
    /// 1. Create events for each mathematical operation (Add, Subtract, Multiply, Divide)
    /// 2. Create events for errors (like division by zero)
    /// 3. Create subscriber classes that handle these events:
    ///    - Logger: Logs all operations to console
    ///    - Auditor: Keeps track of operation count
    ///    - ErrorHandler: Handles and displays errors
    /// 4. Demonstrate all operations and error handling
    /// 
    /// Techniques used: Similar to 6_5_EventApp
    /// - Event declaration and raising
    /// - Multiple subscribers
    /// - Event handler methods
    /// </summary>
    /// 
    
    public class EventCalculator
    {
        // TODO: Declare events for calculation operations
        public event CalculationEventHandler OperationPerformed;
        public event ErrorEventHandler ErrorOccurred;

        protected virtual void OnOperationPerformed(string operation, double operand1, double operand2, double result)
        {
            OperationPerformed?.Invoke(operation, operand1, operand2, result);
        }

        protected virtual void OnErrorOccurred(string operation, string errorMessage)
        {
            ErrorOccurred?.Invoke(operation, errorMessage);
        }

        public double Add(double a, double b)
        {
            double result = a + b;
            OnOperationPerformed("Add", a, b, result);
            return result;
        }

        public double Subtract(double a, double b)
        {
            // TODO: Implement subtraction and raise OperationPerformed event
             double result = a - b;
            OnOperationPerformed("Subtract", a, b, result);
            return result;
        }

        public double Multiply(double a, double b)
        {
            // TODO: Implement multiplication and raise OperationPerformed event
             double result = a * b;
            OnOperationPerformed("Multiply", a, b, result);
            return result;
        }

        public double Divide(double a, double b)
        {
            // TODO: Implement division with error checking
            // Raise ErrorOccurred event if dividing by zero
            // Raise OperationPerformed event for successful division
            if (b == 0)
            {
                OnErrorOccurred("Divide", "Cannot divide by zero.");
                return double.NaN;
            }

            double result = a / b;
            OnOperationPerformed("Divide", a, b, result);
            return result;
        }

        // TODO: Create protected methods to raise events
        // protected virtual void OnOperationPerformed(string operation, double operand1, double operand2, double result)
        // protected virtual void OnErrorOccurred(string operation, string errorMessage)
    }

    // TODO: Create subscriber classes

    public class CalculationLogger
    {
        // TODO: Implement event handlers for logging operations and errors
        // public void OnOperationPerformed(string operation, double operand1, double operand2, double result)
        // public void OnErrorOccurred(string operation, string errorMessage)
        public void OnOperationPerformed(string operation, double operand1, double operand2, double result)
        {
            Console.WriteLine($"[LOG] {operation}: {operand1} and {operand2} = {result}");
        }

        public void OnErrorOccurred(string operation, string errorMessage)
        {
            Console.WriteLine($"[ERROR LOG] {operation} failed: {errorMessage}");
        }
    }

    public class CalculationAuditor
    {
        // TODO: Keep track of operation counts
        // private int _operationCount = 0;
        // public void OnOperationPerformed(string operation, double operand1, double operand2, double result)
        // public void DisplayStatistics()

        private int _operationCount = 0;

        public void OnOperationPerformed(string operation, double operand1, double operand2, double result)
        {
            _operationCount++;
        }

        public void DisplayStatistics()
        {
            Console.WriteLine($"\n[STATISTICS] Total operations performed: {_operationCount}");
        }
    }

    public class ErrorHandler
    {
        // TODO: Handle errors with special formatting
        // public void OnErrorOccurred(string operation, string errorMessage)

        public void OnErrorOccurred(string operation, string errorMessage)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[HANDLER ERROR] Operation '{operation}' error: {errorMessage}");
            Console.ResetColor();
        }
    }

    public class HW1_EventCalculator
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== HOMEWORK 1: EVENT CALCULATOR ===");
            Console.WriteLine("Instructions:");
            Console.WriteLine("1. Implement the EventCalculator class with events for each operation");
            Console.WriteLine("2. Implement subscriber classes: CalculationLogger, CalculationAuditor, ErrorHandler");
            Console.WriteLine("3. Subscribe to events and test all operations including error cases");
            Console.WriteLine();

            // TODO: You should implement the following:
            
            EventCalculator calculator = new EventCalculator();
            CalculationLogger logger = new CalculationLogger();
            CalculationAuditor auditor = new CalculationAuditor();
            ErrorHandler errorHandler = new ErrorHandler();

            // Subscribe to events
            calculator.OperationPerformed += logger.OnOperationPerformed;
            calculator.OperationPerformed += auditor.OnOperationPerformed;
            calculator.ErrorOccurred += logger.OnErrorOccurred;
            calculator.ErrorOccurred += errorHandler.OnErrorOccurred;

            // Test operations
            calculator.Add(10, 5);
            calculator.Subtract(10, 3);
            calculator.Multiply(4, 7);
            calculator.Divide(15, 3);
            calculator.Divide(10, 0); // Should trigger error

            // Display statistics
            auditor.DisplayStatistics();
            

            Console.ReadKey();
        }
    }
}
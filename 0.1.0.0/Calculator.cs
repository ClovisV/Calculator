using System.IO;
using System;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        Double firstNumber, secondNumber, result = 0;
	    int nbError = 0;
        String answerUser, itemOperator;
        String[] tableAnswersUser;
	    ConsoleColor currentBackground = Console.BackgroundColor, currentForeground = Console.ForegroundColor;
	
	    start:
      	    Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.Red;
	        Console.WriteLine("Hello World!\nMy first program. Enjoy! ;)");

            Console.BackgroundColor = ConsoleColor.Green;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("Can you choose two numbers separed by an operator to make an operation, please?\nThe operators are: '+', '-', '*', '/', '%'");

            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Magenta;
            answerUser = Console.ReadLine();
            Console.WriteLine("You answer: {0}", answerUser);

            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.Blue;
	        try 
	        {
        	    tableAnswersUser = Regex.Split(answerUser, @"([-]?\d*[,]?\d*)(\+|\-|\*|\/|\%){1}([-]?\d*[,]?\d*)");
		        foreach(string item in tableAnswersUser)
		        {
			        Console.WriteLine("Split n°{0}: {1}\n", Array.IndexOf(tableAnswersUser, item) + 1, item);
		        }
        	    firstNumber = Double.Parse(tableAnswersUser[1]);
		        itemOperator = tableAnswersUser[2];
        	    secondNumber = Double.Parse(tableAnswersUser[3]);
		        switch(itemOperator)
		        {
			        case "+":
				        result = firstNumber + secondNumber;
				        break;
			        case "-": 
				        result = firstNumber - secondNumber;
				        break;
			        case "*": 
				        result = firstNumber * secondNumber;
				        break;
			        case "/": 
				        result = firstNumber / secondNumber;
				        break;
			        case "%": 
				        result = firstNumber % secondNumber;
				        break;
		        }
        	    Console.WriteLine("The result of {0} {1} {2} is: {3}", firstNumber, itemOperator, secondNumber, result);
	        }
	        catch (Exception e)
	        {
		        Console.WriteLine("Sorry, I can not work.\nProblem : " + e.Message);
		        if (nbError == 3)
		        {
			        goto end;
		        }
		        else
		        {
			        nbError+=1;
			        Console.WriteLine("Can you retry, please?");
			        goto start;
		        }
	        }
        end:
	        Console.BackgroundColor = currentBackground;
	        Console.ForegroundColor = currentForeground;
	        Console.WriteLine("End of program, thanks you for its use! ;)\nTouch any key to stop...");
            Console.ReadLine();
    }
}


/*

((-6/-78)*(8-6))/(67-55):

A								0
----------------------------------------------------------------
B				1			   (/)				2
--------------------------------|-------------------------------
C		1	   (*)		2		|		3	   (-)		4
----------------|---------------|---------------|---------------
D	1  (/)	2	|	3  (-)	4	|		(67)	|		(55)
--------|-------|-------|-------|---------------|---------------
  (-6)	| (-78)	|  (8)	|  (6)	|				|
--------|-------|-------|-------|---------------|---------------

-6	/	-78	*	8	-	6	/	67	-	55
D1	C1	D2	B1	D3	C2	D4	A0	C3	B2	C4

D -> A
x -> X

D4 - C2 - D3
D2 - C1 - D1
C4 - B2 - C3
(D4 - C2 - D3) - B1 - (D2 - C1 - D1)
(C4 - B2 - C3) - A0 - ((D4 - C2 - D3) - B1 - (D2 - C1 - D1))

*/

using System.IO;
using System;
using System.Collections.Generic;

public class Program
{

	public class Node 
	{
		public Node rootNode {get;set;}
		public Node leftNode {get;set;}
		public Node rightNode {get;set;}	
		public string nodeContent {get;set;}
		public Node(string nodeContent)	
		{
			this.nodeContent = nodeContent;
			rootNode = leftNode = rightNode = null;
		}
	}
	
	static void programDescription()
	{
		Console.BackgroundColor = ConsoleColor.Blue;
		Console.ForegroundColor = ConsoleColor.Red;
	    Console.WriteLine("Hello World!\nMy first program. Enjoy! ;)");
	    Console.BackgroundColor = ConsoleColor.Green;
	    Console.ForegroundColor = ConsoleColor.Black;
	    Console.WriteLine("Can you choose an operation with spaces between each parenthesis, each number with or without negative sign and between each operator, please?");
	    Console.WriteLine("The operators are: '+', '-', '*', '/', '%', '\\'.");
	    Console.WriteLine("In addition, the program is based on the number of parentheses to build the tree that will help it calculate this operation.");
	    Console.WriteLine("For example, if there is an open parenthese, you need a closing one or if there are two open, you need two closings.");
	    Console.WriteLine("Here's another explicit example: '( ( 2 + ( 6 - 8) / ( 4 \\ 2 ) ) * 6'.");
	    Console.BackgroundColor = ConsoleColor.White;
	    Console.ForegroundColor = ConsoleColor.Magenta;
	}
	
	static List<Node> createTreeFromArray(string[] arrayInput, List<Node> treeInput, int rootNodePosition)
	{
		int cpt = 0;
		if (cpt < arrayInput.Length)
		{
			int nbPo = 0; 
			int nbPf = 0;
			int numPf = 0;
			if (arrayInput[cpt] == "(")
			{
				nbPo = nbPo + 1;
				while (arrayInput[nbPo] == "(")
				{
					nbPo = nbPo + 1;
				}
				numPf = nbPo;
				while (nbPf < nbPo)
				{
					if (numPf == arrayInput.Length)
					{
						Array.Resize(ref arrayInput,arrayInput.Length+1);
						arrayInput[numPf] = ")";
					}
					if (arrayInput[numPf] == ")")
					{
						nbPf = nbPf + 1;
					}
					else 
					{
						nbPf = 0;
					}
					numPf = numPf + 1;
				}
				string[] arrayOne = new string[numPf-2];
				Array.Copy(arrayInput,1,arrayOne,0,numPf-2);
				if (arrayInput.Length - numPf > 0)
				{
					string[] arrayTwo = new string[arrayInput.Length-numPf-1];
					Array.Copy(arrayInput,numPf+1,arrayTwo,0,arrayInput.Length-numPf-1);
					//root
					if (treeInput.Count == 0)
					{
						treeInput.Add(new Node(arrayInput[numPf]));
					}
					else
					{
						if (treeInput[rootNodePosition].leftNode == null)
						{
							Node root = new Node(arrayInput[numPf]);
							treeInput[rootNodePosition].leftNode = root;
							treeInput.Add(root);
							treeInput[treeInput.Count-1].rootNode = treeInput[rootNodePosition];
						}
						else if (treeInput[rootNodePosition].rightNode == null)
						{
							Node root = new Node(arrayInput[numPf]);
							treeInput[rootNodePosition].rightNode = root;
							treeInput.Add(root);
							treeInput[treeInput.Count-1].rootNode = treeInput[rootNodePosition];
						}
					}
					rootNodePosition = treeInput.Count - 1;
					treeInput = createTreeFromArray(arrayOne,treeInput,rootNodePosition);
					treeInput = createTreeFromArray(arrayTwo,treeInput,rootNodePosition);
				}
				else
				{
					treeInput = createTreeFromArray(arrayOne,treeInput,rootNodePosition);
				}
			}
			else
			{
				//root
				if (treeInput.Count == 0)
				{
					treeInput.Add(new Node(arrayInput[cpt+1]));
				}
				else
				{
					if (treeInput[rootNodePosition].leftNode == null)
					{
						Node root = new Node(arrayInput[cpt+1]);
						treeInput[rootNodePosition].leftNode = root;
						treeInput.Add(root);
						treeInput[treeInput.Count-1].rootNode = treeInput[rootNodePosition];
					}
					else if (treeInput[rootNodePosition].rightNode == null)
					{
						if (arrayInput.Length == 1)
						{
							cpt = -1;
						}
						Node root = new Node(arrayInput[cpt+1]);
						treeInput[rootNodePosition].rightNode = root;
						treeInput.Add(root);
						treeInput[treeInput.Count-1].rootNode = treeInput[rootNodePosition];
					}
				}
				rootNodePosition = treeInput.Count - 1;
				if (cpt < 0)
				{
					return treeInput;
				}
				//left
				Node left =  new Node(arrayInput[cpt]);
				treeInput[rootNodePosition].leftNode = left;
				treeInput.Add(left);
				treeInput[treeInput.Count-1].rootNode = treeInput[rootNodePosition];
				//right
				if (arrayInput[cpt+2] == "(")
				{
					string[] arrayThree = new string[arrayInput.Length-2];
					Array.Copy(arrayInput,2,arrayThree,0,arrayInput.Length-2);
					treeInput = createTreeFromArray(arrayThree,treeInput,rootNodePosition);	
				}
				else
				{
					Node right = new Node(arrayInput[cpt+2]);
					treeInput[rootNodePosition].rightNode = right;
					treeInput.Add(right);
					treeInput[treeInput.Count-1].rootNode = treeInput[rootNodePosition];
				}
			}
		}
		return treeInput;
	}
	
	static void treePrint(List<Node> treeInput)
	{
		string rootNode, leftNode, rightNode, valueNull = "null";
		Console.WriteLine("Your tree:");
		for (int numNode = 0; numNode < treeInput.Count; numNode++)
		{
			if (treeInput[numNode].rootNode != null)
			{
				rootNode = treeInput[numNode].rootNode.nodeContent;
			}
			else
			{
				rootNode = valueNull;
			}
			if (treeInput[numNode].leftNode != null)
			{
				leftNode = treeInput[numNode].leftNode.nodeContent;
			}
			else
			{
				leftNode = valueNull;
			}
			if (treeInput[numNode].rightNode != null)
			{
				rightNode = treeInput[numNode].rightNode.nodeContent;
			}
			else
			{
				rightNode = valueNull;
			}
			Console.WriteLine("\tPosition n°" + numNode + ": " + treeInput[numNode].nodeContent + " => " + rootNode + " | " + leftNode + " | " + rightNode);
		}
	}
	
	static double calculate(double nb1, string op, double nb2) 
	{
		switch(op)
		{
			case "+":
				return nb1 + nb2;
			case "-":
				return nb1 - nb2;
			case "*":
				return nb1 * nb2;
			case "/":
				if(nb2 != 0)
				{
					return nb1 / nb2;
				}
				else
				{
					Console.WriteLine("Error: impossible to divise an number by zero!");
					return -2000;
				}
			case "%":
				return nb1 % nb2;
			case "\\":
				if(nb2 != 0)
				{
					return double.Parse((nb1 / nb2).ToString().Split('.')[0]);
				}
				else
				{
					Console.WriteLine("Error: impossible to divise an number by zero to get the entier part!");
					return -3000;
				}
			default:
				Console.WriteLine("Error: impossible to calculate !");
				return -1000;
		}
	}
		
	static double treeCalcul(List<Node> treeInput)
	{
		while (treeInput.Count > 1)
		{
			for (int numNode = 0; numNode < treeInput.Count; numNode++)
			{
				if (treeInput[numNode].leftNode == null && treeInput[numNode].rightNode == null)
				{
					treeInput.RemoveAt(numNode);
				}
				else if (double.TryParse(treeInput[numNode].leftNode.nodeContent, out double left) && double.TryParse(treeInput[numNode].rightNode.nodeContent, out double right))
				{
					double temporaryResult = calculate(left,treeInput[numNode].nodeContent,right);
					if (treeInput[numNode].rootNode == null)
					{
						return temporaryResult;
					}
					else 
					{
						if (treeInput[numNode].nodeContent == treeInput[numNode].rootNode.leftNode.nodeContent)
						{
							treeInput[numNode].rootNode.leftNode.nodeContent = temporaryResult.ToString();
						}
						else if (treeInput[numNode].nodeContent == treeInput[numNode].rootNode.rightNode.nodeContent)
						{
							treeInput[numNode].rootNode.rightNode.nodeContent = temporaryResult.ToString();
						}	
						treeInput.RemoveAt(numNode);
					}
				}
			}
		}
		return calculate(double.Parse(treeInput[0].leftNode.nodeContent),treeInput[0].nodeContent,double.Parse(treeInput[0].rightNode.nodeContent));
	}
 
	static void Main()
    {
    	ConsoleColor currentBackground = Console.BackgroundColor, currentForeground = Console.ForegroundColor;
		double result = 0;
		string input;
		string[] arrayOriginal;
		List<Node> tree = new List<Node>();
		programDescription();
		input = Console.ReadLine();
		Console.WriteLine("You answer: {0}", input);
		Console.BackgroundColor = ConsoleColor.Red;
		Console.ForegroundColor = ConsoleColor.Blue;
		arrayOriginal = input.Split(new string[]{" "}, StringSplitOptions.RemoveEmptyEntries);
		try
		{
			tree = createTreeFromArray(arrayOriginal, tree, 0);
			treePrint(tree);
			result = treeCalcul(tree);
		}
		catch
		{
			Console.WriteLine("Error: the operation was not well formatted");
		}
		finally
		{
			Console.WriteLine("Your result is: " + result.ToString() + ".");
			Console.BackgroundColor = currentBackground;
			Console.ForegroundColor = currentForeground;
			Console.WriteLine("End of program, thanks you for its use! ;)");
			Console.WriteLine("Touch the 'ENTER' key to stop...");
			Console.ReadLine();
		}
	}
}   
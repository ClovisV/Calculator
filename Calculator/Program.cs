using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Calculator
{
	class Program
	{
		public class Node 
		{
			public Node RootNode {get;set;}
			public Node LeftNode {get;set;}
			public Node RightNode {get;set;}	
			public string NodeContent {get;set;}
			public Node(string nodeContent)	
			{
				this.NodeContent = nodeContent;
				RootNode = LeftNode = RightNode = null;
			}
		}

		static void ProgramDescription()
		{
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine("\n**************************************************************************************************************************************************************************************************************************************");
			Console.WriteLine("\tHello World!\n\tHere is a calculator in command line in C#. Enjoy! ;)");
			Console.WriteLine("\n\tCan you choose an operation with parentheses, numbers with or without negative sign and operators, please?");
			Console.WriteLine("\tThe operators are: addition with '+',  substraction with '-', multiplication with '*', division with '/', remainder with '%', whole part with '\\'.");
			Console.WriteLine("\tIn addition, the Calculator is based on the parentheses to build the tree that will help it calculate this operation if there are at least two little operations.");
			Console.WriteLine("\tFor example: 2+(6-8) = 2+-2 = 0 or ((45--8.90)/(6,89*890))%94 = (53.90/6132.1)%94 ~= 0.00878981099%94 ~= 0.00878981099.");
			Console.WriteLine("**************************************************************************************************************************************************************************************************************************************\n");
			Console.ResetColor();
		}

		static string[] CreateArrayFromKeyboard()
		{
			ConsoleKeyInfo input = new ConsoleKeyInfo();
			string[] arrayOut = {};
			int indice = 0;
			List<string> operators = new List<string>(){"+", "-", "*", "/", "%", "\\"};
			string stringArray="";
			do
			{
				input = Console.ReadKey();
				if (
					int.TryParse(input.KeyChar.ToString(),out int number) && 
					(
						(indice == 1 && arrayOut[0] == "-") ||
						(indice >= 2 && arrayOut[indice-1] == "-" && operators.Contains(arrayOut[indice-2])) ||
						(indice >= 1 && double.TryParse(arrayOut[indice-1],out double precedent)) ||
						(indice >= 2 && arrayOut[indice-1].Replace(',','.').EndsWith("."))
					) ||
					(input.KeyChar.ToString().Replace(',','.') == "." && int.TryParse(arrayOut[indice-1],out int justPrecedent))
				)
				{
					arrayOut[indice-1] = arrayOut[indice-1] + input.KeyChar.ToString().Replace(',','.');
				}
				else if (int.TryParse(input.KeyChar.ToString(),out int firstNumber) || operators.Contains(input.KeyChar.ToString()) || input.KeyChar.ToString() == "(" || input.KeyChar.ToString() == ")")
				{
					Array.Resize(ref arrayOut,indice+1);
					arrayOut[indice] = input.KeyChar.ToString().Replace(',','.');
					indice++;
				}
				else if (input.Key != ConsoleKey.Enter)
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("\r\tError: it is not a number or an operator or a parenthese! => you choose the key '{0}'.", input.KeyChar.ToString());
					Console.ResetColor();
					return null;
				}
			} while (input.Key != ConsoleKey.Enter);
			foreach (string item in arrayOut)
			{
				stringArray  = stringArray + item;
			}
			Console.WriteLine("\r\tYour answer: {0}.", stringArray);
			return arrayOut;
		}

		static List<Node> CreateTreeFromArray(string[] arrayInput, List<Node> treeInput, int rootNodePosition)
		{
			int cpt = 0;
			List<string> operators = new List<string>(){"+", "-", "*", "/", "%", "\\"};
			if (cpt < arrayInput.Length)
			{
				int nbPo = 0; 
				int nbPf = 0;
				int numPf = 0;
				if (arrayInput[cpt] == "(")
				{
					nbPo++;
					while (arrayInput[nbPo] == "(")
					{
						nbPo++;
					}
					numPf = nbPo;
					while (nbPf <= nbPo)
					{
						if (nbPf == nbPo)
						{
							while (numPf < arrayInput.Length && !operators.Contains(arrayInput[numPf]))
							{
								numPf++;
							}
							string[] arrayTemporary = new string[numPf];
							Array.Copy(arrayInput,0,arrayTemporary,0,numPf);
							if ((Array.FindAll(arrayTemporary, itemPo => itemPo == "(").Length + Array.FindAll(arrayTemporary, itemPf => itemPf == ")").Length) % 2 != 0)
							{
								nbPf = 0;
							}
							else
							{
								break;
							}
						}
						else if (arrayInput[numPf] == ")")
						{
							nbPf++;
						}
						else 
						{
							nbPf = 0;
						}
						numPf++;
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
							if (treeInput[rootNodePosition].LeftNode == null)
							{
								Node root = new Node(arrayInput[numPf]);
								treeInput[rootNodePosition].LeftNode = root;
								treeInput.Add(root);
								treeInput[treeInput.Count-1].RootNode = treeInput[rootNodePosition];
							}
							else if (treeInput[rootNodePosition].RightNode == null)
							{
								Node root = new Node(arrayInput[numPf]);
								treeInput[rootNodePosition].RightNode = root;
								treeInput.Add(root);
								treeInput[treeInput.Count-1].RootNode = treeInput[rootNodePosition];
							}
						}
						rootNodePosition = treeInput.Count - 1;
						treeInput = CreateTreeFromArray(arrayOne,treeInput,rootNodePosition);
						treeInput = CreateTreeFromArray(arrayTwo,treeInput,rootNodePosition);
					}
					else
					{
						treeInput = CreateTreeFromArray(arrayOne,treeInput,rootNodePosition);
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
						if (treeInput[rootNodePosition].LeftNode == null)
						{
							Node root = new Node(arrayInput[cpt+1]);
							treeInput[rootNodePosition].LeftNode = root;
							treeInput.Add(root);
							treeInput[treeInput.Count-1].RootNode = treeInput[rootNodePosition];
						}
						else if (treeInput[rootNodePosition].RightNode == null)
						{
							if (arrayInput.Length == 1)
							{
								cpt = -1;
							}
							Node root = new Node(arrayInput[cpt+1]);
							treeInput[rootNodePosition].RightNode = root;
							treeInput.Add(root);
							treeInput[treeInput.Count-1].RootNode = treeInput[rootNodePosition];
						}
					}
					rootNodePosition = treeInput.Count - 1;
					if (cpt < 0)
					{
						return treeInput;
					}
					//left
					Node left =  new Node(arrayInput[cpt]);
					treeInput[rootNodePosition].LeftNode = left;
					treeInput.Add(left);
					treeInput[treeInput.Count-1].RootNode = treeInput[rootNodePosition];
					//right
					if (arrayInput[cpt+2] == "(")
					{
						string[] arrayThree = new string[arrayInput.Length-2];
						Array.Copy(arrayInput,2,arrayThree,0,arrayInput.Length-2);
						treeInput = CreateTreeFromArray(arrayThree,treeInput,rootNodePosition);	
					}
					else
					{
						Node right = new Node(arrayInput[cpt+2]);
						treeInput[rootNodePosition].RightNode = right;
						treeInput.Add(right);
						treeInput[treeInput.Count-1].RootNode = treeInput[rootNodePosition];
					}
				}
			}
			return treeInput;
		}
		
        static void TreePrint(List<Node> treeInput)
		{
			string rootNode, leftNode, rightNode, valueNull = "null";
			Console.WriteLine("\n\tYour tree:");
			for (int numNode = 0; numNode < treeInput.Count; numNode++)
			{
				if (treeInput[numNode].RootNode != null)
				{
					rootNode = treeInput[numNode].RootNode.NodeContent;
				}
				else
				{
					rootNode = valueNull;
				}
				if (treeInput[numNode].LeftNode != null)
				{
					leftNode = treeInput[numNode].LeftNode.NodeContent;
				}
				else
				{
					leftNode = valueNull;
				}
				if (treeInput[numNode].RightNode != null)
				{
					rightNode = treeInput[numNode].RightNode.NodeContent;
				}
				else
				{
					rightNode = valueNull;
				}
				Console.WriteLine("\t\tPosition n°" + numNode + ": " + treeInput[numNode].NodeContent + " => " + rootNode + " | " + leftNode + " | " + rightNode);
			}
		}

		static double Calculate(double nb1, string op, double nb2) 
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
					if (nb2 != 0)
					{
						return nb1 / nb2;
					}
					else
					{
						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine("\n\tError: impossible to divise an number by zero!");
						Console.ResetColor();
						return double.NaN;
					}
				case "%":
					return nb1 % nb2;
				case "\\":
					if (nb2 != 0)
					{
						return double.Parse((nb1 / nb2).ToString().Replace(',','.').Split('.')[0]);
					}
					else
					{
						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine("\n\tError: impossible to divise an number by zero to get the entier part!");
						Console.ResetColor();
						return double.NaN;
					}
				default:
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("\n\tError: impossible to calculate!");
					Console.ResetColor();
					return double.NaN;
			}
		}

		static double TreeCalcul(List<Node> treeInput)
		{
			while (treeInput.Count > 1)
			{
				for (int numNode = 0; numNode < treeInput.Count; numNode++)
				{
					if (treeInput[numNode].LeftNode == null && treeInput[numNode].RightNode == null)
					{
						treeInput.RemoveAt(numNode);
					}
					else if (double.TryParse(treeInput[numNode].LeftNode.NodeContent, out double left) && double.TryParse(treeInput[numNode].RightNode.NodeContent, out double right))
					{
						double temporaryResult = Calculate(left,treeInput[numNode].NodeContent,right);
						if (treeInput[numNode].RootNode == null)
						{
							return temporaryResult;
						}
						else 
						{
							if (treeInput[numNode].NodeContent == treeInput[numNode].RootNode.LeftNode.NodeContent)
							{
								treeInput[numNode].RootNode.LeftNode.NodeContent = temporaryResult.ToString();
							}
							else if (treeInput[numNode].NodeContent == treeInput[numNode].RootNode.RightNode.NodeContent)
							{
								treeInput[numNode].RootNode.RightNode.NodeContent = temporaryResult.ToString();
							}	
							treeInput.RemoveAt(numNode);
						}
					}
				}
			}
			return Calculate(double.Parse(treeInput[0].LeftNode.NodeContent),treeInput[0].NodeContent,double.Parse(treeInput[0].RightNode.NodeContent));
		}

		static void ProgramRetryOrExit(int secondWait)
		{
			Console.WriteLine("\n\tRetry? (Y/N)");
			string inputKey = Console.ReadKey(true).KeyChar.ToString();
			Console.WriteLine("\r\tYou choose the key '{0}'.", inputKey);
			if (inputKey == "y" || inputKey == "Y")
			{
				Main();
			}
			else
			{
				Console.ForegroundColor = ConsoleColor.Magenta;
				Console.WriteLine("\n--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
				Console.WriteLine("\tEnd of the calculator, thanks you for its use! ;)");
				while (secondWait >= 0)
				{
					Console.Write("\r\tClosing in {0} seconds..", secondWait.ToString());
					Thread.Sleep(1000);
					secondWait--;
				}
				Console.WriteLine("\n-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
				Thread.Sleep(1000);
				Console.ResetColor();
			}
			Environment.Exit(0);   
		}

		static void Main()
		{
		    int nbThread = 1;
		    List<Node> tree = new List<Node>();
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
		    Parallel.Invoke(() =>
		    {
			    Console.CancelKeyPress += new ConsoleCancelEventHandler((object sender, ConsoleCancelEventArgs e) =>
			    {
				    if (e.SpecialKey == ConsoleSpecialKey.ControlC)
				    {
					    if (nbThread > 1)
					    {
						    nbThread--;
				        }
					    else if (nbThread == 1)
					    {
						    Console.ForegroundColor = ConsoleColor.Yellow;
						    Console.WriteLine("\tYou canceled with the key '{0}'...", e.SpecialKey.ToString());
						    Console.ResetColor();
						    e.Cancel=true;
						    Environment.Exit(0);
					    }
				    }
			    });
		    },
		    () =>
		    {
			    ProgramDescription();
			    string[] arrayOriginal = CreateArrayFromKeyboard();
			    try
			    {
				    tree = CreateTreeFromArray(arrayOriginal, tree, 0);
				    TreePrint(tree);
				    double result = TreeCalcul(tree);
				    Console.ForegroundColor = ConsoleColor.Green;
				    Console.WriteLine("\n\tYour result is: {0}.", result.ToString());
				    Console.ResetColor();
			    }
			    catch
			    {
				    Console.ForegroundColor = ConsoleColor.Red;
				    Console.WriteLine("\tError: the operation was not well formatted.");
				    Console.ResetColor();
			    }
			    finally
			    {
				    nbThread++;
				    ProgramRetryOrExit(10);
			    }
		    });
	    }
    }
}

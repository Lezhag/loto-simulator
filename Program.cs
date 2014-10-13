using System;
using System.Linq;

namespace loto_simulator
{

    class Program
    {
        const int ROW_LENGTH = 6;      //number of lucky numbers in a lottery
        const int UNIQUE_NUMBERS = 45; //maximum number
        const bool NoNewLine = false;  //helper for output formatting

        static void Main(string[] args)
        {

            int row = 0;                                // number of rows in a lottery ticket (chosen by the user)
            int win = 0;                                // variable for outputting matching numbers
            int[] Wins = new int[ROW_LENGTH + 1];       // array for the number of winning rows

            int[] WinningRow = new int[ROW_LENGTH];     // array for winning numbers
            int[] NumberPool = new int[UNIQUE_NUMBERS]; // array for retrieving unique numbers
            InitPool(NumberPool);                       // initializes NumberPool array with values from 1 to 45
            char choice;                                // menu variable

            Console.WriteLine("Welcome to the Lottery Simulator!\n");

            do
            {
                Console.Write("Please, enter a key for lottery type => \n'm'-manual, 'a'-automatic, 'e'-exit: ");
                choice = Console.ReadKey().KeyChar;
                Console.WriteLine();

                switch (choice)
                {

                    case 'm':
                        #region Initialization
                        //getting the rows, declaring and initating of the jagged array, getting user input
                        Console.Write("Please, choose the number of rows: ");
                        row = Input();
                        int[][] ManualLottery = new int[row][];           // jagged array for the lottery with rows
                        for (int i = 0; i < ManualLottery.Length; i++)
                        {
                            ManualLottery[i] = new int[ROW_LENGTH];
                        }
                        PopulateLottery(ManualLottery);
                        #endregion

                        #region Winning Row
                        GetRow(WinningRow, NumberPool);
                        #endregion

                        #region Results and Printouts
                        Console.WriteLine("\n\nWinning numbers: ");
                        PrintRow(WinningRow);
                        Console.WriteLine("============================");
                        LotteryWins(win, ManualLottery, WinningRow, Wins);
                        TotalWins(Wins);
                        Clear(Wins);
                        #endregion

                        break;

                    case 'a':
                        #region Initialization
                        Console.Write("Please, choose the number of rows: ");
                        row = Input();
                        int[][] AutoLottery = new int[row][];           // jagged array for the lottery with rows
                        for (int i = 0; i < AutoLottery.Length; i++)
                        {
                            AutoLottery[i] = new int[ROW_LENGTH];
                        }
                        PopulateLottery(row, NumberPool, AutoLottery);
                        #endregion

                        #region Winning Row
                        GetRow(WinningRow, NumberPool);
                        #endregion

                        #region Results and Printouts
                        Console.WriteLine("\n\nWinning numbers: ");
                        PrintRow(WinningRow);
                        Console.WriteLine("============================");
                        LotteryWins(win, AutoLottery, WinningRow, Wins);
                        TotalWins(Wins);
                        Clear(Wins);
                        #endregion

                        break;

                    default:
                        break;
                }

            } while (choice != 'e');

        }

        //helper function for initialization of sequential numbers in the NumberPool from 1 to 45

        private static void InitPool(int[] NumberPool)
        {
            int Init = 1;
            for (int i = 0; i < NumberPool.Length; i++)
            {
                NumberPool[i] = Init++;
            }
        }

        //function for populating Lottery automatically with values from GetRow function, also prints out the table of values 

        private static void PopulateLottery(int row, int[] NumberPool, int[][] AutoLottery)
        {
            Console.WriteLine("Automated lottery");
            for (int i = 0; i < row; i++)
            {
                GetRow(AutoLottery[i], NumberPool);
                PrintRow(AutoLottery[i], NoNewLine);
            }
        }

        //function for populating Lottery manually with values from Validated Input function

        private static void PopulateLottery(int[][] ManualLottery)
        {
            Console.WriteLine("Input unique values in the range 1 to {0} for every row", UNIQUE_NUMBERS);
            for (int i = 0; i < ManualLottery.Length; i++)
            {
                Console.WriteLine("Input numbers for {0} row", i + 1);
                ManualLottery[i] = ValidatedInput();
            }
        }

        //clears the Wins array back to zeros

        private static void Clear(int[] Wins)
        {
            for (int i = 0; i < Wins.Length; i++)
            {
                Wins[i] = 0;
            }
        }

        //function prints the tally of wins from the Wins array in descending order 

        private static void TotalWins(int[] Wins)
        {
            Console.WriteLine("Total Wins");
            for (int i = Wins.Length - 1; i >= 0; i--)
            {
                Console.WriteLine("{0} Win(s) in {1} Rows", i, Wins[i]);
            }
            Console.WriteLine();
        }

        //function prints out the lottery ticket (rows) with the number of winnings in each row, keeps tally of wins in the Wins array

        private static void LotteryWins(int Win, int[][] Lottery, int[] WinningRow, int[] Wins)
        {
            Console.WriteLine("Your lottery tickets    Wins");
            for (int i = 0; i < Lottery.Length; i++)
            {
                Win = Match(Lottery[i], WinningRow);
                Wins[Win]++;
                PrintRow(Lottery[i], NoNewLine);
                Console.Write("||");
                Console.Write("\t{0}", Win);
            }
            Console.WriteLine("\n");
        }

        /*simulates lottery numbers 
        function shuffles the number pool each time using Knuth-Fisher-Yates (KFY) algorithm
        and returns shuffled numbers in the recieved array 
        */

        private static void GetRow(int[] Row, int[] Unshuffled)
        {
            Shuffle(Unshuffled);
            for (int i = 0; i < Row.Length; i++)
            {
                Row[i] = Unshuffled[i];
            }
        }

        //function uses Knuth-Fisher-Yates shuffle algorithm to obtain unique uniform values in Unshuffled array
        private static void Shuffle(int[] Unshuffled)
        {

            Random r = new Random();
            //iterations on the array
            for (int i = Unshuffled.Length - 1; i > 0; i--)
            {
                //choosing random array member from smaller range each iteration
                int n = r.Next(i + 1);
                //swapping received numbers
                Swap(ref Unshuffled[i], ref Unshuffled[n]);
            }
        }
        //helper function for swapping values for readability
        private static void Swap(ref int p1, ref int p2)
        {
            int temp = p2;
            p2 = p1;
            p1 = temp;
        }

        /* function for user input validation according to lottery simulation restrictions
        uses Input() function to catch exceptions */
        private static int[] ValidatedInput()
        {
            int[] temp = new int[ROW_LENGTH];

            for (int i = 0; i < temp.Length; i++)
            {
                int num = 0;
                while (num == 0)
                {
                    num = Input();
                    //if num does not throw exceptions then the function moves to check the constraint:
                    //num is not repeated in the row, and within range of [1;45]
                    if (!temp.Contains(num) && (num >= 1) && (num <= UNIQUE_NUMBERS))
                    {
                        temp[i] = num;
                    }
                    else
                    {
                        num = 0;
                        Console.WriteLine("Your number is not unique or not in range(1 to {0}), try again", UNIQUE_NUMBERS);
                    }
                }
            }
            return temp;
        }

        //function catches user generated exceptions from input and returns messages

        private static int Input()
        {
            int _input = 0;

            while (_input == 0)
                try
                {
                    _input = int.Parse(Console.ReadLine());
                }
                catch (StackOverflowException e)
                {
                    Console.WriteLine("Error in input data:" + e.Message);
                }
                catch (FormatException e)
                {
                    Console.WriteLine("Error in input data:" + e.Message);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error in input data:" + e.Message);
                }

            return _input;
        }

        //helper function to print rows of lottery with formatting optional param
        private static void PrintRow(int[] Row, bool Line = true)
        {
            Console.WriteLine();
            for (int i = 0; i < Row.Length; i++)
            {
                Console.Write("{0,2} ", Row[i]);
            }
            if (Line) Console.WriteLine();
        }

        //function checks the number of matches in the Lot array to Win array 
        //- used to find the number of guesses out of the winning row
        private static int Match(int[] Lot, int[] Win)
        {
            int _match = 0;
            for (int i = 0; i < Lot.Length; i++)
            {
                for (int j = 0; j < Win.Length; j++)
                {
                    if (Lot[i] == Win[j])
                    {
                        _match++; break;
                    }
                }
            }
            return _match;
        }
    }


}


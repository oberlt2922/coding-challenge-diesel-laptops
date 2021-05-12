using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace attemp2
{
    class Program
    {
        static void Main(string[] args)
        {
            //VARIABLES
            int anualIncome,
                maxLoan = 0,
                maxPrice,
                numbHouses,
                count = 1,
                houseSelection;
            int? minSqft,
                 minPrice = null,
                 minBed,
                 minBath;
            double monthlyInterest = 0,
                   monthlyTax = 0,
                   monthlyLoan = 0,
                   monthlyTotal = 0;
            Random rand = new Random();
            List<House> houses = new List<House>();


            //GREETING AND ANUAL INCOME INPUT
            WriteLine("Greetings, thank you for choosing our real estate service.");
            do
            {
                anualIncome = (int)ValidateInput("your anual income");
                if (anualIncome < 0)
                {
                    WriteLine("\nAnual income cannot be negative.");
                }
            } while (anualIncome < 0);


            //CALCULATE LOAN AND ENSURE THAT MAXIMUM LOAN IS AT LEAST $50,000
            CalculateLoan(anualIncome, ref monthlyLoan, ref monthlyInterest, ref monthlyTax, ref monthlyTotal, ref maxLoan);


            //VALIDATE MINIMUM SQUARE FEET INPUT
            //SETS MINIMUM AND MAXIMUM HOUSE PRICE BASED ON MINIMUM SQUARE FEET AND MAXIMUM LOAN
            do
            {
                //GET MINIMUM SQFT
                do
                {
                    minSqft = ValidateInput("the minimum square feet.");
                    if (minSqft < 250 && minSqft != null)
                    {
                        WriteLine("\nSquare feet cannot be less than 250.");
                    }
                } while (minSqft < 250 && minSqft != null);

                //SET MIN PRICE
                if (minSqft != null)
                {
                    minPrice = Convert.ToInt32(Math.Ceiling((double)(minSqft / .0125)));
                }

                if (minPrice < 50000 || minPrice == null)
                {
                    minPrice = 50000;
                }

                //SET MAX PRICE
                maxPrice = maxLoan;

                //ERROR MESSAGE
                if (minPrice > maxPrice)
                {
                    WriteLine("\nYour maximum pre-approved loan will not cover any houses with that many square feet.");
                }
            } while (minPrice > maxPrice);


            //VALIDATE MINIMUM BEDROOMS INPUT
            minBed = ValidateInput("the minimum number of bedrooms");

            //VALIDATE MINIMUM BATHROOMS INPUT
            minBath = ValidateInput("the minimum number of bathrooms");

            //GENERATE HOUSES AND ADD THEM TO LIST
            numbHouses = rand.Next(2, 13);
            for (int i = 0; i < numbHouses; i++)
            {
                House house = new House(minSqft, minBed, minBath, minPrice, maxPrice, rand);
                if (house.bedrooms != -99 && house.bathrooms != -99 && house.price != -99 && house.squareFeet != -99)
                {
                    houses.Add(house);
                }
            }
            foreach (House house in houses)
            {
                house.CalculatePriceChange(houses.Count);
            }
            if (houses.Count <= 0)
            {
                WriteLine("\nNo houses fit your specifications.  Press any key to exit.");
                ReadKey();
                Environment.Exit(0);
            }
            else
            {
                WriteLine();
                foreach (House house in houses)
                {
                    WriteLine("{0,-5}{1,-7}{2,-12:C0}{3,-13}{4,-10:N0}{5,-23}{6,7:C2}\t{7,-10}{8,-5}{9,-11}{10}", count++, "Price: ", house.price, "Square Feet: ", house.squareFeet, "Price Per Square Foot: ", house.pricePerSquareFoot, "Bedrooms: ", house.bedrooms, "Bathrooms: ", house.bathrooms);
                }
            }

            //ACCEPT USER'S HOUSE SELECTION
            do
            {
                houseSelection = (int)ValidateInput("your house selection");
            } while (houseSelection < 1 || houseSelection > houses.Count);
            houseSelection--;
            House selectedHouse = houses[(int)houseSelection];

            //LOAN LIFETIME INFO
            DisplayLifetimePayments(selectedHouse.price);

            //LOAN MONTHLY INFO
            DisplayMonthlyPayments(selectedHouse.price);

            //APPLICATION END
            WriteLine("\nThank you for using our service, press any key to exit.");
            ReadKey();






        }
        //METHODS

        //CALCULATES MAXIMUM LOAN AND ENSURES THAT IT IS AT LEAST $50,000
        public static void CalculateLoan(int anualIncome, ref double monthlyLoan, ref double monthlyInterest, ref double monthlyTax, ref double monthlyTotal, ref int maximumLoan)
        {

            monthlyTotal = ((anualIncome / 12) / 3);
            monthlyLoan = monthlyTotal / 2;
            monthlyInterest = monthlyTotal * .6;
            monthlyTax = monthlyTotal * .4;
            maximumLoan = Convert.ToInt32(Math.Ceiling(monthlyLoan * 12 * 30));

            if (maximumLoan < 50000)
            {
                WriteLine("\nYou have been denied a loan, press any key to exit.");
                ReadKey();
                Environment.Exit(0);
            }
            else
            {
                WriteLine("\nYour maximum pre-approved loan amount is: " + maximumLoan);
            }
        }


        //ACCEPTS AND VALIDATES INTEGER INPUTS
        //A STRING IS PASSED INTO THE METHOD TO TELL THE USER WHICH VALUE THEY ARE ENTERING
        public static int? ValidateInput(string inputName)
        {
            string inputString;
            int returnValue;

            WriteLine("\nPlease enter " + inputName);
            inputString = ReadLine();
            if ((inputString == null || inputString == "") && inputName != "your anual income")
            {
                return null;
            }
            while (!Int32.TryParse(inputString, out returnValue))
            {
                WriteLine("\nInvalid input, please enter a whole number");
                inputString = ReadLine();
            }
            return returnValue;
        }

        //DISPLAYS LIFETIME PAYMENT INFORMATION
        public static void DisplayLifetimePayments(int price)
        {
            WriteLine("\nLIFETIME PAYMENT INFORMATION");
            WriteLine("{0,-40}{1:C0}", "Lifetime Payments Total:", (price * 2));
            WriteLine("{0,-40}{1:C0}", "Lifetime Loan Payments:", (price));
            WriteLine("{0,-40}{1:C0}", "Lifetime Interest Payments:", (price * .6));
            WriteLine("{0,-40}{1:C0}", "Lifetime tax and insurance payments:", (price * .4));
        }

        //DISPLAYS MONTHLY PAYMENT INFORMATION
        public static void DisplayMonthlyPayments(int price)
        {
            WriteLine("\nMONTHLY PAYMENT INFORMATION");
            WriteLine("{0,-40}{1:C0}", "Monthly Payments Total:", ((price / 30) / 12) * 2);
            WriteLine("{0,-40}{1:C0}", "Monthly Loan Payments:", (price / 30) / 12);
            WriteLine("{0,-40}{1:C0}", "Monthly Interest Payments:", ((price / 30) / 12) * .6);
            WriteLine("{0,-40}{1:C0}", "Monthly Tax and Insurance Payments:", ((price / 30) / 12) * .4);
        }


    }
}


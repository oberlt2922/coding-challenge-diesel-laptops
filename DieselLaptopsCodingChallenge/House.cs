using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace attemp2
{
    class House
    {
        //PROPERTIES
        public int price { get; set; }
        public int squareFeet { get; set; }
        public double pricePerSquareFoot { get; set; }
        public int bedrooms { get; set; }
        public int bathrooms { get; set; }

        //PASSABLE PROPERTIES
        public int? minPrice { get; set; }
        public int? maxPrice { get; set; }
        public int? minBed { get; set; }
        public int? minBath { get; set; }
        public int? minSqft { get; set; }
        private Random rand;

        //GENERATED PROPERTIES
        public int maxBed { get; set; }
        public int maxBath { get; set; }
        public int maxSqft { get; set; }


        //CONSTRUCTOR
        public House(int? minSqft, int? minBed, int? minBath, int? minPrice, int? maxPrice, Random rand)
        {
            this.minSqft = minSqft;
            this.minBed = minBed;
            this.minBath = minBath;
            this.minPrice = minPrice;
            this.maxPrice = maxPrice;
            this.rand = rand;
            if (minSqft == null && minBed == null && minBath == null)
            {
                this.minSqft = 250;
                this.minBed = 0;
                this.minBath = 0;
            }

            if (minBath == null)
            {
                this.minBath = 1;
            }

            if (minBed == null)
            {
                this.minBed = this.minBath;
            }

            if (minSqft == null)
            {
                this.minSqft = this.minBed * 400;
            }

            this.minPrice = Convert.ToInt32(Math.Ceiling((double)(this.minSqft / .0125)));
            GenerateHouse();
        }

        //METHODS
        private void GenerateHouse()
        {
            if(this.minBath > this.minBed)
            {
                this.minBed = this.minBath;
            }
            if(this.minBed > this.minSqft / 400)
            {
                this.minSqft = minBed * 400;
                this.minPrice = Convert.ToInt32(Math.Ceiling((double)(this.minSqft / .0125)));
            }

            //GENERATE BATHROOMS
            this.maxSqft = Convert.ToInt32(Math.Floor((double)this.maxPrice * .0125));
            this.maxBath = this.maxSqft / 400;

            this.bathrooms = GenerateValue((int)this.minBath, (int)this.maxBath);

            //GENERATE BEDROOMS
            if (this.minBed < this.bathrooms)
            {
                this.minBed = this.bathrooms;
            }
            
            this.maxBed = this.maxBath;
            this.bedrooms = GenerateValue((int)this.minBed, (int)this.maxBed);

            //GENERATE SQFT
            if (this.minSqft < bedrooms * 400)
            {
                this.minSqft = bedrooms * 400;
            }

            this.squareFeet = GenerateValue((int)this.minSqft, (int)this.maxSqft);

            //GENERATE PRICE
            this.minPrice = Convert.ToInt32((double)this.squareFeet / .0125);
            this.price = GenerateValue((int)this.minPrice, (int)this.maxPrice);

            pricePerSquareFoot = (double)this.price / (double)this.squareFeet;
        }

        
        //GENERATE A RANDOM VALUE
        private int GenerateValue(int minValue, int maxValue)
        {
            try
            {
                return rand.Next((int)minValue, maxValue + 1);
            }
            catch(ArgumentOutOfRangeException ex)
            {
                return -99;
            }
        }

        
        //GETS THE MARKUP OR DISCOUNT TO APPLY TO HOUSE PRICES
        public void CalculatePriceChange(int count)
        {
            switch (count)
            {
                case 2:
                    price += Convert.ToInt32(Math.Ceiling(Convert.ToDouble(price) * .1));
                    break;
                case 3:
                    price += Convert.ToInt32(Math.Ceiling(Convert.ToDouble(price) * .08));
                    break;
                case 4:
                    price += Convert.ToInt32(Math.Ceiling(Convert.ToDouble(price) * .06));
                    break;
                case 5:
                    price += Convert.ToInt32(Math.Ceiling(Convert.ToDouble(price) * .04));
                    break;
                case 6:
                    price += Convert.ToInt32(Math.Ceiling(Convert.ToDouble(price) * .02));
                    break;
                case 8:
                    price -= Convert.ToInt32(Math.Ceiling(Convert.ToDouble(price) * .02));
                    break;
                case 9:
                    price -= Convert.ToInt32(Math.Ceiling(Convert.ToDouble(price) * .04));
                    break;
                case 10:
                    price -= Convert.ToInt32(Math.Ceiling(Convert.ToDouble(price) * .06));
                    break;
                case 11:
                    price -= Convert.ToInt32(Math.Ceiling(Convert.ToDouble(price) * .08));
                    break;
                case 12:
                    price -= Convert.ToInt32(Math.Ceiling(Convert.ToDouble(price) * .1));
                    break;
            }

            if (price < 50000)
            {
                price = 50000;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace KNN
{
    class KNN
    {
        public int index;

        // this holds the values 
        public List<double[]>   trainingSetValues = new List<double[]>();
        // this holds the class associated with the values
        public List<string>     trainingSetClasses = new List<string>();

        public List<double[]>   testSetValues = new List<double[]>();
        public List<double[]>   testSetClasses = new List<double[]>();

        // test values 
        public List<double>     testValues = new List<double>();
        public int K;

        public void LoadData(string path, ref List<double []> setVals, ref List<string> setClasses)
        {
            StreamReader file = new StreamReader(path);
            string line;

            index = 0;

            Console.WriteLine("[i] reading data from {0} ...", path);

            while((line = file.ReadLine()) != null)
            {
                // as we have a CSV file basically, split the line at each ','
                string[] splitLine = line.Split(',').ToArray();

                // and add them to a list
                List<string> lineItems = new List<string>(splitLine.Length);           
                lineItems.AddRange(splitLine);

                // create an appropiate array to hold the doubles from this line
                double[] lineDoubles = new double[lineItems.Count - 1];
                // and a string holding the class
                string lineClass = lineItems.ElementAt(lineItems.Count - 1);

                for(int i = 0; i < lineItems.Count - 1; i++)    // last item is the set class
                {
                    // convert each item in the list to a double
                    double val = Double.Parse(lineItems.ElementAt(i));
                    lineDoubles[i] = val;
                }

                // finally, save them

                setVals.Add(lineDoubles);
                setClasses.Add(lineClass);
                this.index++;
            }

            Console.WriteLine("[+] done. read {0} lines.", index);

            file.Close();
        }

        public void ReadInput()
        {
            Console.WriteLine("[?] please input doubles (with a point) followed by the <ENTER> key. ");
            Console.WriteLine("    * input a -1 to stop" + Environment.NewLine);

            // read double inputs until a -1 is entered
            do
            {
                string input = Console.ReadLine();
                double val = Double.Parse(input);
                if (val != -1)
                {
                    this.testValues.Add(val);
                }
                else
                    break;
            } while (1 == 1);

            Console.WriteLine("[+] read {0} input values.", this.testValues.Count);
        }

        public void Classify(int neighborsNumber)
        {
            this.K = neighborsNumber;

            // create an array where we store the distance from our test data and the training data
            // plus the index of the training data element
            double[][] distances = new double[trainingSetValues.Count][];

            for (int i = 0; i < trainingSetValues.Count; i++)
                distances[i] = new double[2];

            // start computing
            Parallel.For(0, trainingSetValues.Count, train =>
                {
                    // compute the Euclidean distance between our values and the training data values
                    var dist = EuclideanDistance(this.testValues, this.trainingSetValues[train]);
                    // store the computed distance
                    distances[train][0] = dist;
                    // and the index of the tested training element
                    distances[train][1] = train;
                }
            );

            // sort and select first K of them
            var sortedDistances = distances.AsParallel().OrderBy(t => t[0]).Take(this.K);

            Console.WriteLine("[+] closest K={0} neighbors: ", this.K);

            // show us the closest neighbors' classes
            foreach(var d in sortedDistances)
            {
                Console.Write("[{0}] => ", d[1]);
                Console.Write(this.trainingSetClasses.ElementAt((int) d[1]));
            }

        }

        static double EuclideanDistance(List<double> sampleOne, double[] sampleTwo)
        {
            double d = 0.0;

            for(int i = 0; i < sampleOne.Count; i++)
            {
                double temp = sampleOne[i] - sampleTwo[i];
                d += temp * temp;
            }
            return Math.Sqrt(d);
        }
    }
}

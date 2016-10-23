using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

namespace KNN
{
    class KNN
    {
        public int lines;

        // this holds the values of the training data
        public List<double[]>   trainingSetValues = new List<double[]>();
        // this holds the class associated with the values
        public List<string>     trainingSetClasses = new List<string>();

        // same for the test input
        public List<double[]>   testSetValues = new List<double[]>();
        public List<string>     testSetClasses = new List<string>();
        
        public int K;

        public void LoadData(string path, ref List<double []> setVals, ref List<string> setClasses)
        {
            StreamReader file = new StreamReader(path);
            string line;

            this.lines = 0;

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
                this.lines++;
            }

            Console.WriteLine("[+] done. read {0} lines.", this.lines);

            file.Close();
        }

        public void Classify(int neighborsNumber)
        {
            this.K = neighborsNumber;

            // create an array where we store the distance from our test data and the training data -> [0]
            // plus the index of the training data element -> [1]
            double[][] distances = new double[trainingSetValues.Count][];

            double accuracy = 0;
            double correct = 0, testNumber = 0;

            for (int i = 0; i < trainingSetValues.Count; i++)
                distances[i] = new double[2];

            Console.WriteLine("[i] classifying...");

            // start computing
            for(var test = 0; test < this.testSetValues.Count; test++)
            {
                Parallel.For(0, trainingSetValues.Count, index =>
                    {
                        var dist = EuclideanDistance(this.testSetValues[test], this.trainingSetValues[index]);
                        distances[index][0] = dist;
                        distances[index][1] = index;
                    }
                );

                Console.WriteLine("[+] closest K={0} neighbors: ", this.K);

                // sort and select first K of them
                var sortedDistances = distances.AsParallel().OrderBy(t => t[0]).Take(this.K);

                string realClass = testSetClasses[test];

                // print and check the result
                foreach (var d in sortedDistances)
                {
                    string predictedClass = trainingSetClasses[(int) d[1]];
                    if (string.Equals(realClass, predictedClass) == true)
                        correct++;
                    testNumber++;
                    Console.WriteLine("[>>>] test {0}: real class: {1} predicted class: {2}", test, realClass, predictedClass);
                }
            }

            Console.WriteLine();

            // compute and print the accuracy
            accuracy = (correct / testNumber) * 100;
            Console.WriteLine("[i] accuracy: {0}%", accuracy);

        }

        static double EuclideanDistance(double[] sampleOne, double[] sampleTwo)
        {
            double d = 0.0;

            for(int i = 0; i < sampleOne.Length; i++)
            {
                double temp = sampleOne[i] - sampleTwo[i];
                d += temp * temp;
            }
            return Math.Sqrt(d);
        }
    }
}

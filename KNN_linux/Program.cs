using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KNN
{
    class Program
    {
        static void Main(string[] args)
        {
            KNN knn = new KNN();

            Console.WriteLine("Enter a k value - use 5 if unsure");
            int x = int.Parse(Console.ReadLine()); 

            knn.LoadData("iris.dat", KNN.DataType.TRAININGDATA);
            knn.LoadData("test.dat", KNN.DataType.TESTDATA);
            knn.Classify(x);
            Console.ReadKey();
        }
    }
}

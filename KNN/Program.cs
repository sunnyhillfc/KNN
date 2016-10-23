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

            knn.LoadData("iris.dat", ref knn.trainingSetValues, ref knn.trainingSetClasses);
            knn.LoadData("test.dat", ref knn.testSetValues, ref knn.testSetClasses);
            knn.Classify(5);
            Console.ReadKey();
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Infokod
{
    class Program
    {

        static int q = 7;
        static int n = 6;
        static int k = 2;
        static int alpha = 5;
        static int[] u = { 6, 6 };
        static int[] zaj = { 0, 3, 0, 0, 2, 0 };
        static int[] v = new int[n];
        static int[,] G;
        static int[,] H;
        static int dmin = n - k + 1;
        static int t = dmin / 2;

        static List<List<int>> possibleE = new List<List<int>>();
        static void Main(string[] args)
        {
            //qarHammingEgysegmatrixxal();       
            qarRS();
        }
        static void qarRS()
        {
            //kiszh GF(5) C(4,2)  alpha=3  u=(4,3) e=(0,0,3,0)

            System.Console.Write("Megadott u: ");
            printVector(u);

            System.Console.WriteLine("n=" + n + " k=" + k);

            System.Console.WriteLine("dmin=wmin=" + dmin);

            System.Console.WriteLine("t=" + t);

            System.Console.WriteLine("Alpha=" + alpha);

            System.Console.WriteLine("zaj: ");
            printVector(zaj);

            /* GENERATE G MATRIX ######################################################################*/

            G = calculateG();
            Console.WriteLine("G matrix");
            matrixPrint(G);
            Console.WriteLine();


            /* GENERATE H MATRIX ######################################################################*/

            H = calculateH();
            Console.WriteLine("H matrix");
            matrixPrint(H);
            Console.WriteLine();



            /* CALCULATE C ###########################################################################*/

            int[] c = calculateC();
            System.Console.Write("c=");
            printVector(c);
            System.Console.WriteLine();


            /* CALCULATE V ######################################################################*/

            for (int i = 0; i < n; i++)
            {
                v[i] = (c[i] + zaj[i]) % q;
            }

            Console.Write("v=");
            printVector(v);


            /* CALCULATE S FROM HandV ###########################################################*/

            int[] s = calculateS();
            Console.Write("s vector:");
            printVector(s);
            Console.WriteLine();

            /* CALCULATE E group ################################################################*/

            int[] arr = new int[n];
            AllPossibleEVectors(arr, n - 1, q);
            int[] s2 = new int[n - k];
            List<List<int>> greatE = new List<List<int>>();

            for (int e = 0; e < possibleE.Count; e++)
            {
                List<int> act = possibleE[e];
                for (int i = 0; i < n - k; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        s2[i] = (s2[i] + H[i, j] * act[j]) % q;
                    }
                }
                if (s2.SequenceEqual(s))
                {
                    greatE.Add(act);
                }
                s2 = new int[n - k];
            }
            IDictionary<double, List<int>> eDictionary = new Dictionary<double, List<int>>();
            foreach (List<int> list in greatE)
            {
                double x = calculateNorm(list);
                if (!eDictionary.ContainsKey(x))
                    eDictionary.Add(x, list);
            }
            Console.WriteLine();
            Console.WriteLine("The lowest weight e");
            eDictionary = eDictionary.OrderBy(obj => obj.Key).ToDictionary(obj => obj.Key, obj => obj.Value);
            eDictionary.ElementAt(0).Value.ForEach(Console.Write);
            System.Console.WriteLine("Súly: " + eDictionary.ElementAt(0).Key);
        }

        public static int[] calculateS()
        {
            int[] s = new int[n - k];

            for (int i = 0; i < n - k; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    s[i] = (s[i] + H[i, j] * v[j]) % q;
                }
            }
            return s;
        }

        private static int[,] calculateG()
        {
            G = new int[k, n];
            //nulladik sor 1111
            for (int i = 0; i < n; i++)
            {
                G[0, i] = 1;
            }
            //nulladik oszlop 1111
            for (int j = 0; j < k; j++)
            {
                G[j, 0] = 1;
            }

            G[1, 1] = alpha;

            //elso sor alfa hatvanyai
            for (int i = 2; i < n; i++)
            {
                G[1, i] = G[1, i - 1] * alpha % q;
            }
            //tobbi sor
            for (int j = 2; j < k; j++)
            {
                int idx = 0;
                for (int i = 1; i < n; i++)
                {
                    idx = (j * i % n);
                    G[j, i] = G[1, idx];
                }
            }
            return G;
        }
       
        private static int[,] calculateH()
        {
            H = new int[n - k, n];
            for (int j = 0; j < n - k; j++)
            {
                H[j, 0] = 1;
            }
            H[0, 1] = alpha;
            //elso sor alfa hatvanyai
            for (int i = 2; i < n; i++)
            {
                H[0, i] = H[0, i - 1] * alpha % q;
            }

            //tobbi sor
            for (int j = 1; j < n - k; j++)
            {
                int idx = 0;
                for (int i = 1; i < n; i++)
                {
                    idx = (((j + 1) * i) % n);
                    H[j, i] = H[0, idx];
                }
            }
            return H;
        }

        private static int[] calculateC()
        {
            int[] c = new int[n];
            for (int j = 0; j < k; j++)
            {
                for (int i = 0; i < n; i++)
                {
                    c[i] = (c[i] + G[j, i] * u[j]) % q;
                }
            }
            return c;
        }

        static void qarHammingEgysegmatrixxal()
        {
            int[,] G = { { 1, 0, 0, 0, 1 }, { 0, 1, 0, 1, 0 }, { 0, 0, 1, 1, 1 } };
            int[] v = { 0, 1, 0, 1, 1 };
            int q = 2;
            int n = G.GetLength(1);
            int k = G.GetLength(0);
            int[,] H = new int[n - k, n];

            Console.WriteLine("G matrix");
            matrixPrint(G);
            Console.WriteLine();

            Console.WriteLine("v vector");
            foreach (var item in v)
                Console.Write(item + " ");
            Console.WriteLine();


            Console.WriteLine("n=" + n + " k=" + k);
            Console.WriteLine();



            // H generator
            for (int i = 0; i < k; i++) //this part copies values from G
            {
                for (int j = k; j < n; j++)
                {
                    int a = -G[i, j];
                    while (a < 0)
                    {
                        a += q;
                    }
                    H[j - k, i] = a % q;
                }
            }

            for (int i = k; i < n; i++) //this part adds I at the end
            {
                H[i - k, i] = 1;
            }
            Console.WriteLine("H matrix:");
            matrixPrint(H);
            Console.WriteLine();


            //calculating sT
            int[] s = new int[n - k];

            for (int i = 0; i < n - k; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    s[i] = (s[i] + H[i, j] * v[j]) % 2;
                }
            }
            Console.WriteLine("s vector:");
            List<int> sList = s.ToList();
            sList.ForEach(Console.Write);
            Console.WriteLine();



            //e csoport
            int[] arr = new int[n];
            AllPossibleEVectors(arr, n - 1, q);
            int[] s2 = new int[n - k];
            List<List<int>> greatE = new List<List<int>>();

            for (int e = 0; e < possibleE.Count; e++)
            {
                List<int> act = possibleE[e];

                for (int i = 0; i < n - k; i++)
                {

                    for (int j = 0; j < n; j++)
                    {
                        s2[i] = (s2[i] + H[i, j] * act[j]) % q;
                    }
                }
                if (s2.SequenceEqual(s))
                {
                    greatE.Add(act);
                }
                s2 = new int[n - k];
            }


            Console.WriteLine("possible e's:");
            foreach (List<int> list in greatE)
            {
                list.ForEach(Console.Write);
                Console.WriteLine();
            }
            var rlist = greatE

                        .OrderBy(row => row.Sum())
                        .ToList();
            Console.WriteLine();
            Console.WriteLine("Ordered e's");
            foreach (List<int> list in rlist)
            {
                list.ForEach(Console.Write);
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.WriteLine("The lowest weight e");
            rlist[0].ForEach(Console.Write);

        }

        static double calculateNorm(List<int> list)
        {
            double norm = 0;
            foreach (int i in list)
            {
                norm += Math.Pow(i, 2.0);
            }
            return Math.Pow(norm, 1.0 / 2.0);
        }

        static void AllPossibleEVectors(int[] arr, int codeLength, int k)
        {
            int[] myint = new int[arr.Length];
            myint = arr;
            if (codeLength >= 0)
            {
                for (int i = 0; i < k; i++)
                {

                    myint[codeLength] = i;
                    AllPossibleEVectors(myint, codeLength - 1, k);
                }
            }
            else
            {
                List<int> sList = myint.ToList();
                possibleE.Add(sList);
            }

        }

        static void matrixPrint(int[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write(matrix[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        private static void printVector(int[] vector)
        {
            foreach (int i in vector)
            {
                System.Console.Write(i + " ");
            }
            System.Console.WriteLine();
        }
    }

}


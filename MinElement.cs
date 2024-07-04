using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam
{
    internal class MinElement
    {
        public int[,] rate; // тариф
        public int[] post; // поставщики
        public int[] potr; // потребители
        public int[,] answer; // ответ

        public void MainMinElement()
        {
            int[,] rate; // тариф
            int[] post; // поставщики
            int[] potr; // потребители
            int[,] answer; // ответ
            int countV;

            GetData("Data.csv", out rate, out post, out potr, out countV);
            CheckCloseness(post, potr);
            Decision(rate, post, potr, out answer);
            Answer(answer, rate, countV);
        }
        public void GetData(string path, out int[,] rate, out int[] post, out int[] potr, out int countV)
        {
            string[] lines = new string[3];
            if (File.Exists(path))
            {
                lines = File.ReadAllLines(path);
            }
            string[] arrayPost = lines[0].Split(";");
            string[] arrayPotr = lines[1].Split(";");
            string[] arrayRate = lines[2].Split(";");
            post = new int[arrayPost.Length];
            potr = new int[arrayPotr.Length];
            rate = new int[arrayPost.Length, arrayPotr.Length];
            for (int i = 0; i < arrayPost.Length; i++)
            {
                post[i] = Convert.ToInt32(arrayPost[i]);
            }
            for (int j = 0; j < arrayPotr.Length; j++)
            {
                potr[j] = Convert.ToInt32(arrayPotr[j]);
            }
            int n = 0;
            for (int i = 0; i < arrayPost.Length; i++)
            {
                for (int j = 0; j < arrayPotr.Length; j++)
                {
                    rate[i, j] = Convert.ToInt32(arrayRate[n]);
                    n++;
                }
            }
            countV = post.Length + potr.Length - 1;
        }
        public void CheckCloseness(int[] a, int[] b) // проверка на закрытость
        {
            int sumA = 0, sumB = 0;
            for (int i = 0; i < a.Length; i++)
            {
                sumA += a[i];
            }
            for (int i = 0; i < b.Length; i++)
            {
                sumB += b[i];
            }
            if (sumA == sumB)
            {
                Console.WriteLine("( Задача является закрытой. )");
            }
            else
            {
                Console.WriteLine("Задача является открытой и не может быть решена данной программой!");
                Environment.Exit(0);
            }
        }
        public void MinMass(int[,] rate, out int In_i, out int In_j)
        {
            In_i = 0;
            In_j = 0;
            int minValue = rate.Cast<int>().Min(); // нахождение минимального элемента
            for (int i = 0; i < rate.GetLength(0); i++)
            {
                for (int j = 0; j < rate.GetLength(1); j++)
                {
                    if (rate[i, j] == minValue) // находит позицию минимального элемента
                    {
                        In_i = i;
                        In_j = j;
                        break;
                    }
                }
            }
        }
        public void Decision(int[,] rate, int[] post, int[] potr, out int[,] answer)
        {
            int i, j;
            int[,] cloneRate = (int[,])rate.Clone(); // клонирование массива
            answer = new int[post.Length, potr.Length]; // задание размерность массива
            while (post.Sum() != 0 && potr.Sum() != 0)
            {
                MinMass(cloneRate, out i, out j);
                if (post[i] > potr[j])
                {
                    answer[i, j] = potr[j];
                    post[i] = post[i] - potr[j];
                    potr[j] = potr[i] - potr[i];
                }
                else
                {
                    answer[i, j] = post[i];
                    potr[j] = potr[j] - post[i];
                    post[i] = post[i] - post[i];
                }
                cloneRate[i, j] = int.MaxValue;
            }
        }
        public void Answer(int[,] answer, int[,] rate, int countV)
        {
            int L = 0;
            int countZ = 0;
            Console.WriteLine("Матрица-ответ: ");
            for (int i = 0; i < answer.GetLength(0); i++)
            {
                for (int j = 0; j < answer.GetLength(1); j++)
                {
                    Console.Write(answer[i, j] + "\t");
                }
                Console.WriteLine();
            }
            for (int i = 0; i < answer.GetLength(0); i++)
            {
                for (int j = 0; j < answer.GetLength(1); j++)
                {
                    L = L + answer[i, j] * rate[i, j];
                    if (answer[i, j] != 0)
                    {
                        countZ++;
                    }
                }
            }
            if (countZ == countV)
            {
                Console.WriteLine("Задача невырожденная");
                Console.WriteLine($"Целевая функция L(x) = {L}");
            }
            string path = "Answer.csv";
            using (StreamWriter streamWriter = new StreamWriter(path, true))
            {
                for (int i = 0; i < answer.GetLength(0); i++)
                {
                    for (int j = 0; j < answer.GetLength(1); j++)
                    {
                        streamWriter.Write(answer[i, j] + ";");
                    }
                    streamWriter.WriteLine();
                }
                streamWriter.WriteLine(L);
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam
{
    internal class PreuferCode
    {
        public struct Ridge
        {
            public int a;
            public int b;

            public void ShowRidge()
            {
                Console.WriteLine(a + "-" + b);
            }
        }
        public void MainPreuferCode()
        {
            List<Ridge> listRidge = new List<Ridge>();
            List<int> listCode = new List<int>();
            int initial;
            GetData("Ridge.csv", ref listRidge);
            Console.WriteLine("Рёбра графа: ");
            foreach (Ridge r in listRidge)
            {
                r.ShowRidge();
            }
            int countInitial;
            bool sheet;
            CheckInitial(ref listRidge, ref listCode, out initial, out countInitial, out sheet);
            RecordTable(listCode);
        }
        public void GetData(string path, ref List<Ridge> listRidge)
        {
            using (StreamReader sr = new StreamReader(path))
            {
                while (sr.EndOfStream != true)
                {
                    string[] arrayData = sr.ReadLine().Split(";");
                    listRidge.Add(new Ridge
                    {
                        a = Convert.ToInt32(arrayData[0]),
                        b = Convert.ToInt32(arrayData[1])
                    });
                }
            }
        }
        public void RecordTable(List<int> listCode)
        {
            string path = "AnswerCodePreufera.csv";
            using (StreamWriter streamWriter = new StreamWriter(path, true))
            {
                foreach (int n in listCode)
                {
                    streamWriter.Write(n + ";");
                }
            }
        }
        public void CheckInitial(ref List<Ridge> listRidge, ref List<int> listCode, out int initial, out int countInitial, out bool sheet)
        {
            initial = 0;
            countInitial = 0;
            sheet = false;
            int count = listRidge.Count - 2;
            for (int n = 0; n <= count; n++)
            {
                List<int> listB = new List<int>();
                List<Ridge> newListRidge = new List<Ridge>();
                for (int i = 0; i < listRidge.Count; i++)
                {
                    bool flag = false;
                    int a = listRidge[i].a;
                    for (int j = 0; j < listRidge.Count; j++)
                    {
                        if (listRidge[j].b != a)
                            flag = true;
                        else
                        {
                            flag = false;
                            break;
                        }
                    }
                    if (flag == true)
                    {
                        initial = a;
                        List<Ridge> list = listRidge.Where(x => x.a == a).ToList();
                        countInitial = list.Count;
                        break;
                    }
                }
                if (countInitial == 1)
                {
                    sheet = true;
                    listB.Add(initial);
                }
                for (int i = 0; i < listRidge.Count; i++)
                {
                    bool flag = false;
                    int b = listRidge[i].b;
                    for (int j = 0; j < listRidge.Count; j++)
                    {
                        if (listRidge[j].a != b)
                            flag = true;
                        else
                        {
                            flag = false;
                            break;
                        }
                    }
                    if (flag == true)
                        listB.Add(listRidge[i].b);
                }
                listB.Sort();
                int minValue = listB[0];
                for (int i = 0; i < listRidge.Count; i++)
                {
                    if (sheet == false)
                    {
                        if (listRidge[i].b != minValue)
                            newListRidge.Add(listRidge[i]);
                        else
                            listCode.Add(listRidge[i].a);
                    }
                }
                if (sheet == true)
                {
                    for (int i = 0; i < listRidge.Count; i++)
                    {
                        if (listRidge[i].a != minValue && listRidge[i].b != minValue)
                        {
                            newListRidge.Add(listRidge[i]);
                        }
                        if (listRidge[i].a == minValue)
                            listCode.Add(listRidge[i].b);
                        if (listRidge[i].b == minValue)
                            listCode.Add(listRidge[i].a);
                    }
                }
                listRidge = newListRidge;
            }
            Console.Write("Код прюфера: ");
            foreach (int t in listCode)
            {
                Console.Write(t + " ");
            }
        }
    }
}
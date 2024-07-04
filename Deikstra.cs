using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Exam.Deikstra;
using static Exam.PreuferCode;

namespace Exam
{
    internal class Deikstra
    {
        public struct Points
        {
            public int pointStart;
            public int pointEnd;
            public int distance;

            
        }
        public struct EstPoint
        {
            public int node;
            public int est;
        }
        public void MainDeikstra()
        {
            int startP;
            int startPoint = 0;
            int H = 0;
            bool flag = false;
            List<Points> points = new List<Points>();
            List<EstPoint> estPoints = new List<EstPoint>();
            List<EstPoint> answerListEst = new List<EstPoint>();
            GetData("Points.csv", ref points, ref estPoints, out startP);
            Decisions(startP, estPoints, points, ref startPoint, ref answerListEst);
            WritingFile(answerListEst, startPoint);
        }
        public void GetData(string path, ref List<Points> points, ref List<EstPoint> estPoints, out int startP)
        {
            int countPoints = 0;
            using (StreamReader sr = new StreamReader(path))
            {
                while (sr.EndOfStream != true)
                {
                    string[] arrayData = sr.ReadLine().Split(";");
                    if (arrayData.Length == 1)
                        countPoints = Convert.ToInt32(arrayData[0]);
                    else
                    {
                        points.Add(new Points
                        {
                            pointStart = Convert.ToInt32(arrayData[0]),
                            pointEnd = Convert.ToInt32(arrayData[1]),
                            distance = Convert.ToInt32(arrayData[2])
                        });
                    }
                }
            }
            for (int i = 1; i <= countPoints; i++)
            {
                if (i == 1)
                {
                    estPoints.Add(new EstPoint
                    {
                        node = i,
                        est = 0,
                    });
                }
                else
                {
                    estPoints.Add(new EstPoint
                    {
                        node = i,
                        est = int.MaxValue,
                    });
                }
            }
            Console.Write("Введите начальную вершину (1): ");
            startP = Convert.ToInt32(Console.ReadLine());
        }
        public void WritingFile(List<EstPoint> answerListEst, int startPoint)
        {
            string path = "AnswerDeikstra.csv";
            using (StreamWriter streamWriter = new StreamWriter(path, true))
            {
                for (int i = 1; i < answerListEst.Count; i++)
                {
                    streamWriter.WriteLine(startPoint + "->" + answerListEst[i].node + " = " + answerListEst[i].est);
                }
            }
        }
        public void Decisions(int startP, List<EstPoint> estPoints, List<Points> points, ref int startPoint, ref List<EstPoint> answerListEst)
        {
            bool flag = false;
            int countIterations = 0;
            startPoint = startP;
           
            while (flag == false)
            {
                int minEst = int.MaxValue;
                for (int i = 0; i < estPoints.Count; i++)
                {
                    if (estPoints[i].est < minEst)
                    {
                        minEst = estPoints[i].est;
                        startP = estPoints[i].node;
                    }
                }
                int estNode = 0; // минимальная оценка вершины
                for (int i = 0; i < estPoints.Count; i++)
                {
                    if (estPoints[i].node == startP)
                    {
                        estNode = estPoints[i].est;
                    }
                }
                List<EstPoint> transitionEst = new List<EstPoint>();
                for (int i = 0; i < points.Count; i++)
                {
                    if (points[i].pointStart == startP)
                    {
                        for (int j = 0; j < estPoints.Count; j++)
                        {
                            if (points[i].pointEnd == estPoints[j].node)
                            {
                                int est = estNode + points[i].distance;
                                if (est < estPoints[j].est)
                                {
                                    transitionEst.Add(new EstPoint
                                    {
                                        node = points[i].pointEnd,
                                        est = est
                                    });
                                }
                                else if (est >= estPoints[j].est)
                                {
                                    transitionEst.Add(new EstPoint
                                    {
                                        node = points[i].pointEnd,
                                        est = estPoints[j].est
                                    });
                                }
                            }
                        }
                    }
                }
                List<EstPoint> dopListEst = new List<EstPoint>();
                for (int i = 0; i < estPoints.Count; i++)
                {
                    bool fl = true;
                    for (int j = 0; j < transitionEst.Count; j++)
                    {
                        if (estPoints[i].node == transitionEst[j].node)
                        {
                            dopListEst.Add(new EstPoint
                            {
                                node = transitionEst[j].node,
                                est = transitionEst[j].est,
                            });
                            fl = false;
                        }
                    }
                    if (fl == true)
                    {
                        dopListEst.Add(new EstPoint
                        {
                            node = estPoints[i].node,
                            est = estPoints[i].est,
                        });
                    }
                }
                List<Points> dopListPoints = new List<Points>();
                for (int i = 0; i < points.Count; i++)
                {
                    if (points[i].pointStart != startP)
                    {
                        dopListPoints.Add(new Points
                        {
                            pointStart = points[i].pointStart,
                            pointEnd = points[i].pointEnd,
                            distance = points[i].distance
                        });
                    }
                }
                points = dopListPoints;
                List<EstPoint> dopAnswerEst = new List<EstPoint>();
                if (answerListEst.Count != 0)
                {
                    for (int i = 0; i < answerListEst.Count; i++)
                    {
                        bool fl = true;
                        for (int j = 0; j < dopListEst.Count; j++)
                        {
                            
                            if (answerListEst[i].node == dopListEst[j].node)
                            {
                                dopAnswerEst.Add(new EstPoint
                                {
                                    node = dopListEst[j].node,
                                    est = dopListEst[j].est,
                                });
                                fl = false;
                            } 
                        }
                        if (fl == true)
                        {
                            dopAnswerEst.Add(new EstPoint
                            {
                                node = answerListEst[i].node,
                                est = answerListEst[i].est,
                            });
                        }
                    }
                    answerListEst = dopAnswerEst;
                }
                else
                {
                    answerListEst = dopListEst;
                }
                    
                transitionEst.Clear();
                for (int i = 0; i < estPoints.Count; i++)
                {
                    for (int j = 0; j < answerListEst.Count; j++)
                    {
                        if (estPoints[i].node != startP && estPoints[i].node == answerListEst[j].node)
                        {
                            transitionEst.Add(new EstPoint
                            {
                                node = answerListEst[j].node,
                                est = answerListEst[j].est,
                            });
                        }
                    }
                    
                }
                estPoints = transitionEst;
                countIterations = points.Count;
                if (points.Count == 0)
                {
                    flag = true;
                }
            }
            foreach (EstPoint p in answerListEst)
            {
                Console.WriteLine(startPoint + "->" + p.node + " = " +  p.est);
            }
            
        }
    }
}
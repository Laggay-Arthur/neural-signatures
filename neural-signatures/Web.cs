using System.Collections.Generic;


namespace neural_signatures
{
    static class Web
    {
        static List<Neiron> neironArray = InitWeb();

        public const int sizex = 60, sizey = 60;//Число входов (100)

        public static void AddNeiron(Neiron neir) => neironArray.Add(neir);

        static List<Neiron> InitWeb()
        {// функция читает из БД сохранённые нейроны и преобразовывает в массив нейронов
            return DataBase.SelectNeiroins();
        }

        static Neiron NeironCreate(Dictionary<string, object> o)
        {// преобразовать структуру данных в класс нейрона
            Neiron res = new Neiron()
            {
                Name = (string)o["name"],
                CountTraining = (int)o["countTraining"]
            };

            object[] weightData = (object[])o["weight"];

            res.Weight = new double[sizex, sizey];

            int index = 0;

            for (int n = 0; n < res.Weight.GetLength(0); n++)
                for (int m = 0; m < res.Weight.GetLength(1); m++)
                {
                    res.Weight[n, m] = double.Parse(weightData[index].ToString());

                    index++;
                }
            return res;
        }

        public static string CheckLitera(ref int[,] arr)
        {
            string res = null;

            double max = 0;

            foreach (var n in neironArray)
            {
                double d = n.GetRes(ref arr);

                if (d > max)
                {
                    max = d;
                    res = n.Name;
                }
            }
            return res;
        }

        public static void InsertNeironToDB(Neiron neir) => DataBase.insertNeiron(neir);

        public static string[] GetSignatures()
        {
            var res = new List<string>();

            for (int i = 0; i < neironArray.Count; i++) res.Add(neironArray[i].Name);
            res.Sort();

            return res.ToArray();
        }

        public static int SetTraining(string trainingName, ref int[,] data)
        {
            Neiron neiron = neironArray.Find(v => v.Name.Equals(trainingName));

            if (neiron == null)
            {
                neiron.Clear(trainingName, sizex, sizey);

                neiron = new Neiron();

                neironArray.Add(neiron);
            }
            return neiron.Training(ref data);
        }
    }
}
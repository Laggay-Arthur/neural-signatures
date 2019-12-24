using System;


namespace neural_signatures
{
    class Neiron
    {
        private int countTraining;// Кол-во тренеровок нейрона
        private string name;// Имя нейрона
        public double[,] Weight { get; set; } = new double[60, 60];
        public Neiron() { }

        public string Name
        {
            get => name;
            set => Name = value;
        }

        public int CountTraining
        {
            get => countTraining;
            set => countTraining++;
        }       

        public Neiron(string name, int countTraining)
        {
            this.name = name;
            this.countTraining = countTraining;
        }

        public void Clear(string name, int x, int y)
        {
            this.name = name;
            Weight = new double[x, y];

            for (int n = 0; n < Weight.GetLength(0); n++)
                for (int m = 0; m < Weight.GetLength(1); m++) Weight[n, m] = 0;

            countTraining = 0;
        }

        public double GetRes(ref int[,] data)
        {
            if (Weight == null || Weight.GetLength(0) != data.GetLength(0) || Weight.GetLength(1) != data.GetLength(1)) return -1;

            double res = 0;

            for (int n = 0; n < Weight.GetLength(0); n++)
                for (int m = 0; m < Weight.GetLength(1); m++)
                    res += 1 - Math.Abs(Weight[n, m] - data[n, m]);

            return res / (Weight.GetLength(0) * Weight.GetLength(1));
        }

        public int Training(ref int[,] data)
        {
            if (data == null || Weight == null || Weight.GetLength(0) != data.GetLength(0) || Weight.GetLength(1) != data.GetLength(1)) return countTraining;

            countTraining++;

            for (int n = 0; n < Weight.GetLength(0); n++)
                for (int m = 0; m < Weight.GetLength(1); m++)
                {
                    double v = data[n, m] == 0 ? 0 : 1;

                    Weight[n, m] += 2 * (v - 0.5f) / countTraining;

                    if (Weight[n, m] > 1) Weight[n, m] = 1;
                    if (Weight[n, m] < 0) Weight[n, m] = 0;
                }
            return countTraining;
        }
    }
}
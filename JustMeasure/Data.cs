namespace JustMeasure
{
    public class Data
    {
        private double _coordX;
        private double _coordY;

        public Data()
        { }

        public Data(double coordX, double coordY)
        {
            _coordX = coordX;
            _coordY = coordY;
        }
        public double X
        {
            get { return _coordX; }
            set { _coordX = value; }
        }

        public double Y
        {
            get { return _coordY; }
            set { _coordY = value; }
        }
    }
}

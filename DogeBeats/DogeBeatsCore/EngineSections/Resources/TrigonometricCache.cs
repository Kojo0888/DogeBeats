using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogeBeats.Modules.Centers
{
    public class TrigonometricCache
    {
        private Dictionary<decimal, double> _values { get; set; } = new Dictionary<decimal, double>();

        private decimal _step { get; set; }

        public TrigonometricCache(string typename, double step)
        {
            _step = (decimal)step;

            if(typename.ToLower() == "sin")
            {
                PopulateSin();
            }
        }

        public double Get(double value)
        {
            decimal key = (decimal)value;// - (value % _step);
            if (_values.ContainsKey(key))
            {
                return _values[key];
            }
            else throw new Exception("Trigonometric does not have such value like " + key);
        }

        private void PopulateSin()
        {
            _values = new Dictionary<decimal, double>();

            for (decimal i = 0; i <= 2; i += _step)
            {
                _values.Add(i, Math.Sin((double)i));
            }
        }
    }
}

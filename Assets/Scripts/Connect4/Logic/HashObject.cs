using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4.Classes
{

    public enum Flag
    {
        Exact,
        Upperbound,
        Lowerbound
    }
    public class HashObject
    {
        UInt64 key;
        int dubina;
        int _value;
        Flag flag;


        public UInt64 Key
        {
            get { return key; }
            set { key = value; }
        }

        public int Dubina
        {
            get
            {
                return dubina;
            }
            set
            {
                dubina = value;
            }
        }
        public int Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }
        public Flag Flag
        {
            get
            {
                return flag;
            }
            set
            {
                flag = value;
            }
        }
    }
}

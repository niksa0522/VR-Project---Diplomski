using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4.Classes
{

    



    public class TranspositionTable
    {

        HashObject[] table;

        public TranspositionTable(uint size)
        {
            table = new HashObject[size];
        }
        public void Clear()
        {
            for (int i = 0; i < table.Length; i++)
            {
                table[i] = null;
            }
        }
        uint index(UInt64 key)
        {
            return (uint)(key % (UInt64)table.Length);
        }

        public void put(UInt64 key, int value, int dubina, Flag flag)
        {
            if (key < ((UInt64)(1) << 56))
            {
                uint i = index(key);
                table[i] = new HashObject();
                table[i].Key = key;
                table[i].Value = value;
                table[i].Dubina = dubina;
                table[i].Flag = flag;
            }
        }

        public HashObject get(UInt64 key)
        {
            if (key < ((UInt64)(1) << 56))
            {
                uint i = index(key);
                if(table[i] != null)
                {
                    if (table[i].Key == key)
                        return table[i];
                }
                else
                    return null;
            }
            return null;
        }




    }


    
}

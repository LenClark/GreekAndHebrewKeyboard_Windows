using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GkHebKeyboard
{
    class classKeyData
    {
        int keyboardRow, keyboardCol;
        String nativeKeyValue;

        public classKeyData()
        {
            nativeKeyValue = "";
        }

        public int KeyboardRow
        {
            get { return keyboardRow; }
            set { keyboardRow = value; }
        }

        public int KeyboardCol
        {
            get { return keyboardCol; }
            set { keyboardCol = value; }
        }

        public String NativeKeyValue
        {
            get { return nativeKeyValue; }
            set { nativeKeyValue = value; }
        }
    }
}

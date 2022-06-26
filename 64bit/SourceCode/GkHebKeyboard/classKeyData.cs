using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GkHebKeyboard
{
    internal class classKeyData
    {
        int keyboardRow, keyboardCol;
        String nativeKeyValue;

        public int KeyboardRow { get => keyboardRow; set => keyboardRow = value; }
        public int KeyboardCol { get => keyboardCol; set => keyboardCol = value; }
        public string NativeKeyValue { get => nativeKeyValue; set => nativeKeyValue = value; }

        public classKeyData()
        {
            nativeKeyValue = "";
        }
    }
}

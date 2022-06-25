using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GkHebKeyboard
{
    internal class classGreek
    {
        /*****************************************************************************************************************
         *                                                                                                               *
         *                                              classGreek                                                       *
         *                                              ==========                                                       *
         *                                                                                                               *
         *  All manipulations of Greek text will, where possible, be handled here.  Envisaged tasks are:                 *
         *                                                                                                               *
         *  1. Store bare vowels                                                                                         *
         *  2. Store bare consonants                                                                                     *
         *  3. Store all base letters                                                                                    *
         *  4. Handle the transition from a letter with "furniture" (e.g. accents, diaraesis, etc - excluding            *
         *       breathings) to related letters with either no diacritics or with a breathing only;                      *
         *  5. Handle the transition from a letter with breathings to the base equivalent                                *
         *  6. Potentially, handle the addition of diacritics (not currently needed)                                     *
         *  7. Potentially, handle the transition from transliterations to the real Greek character (not currently       *
         *       needed)                                                                                                 *
         *                                                                                                               *
         *****************************************************************************************************************/

        const int mainCharsStart = 0x0386, mainCharsEnd = 0x03ce, furtherCharsStart = 0x1f00, furtherCharsEnd = 0x1ffc;

        // hexadecimal punctuation characters are: the middle dot, the ano teleia (partial stop, which is 
        //    functionally the same as the middle dot and the unicode documentation says that the middle
        //    dot is the "preferred character") and the erotimatiko (the Greek question mark, identical in 
        //    appearance to the semi colon).  So, elements 1 (zero-based) and 5 are equivalent and elements
        //    3 and 4 are equivalent.  Both are included in case source material has varied usage.
        String[] allowedPunctuation = { ".", ";", ",", "\u00b7", "\u0387", "\u037e" };
        SortedDictionary<int, String> allGkChars = new SortedDictionary<int, string>();
        //        SortedList<int, int> conversionWithBreathings = new SortedList<int, int>();
        SortedDictionary<String, String> addRoughBreathing, addSmoothBreathing, addAccute, addGrave, addCirc, addDiaeresis, addIotaSub;

        public SortedDictionary<string, string> AddRoughBreathing { get => addRoughBreathing; set => addRoughBreathing = value; }
        public SortedDictionary<string, string> AddSmoothBreathing { get => addSmoothBreathing; set => addSmoothBreathing = value; }
        public SortedDictionary<string, string> AddAccute { get => addAccute; set => addAccute = value; }
        public SortedDictionary<string, string> AddGrave { get => addGrave; set => addGrave = value; }
        public SortedDictionary<string, string> AddCirc { get => addCirc; set => addCirc = value; }
        public SortedDictionary<string, string> AddDiaeresis { get => addDiaeresis; set => addDiaeresis = value; }
        public SortedDictionary<string, string> AddIotaSub { get => addIotaSub; set => addIotaSub = value; }

        public void constructGreekLists()
        {
            /************************************************************************************************************
             *                                                                                                          *                                                                                *
             *                                       constructGreekLists                                                *
             *                                       ===================                                                *
             *                                                                                                          *
             *  The coding works on the following basis:                                                                *
             *      a) Each base vowel has an integer value in ascending order.  So:                                    *
             *              α = 1                                                                                       *
             *              ε = 2                                                                                       *
             *              η = 3                                                                                       *
             *              ι = 4                                                                                       *
             *              ο = 5                                                                                       *
             *              υ = 6                                                                                       *
             *              ω = 7                                                                                       *
             *         This value applies whether it is miniscule or majiscule                                          *
             *      b) Add 10 if the vowel has a "grave" accent (varia)                                                 *
             *      c) Add 20 if the vowel has an "accute" accent (oxia)                                                *
             *      d) Add 30 if the vowel has a "circumflex" accent (perispomeni)                                      *
             *      e) Add 40 if the vowel is in the base table and has an accute accent (oxia) i.e. it is an           *
             *           alternative coding - it's actually a tonos, not an oxia                                        *
             *      f) Add 50 if the vowel has a vrachy (cannot have a breathing or accent)                             *
             *      g) Add 60 if the vowel has a macron                                                                 *
             *      h) Add 100 if the vowel has a smooth breathing (psili)                                              *
             *      i) Add 200 if the vowel has a rough breathing (dasia)                                               *
             *      j) Add 300 if the vowel has dieresis (dialytika) - only ι and υ                                     *
             *      k) Add 1000 if the vowel has an iota subscript - only α, η and ω                                    *
             *      l) Add 10000 if a majuscule                                                                         *
             *                                                                                                          *
             *  charCode1 (a and b):                                                                                    *
             *    purpose: to indicate whether a char is vowel (and, if so, which), another, extra character (i.e. rho  *
             *             or final sigma), a simple letter or puntuation.                                              *
             *    Code values are:                                                                                      *
             *     -1     A null value - to be ignored                                                                  *
             *      0 - 6 alpha to omega respectively - not simple                                                      *
             *      7     rho - not simple                                                                              *
             *      8     final sigma                                                                                   *
             *      9     simple alphabet                                                                               *
             *      10    punctuation                                                                                   *
             *                                                                                                          *
             *   charCode2:                                                                                             *
             *     purpose: identify whether char has a smooth breathing, rough breathing or none.  All part a chars    * 
             *              are without breathing, so only part b characters are coded. (Note, however, that 0x03ca and *
             *              0x03cb are diereses (code value 3).                                                         *
             *     Code values are:                                                                                     *
             *       0   No breathing                                                                                   *
             *       1   Smooth breathing                                                                               *
             *       2   Rough breathing                                                                                *
             *       3   Dieresis                                                                                       *
             *                                                                                                          *
             ************************************************************************************************************/

            /*............................................................*
             *  A table matching the main Greek table in Unicode          *
             *............................................................*/

            int[] gkTable1 = { 0x03b1, -1, 0x03b5, 0x03b7, 0x03b9, -1, 0x03bf, -1, 0x03c5, 0x03c9, 0x03ca,
                           0x03b1, 0x03b2, 0x03b3, 0x03b4, 0x03b5, 0x03b6, 0x03b7, 0x03b8, 0x03b9, 0x03ba, 0x03bb, 0x03bc, 0x03bd, 0x03be, 0x03bf,
                           0x03c0, 0x03c1, -1, 0x03c3, 0x03c4, 0x03c5, 0x03c6, 0x03c7, 0x03c8, 0x03c9, 0x03ca, 0x03cb, 0x03b1, 0x03b5, 0x03b7, 0x03b9, 0x03cb,
                           0x03b1, 0x03b2, 0x03b3, 0x03b4, 0x03b5, 0x03b6, 0x03b7, 0x03b8, 0x03b9, 0x03ba, 0x03bb, 0x03bc, 0x03bd, 0x03be, 0x03bf,
                           0x03c0, 0x03c1, 0x03c2, 0x03c3, 0x03c4, 0x03c5, 0x03c6, 0x03c7, 0x03c8, 0x03c9, 0x03ca, 0x03cb, 0x03bf, 0x03c5, 0x03c9};

            /*............................................................*
             *  A table matching the additional Greek table in Unicode    *
             *............................................................*/

            int[] gkTable2 = { 0x1f00, 0x1f01, 0x1f00, 0x1f01, 0x1f00, 0x1f01, 0x1f00, 0x1f01, 0x1f00, 0x1f01, 0x1f00, 0x1f01, 0x1f00, 0x1f01, 0x1f00, 0x1f01,
                           0x1f10, 0x1f11, 0x1f10, 0x1f11, 0x1f10, 0x1f11, -1, -1, 0x1f10, 0x1f11, 0x1f10, 0x1f11, 0x1f10, 0x1f11, -1, -1,
                           0x1f20, 0x1f21, 0x1f20, 0x1f21, 0x1f20, 0x1f21, 0x1f20, 0x1f21, 0x1f20, 0x1f21, 0x1f20, 0x1f21, 0x1f20, 0x1f21, 0x1f20, 0x1f21,
                           0x1f30, 0x1f31, 0x1f30, 0x1f31, 0x1f30, 0x1f31, 0x1f30, 0x1f31, 0x1f30, 0x1f31, 0x1f30, 0x1f31, 0x1f30, 0x1f31, 0x1f30, 0x1f31,
                           0x1f40, 0x1f41, 0x1f40, 0x1f41, 0x1f40, 0x1f41, -1, -1, 0x1f40, 0x1f41, 0x1f40, 0x1f41, 0x1f40, 0x1f41, -1, -1,
                           0x1f50, 0x1f51, 0x1f50, 0x1f51, 0x1f50, 0x1f51, 0x1f50, 0x1f51, -1, 0x1f51, -1, 0x1f51, -1, 0x1f51, -1, 0x1f51,
                           0x1f60, 0x1f61, 0x1f60, 0x1f61, 0x1f60, 0x1f61, 0x1f60, 0x1f61, 0x1f60, 0x1f61, 0x1f60, 0x1f61, 0x1f60, 0x1f61, 0x1f60, 0x1f61,
                           0x03b1, 0x03b1, 0x03b5, 0x03b5, 0x03b7, 0x03b7, 0x03b9, 0x03b9, 0x03bf, 0x03bf, 0x03c5, 0x03c5, 0x03c9, 0x03c9, -1, -1,
                           0x1f00, 0x1f01, 0x1f00, 0x1f01, 0x1f00, 0x1f01, 0x1f00, 0x1f01, 0x1f00, 0x1f01, 0x1f00, 0x1f01, 0x1f00, 0x1f01, 0x1f00, 0x1f01,
                           0x1f20, 0x1f21, 0x1f20, 0x1f21, 0x1f20, 0x1f21, 0x1f20, 0x1f21, 0x1f20, 0x1f21, 0x1f20, 0x1f21, 0x1f20, 0x1f21, 0x1f20, 0x1f21,
                           0x1f60, 0x1f61, 0x1f60, 0x1f61, 0x1f60, 0x1f61, 0x1f60, 0x1f61, 0x1f60, 0x1f61, 0x1f60, 0x1f61, 0x1f60, 0x1f61, 0x1f60, 0x1f61,
                           0x03b1, 0x03b1, 0x03b1, 0x03b1, 0x03b1, -1, 0x03b1, 0x03b1, 0x03b1, 0x03b1, 0x03b1, 0x03b1, 0x03b1, -1, -1, -1,
                           -1, -1, 0x03b7, 0x03b7, 0x03b7, -1, 0x03b7, 0x03b7, 0x03b5, 0x03b5, 0x03b7, 0x03b7, 0x03b7, -1, -1, -1,
                           0x03b9, 0x03b9, 0x03ca, 0x03ca, -1, -1, 0x03b9, 0x03ca, 0x03b9, 0x03b9, 0x03b9, 0x03b9, -1, -1, -1, -1,
                           0x03c5, 0x03c5, 0x03cb, 0x03cb, 0x1fe4, 0x1fe5, 0x03c5, 0x03cb, 0x03c5, 0x03c5, 0x03c5, 0x03c5, 0x1fe5, -1, -1, -1,
                           -1, -1, 0x03c9, 0x03c9, 0x03c9, -1, 0x03c9, 0x03c9, 0x03bf, 0x03bf, 0x03c9, 0x03c9, 0x03c9, -1, -1, -1 };
            int idx, mainCharIndex, furtherCharIndex;
            String charRepresentation;

            // Load the two Unicode tables into memory.  These are stored as:
            //           - base characters (and a few extras);
            //           - characters with accents, breathings, iota subscript, etc.
            // allGkChars: key = the code value, value = the code converted to a string character
            idx = 0;
            for (mainCharIndex = mainCharsStart; mainCharIndex <= mainCharsEnd; mainCharIndex++)
            {
                charRepresentation = (Convert.ToChar(mainCharIndex)).ToString();
                if (String.Compare(charRepresentation, "΋") == 0)
                {
                    idx++;
                    continue;
                }
                allGkChars.Add(mainCharIndex, charRepresentation);
                //                conversionWithBreathings.Add(mainCharIndex, gkTable1[idx]);
                idx++;
            }
            idx = 0;
            for (furtherCharIndex = furtherCharsStart; furtherCharIndex <= furtherCharsEnd; furtherCharIndex++)
            {
                charRepresentation = (Convert.ToChar(furtherCharIndex)).ToString();
                if (String.Compare(charRepresentation, "΋") == 0)
                {
                    idx++;
                    continue;
                }
                allGkChars.Add(furtherCharIndex, charRepresentation);
                //                conversionWithBreathings.Add(furtherCharIndex, gkTable2[idx]);
                idx++;
            }
            charRepresentation = (Convert.ToChar(0x03dc).ToString());  // Majuscule and miuscule digamma
            allGkChars.Add(0x03dc, charRepresentation);
            //            conversionWithBreathings.Add(0x03dc, 0x03dd);
            charRepresentation = (Convert.ToChar(0x03dd).ToString());
            allGkChars.Add(0x03dd, charRepresentation);
            //            conversionWithBreathings.Add(0x03dd, 0x03dd);

            // Set up the vowel conversion table
            addRoughBreathing = new SortedDictionary<string, string>();
            addRoughBreathing.Add("α", "\u1f01");
            addRoughBreathing.Add("\u1f70", "\u1f03");
            addRoughBreathing.Add("ά", "\u1f05");
            addRoughBreathing.Add("\u1fb6", "\u1f07");
            addRoughBreathing.Add("\u1fb3", "\u1f81");
            addRoughBreathing.Add("\u1fb2", "\u1f83");
            addRoughBreathing.Add("\u1fb4", "\u1f85");
            addRoughBreathing.Add("\u1fb7", "\u1f87");
            addRoughBreathing.Add("ε", "\u1f11");
            addRoughBreathing.Add("\u1f72", "\u1f13");
            addRoughBreathing.Add("έ", "\u1f15");
            addRoughBreathing.Add("η", "\u1f21");
            addRoughBreathing.Add("\u1f74", "\u1f23");
            addRoughBreathing.Add("ή", "\u1f25");
            addRoughBreathing.Add("\u1fc6", "\u1f27");
            addRoughBreathing.Add("\u1fc3", "\u1f91");
            addRoughBreathing.Add("\u1fc2", "\u1f93");
            addRoughBreathing.Add("\u1fc4", "\u1f95");
            addRoughBreathing.Add("\u1fc7", "\u1f97");
            addRoughBreathing.Add("ι", "\u1f31");
            addRoughBreathing.Add("\u1f76", "\u1f33");
            addRoughBreathing.Add("ί", "\u1f35");
            addRoughBreathing.Add("\u1fd6", "\u1f37");
            addRoughBreathing.Add("ο", "\u1f41");
            addRoughBreathing.Add("\u1f78", "\u1f43");
            addRoughBreathing.Add("ό", "\u1f45");
            addRoughBreathing.Add("υ", "\u1f51");
            addRoughBreathing.Add("\u1f7a", "\u1f53");
            addRoughBreathing.Add("ύ", "\u1f55");
            addRoughBreathing.Add("\u1fe6", "\u1f57");
            addRoughBreathing.Add("ω", "\u1f61");
            addRoughBreathing.Add("\u1f7c", "\u1f63");
            addRoughBreathing.Add("ώ", "\u1f65");
            addRoughBreathing.Add("\u1ff6", "\u1f67");
            addRoughBreathing.Add("\u1ff3", "\u1fa1");
            addRoughBreathing.Add("\u1ff2", "\u1fa3");
            addRoughBreathing.Add("\u1ff4", "\u1fa5");
            addRoughBreathing.Add("\u1ff7", "\u1fa7");
            addRoughBreathing.Add("ρ", "ῥ");
            addRoughBreathing.Add("Α", "\u1f09");
            addRoughBreathing.Add("\u1fba", "\u1f0b");
            addRoughBreathing.Add("Ά", "\u1f0d");
            addRoughBreathing.Add("\u1fbc", "\u1f89");
            addRoughBreathing.Add("Ε", "\u1f19");
            addRoughBreathing.Add("\u1fc8", "\u1f1b");
            addRoughBreathing.Add("Έ", "\u1f1d");
            addRoughBreathing.Add("Η", "\u1f29");
            addRoughBreathing.Add("\u1fca", "\u1f2b");
            addRoughBreathing.Add("Ή", "\u1f2d");
            addRoughBreathing.Add("\u1fcc", "\u1f99");
            addRoughBreathing.Add("Ι", "\u1f39");
            addRoughBreathing.Add("\u1fda", "\u1f3b");
            addRoughBreathing.Add("Ί", "\u1f3d");
            addRoughBreathing.Add("Ο", "\u1f49");
            addRoughBreathing.Add("\u1ff8", "\u1f4b");
            addRoughBreathing.Add("Ό", "\u1f4d");
            addRoughBreathing.Add("Υ", "\u1f59");
            addRoughBreathing.Add("\u1fea", "\u1f5b");
            addRoughBreathing.Add("Ύ", "\u1f5d");
            addRoughBreathing.Add("Ω", "\u1f69");
            addRoughBreathing.Add("\u1ffa", "\u1f6b");
            addRoughBreathing.Add("Ώ", "\u1f6d");
            addRoughBreathing.Add("\u1ffc", "\u1fa9");
            addRoughBreathing.Add("Ρ", "Ῥ");

            addSmoothBreathing = new SortedDictionary<string, string>();
            addSmoothBreathing.Add("α", "\u1f00");
            addSmoothBreathing.Add("\u1f70", "\u1f02");
            addSmoothBreathing.Add("ά", "\u1f04");
            addSmoothBreathing.Add("\u1fb6", "\u1f06");
            addSmoothBreathing.Add("\u1fb3", "\u1f80");
            addSmoothBreathing.Add("\u1fb2", "\u1f82");
            addSmoothBreathing.Add("\u1fb4", "\u1f84");
            addSmoothBreathing.Add("\u1fb7", "\u1f86");
            addSmoothBreathing.Add("ε", "\u1f10");
            addSmoothBreathing.Add("\u1f72", "\u1f12");
            addSmoothBreathing.Add("έ", "\u1f14");
            addSmoothBreathing.Add("η", "\u1f20");
            addSmoothBreathing.Add("\u1f74", "\u1f22");
            addSmoothBreathing.Add("ή", "\u1f24");
            addSmoothBreathing.Add("\u1fc6", "\u1f26");
            addSmoothBreathing.Add("\u1fc3", "\u1f90");
            addSmoothBreathing.Add("\u1fc2", "\u1f92");
            addSmoothBreathing.Add("\u1fc4", "\u1f94");
            addSmoothBreathing.Add("\u1fc7", "\u1f96");
            addSmoothBreathing.Add("ι", "\u1f30");
            addSmoothBreathing.Add("\u1f76", "\u1f32");
            addSmoothBreathing.Add("ί", "\u1f34");
            addSmoothBreathing.Add("\u1fd6", "\u1f36");
            addSmoothBreathing.Add("ο", "\u1f40");
            addSmoothBreathing.Add("\u1f78", "\u1f42");
            addSmoothBreathing.Add("ό", "\u1f44");
            addSmoothBreathing.Add("υ", "\u1f50");
            addSmoothBreathing.Add("\u1f7a", "\u1f52");
            addSmoothBreathing.Add("ύ", "\u1f54");
            addSmoothBreathing.Add("\u1fe6", "\u1f56");
            addSmoothBreathing.Add("ω", "\u1f60");
            addSmoothBreathing.Add("\u1f7c", "\u1f62");
            addSmoothBreathing.Add("ώ", "\u1f64");
            addSmoothBreathing.Add("\u1ff6", "\u1f66");
            addSmoothBreathing.Add("\u1ff3", "\u1fa0");
            addSmoothBreathing.Add("\u1ff2", "\u1fa2");
            addSmoothBreathing.Add("\u1ff4", "\u1fa4");
            addSmoothBreathing.Add("\u1ff7", "\u1fa6");
            addSmoothBreathing.Add("ρ", "ῤ");
            addSmoothBreathing.Add("Α", "\u1f08");
            addSmoothBreathing.Add("\u1fba", "\u1f0a");
            addSmoothBreathing.Add("Ά", "\u1f0c");
            addSmoothBreathing.Add("\u1fbc", "\u1f88");
            addSmoothBreathing.Add("Ε", "\u1f18");
            addSmoothBreathing.Add("\u1fc8", "\u1f1a");
            addSmoothBreathing.Add("Έ", "\u1f1c");
            addSmoothBreathing.Add("Η", "\u1f28");
            addSmoothBreathing.Add("\u1fca", "\u1f2a");
            addSmoothBreathing.Add("Ή", "\u1f2c");
            addSmoothBreathing.Add("\u1fcc", "\u1f98");
            addSmoothBreathing.Add("Ι", "\u1f38");
            addSmoothBreathing.Add("\u1fda", "\u1f3a");
            addSmoothBreathing.Add("Ί", "\u1f3c");
            addSmoothBreathing.Add("Ο", "\u1f48");
            addSmoothBreathing.Add("\u1ff8", "\u1f4a");
            addSmoothBreathing.Add("Ό", "\u1f4c");
            addSmoothBreathing.Add("Υ", "\u1f58");
            addSmoothBreathing.Add("\u1fea", "\u1f5a");
            addSmoothBreathing.Add("Ύ", "\u1f5c");
            addSmoothBreathing.Add("Ω", "\u1f68");
            addSmoothBreathing.Add("\u1ffa", "\u1f6a");
            addSmoothBreathing.Add("Ώ", "\u1f6c");
            addSmoothBreathing.Add("\u1ffc", "\u1fa8");

            addAccute = new SortedDictionary<string, string>();
            addAccute.Add("α", "ά");
            addAccute.Add("\u1f00", "\u1f04");
            addAccute.Add("\u1f01", "\u1f05");
            addAccute.Add("\u1fb3", "\u1fb4");
            addAccute.Add("\u1f80", "\u1f84");
            addAccute.Add("\u1f81", "\u1f85");
            addAccute.Add("ε", "έ");
            addAccute.Add("\u1f10", "\u1f14");
            addAccute.Add("\u1f11", "\u1f15");
            addAccute.Add("η", "ή");
            addAccute.Add("\u1f20", "\u1f24");
            addAccute.Add("\u1f21", "\u1f25");
            addAccute.Add("\u1fc3", "\u1fc4");
            addAccute.Add("\u1f90", "\u1f94");
            addAccute.Add("\u1f91", "\u1f95");
            addAccute.Add("ι", "ί");
            addAccute.Add("\u1f30", "\u1f34");
            addAccute.Add("\u1f31", "\u1f35");
            addAccute.Add("ϊ", "\u1fd3");
            addAccute.Add("ο", "ό");
            addAccute.Add("\u1f40", "\u1f44");
            addAccute.Add("\u1f41", "\u1f45");
            addAccute.Add("υ", "ύ");
            addAccute.Add("\u1f50", "\u1f54");
            addAccute.Add("\u1f51", "\u1f55");
            addAccute.Add("ϋ", "ΰ");
            addAccute.Add("ω", "ώ");
            addAccute.Add("\u1f60", "\u1f64");
            addAccute.Add("\u1f61", "\u1f65");
            addAccute.Add("\u1ff3", "\u1ff4");
            addAccute.Add("\u1fa0", "\u1fa4");
            addAccute.Add("\u1fa1", "\u1fa5");
            addAccute.Add("Α", "Ά");
            addAccute.Add("\u1f08", "\u1f0c");
            addAccute.Add("\u1f09", "\u1f0d");
            addAccute.Add("\u1f88", "\u1f8c");
            addAccute.Add("\u1f89", "\u1f8d");
            addAccute.Add("Ε", "Έ");
            addAccute.Add("\u1f18", "\u1f1c");
            addAccute.Add("\u1f19", "\u1f1d");
            addAccute.Add("Η", "Ή");
            addAccute.Add("\u1f28", "\u1f2c");
            addAccute.Add("\u1f29", "\u1f2d");
            addAccute.Add("\u1f98", "\u1f9c");
            addAccute.Add("\u1f99", "\u1f9d");
            addAccute.Add("Ι", "Ί");
            addAccute.Add("\u1f38", "\u1f3c");
            addAccute.Add("\u1f39", "\u1f3d");
            addAccute.Add("Ϊ", "\u1fd9");
            addAccute.Add("Ο", "Ό");
            addAccute.Add("\u1f48", "\u1f4c");
            addAccute.Add("\u1f49", "\u1f4d");
            addAccute.Add("Υ", "Ύ");
            addAccute.Add("\u1f58", "\u1f5c");
            addAccute.Add("\u1f59", "\u1f5d");
            addAccute.Add("Ϋ", "Ϋ");
            addAccute.Add("Ω", "Ώ");
            addAccute.Add("\u1f68", "\u1f6c");
            addAccute.Add("\u1f69", "\u1f6d");
            addAccute.Add("\u1fa8", "\u1fac");
            addAccute.Add("\u1fa9", "\u1fad");

            addGrave = new SortedDictionary<string, string>();
            addGrave.Add("α", "\u1f70");
            addGrave.Add("\u1f00", "\u1f02");
            addGrave.Add("\u1f01", "\u1f03");
            addGrave.Add("\u1fb3", "\u1fb2");
            addGrave.Add("\u1f80", "\u1f82");
            addGrave.Add("\u1f81", "\u1f83");
            addGrave.Add("ε", "\u1f72");
            addGrave.Add("\u1f10", "\u1f12");
            addGrave.Add("\u1f11", "\u1f13");
            addGrave.Add("η", "\u1f74");
            addGrave.Add("\u1f20", "\u1f22");
            addGrave.Add("\u1f21", "\u1f23");
            addGrave.Add("\u1fc3", "\u1fc2");
            addGrave.Add("\u1f90", "\u1f92");
            addGrave.Add("\u1f91", "\u1f93");
            addGrave.Add("ι", "\u1f76");
            addGrave.Add("\u1f30", "\u1f32");
            addGrave.Add("\u1f31", "\u1f33");
            addGrave.Add("ϊ", "\u1fd2");
            addGrave.Add("ο", "\u1f78");
            addGrave.Add("\u1f40", "\u1f42");
            addGrave.Add("\u1f41", "\u1f43");
            addGrave.Add("υ", "\u1f7a");
            addGrave.Add("\u1f50", "\u1f52");
            addGrave.Add("\u1f51", "\u1f53");
            addGrave.Add("ϋ", "\u1fe2");
            addGrave.Add("ω", "\u1f7c");
            addGrave.Add("\u1f60", "\u1f62");
            addGrave.Add("\u1f61", "\u1f63");
            addGrave.Add("\u1ff3", "\u1ff2");
            addGrave.Add("\u1fa0", "\u1fa2");
            addGrave.Add("\u1fa1", "\u1fa3");
            addGrave.Add("Α", "\u1fba");
            addGrave.Add("\u1f08", "\u1f0a");
            addGrave.Add("\u1f09", "\u1f0b");
            addGrave.Add("\u1f88", "\u1f8a");
            addGrave.Add("\u1f89", "\u1f8b");
            addGrave.Add("Ε", "\u1fc8");
            addGrave.Add("\u1f18", "\u1f1a");
            addGrave.Add("\u1f19", "\u1f1b");
            addGrave.Add("Η", "\u1fca");
            addGrave.Add("\u1f28", "\u1f2a");
            addGrave.Add("\u1f29", "\u1f2b");
            addGrave.Add("\u1f98", "\u1f9a");
            addGrave.Add("\u1f99", "\u1f9b");
            addGrave.Add("Ι", "\u1fda");
            addGrave.Add("\u1f38", "\u1f3a");
            addGrave.Add("\u1f39", "\u1f3b");
            addGrave.Add("Ο", "\u1ff8");
            addGrave.Add("\u1f48", "\u1f4a");
            addGrave.Add("\u1f49", "\u1f4b");
            addGrave.Add("Υ", "\u1fea");
            addGrave.Add("\u1f58", "\u1f5a");
            addGrave.Add("\u1f59", "\u1f5b");
            addGrave.Add("Ω", "\u1ffa");
            addGrave.Add("\u1f68", "\u1f6a");
            addGrave.Add("\u1f69", "\u1f6b");
            addGrave.Add("\u1fa8", "\u1faa");
            addGrave.Add("\u1fa9", "\u1fab");

            addCirc = new SortedDictionary<string, string>();
            addCirc.Add("α", "\u1fb6");
            addCirc.Add("\u1f00", "\u1f06");
            addCirc.Add("\u1f01", "\u1f07");
            addCirc.Add("\u1fb3", "\u1fb7");
            addCirc.Add("\u1f80", "\u1f86");
            addCirc.Add("\u1f81", "\u1f87");
            addCirc.Add("η", "\u1fc6");
            addCirc.Add("\u1f20", "\u1f26");
            addCirc.Add("\u1f21", "\u1f27");
            addCirc.Add("\u1fc3", "\u1fc7");
            addCirc.Add("\u1fc7", "\u1fc2");
            addCirc.Add("\u1f90", "\u1f96");
            addCirc.Add("\u1f91", "\u1f97");
            addCirc.Add("ι", "\u1fd6");
            addCirc.Add("\u1f30", "\u1f36");
            addCirc.Add("\u1f31", "\u1f37");
            addCirc.Add("ϊ", "\u1fd7");
            addCirc.Add("υ", "\u1fe6");
            addCirc.Add("\u1f50", "\u1f56");
            addCirc.Add("\u1f51", "\u1f57");
            addCirc.Add("ϋ", "\u1fe7");
            addCirc.Add("ω", "\u1ff6");
            addCirc.Add("\u1f60", "\u1f66");
            addCirc.Add("\u1f61", "\u1f67");
            addCirc.Add("\u1ff3", "\u1ff7");  //#
            addCirc.Add("\u1fa0", "\u1fa6");
            addCirc.Add("\u1fa1", "\u1fa7");
            addCirc.Add("\u1f08", "\u1f0e");
            addCirc.Add("\u1f09", "\u1f0f");
            addCirc.Add("\u1f88", "\u1f8e");
            addCirc.Add("\u1f89", "\u1f8f"); //Α
            addCirc.Add("\u1f28", "\u1f2e");
            addCirc.Add("\u1f29", "\u1f2f");
            addCirc.Add("\u1f98", "\u1f9e");
            addCirc.Add("\u1f99", "\u1f9f"); //Η
            addCirc.Add("\u1f38", "\u1f3e");
            addCirc.Add("\u1f39", "\u1f3f"); //Ι
            addCirc.Add("\u1f58", "\u1f5e");
            addCirc.Add("\u1f59", "\u1f5f"); //Υ
            addCirc.Add("\u1f68", "\u1f6e");
            addCirc.Add("\u1f69", "\u1f6f");
            addCirc.Add("\u1fa8", "\u1fae");
            addCirc.Add("\u1fa9", "\u1faf"); //Ω

            addDiaeresis = new SortedDictionary<string, string>();
            addDiaeresis.Add("ι", "ϊ");
            addDiaeresis.Add("\u1f76", "\u1fd2");
            addDiaeresis.Add("ί", "\u1fd3");
            addDiaeresis.Add("\u1fd6", "\u1fd7");
            addDiaeresis.Add("υ", "ϋ");
            addDiaeresis.Add("\u1f7a", "\u1fe2");
            addDiaeresis.Add("ύ", "ΰ");
            addDiaeresis.Add("\u1fe6", "\u1fe7");
            addDiaeresis.Add("Ι", "Ϊ");
            addDiaeresis.Add("Υ", "Ϋ");

            addIotaSub = new SortedDictionary<string, string>();
            addIotaSub.Add("α", "\u1fb3");
            addIotaSub.Add("\u1f00", "\u1f80");
            addIotaSub.Add("\u1f01", "\u1f81");
            addIotaSub.Add("\u1f70", "\u1fb2");
            addIotaSub.Add("ά", "\u1fb4");
            addIotaSub.Add("\u1fb6", "\u1fb7");
            addIotaSub.Add("\u1f02", "\u1f82");
            addIotaSub.Add("\u1f03", "\u1f83");
            addIotaSub.Add("\u1f04", "\u1f84");
            //            addIotaSub.Add("\u1f05", "\u1f85");
            addIotaSub.Add("\u1f06", "\u1f86");
            addIotaSub.Add("\u1f07", "\u1f87");
            addIotaSub.Add("η", "\u1fc3");
            addIotaSub.Add("\u1f20", "\u1f90");
            addIotaSub.Add("\u1f21", "\u1f91");
            addIotaSub.Add("\u1f74", "\u1fc2");
            addIotaSub.Add("ή", "\u1fc4");
            addIotaSub.Add("\u1fc6", "\u1fc7");
            addIotaSub.Add("\u1f22", "\u1f92");
            addIotaSub.Add("\u1f23", "\u1f93");
            addIotaSub.Add("\u1f24", "\u1f94");
            //            addIotaSub.Add("\u1f25", "\u1f95");
            addIotaSub.Add("\u1f26", "\u1f96");
            addIotaSub.Add("\u1f27", "\u1f97");
            addIotaSub.Add("ω", "\u1ff3");
            addIotaSub.Add("\u1f60", "\u1fa0");
            addIotaSub.Add("\u1f61", "\u1fa1");
            addIotaSub.Add("\u1f7c", "\u1ff2");
            addIotaSub.Add("ώ", "\u1ff4");
            addIotaSub.Add("\u1ff6", "\u1ff7");
            addIotaSub.Add("\u1f62", "\u1fa2");
            addIotaSub.Add("\u1f63", "\u1fa3");
            addIotaSub.Add("\u1f64", "\u1fa4");
            //            addIotaSub.Add("\u1f65", "\u1fa5");
            addIotaSub.Add("\u1f66", "\u1fa6");
            addIotaSub.Add("\u1f67", "\u1fa7");
            addIotaSub.Add("Α", "\u1fbc");
            addIotaSub.Add("\u1f08", "\u1f88");
            addIotaSub.Add("\u1f09", "\u1f89");
            addIotaSub.Add("\u1f0a", "\u1f8a");
            addIotaSub.Add("\u1f0b", "\u1f8b");
            addIotaSub.Add("\u1f0c", "\u1f8c");
            //            addIotaSub.Add("\u1f0d", "\u1f8d");
            addIotaSub.Add("\u1f0e", "\u1f8e");
            addIotaSub.Add("\u1f0f", "\u1f8f");
            addIotaSub.Add("Η", "\u1fcc");
            addIotaSub.Add("\u1f28", "\u1f98");
            addIotaSub.Add("\u1f29", "\u1f99");
            addIotaSub.Add("\u1f2a", "\u1f9a");
            addIotaSub.Add("\u1f2b", "\u1f9b");
            addIotaSub.Add("\u1f2c", "\u1f9c");
            //            addIotaSub.Add("\u1f2d", "\u1f9d");
            addIotaSub.Add("\u1f2e", "\u1f9e");
            addIotaSub.Add("\u1f2f", "\u1f9f");
            addIotaSub.Add("Ω", "\u1ffc");
            addIotaSub.Add("\u1f68", "\u1fa8");
            addIotaSub.Add("\u1f69", "\u1fa9");
            addIotaSub.Add("\u1f6a", "\u1faa");
            addIotaSub.Add("\u1f6b", "\u1fab");
            addIotaSub.Add("\u1f6c", "\u1fac");
            //            addIotaSub.Add("\u1f6d", "\u1fad");
            addIotaSub.Add("\u1f6e", "\u1fae");
            addIotaSub.Add("\u1f6f", "\u1faf");
        }

        public String processRoughBreathing(String originalChar)
        {
            String replacementChar;

            if (addRoughBreathing.ContainsKey(originalChar))
            {
                addRoughBreathing.TryGetValue(originalChar, out replacementChar);
                return replacementChar;
            }
            else return originalChar;
        }

        public String processSmoothBreathing(String originalChar)
        {
            String replacementChar;

            if (addSmoothBreathing.ContainsKey(originalChar))
            {
                addSmoothBreathing.TryGetValue(originalChar, out replacementChar);
                return replacementChar;
            }
            else return originalChar;
        }

        public String processAccute(String originalChar)
        {
            String replacementChar;

            if (addAccute.ContainsKey(originalChar))
            {
                addAccute.TryGetValue(originalChar, out replacementChar);
                return replacementChar;
            }
            else return originalChar;
        }

        public String processGrave(String originalChar)
        {
            String replacementChar;

            if (addGrave.ContainsKey(originalChar))
            {
                addGrave.TryGetValue(originalChar, out replacementChar);
                return replacementChar;
            }
            else return originalChar;
        }

        public String processCirc(String originalChar)
        {
            String replacementChar;

            if (addCirc.ContainsKey(originalChar))
            {
                addCirc.TryGetValue(originalChar, out replacementChar);
                return replacementChar;
            }
            else return originalChar;
        }

        public String processDiaeresis(String originalChar)
        {
            String replacementChar;

            if (addDiaeresis.ContainsKey(originalChar))
            {
                addDiaeresis.TryGetValue(originalChar, out replacementChar);
                return replacementChar;
            }
            else return originalChar;
        }

        public String processIotaSub(String originalChar)
        {
            String replacementChar;

            if (addIotaSub.ContainsKey(originalChar))
            {
                addIotaSub.TryGetValue(originalChar, out replacementChar);
                return replacementChar;
            }
            else return originalChar;
        }
    }
}

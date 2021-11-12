using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing;

namespace GkHebKeyboard
{
    class classKeyboard
    {
        const int noOfRows = 5, noOfCols = 14, keyGap = 4;

        bool isHebrew = false, isCtrlSet = false, isMiniscule = true;
        int caseState = 0;
        String uniSeparator = "\u2063", keyDataFile;
        int[,] keyWidths, gkKeyCode, hebKeyCode;
        String[,] hebKeyFace, hebKeyHint, hebAccentHints, gkKeyFaceMin, gkKeyFaceMaj, gkKeyHintMin, gkKeyHintMaj, gkKeyVal, hebKeyVal;
        String[,] hebAccents = { { "Esc", "\u0591", "\u0592", "\u0593", "\u0594", "\u0595", "\u0596", "\u0597", "\u0598", "\u0599",
                                        "\u059A", "\u059B", "\u059C", "BkSp" },
                                     { "Clear", "\u059D", "\u059E", "\u059F", "\u05A0", "\u05A1", "\u05A2", "\u05A3", "\u05A4", "\u05A5",
                                        "\u05A6", "\u05A7", "\u05A8", "Del" },
                                     { "Caps", "\u05A9", "\u05AA", "\u05AB", "\u05AC", "\u05AD", "\u05AE", "\u05AF", "\u05BD", "\u05BF",
                                        "\u05C0", "\u05C5", "\u05C6", "Enter" },
                                     { "Shift", "\u05C7", "\u05F0", "\u05F1", "\u05F2", "\u05F3", "\u05F4", "", "", "", "", "", "Shift", "" } };
        classGlobal globalVars;
        TabPage gkKeyboard, hebKeyboard;
        TabControl tabCtlMain;
        Panel pnlGkKeyboard;
        RichTextBox rtxtHebOrGk;
        Button btnCapsLight;
        RadioButton rbtnHebrew, rbtnGreek;
        Button[,] gkKeys, hebKeys;
        ToolTip[,] gkToolTips = new ToolTip[noOfRows, noOfCols], hebToolTips = new ToolTip[noOfRows, noOfCols];
        List<String> nonPrintSet = new List<string>();
        classGreek greekProcessing;

        public classGlobal GlobalVars { get => globalVars; set => globalVars = value; }
        public classGreek GreekProcessing { get => greekProcessing; set => greekProcessing = value; }
        public Panel PnlGkKeyboard { get => pnlGkKeyboard; set => pnlGkKeyboard = value; }
        public TabControl TabCtlMain { get => tabCtlMain; set => tabCtlMain = value; }
        public TabPage GkKeyboard { get => gkKeyboard; set => gkKeyboard = value; }
        public TabPage HebKeyboard { get => hebKeyboard; set => hebKeyboard = value; }
        public RichTextBox RtxtHebOrGk { get => rtxtHebOrGk; set => rtxtHebOrGk = value; }
        public Button BtnCapsLight { get => btnCapsLight; set => btnCapsLight = value; }
        public RadioButton RbtnHebrew { get => rbtnHebrew; set => rbtnHebrew = value; }
        public RadioButton RbtnGreek { get => rbtnGreek; set => rbtnGreek = value; }

        public void initialiseKeyboard(classGreek inGreek, classGlobal inGlobal)
        {
            int keyRow, keyCol, maxForRow, keyWidth, accummulativeWidth, tagCount = 0, baseHeight, x, y;
            String[] keyContents = new String[4];
            Char[] splitChar = { uniSeparator[0] };

            greekProcessing = inGreek;
            globalVars = inGlobal;
            keyDataFile = globalVars.BasePath + @"\KeyData\";
            //            nonPrintKeys = loadNonPrintKeys(19);
            keyWidths = loadKeyWidths();
            hebKeyFace = loadFileData("hebKeyFace.txt");
            hebKeyHint = loadFileData("hebKeyHint.txt");
            hebAccentHints = loadFileData("hebAccentHints.txt");
            gkKeyFaceMin = loadFileData("gkKeyFaceMin.txt");
            gkKeyFaceMaj = loadFileData("gkKeyFaceMaj.txt");
            gkKeyHintMin = loadFileData("gkKeyHintMin.txt");
            gkKeyHintMaj = loadFileData("gkKeyHintMaj.txt");
            // The prime activity in setting up the app is to create and populate the keys of the "keyboard"
            /***************************************
             * 
             * gkKeys/hebKeys: a global array containing all references to each key
             */
            gkKeys = new Button[noOfRows, noOfCols];
            hebKeys = new Button[noOfRows, noOfCols];
            /***************************************
             * 
             * keyCode: a global array containing the physical key data for each key (if scanned)
             */
            gkKeyCode = new int[noOfRows, noOfCols];
            gkKeyVal = new String[noOfRows, noOfCols];
            hebKeyCode = new int[noOfRows, noOfCols];
            hebKeyVal = new String[noOfRows, noOfCols];
            /****************************************
             * 
             * Now actually create the two sets of keys
             */
            initialiseKeyCode(noOfRows, noOfCols);
            maxForRow = 0;
            baseHeight = 8;
            for (keyRow = 0; keyRow < noOfRows; keyRow++)
            {
                switch (keyRow)
                {
                    case 0:
                    case 1:
                    case 2: maxForRow = noOfCols; break;
                    case 3: maxForRow = 13; break;
                    case 4: maxForRow = 8; break;
                }
                accummulativeWidth = 16;
                for (keyCol = 0; keyCol < maxForRow; keyCol++)
                {
                    keyWidth = keyWidths[keyRow, keyCol];
                    gkKeys[keyRow, keyCol] = new Button();
                    hebKeys[keyRow, keyCol] = new Button();
                    switch (keyWidth)
                    {
                        case 47:
                            {
                                keyAllocation(keyRow, keyCol, globalVars.BasePath + @"\blank47.png");
                                keyWidth = 48;
                            }
                            break;
                        case 48: keyAllocation(keyRow, keyCol, globalVars.BasePath + @"\blank48.png"); break;
                        case 64: keyAllocation(keyRow, keyCol, globalVars.BasePath + @"\blank64w.png"); break;
                        case 96: keyAllocation(keyRow, keyCol, globalVars.BasePath + @"\blank96w.png"); break;
                        case 310: keyAllocation(keyRow, keyCol, globalVars.BasePath + @"\space310w.png"); break;
                    }
                    gkKeys[keyRow, keyCol].Text = gkKeyFaceMin[keyRow, keyCol];
                    hebKeys[keyRow, keyCol].Text = hebKeyFace[keyRow, keyCol];
                    gkKeys[keyRow, keyCol].TextAlign = ContentAlignment.MiddleCenter;
                    hebKeys[keyRow, keyCol].TextAlign = ContentAlignment.MiddleCenter;
                    gkKeys[keyRow, keyCol].Left = accummulativeWidth;
                    hebKeys[keyRow, keyCol].Left = accummulativeWidth;
                    gkKeys[keyRow, keyCol].Top = baseHeight + (keyRow * (48 + keyGap));
                    hebKeys[keyRow, keyCol].Top = baseHeight + (keyRow * (48 + keyGap));
                    gkKeys[keyRow, keyCol].Height = 48;
                    hebKeys[keyRow, keyCol].Height = 48;
                    gkKeys[keyRow, keyCol].Width = keyWidth;
                    hebKeys[keyRow, keyCol].Width = keyWidth;
                    gkKeys[keyRow, keyCol].Font = new Font("Times New Roman", 14);
                    hebKeys[keyRow, keyCol].Font = new Font("Times New Roman", 14);
                    gkKeys[keyRow, keyCol].Tag = ++tagCount;
                    hebKeys[keyRow, keyCol].Tag = tagCount;
                    gkKeys[keyRow, keyCol].Click += gkKeyboard_button_click;
                    hebKeys[keyRow, keyCol].Click += hebKeyboard_button_click;
                    gkKeyboard.Controls.Add(gkKeys[keyRow, keyCol]);
                    hebKeyboard.Controls.Add(hebKeys[keyRow, keyCol]);

                    gkToolTips[keyRow, keyCol] = new ToolTip();
                    gkToolTips[keyRow, keyCol].AutomaticDelay = 200;
                    gkToolTips[keyRow, keyCol].AutoPopDelay = 2147483647;
                    gkToolTips[keyRow, keyCol].ToolTipTitle = "Key value";
                    gkToolTips[keyRow, keyCol].SetToolTip(gkKeys[keyRow, keyCol], gkKeyHintMin[keyRow, keyCol]);

                    hebToolTips[keyRow, keyCol] = new ToolTip();
                    hebToolTips[keyRow, keyCol].AutomaticDelay = 200;
                    hebToolTips[keyRow, keyCol].AutoPopDelay = 2147483647;
                    hebToolTips[keyRow, keyCol].ToolTipTitle = "Key value";
                    hebToolTips[keyRow, keyCol].SetToolTip(hebKeys[keyRow, keyCol], hebKeyHint[keyRow, keyCol]);

                    accummulativeWidth += keyWidth + keyGap;
                }
            }
        }

        private void keyAllocation(int row, int col, String fileName)
        {
            gkKeys[row, col].Image = Image.FromFile(fileName);
            hebKeys[row, col].Image = Image.FromFile(fileName);
        }

        private int[,] loadKeyWidths()
        {
            /*****************************************************
             *                                                   *
             *                   loadKeyWidths                   *
             *                   =============                   *
             *                                                   *
             *  Loads the widths of each key image from file.    *
             *                                                   *
             *****************************************************/
            int idx, jdx;
            String fileBuffer;
            int[,] targetArray;
            StreamReader srSource;

            srSource = new StreamReader(keyDataFile + @"keyWidths.txt");
            targetArray = new int[noOfRows, noOfCols];
            fileBuffer = srSource.ReadLine();
            idx = 0; jdx = 0;
            while (fileBuffer != null)
            {
                targetArray[idx, jdx] = Convert.ToInt32(fileBuffer);
                fileBuffer = srSource.ReadLine();
                if (++jdx == noOfCols)
                {
                    jdx = 0;
                    if (++idx == noOfRows) break;
                }
            }
            srSource.Close();
            return targetArray;
        }

        private String[,] loadFileData(String fileName)
        {
            /*****************************************************
             *                                                   *
             *                   loadFileData                    *
             *                   ============                    *
             *                                                   *
             *  Parameter:                                       *
             *    fileName:  Allows various sets of data from a  *
             *               single method.                      *
             *                                                   *
             *  Purpose:                                         *
             *    A flexible means of loading:                   *
             *    a) Gk and Heb characters;                      *
             *    b) Information for key hints                   *
             *    c) Heb accents                                 *
             *                                                   *
             *****************************************************/
            int idx, jdx;
            String fileBuffer;
            String[,] targetArray;
            StreamReader srSource;

            srSource = new StreamReader(keyDataFile + fileName);
            targetArray = new String[noOfRows, noOfCols];
            fileBuffer = srSource.ReadLine();
            idx = 0; jdx = 0;
            while (fileBuffer != null)
            {
                if ((fileBuffer.Length > 0) && (fileBuffer[0] == '\\'))
                {
                    fileBuffer = String.Format("{0:C}", (char)int.Parse(fileBuffer.Substring(2), System.Globalization.NumberStyles.HexNumber));
                }
                targetArray[idx, jdx] = fileBuffer;
                if (++jdx == noOfCols)
                {
                    jdx = 0;
                    if (++idx == noOfRows) break;
                }
                fileBuffer = srSource.ReadLine();
            }
            srSource.Close();
            return targetArray;
        }

        private void initialiseKeyCode(int x, int y)
        {
            int idx, jdx;

            for (idx = 0; idx < x; idx++)
            {
                for (jdx = 0; jdx < y; jdx++)
                {
                    gkKeyCode[idx, jdx] = -1;
                    gkKeyVal[idx, jdx] = "";
                    hebKeyCode[idx, jdx] = -1;
                    hebKeyVal[idx, jdx] = "";
                }
            }
        }

        private void gkKeyboard_button_click(object sender, EventArgs e)
        {
            /**********************************************************************
             *                                                                    *
             *                     gkKeyboard_button_click                        *
             *                     =======================                        *
             *                                                                    *
             *  Specifically handles key presses from the Greek virtual keyboard. *
             *    (see hebKeyboard_button_click for responses to the Hebrew       *
             *    keyboard).                                                      *
             *                                                                    *
             **********************************************************************/
            bool isHeb = false;
            int clickedTag, textPosition;
            String keyVal = "", leftOfWord, rightOfWord;
            Button clickedButton;

            rtxtHebOrGk.Focus();
            clickedButton = (Button)sender;
            clickedTag = Convert.ToInt32(clickedButton.Tag.ToString());
            if (isMiniscule)
            {
                switch (clickedTag)
                {
                    case 2: keyVal = "1"; break;
                    case 3: keyVal = "2"; break;
                    case 4: keyVal = "3"; break;
                    case 5: keyVal = "4"; break;
                    case 6: keyVal = "5"; break;
                    case 7: keyVal = "6"; break;
                    case 8: keyVal = "7"; break;
                    case 9: keyVal = "8"; break;
                    case 10: keyVal = "9"; break;
                    case 11: keyVal = "0"; break;
                    case 12: keyVal = ""; break;
                    case 13: keyVal = ""; break;
                    case 14: handleBackspace(); break;
                    case 15: rtxtHebOrGk.Text = ""; break;
                    case 16: handleGkExtra(globalVars.GreekProcessing.AddAccute); break;
                    case 17: keyVal = "ς"; break;
                    case 18: keyVal = "ε"; break;
                    case 19: keyVal = "ρ"; break;
                    case 20: keyVal = "τ"; break;
                    case 21: keyVal = "υ"; break;
                    case 22: keyVal = "θ"; break;
                    case 23: keyVal = "ι"; break;
                    case 24: keyVal = "ο"; break;
                    case 25: keyVal = "π"; break;
                    case 26: handleGkExtra(globalVars.GreekProcessing.AddCirc); break;
                    case 27: handleGkExtra(globalVars.GreekProcessing.AddGrave); break;
                    case 28: handleDel(); break;

                    case 29: handleCapsPress(); break;
                    case 30: keyVal = "α"; break;
                    case 31: keyVal = "σ"; break;
                    case 32: keyVal = "δ"; break;
                    case 33: keyVal = "φ"; break;
                    case 34: keyVal = "γ"; break;
                    case 35: keyVal = "η"; break;
                    case 36: keyVal = "ξ"; break;
                    case 37: keyVal = "κ"; break;
                    case 38: keyVal = "λ"; break;
                    case 39: handleGkExtra(globalVars.GreekProcessing.AddRoughBreathing); break;
                    case 40: handleGkExtra(globalVars.GreekProcessing.AddSmoothBreathing); break;
                    case 41: handleGkExtra(globalVars.GreekProcessing.AddIotaSub); break;
                    case 42: keyVal = "\n"; break;

                    case 43: handleShiftPress(1); break;
                    case 44: keyVal = "ζ"; break;
                    case 45: keyVal = "χ"; break;
                    case 46: keyVal = "ψ"; break;
                    case 47: keyVal = "ω"; break;
                    case 48: keyVal = "β"; break;
                    case 49: keyVal = "ν"; break;
                    case 50: keyVal = "μ"; break;
                    case 51: keyVal = "·"; break;
                    case 52: keyVal = ";"; break;
                    case 53: handleGkExtra(globalVars.GreekProcessing.AddDiaeresis); break;
                    case 55: handleShiftPress(1); break;

                    case 56: handleCtrlPress(); break;

                    case 59: keyVal = " "; break;
                    case 62: tabCtlMain.SelectedIndex = 1; break;
                    default: break;
                }
                if ((clickedTag >= 17) && (clickedTag <= 25)) isHeb = true;
                if ((clickedTag >= 30) && (clickedTag <= 38)) isHeb = true;
                if ((clickedTag > 43) && (clickedTag <= 53)) isHeb = true;
                if (clickedTag == 42) isHeb = true;
                if (clickedTag == 59) isHeb = true;
                if (isHeb)
                {
                    if ((rtxtHebOrGk.Text == null) || (rtxtHebOrGk.Text.Length == 0))
                    {
                        rtxtHebOrGk.Text = keyVal;
                        rtxtHebOrGk.SelectionStart = 1;
                    }
                    else
                    {
                        textPosition = rtxtHebOrGk.SelectionStart;
                        if (textPosition == 0)
                        {
                            rtxtHebOrGk.Text = keyVal + rtxtHebOrGk.Text;
                            rtxtHebOrGk.SelectionStart = 0;
                        }
                        else
                        {
                            if (textPosition == rtxtHebOrGk.Text.Length)
                            {
                                rtxtHebOrGk.Text += keyVal;
                                rtxtHebOrGk.SelectionStart = rtxtHebOrGk.Text.Length - 1;
                            }
                            else
                            {
                                leftOfWord = rtxtHebOrGk.Text.Substring(0, textPosition);
                                rightOfWord = rtxtHebOrGk.Text.Substring(textPosition);
                                rtxtHebOrGk.Text = leftOfWord + keyVal + rightOfWord;
                                rtxtHebOrGk.SelectionStart = leftOfWord.Length;
                            }
                        }
                    }
                    rtxtHebOrGk.SelectionStart = rtxtHebOrGk.SelectionStart + 1;
                }
            }
            else
            {
                switch (clickedTag)
                {
                    case 2: keyVal = "1"; break;
                    case 3: keyVal = "2"; break;
                    case 4: keyVal = "3"; break;
                    case 5: keyVal = "4"; break;
                    case 6: keyVal = "5"; break;
                    case 7: keyVal = "6"; break;
                    case 8: keyVal = "7"; break;
                    case 9: keyVal = "8"; break;
                    case 10: keyVal = "9"; break;
                    case 11: keyVal = "0"; break;
                    case 12: keyVal = ""; break;
                    case 13: keyVal = ""; break;
                    case 14: handleBackspace(); break;
                    case 15: rtxtHebOrGk.Text = ""; break;
                    case 16: handleGkExtra(globalVars.GreekProcessing.AddAccute); break;
                    case 18: keyVal = "Ε"; break;
                    case 19: keyVal = "Ρ"; break;
                    case 20: keyVal = "Τ"; break;
                    case 21: keyVal = "Υ"; break;
                    case 22: keyVal = "Θ"; break;
                    case 23: keyVal = "Ι"; break;
                    case 24: keyVal = "Ο"; break;
                    case 25: keyVal = "Π"; break;
                    case 26: handleGkExtra(globalVars.GreekProcessing.AddCirc); break;
                    case 27: handleGkExtra(globalVars.GreekProcessing.AddGrave); break;
                    case 28: handleDel(); break;

                    case 29: handleCapsPress(); break;
                    case 30: keyVal = "Α"; break;
                    case 31: keyVal = "Σ"; break;
                    case 32: keyVal = "Δ"; break;
                    case 33: keyVal = "Φ"; break;
                    case 34: keyVal = "Γ"; break;
                    case 35: keyVal = "Η"; break;
                    case 36: keyVal = "Ξ"; break;
                    case 37: keyVal = "Κ"; break;
                    case 38: keyVal = "Λ"; break;
                    case 39: handleGkExtra(globalVars.GreekProcessing.AddRoughBreathing); break;
                    case 40: handleGkExtra(globalVars.GreekProcessing.AddSmoothBreathing); break;
                    case 41: handleGkExtra(globalVars.GreekProcessing.AddIotaSub); break;
                    case 42: keyVal = "\n"; break;

                    case 43: handleShiftPress(1); break;
                    case 44: keyVal = "Ζ"; break;
                    case 45: keyVal = "Χ"; break;
                    case 46: keyVal = "Ψ"; break;
                    case 47: keyVal = "Ω"; break;
                    case 48: keyVal = "Β"; break;
                    case 49: keyVal = "Ν"; break;
                    case 50: keyVal = "Μ"; break;
                    case 51: keyVal = ","; break;
                    case 52: keyVal = "."; break;
                    case 53: handleGkExtra(globalVars.GreekProcessing.AddDiaeresis); break;
                    case 55: handleShiftPress(1); break;
                    case 56: handleCtrlPress(); break;

                    case 59: keyVal = " "; break;
                    case 62: tabCtlMain.SelectedIndex = 1; break;
                    default: break;
                }
                if ((clickedTag >= 18) && (clickedTag <= 25)) isHeb = true;
                if ((clickedTag >= 30) && (clickedTag <= 38)) isHeb = true;
                if ((clickedTag >= 43) && (clickedTag <= 53)) isHeb = true;
                if (clickedTag == 42) isHeb = true;
                if (clickedTag == 59) isHeb = true;
                if (isHeb)
                {
                    if ((rtxtHebOrGk.Text == null) || (rtxtHebOrGk.Text.Length == 0))
                    {
                        rtxtHebOrGk.Text = keyVal;
                        rtxtHebOrGk.SelectionStart = 1;
                    }
                    else
                    {
                        textPosition = rtxtHebOrGk.SelectionStart;
                        if (textPosition == 0)
                        {
                            rtxtHebOrGk.Text = keyVal + rtxtHebOrGk.Text;
                            rtxtHebOrGk.SelectionStart = 0;
                        }
                        else
                        {
                            if (textPosition == rtxtHebOrGk.Text.Length)
                            {
                                rtxtHebOrGk.Text += keyVal;
                                rtxtHebOrGk.SelectionStart = rtxtHebOrGk.Text.Length - 1;
                            }
                            else
                            {
                                leftOfWord = rtxtHebOrGk.Text.Substring(0, textPosition);
                                rightOfWord = rtxtHebOrGk.Text.Substring(textPosition);
                                rtxtHebOrGk.Text = leftOfWord + keyVal + rightOfWord;
                                rtxtHebOrGk.SelectionStart = leftOfWord.Length;
                            }
                        }
                    }
                    rtxtHebOrGk.SelectionStart = rtxtHebOrGk.SelectionStart + 1;
                    if (caseState == 1) handleShiftPress(1);
                }
            }
            rtxtHebOrGk.Focus();
        }

        private void hebKeyboard_button_click(object sender, EventArgs e)
        {
            bool isHeb = false;
            int clickedTag, textPosition;
            String keyVal = "", leftOfWord, rightOfWord;
            Button clickedButton;

            rtxtHebOrGk.Focus();
            clickedButton = (Button)sender;
            clickedTag = Convert.ToInt32(clickedButton.Tag.ToString());
            if (isMiniscule)
            {
                switch (clickedTag)
                {
                    case 1: break;
                    case 2: keyVal = "\u05B1"; break;
                    case 3: keyVal = "\u05B3"; break;
                    case 4: keyVal = "\u05B2"; break;
                    case 5: keyVal = "\u05B6"; break;
                    case 6: keyVal = "\u05BC"; break;
                    case 7: keyVal = "\u05B5"; break;
                    case 8: keyVal = "\u05B4"; break;
                    case 9: keyVal = "\u05B0"; break;
                    case 10: keyVal = "\u05B8"; break;
                    case 11: keyVal = "\u05B7"; break;
                    case 12: keyVal = "\u05BB"; break;
                    case 13: keyVal = "\u05B9"; break;
                    case 14: handleBackspace(); break;
                    case 15: rtxtHebOrGk.Text = ""; break;
                    case 16: keyVal = "\u05C1"; break;
                    case 17: keyVal = "\u05C2"; break;
                    case 18: keyVal = "\u05E7"; break;
                    case 19: keyVal = "\u05E8"; break;
                    case 20: keyVal = "\u05D0"; break;
                    case 21: keyVal = "\u05D8"; break;
                    case 22: keyVal = "\u05D5"; break;
                    case 23: keyVal = "\u05DF"; break;
                    case 24: keyVal = "\u05DD"; break;
                    case 25: keyVal = "\u05E4"; break;
                    case 26: keyVal = "\u05BE"; break;
                    case 27: keyVal = "\u05ab"; break;
                    case 28: handleDel(); break;

                    case 30: keyVal = "\u05E9"; break;
                    case 31: keyVal = "\u05D3"; break;
                    case 32: keyVal = "\u05D2"; break;
                    case 33: keyVal = "\u05DB"; break;
                    case 34: keyVal = "\u05E2"; break;
                    case 35: keyVal = "\u05D9"; break;
                    case 36: keyVal = "\u05D7"; break;
                    case 37: keyVal = "\u05DC"; break;
                    case 38: keyVal = "\u05DA"; break;
                    case 39: keyVal = "\u05E3"; break;
                    case 40: keyVal = "\u05C3"; break;
                    case 42: keyVal = "\n"; break;
                    case 43: handleShiftPress(2); break; // New

                    case 45: keyVal = "\u05D6"; break;
                    case 46: keyVal = "\u05E1"; break;
                    case 47: keyVal = "\u05D1"; break;
                    case 48: keyVal = "\u05D4"; break;
                    case 49: keyVal = "\u05E0"; break;
                    case 50: keyVal = "\u05DE"; break;
                    case 51: keyVal = "\u05E6"; break;
                    case 52: keyVal = "\u05EA"; break;
                    case 53: keyVal = "\u05E5"; break;

                    case 56: handleCtrlPress(); break;
                    case 59: keyVal = " "; break;
                    case 62: tabCtlMain.SelectedIndex = 0; break;
                    default: break;
                }
                if ((clickedTag > 1) && (clickedTag <= 13)) isHeb = true;
                if ((clickedTag >= 16) && (clickedTag <= 27)) isHeb = true;
                if ((clickedTag >= 30) && (clickedTag <= 40)) isHeb = true;
                if ((clickedTag >= 45) && (clickedTag <= 53)) isHeb = true;
                if (clickedTag == 42) isHeb = true;
                if (clickedTag == 59) isHeb = true;
                if (isHeb)
                {
                    if ((rtxtHebOrGk.Text == null) || (rtxtHebOrGk.Text.Length == 0))
                    {
                        rtxtHebOrGk.Text = keyVal;
                        rtxtHebOrGk.SelectionStart = 1;
                    }
                    else
                    {
                        textPosition = rtxtHebOrGk.SelectionStart;
                        if (textPosition == 0)
                        {
                            rtxtHebOrGk.Text = keyVal + rtxtHebOrGk.Text;
                            rtxtHebOrGk.SelectionStart = 0;
                        }
                        else
                        {
                            if (textPosition == rtxtHebOrGk.Text.Length)
                            {
                                rtxtHebOrGk.Text += keyVal;
                                rtxtHebOrGk.SelectionStart = rtxtHebOrGk.Text.Length - 1;
                            }
                            else
                            {
                                leftOfWord = rtxtHebOrGk.Text.Substring(0, textPosition);
                                rightOfWord = rtxtHebOrGk.Text.Substring(textPosition);
                                rtxtHebOrGk.Text = leftOfWord + keyVal + rightOfWord;
                                rtxtHebOrGk.SelectionStart = leftOfWord.Length;
                            }
                        }
                    }
                    rtxtHebOrGk.SelectionStart = rtxtHebOrGk.SelectionStart + 1;
                }
            }
            else
            {
                switch (clickedTag)
                {
                    case 1: break;
                    case 2: keyVal = hebAccents[0, 1]; break;
                    case 3: keyVal = hebAccents[0, 2]; break;
                    case 4: keyVal = hebAccents[0, 3]; break;
                    case 5: keyVal = hebAccents[0, 4]; break;
                    case 6: keyVal = hebAccents[0, 5]; break;
                    case 7: keyVal = hebAccents[0, 6]; break;
                    case 8: keyVal = hebAccents[0, 7]; break;
                    case 9: keyVal = hebAccents[0, 8]; break;
                    case 10: keyVal = hebAccents[0, 9]; break;
                    case 11: keyVal = hebAccents[0, 10]; break;
                    case 12: keyVal = hebAccents[0, 11]; break;
                    case 13: keyVal = hebAccents[0, 12]; break;
                    case 14: handleBackspace(); break;
                    case 15: rtxtHebOrGk.Text = ""; break;
                    case 16: keyVal = hebAccents[1, 1]; break;
                    case 17: keyVal = hebAccents[1, 2]; break;
                    case 18: keyVal = hebAccents[1, 3]; break;
                    case 19: keyVal = hebAccents[1, 4]; break;
                    case 20: keyVal = hebAccents[1, 5]; break;
                    case 21: keyVal = hebAccents[1, 6]; break;
                    case 22: keyVal = hebAccents[1, 7]; break;
                    case 23: keyVal = hebAccents[1, 8]; break;
                    case 24: keyVal = hebAccents[1, 9]; break;
                    case 25: keyVal = hebAccents[1, 10]; break;
                    case 26: keyVal = hebAccents[1, 11]; break;
                    case 27: keyVal = hebAccents[1, 12]; break;
                    case 28: handleDel(); break;

                    case 30: keyVal = hebAccents[2, 1]; break;
                    case 31: keyVal = hebAccents[2, 2]; break;
                    case 32: keyVal = hebAccents[2, 3]; break;
                    case 33: keyVal = hebAccents[2, 4]; break;
                    case 34: keyVal = hebAccents[2, 5]; break;
                    case 35: keyVal = hebAccents[2, 6]; break;
                    case 36: keyVal = hebAccents[2, 7]; break;
                    case 37: keyVal = hebAccents[2, 8]; break;
                    case 38: keyVal = hebAccents[2, 9]; break;
                    case 39: keyVal = hebAccents[2, 10]; break;
                    case 40: keyVal = hebAccents[2, 11]; break;
                    case 41: keyVal = hebAccents[2, 12]; break;
                    case 42: keyVal = "\n"; break;

                    case 43: handleShiftPress(2); break; // New
                    case 44: keyVal = hebAccents[3, 1]; break;
                    case 45: keyVal = hebAccents[3, 2]; break;
                    case 46: keyVal = hebAccents[3, 3]; break;
                    case 47: keyVal = hebAccents[3, 4]; break;
                    case 48: keyVal = hebAccents[3, 5]; break;
                    case 49: keyVal = hebAccents[3, 6]; break;
                    //case 50: keyVal = hebAccents[3, 1]; break;
                    //case 51: keyVal = "\u05E6"; break;
                    //case 52: keyVal = "\u05EA"; break;
                    //case 53: keyVal = "\u05E5"; break;
                    case 55: handleShiftPress(2); break; // New

                    case 56: handleCtrlPress(); break;
                    case 59: keyVal = " "; break;
                    case 62: tabCtlMain.SelectedIndex = 0; break;
                    default: break;
                }
                if ((clickedTag > 1) && (clickedTag <= 13)) isHeb = true;
                if ((clickedTag >= 16) && (clickedTag <= 27)) isHeb = true;
                if ((clickedTag >= 30) && (clickedTag <= 41)) isHeb = true;
                if ((clickedTag >= 44) && (clickedTag <= 49)) isHeb = true;
                if (clickedTag == 42) isHeb = true;
                if (clickedTag == 59) isHeb = true;
                if (isHeb)
                {
                    if ((rtxtHebOrGk.Text == null) || (rtxtHebOrGk.Text.Length == 0))
                    {
                        rtxtHebOrGk.Text = keyVal;
                        rtxtHebOrGk.SelectionStart = 1;
                    }
                    else
                    {
                        textPosition = rtxtHebOrGk.SelectionStart;
                        if (textPosition == 0)
                        {
                            rtxtHebOrGk.Text = keyVal + rtxtHebOrGk.Text;
                            rtxtHebOrGk.SelectionStart = 0;
                        }
                        else
                        {
                            if (textPosition == rtxtHebOrGk.Text.Length)
                            {
                                rtxtHebOrGk.Text += keyVal;
                                rtxtHebOrGk.SelectionStart = rtxtHebOrGk.Text.Length - 1;
                            }
                            else
                            {
                                leftOfWord = rtxtHebOrGk.Text.Substring(0, textPosition);
                                rightOfWord = rtxtHebOrGk.Text.Substring(textPosition);
                                rtxtHebOrGk.Text = leftOfWord + keyVal + rightOfWord;
                                rtxtHebOrGk.SelectionStart = leftOfWord.Length;
                            }
                        }
                    }
                    rtxtHebOrGk.SelectionStart = rtxtHebOrGk.SelectionStart + 1;
                    if (caseState == 1) handleShiftPress(2);
                }
                rtxtHebOrGk.Focus();
            }
        }

        private void handleBackspace()
        {
            int csrPstn;
            Char hebChar;
            String leftPart, rightPart;

            csrPstn = rtxtHebOrGk.SelectionStart;
            if (csrPstn == 0) return;
            if (isCtrlSet)
            {
                if (isHebrew)
                {
                    hebChar = rtxtHebOrGk.Text[csrPstn - 1];
                    if (csrPstn == rtxtHebOrGk.Text.Length)
                    {
                        rtxtHebOrGk.Text = rtxtHebOrGk.Text.Substring(0, rtxtHebOrGk.Text.Length - 1);
                    }
                    else
                    {
                        leftPart = rtxtHebOrGk.Text.Substring(0, csrPstn - 1);
                        rightPart = rtxtHebOrGk.Text.Substring(csrPstn);
                        rtxtHebOrGk.Text = leftPart + rightPart;
                    }
                    csrPstn--;
                    rtxtHebOrGk.SelectionStart = csrPstn;
                }
                else
                {
                    if (csrPstn == rtxtHebOrGk.Text.Length)
                    {
                        rtxtHebOrGk.Text = rtxtHebOrGk.Text.Substring(0, rtxtHebOrGk.Text.Length - 1);
                    }
                    else
                    {
                        leftPart = rtxtHebOrGk.Text.Substring(0, csrPstn - 1);
                        rightPart = rtxtHebOrGk.Text.Substring(csrPstn);
                        rtxtHebOrGk.Text = leftPart + rightPart;
                    }
                    rtxtHebOrGk.SelectionStart = csrPstn - 1;
                }
            }
            else
            {
                if (isHebrew)
                {
                    do
                    {
                        hebChar = rtxtHebOrGk.Text[csrPstn - 1];
                        if (csrPstn == rtxtHebOrGk.Text.Length)
                        {
                            rtxtHebOrGk.Text = rtxtHebOrGk.Text.Substring(0, rtxtHebOrGk.Text.Length - 1);
                        }
                        else
                        {
                            leftPart = rtxtHebOrGk.Text.Substring(0, csrPstn - 1);
                            rightPart = rtxtHebOrGk.Text.Substring(csrPstn);
                            rtxtHebOrGk.Text = leftPart + rightPart;
                        }
                        csrPstn--;

                    } while ((csrPstn > 0) && ((hebChar < (char)0x05D0) || (hebChar > (char)0x05EA)));
                    rtxtHebOrGk.SelectionStart = csrPstn;
                }
                else
                {
                    if (csrPstn == rtxtHebOrGk.Text.Length)
                    {
                        rtxtHebOrGk.Text = rtxtHebOrGk.Text.Substring(0, rtxtHebOrGk.Text.Length - 1);
                    }
                    else
                    {
                        leftPart = rtxtHebOrGk.Text.Substring(0, csrPstn - 1);
                        rightPart = rtxtHebOrGk.Text.Substring(csrPstn);
                        rtxtHebOrGk.Text = leftPart + rightPart;
                    }
                    rtxtHebOrGk.SelectionStart = csrPstn - 1;
                }
            }
        }

        private void handleDel()
        {
            int csrPstn;
            Char hebChar;
            String leftPart, rightPart;

            csrPstn = rtxtHebOrGk.SelectionStart;
            if (csrPstn == rtxtHebOrGk.Text.Length) return;
            if (isHebrew)
            {
                do
                {
                    hebChar = rtxtHebOrGk.Text[csrPstn];
                    if (csrPstn == 0)
                    {
                        rtxtHebOrGk.Text = rtxtHebOrGk.Text.Substring(1, rtxtHebOrGk.Text.Length - 1);
                    }
                    else
                    {
                        leftPart = rtxtHebOrGk.Text.Substring(0, csrPstn);
                        rightPart = rtxtHebOrGk.Text.Substring(csrPstn + 1);
                        rtxtHebOrGk.Text = leftPart + rightPart;
                    }

                } while ((csrPstn > 0) && ((hebChar < (char)0x05D0) || (hebChar > (char)0x05EA)));
                rtxtHebOrGk.SelectionStart = csrPstn;
            }
            else
            {
                if (csrPstn == 0)
                {
                    rtxtHebOrGk.Text = rtxtHebOrGk.Text.Substring(1, rtxtHebOrGk.Text.Length - 1);
                }
                else
                {
                    leftPart = rtxtHebOrGk.Text.Substring(0, csrPstn);
                    rightPart = rtxtHebOrGk.Text.Substring(csrPstn + 1);
                    rtxtHebOrGk.Text = leftPart + rightPart;
                }
                rtxtHebOrGk.SelectionStart = csrPstn;
            }
        }

        private void handleCtrlPress()
        {
        }

        private void handleCapsPress()
        {
            if ((caseState == 0) || (caseState == 1))
            {
                isMiniscule = false;
                caseState = 2;
                btnCapsLight.Image = Image.FromFile(@"..\Resources\lightOn.png");
            }
            else
            {
                isMiniscule = true;
                caseState = 0;
                btnCapsLight.Image = Image.FromFile(@"..\Resources\lightOff.png");
            }
            changeKeys(tabCtlMain.SelectedIndex == 1, isMiniscule);
        }

        private void handleShiftPress(int languageCode)
        {
            if (caseState == 2) return;
            if (caseState == 0)
            {
                isMiniscule = false;
                caseState = 1;
            }
            else
            {
                isMiniscule = true;
                caseState = 0;
            }
            changeKeys(tabCtlMain.SelectedIndex == 1, isMiniscule);
        }

        public void changeKeys(bool isHebDisp, bool isMiniscule)
        {
            int keyRow, keyCol, maxForRow;

            if (isHebDisp)
            {
                if (isMiniscule)
                {
                    maxForRow = 0;
                    for (keyRow = 0; keyRow < noOfRows; keyRow++)
                    {
                        switch (keyRow)
                        {
                            case 0:
                            case 1:
                            case 2: maxForRow = noOfCols; break;
                            case 3: maxForRow = 13; break;
                            case 4: maxForRow = 8; break;
                        }
                        for (keyCol = 0; keyCol < maxForRow; keyCol++)
                        {
                            hebKeys[keyRow, keyCol].Text = hebKeyFace[keyRow, keyCol];
                            hebKeys[keyRow, keyCol].Font = new Font("Times New Roman", 14);
                            hebToolTips[keyRow, keyCol].SetToolTip(hebKeys[keyRow, keyCol], hebKeyHint[keyRow, keyCol]);
                            hebKeys[keyRow, keyCol].Refresh();
                        }
                    }
                }
                else
                {
                    int[,] accentControl = { { 0,1,1,1,1,1,1,1,1,1,1,1,1,0}, { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
                                             { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 0, 0 }, { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } };
                    maxForRow = 0;
                    for (keyRow = 0; keyRow < noOfRows - 1; keyRow++)
                    {
                        switch (keyRow)
                        {
                            case 0:
                            case 1:
                            case 2: maxForRow = noOfCols; break;
                            case 3: maxForRow = 13; break;
                                //case 4: maxForRow = 8; break;
                        }
                        for (keyCol = 0; keyCol < maxForRow; keyCol++)
                        {
                            if (accentControl[keyRow, keyCol] == 1) hebKeys[keyRow, keyCol].Text = "\u25CC" + hebAccents[keyRow, keyCol];
                            else hebKeys[keyRow, keyCol].Text = hebAccents[keyRow, keyCol];
                            hebKeys[keyRow, keyCol].Font = new Font("Times New Roman", 14);
                            hebToolTips[keyRow, keyCol].SetToolTip(hebKeys[keyRow, keyCol], hebAccentHints[keyRow, keyCol]);
                            hebKeys[keyRow, keyCol].Refresh();
                        }
                    }
                }
                hebKeyboard.Refresh();
            }
            else
            {
                if (isMiniscule)
                {
                    maxForRow = 0;
                    for (keyRow = 0; keyRow < noOfRows; keyRow++)
                    {
                        switch (keyRow)
                        {
                            case 0:
                            case 1:
                            case 2: maxForRow = noOfCols; break;
                            case 3: maxForRow = 13; break;
                            case 4: maxForRow = 8; break;
                        }
                        for (keyCol = 0; keyCol < maxForRow; keyCol++)
                        {
                            gkKeys[keyRow, keyCol].Text = gkKeyFaceMin[keyRow, keyCol];
                            gkKeys[keyRow, keyCol].Font = new Font("Times New Roman", 14);
                            gkToolTips[keyRow, keyCol].SetToolTip(gkKeys[keyRow, keyCol], gkKeyHintMin[keyRow, keyCol]);
                            gkKeys[keyRow, keyCol].Refresh();
                        }
                    }
                }
                else
                {
                    maxForRow = 0;
                    for (keyRow = 0; keyRow < noOfRows; keyRow++)
                    {
                        switch (keyRow)
                        {
                            case 0:
                            case 1:
                            case 2: maxForRow = noOfCols; break;
                            case 3: maxForRow = 13; break;
                            case 4: maxForRow = 8; break;
                        }
                        for (keyCol = 0; keyCol < maxForRow; keyCol++)
                        {
                            gkKeys[keyRow, keyCol].Text = gkKeyFaceMaj[keyRow, keyCol];
                            gkKeys[keyRow, keyCol].Font = new Font("Times New Roman", 14);
                            gkToolTips[keyRow, keyCol].SetToolTip(gkKeys[keyRow, keyCol], gkKeyHintMaj[keyRow, keyCol]);
                            gkKeys[keyRow, keyCol].Refresh();
                        }
                    }
                }
                gkKeyboard.Refresh();
            }
        }

        private void handleGkExtra(SortedDictionary<String, String> conversionTable)
        {
            // oxia (accute)
            int csrPstn;
            String currentLetter, replacementLetter;

            csrPstn = rtxtHebOrGk.SelectionStart - 1;
            if (csrPstn < 0) return;
            currentLetter = rtxtHebOrGk.Text.Substring(csrPstn, 1);
            if (conversionTable.ContainsKey(currentLetter))
            {
                conversionTable.TryGetValue(currentLetter, out replacementLetter);
                if (csrPstn == 0)
                {
                    rtxtHebOrGk.Text = replacementLetter + rtxtHebOrGk.Text.Substring(1, rtxtHebOrGk.Text.Length - 1);
                }
                else
                {
                    if (csrPstn == rtxtHebOrGk.Text.Length - 1)
                    {
                        rtxtHebOrGk.Text = rtxtHebOrGk.Text.Substring(0, rtxtHebOrGk.Text.Length - 1) + replacementLetter;
                    }
                    else
                    {
                        rtxtHebOrGk.Text = rtxtHebOrGk.Text.Substring(0, csrPstn) + replacementLetter + rtxtHebOrGk.Text.Substring(csrPstn + 1);
                    }
                }
                rtxtHebOrGk.SelectionStart = csrPstn + 1;
            }
        }

        public void virtualKeypress(Char keyValue)
        {
            rtxtHebOrGk.Focus();
            if (isHebrew)
            {
                switch (keyValue)
                {
                    case (char)7: handleDel(); break;
                    case (char)27: rtxtHebOrGk.Text = ""; break;
                    case '\b': handleBackspace(); break;
                    case 'A': addHebVowel("\u05B8"); break;
                    case 'C': addHebConsonant("\u05E5"); break;
                    case 'E': addHebVowel("\u05B5"); break;
                    case 'H': addHebConsonant("\u05D7"); break;
                    case 'K': addHebConsonant("\u05DA"); break;
                    case 'M': addHebConsonant("\u05DD"); break;
                    case 'N': addHebConsonant("\u05DF"); break;
                    case 'P': addHebConsonant("\u05E3"); break;
                    case 'S': addHebConsonant("\u05E9"); break;
                    case 'T': addHebConsonant("\u05EA"); break;
                    case 'Y': addHebConsonant("\u05E2"); break;
                    case 'a': addHebVowel("\u05B7"); break;
                    case 'b': addHebConsonant("\u05D1"); break;
                    case 'c': addHebConsonant("\u05E6"); break;
                    case 'd': addHebConsonant("\u05D3"); break;
                    case 'e': addHebVowel("\u05B6"); break; //
                    case 'g': addHebConsonant("\u05D2"); break;
                    case 'h': addHebConsonant("\u05D4"); break;
                    case 'i': addHebVowel("\u05B4"); break; //
                    case 'k': addHebConsonant("\u05DB"); break;
                    case 'l': addHebConsonant("\u05DC"); break;
                    case 'm': addHebConsonant("\u05DE"); break;
                    case 'n': addHebConsonant("\u05E0"); break;
                    case 'o': addHebVowel("\u05B9"); break; //
                    case 'p': addHebConsonant("\u05E4"); break;
                    case 'q': addHebConsonant("\u05E7"); break;
                    case 'r': addHebConsonant("\u05E8"); break;
                    case 's': addHebConsonant("\u05E1"); break;
                    case 't': addHebConsonant("\u05D8"); break;
                    case 'u': addHebVowel("\u05BB"); break; //
                    case 'v': addHebConsonant("\u05D5"); break;
                    case 'x': addHebConsonant("\u05D0"); break;
                    case 'y': addHebConsonant("\u05D9"); break;
                    case 'z': addHebConsonant("\u05D6"); break;
                    case '\'': addHebVowel("\u05B0"); break;
                    case '<': addHebVowel("\u05C2"); break;
                    case '>': addHebVowel("\u05C1"); break;
                    case '-': addHebVowel("\u05BE"); break;
                    case '^': addHebVowel("\u05AB"); break;
                    case '.': addHebVowel("\u05BC"); break;
                    case ':': addHebVowel("\u05C3"); break;
                    case ' ': addHebConsonant(" "); break;
                    case '0': addHebVowel("\u0591"); break;
                    case '1': addHebVowel("\u0592"); break;
                    case '2': addHebVowel("\u0593"); break;
                    case '3': addHebVowel("\u0594"); break;
                    case '4': addHebVowel("\u0595"); break;
                    case '5': addHebVowel("\u0596"); break;
                    case '6': addHebVowel("\u0597"); break;
                    case '7': addHebVowel("\u0598"); break;
                    case '8': addHebVowel("\u0599"); break;
                    case '9': addHebVowel("\u059A"); break;
                    case (Char)129: addHebVowel("\u059B"); break;
                    case (Char)130: addHebVowel("\u059C"); break;
                    case (Char)131: addHebVowel("\u059D"); break;
                    case (Char)132: addHebVowel("\u059E"); break;
                    case (Char)133: addHebVowel("\u059F"); break;
                    case (Char)134: addHebVowel("\u05A0"); break;
                    case (Char)135: addHebVowel("\u05A1"); break;
                    case (Char)136: addHebVowel("\u05A2"); break;
                    case (Char)137: addHebVowel("\u05A3"); break;
                    case (Char)138: addHebVowel("\u05A4"); break;
                    case (Char)139: addHebVowel("\u05A5"); break;
                    case (Char)140: addHebVowel("\u05A6"); break;
                    case (Char)141: addHebVowel("\u05A7"); break;
                    case (Char)142: addHebVowel("\u05A8"); break;
                    case (Char)143: addHebVowel("\u05A9"); break;
                    case (Char)144: addHebVowel("\u05AA"); break;
                    case (Char)145: addHebVowel("\u05AB"); break;
                    case (Char)146: addHebVowel("\u05AC"); break;
                    case (Char)147: addHebVowel("\u05AD"); break;
                    case (Char)148: addHebVowel("\u05AE"); break;
                    case (Char)149: addHebVowel("\u05AF"); break;
                    case (Char)150: addHebVowel("\u05BD"); break;
                    case (Char)151: addHebVowel("\u05BF"); break;
                    case (Char)152: addHebVowel("\u05C0"); break;
                    case (Char)153: addHebVowel("\u05C5"); break;
                    case (Char)154: addHebVowel("\u05C6"); break;
                    case (Char)155: addHebVowel("\u05C7"); break;
                    case (Char)156: addHebVowel("\u05F0"); break;
                    case (Char)157: addHebVowel("\u05F1"); break;
                    case (Char)158: addHebVowel("\u05F2"); break;
                    case (Char)159: addHebVowel("\u05F3"); break;
                    case (Char)160: addHebVowel("\u05F4"); break;
                    default: break;
                }
            }
            else
            {
                switch (keyValue)
                {
                    case (char)7: handleDel(); break;
                    case '\b': handleBackspace(); break;
                    case (char)27: rtxtHebOrGk.Text = ""; break;
                    case 'a': addGkChar("\u03B1"); break;
                    case 'b': addGkChar("\u03B2"); break;
                    case 'c': addGkChar("\u03C7"); break;
                    case 'd': addGkChar("\u03B4"); break;
                    case 'e': addGkChar("\u03B5"); break;
                    case 'f': addGkChar("\u03C6"); break;
                    case 'g': addGkChar("\u03B3"); break;
                    //                    case 'h': addGkChar("\u03B7"); break;
                    case 'i': addGkChar("\u03B9"); break;
                    //                    case 'j': addGkChar("\u03C9"); break;
                    case 'k': addGkChar("\u03BA"); break;
                    case 'l': addGkChar("\u03BB"); break;
                    case 'm': addGkChar("\u03BC"); break;
                    case 'n': addGkChar("\u03BD"); break;
                    case 'o': addGkChar("\u03BF"); break;
                    case 'p': addGkChar("\u03C0"); break;
                    //                    case 'q': addGkChar("\u03B8"); break;
                    case 'r': addGkChar("\u03C1"); break;
                    case 's': addGkChar("\u03C3"); break;
                    case 't': addGkChar("\u03C4"); break;
                    case 'u': addGkChar("\u03C5"); break;
                    case 'w': addGkChar("\u03DD"); break;
                    case 'x': addGkChar("\u03BE"); break;
                    //                    case 'y': addGkChar("\u03C8"); break;
                    case 'z': addGkChar("\u03B6"); break;
                    case 'A': addGkChar("\u0391"); break;
                    case 'B': addGkChar("\u0392"); break;
                    case 'C': addGkChar("\u03A7"); break;
                    case 'D': addGkChar("\u0394"); break;
                    case 'E': addGkChar("\u0395"); break;
                    case 'F': addGkChar("\u03A6"); break;
                    case 'G': addGkChar("\u0393"); break;
                    //                    case 'H': addGkChar("\u0397"); break;
                    case 'I': addGkChar("\u0399"); break;
                    //                    case 'J': addGkChar("\u03A9"); break;
                    case 'K': addGkChar("\u039A"); break;
                    case 'L': addGkChar("\u039B"); break;
                    case 'M': addGkChar("\u039C"); break;
                    case 'N': addGkChar("\u039D"); break;
                    case 'O': addGkChar("\u039F"); break;
                    case 'P': addGkChar("\u03A0"); break;
                    //                    case 'Q': addGkChar("\u0398"); break;
                    case 'R': addGkChar("\u03A1"); break;
                    case 'S': addGkChar("\u03A3"); break;
                    case 'T': addGkChar("\u03A4"); break;
                    case 'U': addGkChar("\u03A5"); break;
                    case 'W': addGkChar("\u03DC"); break;
                    case 'X': addGkChar("\u039E"); break;
                    //                    case 'Y': addGkChar("\u03A8"); break;
                    case 'Z': addGkChar("\u0396"); break;
                    //                    case '@': addGkChar("\u03C2"); break;
                    case ',': addGkChar("\u0387"); break;
                    case '?': addGkChar("\u037E"); break;
                    case ')': addGkChar(")"); break;
                    case '(': addGkChar("("); break;
                    case '/': addGkChar("/"); break;
                    case '\\': addGkChar("\\"); break;
                    case '~': addGkChar("~"); break;
                    case '\'': addGkChar("'"); break;
                    case ':': addGkChar(":"); break;
                    case ' ': addGkChar(" "); break;
                    case '.': addGkChar("."); break;
                    case (Char)143: addGkChar("\u03B7"); break;
                    case (Char)153: addGkChar("\u03C9"); break;
                    case (Char)154: addGkChar("\u03C8"); break;
                    case (Char)157: addGkChar("\u03C2"); break;
                    case (Char)158: addGkChar("\u03B8"); break;
                    case (Char)169: addGkChar("\u0397"); break;
                    case (Char)179: addGkChar("\u03A9"); break;
                    case (Char)180: addGkChar("\u03A8"); break;
                    case (Char)184: addGkChar("\u0398"); break;
                }
            }
            rtxtHebOrGk.Focus();
        }

        private void addGkChar(String inputCharacter)
        {
            int textPosition;
            String newCharacter, replacementChar, leftOfWord, rightOfWord;
            String[] nonAlpha1 = { "/", "~", "\\", ")", "(", "'", ",", "?", ":", "\u0387", "\u037E", " ", "." };
            String[] nonAlpha2 = { "/", "~", "\\", ")", "(", "'", ":" };
            SortedList<int, String> previousCharacter = new SortedList<int, string>();

            newCharacter = inputCharacter;
            if ((rtxtHebOrGk.Text == null) || (rtxtHebOrGk.Text.Length == 0))
            {
                if (nonAlpha1.Contains(newCharacter)) return;
                rtxtHebOrGk.Text = newCharacter;
                rtxtHebOrGk.SelectionStart = 1;
            }
            else
            {
                textPosition = rtxtHebOrGk.SelectionStart;
                if (textPosition == 0)
                {
                    if (nonAlpha1.Contains(newCharacter)) return;
                    rtxtHebOrGk.Text = newCharacter + rtxtHebOrGk.Text;
                    rtxtHebOrGk.SelectionStart = 0;
                }
                else
                {
                    if (textPosition == rtxtHebOrGk.Text.Length)
                    {
                        if (nonAlpha2.Contains(newCharacter)) rtxtHebOrGk.Text = modifyFinalChar(rtxtHebOrGk.Text, newCharacter);
                        else rtxtHebOrGk.Text += newCharacter;
                        rtxtHebOrGk.SelectionStart = rtxtHebOrGk.Text.Length - 1;
                    }
                    else
                    {
                        leftOfWord = rtxtHebOrGk.Text.Substring(0, textPosition);
                        rightOfWord = rtxtHebOrGk.Text.Substring(textPosition);
                        if (nonAlpha2.Contains(newCharacter))
                        {
                            leftOfWord = modifyFinalChar(leftOfWord, newCharacter);
                            rtxtHebOrGk.Text = leftOfWord + rightOfWord;
                        }
                        else rtxtHebOrGk.Text = leftOfWord + newCharacter + rightOfWord;
                        rtxtHebOrGk.SelectionStart = leftOfWord.Length;
                    }
                }
            }
            rtxtHebOrGk.SelectionStart = rtxtHebOrGk.SelectionStart + 1;
        }

        private String modifyFinalChar(String targetString, String modifyingChar)
        {
            String charToBeModified, modifiedChar;

            charToBeModified = targetString.Substring(targetString.Length - 1);
            switch (modifyingChar[0])
            {
                case ')': modifiedChar = greekProcessing.processSmoothBreathing(charToBeModified); break;
                case '(': modifiedChar = greekProcessing.processRoughBreathing(charToBeModified); break;
                case '/': modifiedChar = greekProcessing.processAccute(charToBeModified); break;
                case '\\': modifiedChar = greekProcessing.processGrave(charToBeModified); break;
                case '~': modifiedChar = greekProcessing.processCirc(charToBeModified); break;
                case ':': modifiedChar = greekProcessing.processDiaeresis(charToBeModified); break;
                case '\'': modifiedChar = greekProcessing.processIotaSub(charToBeModified); break;
                default: modifiedChar = modifyingChar; break;
            }
            return targetString.Substring(0, targetString.Length - 1) + modifiedChar;
        }

        private void addHebConsonant(String newConsonant)
        {
            int textPosition;
            String newCharacter, leftOfWord, rightOfWord;
            SortedList<int, String> previousCharacter = new SortedList<int, string>();

            newCharacter = newConsonant;
            if ((rtxtHebOrGk.Text == null) || (rtxtHebOrGk.Text.Length == 0))
            {
                rtxtHebOrGk.Text = newCharacter;
                rtxtHebOrGk.SelectionStart = 1;
            }
            else
            {
                textPosition = rtxtHebOrGk.SelectionStart;
                if (textPosition == 0)
                {
                    rtxtHebOrGk.Text = newCharacter + rtxtHebOrGk.Text;
                    rtxtHebOrGk.SelectionStart = 0;
                }
                else
                {
                    if (textPosition == rtxtHebOrGk.Text.Length)
                    {
                        rtxtHebOrGk.Text += newCharacter;
                        rtxtHebOrGk.SelectionStart = rtxtHebOrGk.Text.Length - 1;
                    }
                    else
                    {
                        leftOfWord = rtxtHebOrGk.Text.Substring(0, textPosition);
                        rightOfWord = rtxtHebOrGk.Text.Substring(textPosition);
                        rtxtHebOrGk.Text = leftOfWord + newCharacter + rightOfWord;
                        rtxtHebOrGk.SelectionStart = leftOfWord.Length;
                    }
                }
            }
            rtxtHebOrGk.SelectionStart = rtxtHebOrGk.SelectionStart + 1;
        }

        private void addHebVowel(String newVowel)
        {
            /***********************************************************************************
             *                                                                                 *
             *                                   addHebVowel                                   *
             *                                   ===========                                   *
             *                                                                                 *
             *  We will *not* protect the user from silly input.                               *
             *                                                                                 *
             *  Hataf vowels will be created from the corresponding full vowel followed        *
             *    *immediately* by a sheva.                                                    *
             *                                                                                 *
             ***********************************************************************************/

            int textPosition;
            String newCharacter, prevCharacter, leftOfWord, rightOfWord;
            SortedList<int, String> previousCharacter = new SortedList<int, string>();

            newCharacter = newVowel;
            if ((rtxtHebOrGk.Text == null) || (rtxtHebOrGk.Text.Length == 0)) return;  // A vowel doesn't make sense here
            textPosition = rtxtHebOrGk.SelectionStart;
            if (textPosition == 0) return;  // A vowel doesn't make sense here, either
            if (textPosition == rtxtHebOrGk.Text.Length)
            {
                if (newVowel[0] == '\u05b0')
                {
                    prevCharacter = rtxtHebOrGk.Text.Substring(textPosition - 1, 1);
                    switch (prevCharacter[0])
                    {
                        case '\u05B6': rtxtHebOrGk.Text = replaceFinalChar(rtxtHebOrGk.Text, "\u05B1"); break;
                        case '\u05B7': rtxtHebOrGk.Text = replaceFinalChar(rtxtHebOrGk.Text, "\u05B2"); break;
                        case '\u05B8': rtxtHebOrGk.Text = replaceFinalChar(rtxtHebOrGk.Text, "\u05B3"); break;
                        default: rtxtHebOrGk.Text += newCharacter; break;
                    }
                }
                else rtxtHebOrGk.Text += newVowel;
                rtxtHebOrGk.SelectionStart = rtxtHebOrGk.Text.Length - 1;
            }
            else
            {
                leftOfWord = rtxtHebOrGk.Text.Substring(0, textPosition);
                rightOfWord = rtxtHebOrGk.Text.Substring(textPosition);
                if (newVowel[0] == '\u05b0')
                {
                    prevCharacter = leftOfWord.Substring(textPosition - 1, 1);
                    switch (prevCharacter[0])
                    {
                        case '\u05B6': rtxtHebOrGk.Text = replaceFinalChar(leftOfWord, "\u05B1") + rightOfWord; break;
                        case '\u05B7': rtxtHebOrGk.Text = replaceFinalChar(leftOfWord, "\u05B2") + rightOfWord; break;
                        case '\u05B8': rtxtHebOrGk.Text = replaceFinalChar(leftOfWord, "\u05B3") + rightOfWord; break;
                        default: rtxtHebOrGk.Text = leftOfWord + newCharacter + rightOfWord; break;
                    }
                }
                else rtxtHebOrGk.Text = leftOfWord + newVowel + rightOfWord;
                rtxtHebOrGk.SelectionStart = leftOfWord.Length;
            }
            rtxtHebOrGk.SelectionStart = rtxtHebOrGk.SelectionStart + 1;
        }

        private String replaceFinalChar(String targetString, String newChar)
        {
            String newString;

            newString = targetString.Substring(0, targetString.Length - 1) + newChar;
            return newString;
        }

        public void modifyHebrewGreekFlag(bool hebFlag)
        {
            isHebrew = hebFlag;
            changeKeys(isHebrew, isMiniscule);
        }
    }
}

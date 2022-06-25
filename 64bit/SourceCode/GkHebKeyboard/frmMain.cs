using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using System.IO;

namespace GkHebKeyboard
{
    public partial class frmMain : Form
    {
        /*----------------------------------------------------^
         *                                                    *
         *  Global Variables                                  *
         *                                                    *
         *----------------------------------------------------*/

        classGlobal globalVars;
        classGreek greekProcessing;
        classKeyboard keyboard;
        frmHelp keyboardHelp;

        /*----------------------------------------------------^
         *                                                    *
         *  Variables for the keyboard scanning process       *
         *                                                    *
         *----------------------------------------------------*/

        int keyPressCode = 0;
        Char keyDownCode = '#';

        bool isInNeedOfActivation = true, isHebrew = true, isMiniscule = true;
        String currentFileName = "", basePath, defaultPath = @"..\Resources\";
        SortedDictionary<int, classKeyData> keyStore = new SortedDictionary<int, classKeyData>();

        SortedDictionary<int, classKeyData> relateEngValToKey = new SortedDictionary<int, classKeyData>();
        classKeyData allKeyData = new classKeyData();

        RegistryKey baseKey;


        public frmMain()
        {
            float fontSize;
            Object regValue;

            InitializeComponent();

            globalVars = new classGlobal();
            greekProcessing = new classGreek();
            keyboard = new classKeyboard();
            defaultPath = Path.GetFullPath(defaultPath);
            baseKey = Registry.CurrentUser.OpenSubKey(@"software\LFCConsulting\GkHebKeyboard", true);
            if (baseKey == null)
            {
                baseKey = Registry.CurrentUser.CreateSubKey(@"software\LFCConsulting\GkHebKeyboard");
            }
            if (baseKey == null)
            {
                // We weren't able to create a key, so use default settings
                isHebrew = false;
                fontSize = 12F;
                basePath = defaultPath;
            }
            else 
            { 
                regValue = baseKey.GetValue("Opening Language");
                if (regValue != null)
                {
                    if (Convert.ToInt32(regValue.ToString()) == 2) isHebrew = false;
                    else tabCtrlKeyboard.SelectedIndex = 1;
                }
                else isHebrew = false;
                regValue = baseKey.GetValue("Starting Font Size");
                if (regValue == null) fontSize = 12F;
                else fontSize = (float)Convert.ToDecimal(regValue.ToString());
                regValue = baseKey.GetValue("Base Path");
                if (regValue == null)
                {
                    basePath = defaultPath;
                }
                else
                {
                    basePath = regValue.ToString();
                }
            }
            if( basePath[0] == '"') basePath = basePath.Substring(1);
            if( basePath[basePath.Length - 1] == '"') basePath = basePath.Substring( 0, basePath.Length - 1 );
            rtxtHebOrGk.Font = new Font(rtxtHebOrGk.Font.FontFamily, fontSize);
            mnuMainCbFontSize.SelectedItem = fontSize.ToString();
            globalVars.BasePath = basePath;
            keyboard.TabCtlMain = tabCtrlKeyboard;
            keyboard.GkKeyboard = tabPgeGkKeyboard;
            keyboard.HebKeyboard = tabPgeHebKeyboard;
            keyboard.RtxtHebOrGk = rtxtHebOrGk;
            keyboard.BtnCapsLight = btnCapsLight;
            keyboard.GlobalVars = globalVars;
        }

        private void frmMain_Activated(object sender, EventArgs e)
        {

            if (isInNeedOfActivation)
            {
                // Now, set up the final presentation
                keyboard.initialiseKeyboard(greekProcessing, globalVars);
                greekProcessing.constructGreekLists();
                populateGrids();
                rtxtHebOrGk.Focus();
                isInNeedOfActivation = false;
            }
        }

        private void tabCtrlKeyboard_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void populateGrids()
        {
            const int maxNo = 22;

            int idx, totalNo;

            dgvHebCons.RowCount = 2;
            dgvHebCons.Rows[0].HeaderCell.Value = "Base letter:";
            dgvHebCons.Rows[0].Cells[0].Value = "x";
            dgvHebCons.Rows[0].Cells[1].Value = "b";
            dgvHebCons.Rows[0].Cells[2].Value = "g";
            dgvHebCons.Rows[0].Cells[3].Value = "d";
            dgvHebCons.Rows[0].Cells[4].Value = "h";
            dgvHebCons.Rows[0].Cells[5].Value = "z";
            dgvHebCons.Rows[0].Cells[6].Value = "v";
            dgvHebCons.Rows[0].Cells[7].Value = "H";
            dgvHebCons.Rows[0].Cells[8].Value = "t";
            dgvHebCons.Rows[0].Cells[9].Value = "y";
            dgvHebCons.Rows[0].Cells[10].Value = "k";
            dgvHebCons.Rows[0].Cells[11].Value = "l";
            dgvHebCons.Rows[0].Cells[12].Value = "m";
            dgvHebCons.Rows[0].Cells[13].Value = "n";
            dgvHebCons.Rows[0].Cells[14].Value = "s";
            dgvHebCons.Rows[0].Cells[15].Value = "Y";
            dgvHebCons.Rows[0].Cells[16].Value = "p";
            dgvHebCons.Rows[0].Cells[17].Value = "c";
            dgvHebCons.Rows[0].Cells[18].Value = "q";
            dgvHebCons.Rows[0].Cells[19].Value = "r";
            dgvHebCons.Rows[0].Cells[20].Value = "S";
            dgvHebCons.Rows[0].Cells[21].Value = "T";
            dgvHebCons.Rows[1].HeaderCell.Value = "Final letter:";
            dgvHebCons.Rows[1].Cells[10].Value = "K";
            dgvHebCons.Rows[1].Cells[12].Value = "M";
            dgvHebCons.Rows[1].Cells[13].Value = "N";
            dgvHebCons.Rows[1].Cells[16].Value = "P";
            dgvHebCons.Rows[1].Cells[17].Value = "C";
            dgvAdditional.RowCount = 1;
            dgvAdditional.Rows[0].HeaderCell.Value = "Key code:";
            dgvAdditional.Rows[0].Cells[0].Value = "A";
            dgvAdditional.Rows[0].Cells[1].Value = "a";
            dgvAdditional.Rows[0].Cells[2].Value = "E";
            dgvAdditional.Rows[0].Cells[3].Value = "e";
            dgvAdditional.Rows[0].Cells[4].Value = "i";
            dgvAdditional.Rows[0].Cells[5].Value = "o";
            dgvAdditional.Rows[0].Cells[6].Value = "u";
            dgvAdditional.Rows[0].Cells[7].Value = "'";
            dgvAdditional.Rows[0].Cells[8].Value = ".";
            dgvAdditional.Rows[0].Cells[9].Value = "<";
            dgvAdditional.Rows[0].Cells[10].Value = ">";
            dgvAdditional.Rows[0].Cells[11].Value = "-";
            dgvAdditional.Rows[0].Cells[12].Value = "^";
            dgvAdditional.Rows[0].Cells[13].Value = ":";
            dgvGkChars.RowCount = 5;
            dgvGkChars.Rows[0].HeaderCell.Value = "Miniscules:";
            dgvGkChars.Rows[0].Cells[0].Value = "a";
            dgvGkChars.Rows[0].Cells[1].Value = "b";
            dgvGkChars.Rows[0].Cells[2].Value = "g";
            dgvGkChars.Rows[0].Cells[3].Value = "d";
            dgvGkChars.Rows[0].Cells[4].Value = "e";
            dgvGkChars.Rows[0].Cells[5].Value = "z";
            dgvGkChars.Rows[0].Cells[6].Value = "^e";
            dgvGkChars.Rows[0].Cells[7].Value = "^t";
            dgvGkChars.Rows[0].Cells[8].Value = "i";
            dgvGkChars.Rows[0].Cells[9].Value = "k";
            dgvGkChars.Rows[0].Cells[10].Value = "l";
            dgvGkChars.Rows[0].Cells[11].Value = "m";
            dgvGkChars.Rows[0].Cells[12].Value = "n";
            dgvGkChars.Rows[0].Cells[13].Value = "x";
            dgvGkChars.Rows[0].Cells[14].Value = "o";
            dgvGkChars.Rows[0].Cells[15].Value = "p";
            dgvGkChars.Rows[0].Cells[16].Value = "r";
            dgvGkChars.Rows[0].Cells[17].Value = "s";
            dgvGkChars.Rows[0].Cells[18].Value = "^s";
            dgvGkChars.Rows[0].Cells[19].Value = "t";
            dgvGkChars.Rows[0].Cells[20].Value = "u";
            dgvGkChars.Rows[0].Cells[21].Value = "^p";
            dgvGkChars.Rows[1].HeaderCell.Value = "Majiscules:";
            dgvGkChars.Rows[1].Cells[0].Value = "A";
            dgvGkChars.Rows[1].Cells[1].Value = "B";
            dgvGkChars.Rows[1].Cells[2].Value = "G";
            dgvGkChars.Rows[1].Cells[3].Value = "D";
            dgvGkChars.Rows[1].Cells[4].Value = "E";
            dgvGkChars.Rows[1].Cells[5].Value = "Z";
            dgvGkChars.Rows[1].Cells[6].Value = "^E";
            dgvGkChars.Rows[1].Cells[7].Value = "^T";
            dgvGkChars.Rows[1].Cells[8].Value = "I";
            dgvGkChars.Rows[1].Cells[9].Value = "K";
            dgvGkChars.Rows[1].Cells[10].Value = "L";
            dgvGkChars.Rows[1].Cells[11].Value = "M";
            dgvGkChars.Rows[1].Cells[12].Value = "N";
            dgvGkChars.Rows[1].Cells[13].Value = "X";
            dgvGkChars.Rows[1].Cells[14].Value = "O";
            dgvGkChars.Rows[1].Cells[15].Value = "P";
            dgvGkChars.Rows[1].Cells[16].Value = "R";
            dgvGkChars.Rows[1].Cells[17].Value = "S";
            dgvGkChars.Rows[1].Cells[19].Value = "T";
            dgvGkChars.Rows[1].Cells[20].Value = "U";
            dgvGkChars.Rows[1].Cells[21].Value = "^P";
            dgvGkChars.Rows[2].Cells[0].Value = "φ";
            dgvGkChars.Rows[2].Cells[1].Value = "χ";
            dgvGkChars.Rows[2].Cells[2].Value = "ω";
            dgvGkChars.Rows[2].Cells[3].Value = "ϝ";
            dgvGkChars.Rows[2].Cells[4].Value = "΄";
            dgvGkChars.Rows[2].Cells[5].Value = "῀";
            dgvGkChars.Rows[2].Cells[6].Value = "῀";
            dgvGkChars.Rows[2].Cells[7].Value = "῾";
            dgvGkChars.Rows[2].Cells[8].Value = "᾽";
            dgvGkChars.Rows[2].Cells[9].Value = "ι";
            dgvGkChars.Rows[2].Cells[10].Value = "·";
            dgvGkChars.Rows[2].Cells[11].Value = ";";
            dgvGkChars.Rows[2].Cells[12].Value = "¨";
            dgvGkChars.Rows[2].Cells[4].ToolTipText = "oxia (accute)";
            dgvGkChars.Rows[2].Cells[5].ToolTipText = "perispomeni (circumflex)";
            dgvGkChars.Rows[2].Cells[6].ToolTipText = "varia (grave)";
            dgvGkChars.Rows[2].Cells[7].ToolTipText = "daseia (rough breathing)";
            dgvGkChars.Rows[2].Cells[8].ToolTipText = "koronis (smooth breathing)";
            dgvGkChars.Rows[2].Cells[9].ToolTipText = "prosgegrammeni (iota subscript)";
            dgvGkChars.Rows[2].Cells[10].ToolTipText = "ano teleia";
            dgvGkChars.Rows[2].Cells[11].ToolTipText = "erotimatio (question mark)";
            dgvGkChars.Rows[2].Cells[12].ToolTipText = "dieresis (umlaut)";
            dgvGkChars.Rows[2].DefaultCellStyle.Font = new Font("Times New Roman", 14F, FontStyle.Regular);
            dgvGkChars.Rows[3].HeaderCell.Value = "Miniscules/normal:";
            dgvGkChars.Rows[3].Cells[0].Value = "f";
            dgvGkChars.Rows[3].Cells[1].Value = "c";
            dgvGkChars.Rows[3].Cells[2].Value = "^o";
            dgvGkChars.Rows[3].Cells[3].Value = "w";
            dgvGkChars.Rows[3].Cells[4].Value = "/";
            dgvGkChars.Rows[3].Cells[5].Value = "~";
            dgvGkChars.Rows[3].Cells[6].Value = "\\";
            dgvGkChars.Rows[3].Cells[7].Value = ")";
            dgvGkChars.Rows[3].Cells[8].Value = "(";
            dgvGkChars.Rows[3].Cells[9].Value = "'";
            dgvGkChars.Rows[3].Cells[10].Value = ",";
            dgvGkChars.Rows[3].Cells[11].Value = "?";
            dgvGkChars.Rows[3].Cells[12].Value = ":";
            dgvGkChars.Rows[4].HeaderCell.Value = "Majiscules:";
            dgvGkChars.Rows[4].Cells[0].Value = "F";
            dgvGkChars.Rows[4].Cells[1].Value = "C";
            dgvGkChars.Rows[4].Cells[2].Value = "^O";
            dgvGkChars.Rows[4].Cells[3].Value = "W";
            String[] hebAccents = { "\u0591", "\u0592", "\u0593", "\u0594", "\u0595", "\u0596", "\u0597", "\u0598", "\u0599",
                                        "\u059A", "\u059B", "\u059C", "\u059D", "\u059E", "\u059F", "\u05A0", "\u05A1", "\u05A2",
                                        "\u05A3", "\u05A4", "\u05A5", "\u05A6", "\u05A7", "\u05A8", "\u05A9", "\u05AA", "\u05AB",
                                        "\u05AC", "\u05AD", "\u05AE", "\u05AF", "\u05BD", "\u05BF", "\u05C0", "\u05C5", "\u05C6",
                                        "\u05C7", "\u05F0", "\u05F1", "\u05F2", "\u05F3", "\u05F4" };
            String[] hebAccentHints = { "Etnahta", "Segol", "Shalshelet", "Zaqef Qatan", "Zaqef Gadol", "Tipeha", "Revia", "Zarqa",
                                        "Pashta", "Yetiv", "Tevir", "Geresh", "Geresh Muqdam", "Gershayim", "Qarney Para", "Telisha Gedola",
                                        "Pazer", "Atnah Hafukh", "Munah", "Mahapakh", "Merkha", "Merkha Kefula", "Darga", "Qadma",
                                        "Telisha Qetana", "Yerah Ben Yomo", "Ole", "Iluy", "Dehi", "Zinor", "Masora Circle", "Point Meteg",
                                        "Point Rafe", "Punctuation Paseq", "Mark Lower Dot", "Punctuation Nun Hafukha", "Qamats Qatan",
                                        "Ligature Yiddish Double Vav", "Ligature Yiddish Vav Yod", "Ligature Yiddish Double Yod",
                                        "Punctuation Geresh", "Punctuation Gershayim" };
            String[] HebEquivalents = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "^0", "^1", "^2", "^3", "^4", "^5", "^6", "^7", "^8", "^9",
                                        "^a", "^b", "^c", "^d", "^e", "^f", "^g", "^h", "^i", "^j", "^k", "^l", "^m", "^n", "^o", "^p", "^q", "^r",
                                        "^s", "^t", "^u", "^v" };  //, "^w", "^x", "^y", "^z" };
            int[] exceptions = { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0 };
            totalNo = hebAccents.Length;
            dgvAccents.RowCount = 4;
            dgvAccents.ColumnCount = maxNo;
            dgvAccents.Rows[0].Height = 32;
            dgvAccents.Rows[2].Height = 32;
            dgvAccents.Rows[0].HeaderCell.Value = "Accents:";
            dgvAccents.Rows[1].HeaderCell.Value = "Key: Ctrl + ...";
            dgvAccents.Rows[1].DefaultCellStyle.Font = new Font("Times New Roman", 10F);
            dgvAccents.Rows[3].DefaultCellStyle.Font = new Font("Times New Roman", 10F);
            for (idx = 0; idx < maxNo; idx++)
            {
                dgvAccents.Rows[0].Cells[idx].Value = "\u25CC" + hebAccents[idx];
                dgvAccents.Rows[0].Cells[idx].ToolTipText = hebAccentHints[idx];
                dgvAccents.Columns[idx].Width = 32;
                dgvAccents.Rows[1].Cells[idx].Value = HebEquivalents[idx];
            }
            dgvAccents.Rows[2].HeaderCell.Value = "Accents:";
            for (idx = maxNo; idx < totalNo; idx++)
            {
                if (exceptions[idx] == 1) dgvAccents.Rows[2].Cells[idx - maxNo].Value = "\u25CC" + hebAccents[idx];
                else dgvAccents.Rows[2].Cells[idx - maxNo].Value = hebAccents[idx];
                dgvAccents.Rows[2].Cells[idx - maxNo].ToolTipText = hebAccentHints[idx];
                dgvAccents.Rows[3].Cells[idx - maxNo].Value = HebEquivalents[idx];
            }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if ((rtxtHebOrGk != null) && (rtxtHebOrGk.Text.Length > 0))
            {
                Clipboard.SetText(rtxtHebOrGk.Text);
            }
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            int csrPstn;

            csrPstn = rtxtHebOrGk.SelectionStart;
            if (csrPstn < rtxtHebOrGk.Text.Length)
            {
                csrPstn++;
                rtxtHebOrGk.SelectionStart = csrPstn;
            }
            rtxtHebOrGk.Focus();
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            int csrPstn;

            csrPstn = rtxtHebOrGk.SelectionStart;
            if (csrPstn > 0)
            {
                csrPstn--;
                rtxtHebOrGk.SelectionStart = csrPstn;
            }
            rtxtHebOrGk.Focus();
        }

        private void btnEnd_Click(object sender, EventArgs e)
        {
            int csrPstn, nPstn;

            csrPstn = rtxtHebOrGk.SelectionStart;
            if (csrPstn < rtxtHebOrGk.Text.Length)
            {
                nPstn = rtxtHebOrGk.Text.IndexOf('\n', csrPstn);
                if (nPstn > -1)
                {
                    csrPstn = nPstn;
                }
                else
                {
                    csrPstn = rtxtHebOrGk.Text.Length;
                }
                rtxtHebOrGk.SelectionStart = csrPstn;
            }
            rtxtHebOrGk.Focus();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            int csrPstn, nPstn;

            csrPstn = rtxtHebOrGk.SelectionStart;
            if (csrPstn >= rtxtHebOrGk.Text.Length) csrPstn = rtxtHebOrGk.Text.Length - 1;
            if (rtxtHebOrGk.Text[csrPstn] == '\n') csrPstn--;
            if (csrPstn > 0)
            {
                nPstn = rtxtHebOrGk.Text.LastIndexOf('\n', csrPstn);
                if (nPstn > 0)
                {
                    csrPstn = nPstn + 1;
                }
                else
                {
                    csrPstn = 0;
                }
                rtxtHebOrGk.SelectionStart = csrPstn;
            }
            rtxtHebOrGk.Focus();
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            int csrPstn, nPstn, currLineCount, nextLineCount, nextCr;

            csrPstn = rtxtHebOrGk.SelectionStart;
            if (rtxtHebOrGk.Text[csrPstn] == '\n') csrPstn--;
            // Is there a following line
            nPstn = rtxtHebOrGk.Text.IndexOf('\n', csrPstn);
            if (nPstn > -1)
            {
                nextCr = nPstn;
                // Where in the current line are we?
                nPstn = rtxtHebOrGk.Text.LastIndexOf('\n', csrPstn);
                if (nPstn == -1)
                {
                    // We're on the first line, so csrPstn is it
                    currLineCount = csrPstn + 1;
                }
                else
                {
                    currLineCount = csrPstn - nPstn;
                }
                // So now, find the equivalent position on the next line.
                nPstn = rtxtHebOrGk.Text.IndexOf('\n', nextCr + 1);
                if (nPstn == -1)
                {
                    // The *next* line is the last
                    nPstn = rtxtHebOrGk.Text.Length - 1;
                }
                if (nPstn > nextCr + currLineCount)
                {
                    nextLineCount = nextCr + currLineCount;
                }
                else
                {
                    if (nPstn == rtxtHebOrGk.Text.Length - 1)
                    {
                        nextLineCount = nPstn;
                    }
                    else
                    {
                        nextLineCount = nPstn;
                    }
                }
                rtxtHebOrGk.SelectionStart = nextLineCount;
            }
            rtxtHebOrGk.Focus();
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            int csrPstn, nPstn, currLineCount, nextLineCount, prevCr;

            csrPstn = rtxtHebOrGk.SelectionStart;
            if (rtxtHebOrGk.Text[csrPstn] == '\n') csrPstn--;
            // Is there a previous line
            nPstn = rtxtHebOrGk.Text.LastIndexOf('\n', csrPstn);
            if (nPstn > -1)
            {
                // We're not on the first line
                currLineCount = csrPstn - nPstn - 1;
                prevCr = nPstn;
                // So now, find the equivalent position on the previous line.
                nPstn = rtxtHebOrGk.Text.LastIndexOf('\n', prevCr - 1);
                if (nPstn == -1)
                {
                    // The *previous* line is the first
                    nextLineCount = currLineCount;
                }
                else
                {
                    nextLineCount = nPstn + currLineCount + 1;
                }
                if (nextLineCount > prevCr) nextLineCount = prevCr;
                rtxtHebOrGk.SelectionStart = nextLineCount;
            }
            rtxtHebOrGk.Focus();
        }

        private void btnFullHome_Click(object sender, EventArgs e)
        {
            int csrPstn;

            csrPstn = rtxtHebOrGk.SelectionStart;
            if (csrPstn > 0)
            {
                csrPstn = 0;
                rtxtHebOrGk.SelectionStart = csrPstn;
            }
            rtxtHebOrGk.Focus();
        }

        private void btnFullEnd_Click(object sender, EventArgs e)
        {
            int csrPstn;

            csrPstn = rtxtHebOrGk.SelectionStart;
            if (csrPstn < rtxtHebOrGk.Text.Length)
            {
                csrPstn = rtxtHebOrGk.Text.Length;
                rtxtHebOrGk.SelectionStart = csrPstn;
            }
            rtxtHebOrGk.Focus();
        }

        private void mnuFileSaveAs_Click(object sender, EventArgs e)
        {
            StreamWriter swKeyboard;
            Object regValue;

            if ((rtxtHebOrGk.Text != null) && (rtxtHebOrGk.Text.Length > 0))
            {
                if (baseKey != null)
                {
                    regValue = baseKey.GetValue("Source Path");
                    if (regValue != null)
                    {
                        dlgSave.InitialDirectory = regValue.ToString();
                    }
                }
                if (dlgSave.ShowDialog() == DialogResult.OK)
                {
                    currentFileName = dlgSave.FileName;
                    swKeyboard = new StreamWriter(currentFileName);
                    swKeyboard.Write(rtxtHebOrGk.Text);
                    swKeyboard.Close();
                    swKeyboard.Dispose();
                    MessageBox.Show("Contents of the Text Box saved", "Save Text entry", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void mnuFileSave_Click(object sender, EventArgs e)
        {
            StreamWriter swKeyboard;

            if ((rtxtHebOrGk.Text != null) && (rtxtHebOrGk.Text.Length > 0))
            {
                if (currentFileName.Length == 0)
                {
                    mnuFileSaveAs_Click(sender, e);
                }
                else
                {
                    swKeyboard = new StreamWriter(currentFileName);
                    swKeyboard.Write(rtxtHebOrGk.Text);
                    swKeyboard.Close();
                    swKeyboard.Dispose();
                    MessageBox.Show("Contents of the Text Box saved", "Save Text entry", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void mnuFileOpen_Click(object sender, EventArgs e)
        {
            StreamReader srKeyboard;
            Object regValue;

            if (baseKey != null)
            {
                regValue = baseKey.GetValue("Source Path");
                if (regValue != null)
                {
                    dlgOpen.InitialDirectory = regValue.ToString();
                }
            }
            if (dlgOpen.ShowDialog() == DialogResult.OK)
            {
                srKeyboard = new StreamReader(dlgOpen.FileName);
                rtxtHebOrGk.Text = srKeyboard.ReadToEnd();
                srKeyboard.Close();
                srKeyboard.Dispose();
            }
        }

        private void mnuFilePreferences_Click(object sender, EventArgs e)
        {
            frmPreferences paramSet = new frmPreferences();

            paramSet.initialisePreferences(baseKey, globalVars);
            paramSet.ShowDialog();
            paramSet.Dispose();
        }

        private void rtxtHebOrGk_KeyDown(object sender, KeyEventArgs e)
        {
            /**************************************************************************************
             *                                                                                    *
             *                              rtxtHebOrGk_KeyDown                                    *
             *                              ==================                                    *
             *                                                                                    *
             *  This will handle the non-visible key-presses that also don't generate a known     *
             *    escape sequence (e.g. the Del key).  Any codes that need to be acted on will be *
             *    stored in the global variable, keyPressCode.                                    *
             *                                                                                    *
             *  Values of keyPressCode:                                                           *
             *    -1      Already actioned (e.g. left arrow);                                     *
             *     0      No action taken                                                         *
             *     1      A printable character has been registered                               *
             *     7      Delete key pressed                                                      *
             *                                                                                    *
             **************************************************************************************/

            int keyPressVal;

            keyPressVal = e.KeyValue;

            if (e.Modifiers == Keys.Control) keyPressCode = 14;
            switch (e.KeyValue)
            {
                case 27: keyPressCode = 27; break;  // Esc key
                case 33: btnFullHome_Click(null, e); keyPressCode = -1; e.SuppressKeyPress = true; break;
                case 34: btnFullEnd_Click(null, e); keyPressCode = -1; e.SuppressKeyPress = true; break;
                case 35: btnEnd_Click(null, e); keyPressCode = -1; e.SuppressKeyPress = true; break;
                case 36: btnHome_Click(null, e); keyPressCode = -1; e.SuppressKeyPress = true; break;
                case 37: btnRight_Click(null, e); keyPressCode = -1; e.SuppressKeyPress = true; break;  // Note: this is actually **left**
                case 38: btnUp_Click(null, e); keyPressCode = -1; e.SuppressKeyPress = true; break;
                case 39: btnLeft_Click(null, e); keyPressCode = -1; e.SuppressKeyPress = true; break;  // Note: this is actually **right**
                case 40: btnDown_Click(null, e); keyPressCode = -1; e.SuppressKeyPress = true; break;
                case 46: keyPressCode = 7; break; // Artificial code for Del key
                default: break;
            }

        }

        private void rtxtHebOrGk_KeyPress(object sender, KeyPressEventArgs e)
        {
            /**************************************************************************************
             *                                                                                    *
             *                              rtxtHebOrGk_KeyPress                                   *
             *                              ===================                                   *
             *                                                                                    *
             *  This will handle the visible key-presses  Codes will be stored in the global      *
             *    variable, keyDownCode.                                                          *
             *                                                                                    *
             **************************************************************************************/

            keyDownCode = e.KeyChar;
            keyPressCode = 1;
            e.Handled = true;
        }

        private void rtxtHebOrGk_KeyUp(object sender, KeyEventArgs e)
        {
            /**************************************************************************************
             *                                                                                    *
             *                                rtxtHebOrGk_KeyUp                                    *
             *                                ================                                    *
             *                                                                                    *
             *  This completes the suite of key-press methods.  It will check the global          *
             *    variables and act on them.                                                      *
             *                                                                                    *
             **************************************************************************************/

            if (keyPressCode > 0)
            {
                if (keyPressCode == 1)
                {
                    if (e.Control)
                    {
                        switch (e.KeyCode)
                        {
                            case Keys.D0: keyDownCode = (Char)129; break;
                            case Keys.D1: keyDownCode = (Char)130; break;
                            case Keys.D2: keyDownCode = (Char)131; break;
                            case Keys.D3: keyDownCode = (Char)132; break;
                            case Keys.D4: keyDownCode = (Char)133; break;
                            case Keys.D5: keyDownCode = (Char)134; break;
                            case Keys.D6: keyDownCode = (Char)135; break;
                            case Keys.D7: keyDownCode = (Char)136; break;
                            case Keys.D8: keyDownCode = (Char)137; break;
                            case Keys.D9: keyDownCode = (Char)138; break;
                            case Keys.A: keyDownCode = (Char)139; break;
                            case Keys.B: keyDownCode = (Char)140; break;
                            case Keys.C: keyDownCode = (Char)141; break;
                            case Keys.D: keyDownCode = (Char)142; break;
                            case Keys.E:
                                if (e.Shift) keyDownCode = (Char)169;
                                else keyDownCode = (Char)143;
                                break;
                            case Keys.F: keyDownCode = (Char)144; break;
                            case Keys.G: keyDownCode = (Char)145; break;
                            case Keys.H: keyDownCode = (Char)146; break;
                            case Keys.I: keyDownCode = (Char)147; break;
                            case Keys.J: keyDownCode = (Char)148; break;
                            case Keys.K: keyDownCode = (Char)149; break;
                            case Keys.L: keyDownCode = (Char)150; break;
                            case Keys.M: keyDownCode = (Char)151; break;
                            case Keys.N: keyDownCode = (Char)152; break;
                            case Keys.O:
                                if (e.Shift) keyDownCode = (Char)179;
                                else keyDownCode = (Char)153;
                                break;
                            case Keys.P:
                                if (e.Shift) keyDownCode = (Char)180;
                                else keyDownCode = (Char)154;
                                break;
                            case Keys.Q: keyDownCode = (Char)155; break;
                            case Keys.R: keyDownCode = (Char)156; break;
                            case Keys.S:
                                if (e.Shift) keyDownCode = 'S';
                                else keyDownCode = (Char)157;
                                break;
                            case Keys.T:
                                if (e.Shift) keyDownCode = (Char)184;
                                else keyDownCode = (Char)158;
                                break;
                            case Keys.U: keyDownCode = (Char)159; break;
                            case Keys.V: keyDownCode = (Char)160; break;
                        }
                    }
                    keyboard.virtualKeypress(keyDownCode);
                }
                else
                {
                    if ((keyPressCode == 17) || (keyPressCode == 14)) keyboard.virtualKeypress(keyDownCode);
                    else keyboard.virtualKeypress((char)keyPressCode);
                }
            }
            keyPressCode = 0;
            keyDownCode = '#';
            e.SuppressKeyPress = true;
        }

        private void mnuMainCbFontSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            float newFontSize;

            newFontSize = (float)Convert.ToDecimal(mnuMainCbFontSize.SelectedItem.ToString());
            rtxtHebOrGk.Font = new Font(rtxtHebOrGk.Font.FontFamily, newFontSize);
        }

        private void mnuHelpAbout_Click(object sender, EventArgs e)
        {
            frmAbout currentAboutBox = new frmAbout();

            currentAboutBox.ShowDialog();
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            keyboardHelp = new frmHelp();

            keyboardHelp.initialiseHelp(globalVars);
            keyboardHelp.Show();
        }

        private void btnCursorInfo_Click(object sender, EventArgs e)
        {
            frmCursorKeys cursorInfo = new frmCursorKeys();

            cursorInfo.Show();
        }

        private void mnuFileExit_Click(object sender, EventArgs e)
        {
            btnClose_Click(sender, e);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}

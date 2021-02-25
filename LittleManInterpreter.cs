using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LittleManComputer
{
    class LittleManInterpreter
    {
        int[] memory;
        int accumulator;

        readonly string[] enteredMachineCode;
        readonly string[] enteredInput;

        public LittleManInterpreter(string[] enteredMachineCode, string[] enteredInput)
        {
            this.enteredMachineCode = enteredMachineCode;
            this.enteredInput = enteredInput;

            // LIST.RemoveAll(string.IsNullOrWhiteSpace);
        }

        public string GetLineNumbers()
        {
            string lineNumbers = "";
            int lineNumber = 0;
            for (int i=0; i<enteredMachineCode.Length; i++)
            {
                string currentLine = enteredMachineCode[i];
                if (string.IsNullOrEmpty(currentLine.Replace(" ","")))
                {
                    lineNumbers += "\r\n";
                } 
                else
                {
                    lineNumbers += lineNumber.ToString() + "\r\n";
                    lineNumber += 1;
                }
            }
            return lineNumbers;
        }

        private string GetEasyReadableCommand(int command)
        {
            if (100 <= command && command < 200)
            {
                return "ADD " + (command - 100).ToString();
            }
            if (200 <= command && command < 300)
            {
                return "SUB " + (command - 100).ToString();
            }
            if (300 <= command && command < 400)
            {
                return "STA " + (command - 100).ToString();
            }
            if (500 <= command && command < 600)
            {
                return "LDA " + (command - 100).ToString();
            }
            if (600 <= command && command < 700)
            {
                return "BRA " + (command - 100).ToString();
            }
            if (700 <= command && command < 800)
            {
                return "BRZ " + (command - 100).ToString();
            }
            if (800 <= command && command < 900)
            {
                return "BRP + " + (command - 100).ToString();
            }
            if (command == 901)
            {
                return "INP";
            }
            if (command == 902)
            {
                return "OUT";
            }
            if (command == 0)
            {
                return "HLT";
            }
            return "DAT " + command.ToString();
        } 

        public string GetEasyReadableCode()
        {
            string easyCode = "";
            for (int i = 0; i < enteredMachineCode.Length; i++)
            {
                string currentLine = enteredMachineCode[i];
                if (string.IsNullOrEmpty(currentLine.Replace(" ", "")))
                {
                    easyCode += "\r\n";
                }
                else
                {
                    bool isNumber = int.TryParse(currentLine, out int command);
                    easyCode += (isNumber ? GetEasyReadableCommand(command) : "ERROR") + "\r\n";
                }
            }
            return easyCode;
        }
    }
}

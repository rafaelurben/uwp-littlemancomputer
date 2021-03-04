using System;
using Windows.UI.Xaml.Controls;

namespace LittleManComputer
{
    internal class LittleManInterpreter
    {
        private int[] memory;
        private int accumulator;
        private int memoryPosition;
        private int inputPosition;

        private bool finished = false;
        private bool error = false;

        private string output = "";

        private readonly string[] enteredMachineCode;
        private readonly string[] enteredInput;

        public LittleManInterpreter(string[] enteredMachineCode, string[] enteredInput)
        {
            this.enteredMachineCode = enteredMachineCode;
            this.enteredInput = enteredInput;
        }

        // GUI Helpers

        public string GetLineNumbers()
        {
            string lineNumbers = "";
            int lineNumber = 0;
            for (int i = 0; i < enteredMachineCode.Length; i++)
            {
                string currentLine = enteredMachineCode[i];
                if (string.IsNullOrEmpty(currentLine.Replace(" ", "")))
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
                return "SUB " + (command - 200).ToString();
            }
            if (300 <= command && command < 400)
            {
                return "STA " + (command - 300).ToString();
            }
            if (500 <= command && command < 600)
            {
                return "LDA " + (command - 500).ToString();
            }
            if (600 <= command && command < 700)
            {
                return "BRA " + (command - 600).ToString();
            }
            if (700 <= command && command < 800)
            {
                return "BRZ " + (command - 700).ToString();
            }
            if (800 <= command && command < 900)
            {
                return "BRP " + (command - 800).ToString();
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

        public string GetMemory()
        {
            string str = "";
            for (int i = 0; i < memory.Length; i++)
            {
                if (memoryPosition == i)
                {
                    str += "[" + i.ToString().PadLeft(2, '0') + "] " + memory[i].ToString().PadLeft(3, '0') + " <--\r\n";
                }
                else
                {
                    str += "[" + i.ToString().PadLeft(2, '0') + "] " + memory[i].ToString().PadLeft(3, '0') + "\r\n";
                }
            }
            return str;
        }

        public string GetAccumulator()
        {
            return accumulator.ToString();
        }

        public string GetOutput()
        {
            return output;
        }

        public bool IsFinished()
        {
            return finished;
        }

        // In and output

        private void Output(string text)
        {
            output += text + "\r\n";
        }

        private int Input()
        {
            try
            {
                if (enteredInput.Length <= inputPosition)
                {
                    throw new Exception("No more input available!");
                }

                string currentLine = enteredInput[inputPosition];
                inputPosition++;

                if (int.TryParse(currentLine, out int number))
                {
                    return number;
                }
                else
                {
                    throw new Exception("Input is not a number!");
                }
            }
            catch (Exception e)
            {
                finished = false;
                error = true;
                Output("ERROR (Invalid Input)");

                try
                {
                    ContentDialog dialog = new ContentDialog
                    {
                        Title = "FATAL: Unable to load input!",
                        Content = "The following error occurred while trying to read the input: " + e.Message,
                        CloseButtonText = "Close"
                    };

                    _ = dialog.ShowAsync();
                }
                catch (Exception)
                {
                }
                return 0;
            }
        }

        // Interpreter

        public void LoadIntoMemory()
        {
            memory = new int[100];
            accumulator = 0;
            memoryPosition = 0;
            inputPosition = 0;

            finished = false;
            error = false;

            int pos = 0;
            for (int i = 0; i < enteredMachineCode.Length; i++)
            {
                string currentLine = enteredMachineCode[i];
                if (!(string.IsNullOrEmpty(currentLine.Replace(" ", ""))))
                {
                    if (int.TryParse(currentLine, out int number))
                    {
                        memory[pos] = number;
                        pos++;
                    }
                    else
                    {
                        finished = true;
                        error = true;
                        Output("ERROR (NaN)");

                        try
                        {
                            ContentDialog dialog = new ContentDialog
                            {
                                Title = "FATAL: Not a number",
                                Content = "Only numbers can be loaded into memory!",
                                CloseButtonText = "Close"
                            };

                            _ = dialog.ShowAsync();
                        }
                        catch (Exception)
                        {
                        }
                        return;
                    }
                }
            }
        }

        public void RunStep()
        {
            if (!finished && !error)
            {
                int command = memory[memoryPosition];
                memoryPosition++;

                if (100 <= command && command < 200)
                {
                    accumulator += memory[command - 100];
                }
                else if (200 <= command && command < 300)
                {
                    accumulator -= memory[command - 200];
                }
                else if (300 <= command && command < 400)
                {
                    memory[command - 300] = accumulator;
                }
                else if (500 <= command && command < 600)
                {
                    accumulator = memory[command - 500];
                }
                else if (600 <= command && command < 700)
                {
                    memoryPosition = command - 600;
                }
                else if (700 <= command && command < 800)
                {
                    if (accumulator == 0)
                    {
                        memoryPosition = command - 700;
                    }
                }
                else if (800 <= command && command < 900)
                {
                    if (accumulator >= 0)
                    {
                        memoryPosition = command - 800;
                    }
                }
                else if (command == 901)
                {
                    accumulator = Input();
                }
                else if (command == 902)
                {
                    Output(accumulator.ToString());
                }
                else if (command == 0)
                {
                    finished = true;
                }
                else
                {
                    finished = true;
                    error = true;
                    Output("ERROR (Unknown Command)");

                    try
                    {
                        ContentDialog dialog = new ContentDialog
                        {
                            Title = "Unknown command",
                            Content = "Unknown command at memory position " + memoryPosition.ToString() + ": " + command.ToString(),
                            CloseButtonText = "Ok"
                        };

                        _ = dialog.ShowAsync();
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }

        public string Run()
        {
            LoadIntoMemory();

            while (!finished && !error)
            {
                RunStep();
            }
            if (!error)
            {
                Output("FINISHED");
            }
            return output;
        }
    }
}
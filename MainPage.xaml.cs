using System.Text.RegularExpressions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace LittleManComputer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private LittleManInterpreter interpreter = new LittleManInterpreter(new string[0], new string[0]);

        public MainPage()
        {
            this.InitializeComponent();
        }

        // Text updated

        private void Input_MachineCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            interpreter = new LittleManInterpreter(
                Regex.Split(Input_MachineCode.Text, "\r\n|\r|\n"),
                Regex.Split(Input_Input.Text, "\r\n|\r|\n")
            );

            Output_LineNumbers.Text = interpreter.GetLineNumbers();
            Output_MachineCodeEasy.Text = interpreter.GetEasyReadableCode();
        }

        private void Input_Input_TextChanged(object sender, TextChangedEventArgs e)
        {
            interpreter = new LittleManInterpreter(
                Regex.Split(Input_MachineCode.Text, "\r\n|\r|\n"),
                Regex.Split(Input_Input.Text, "\r\n|\r|\n")
            );
            ClearDisplay();
        }

        // Helper functions

        private void UpdateDisplay()
        {
            string output = interpreter.GetOutput();
            Output_Output.Text = output;
            string memory = interpreter.GetMemory();
            Output_Memory.Text = memory;
            string accumulator = interpreter.GetAccumulator();
            Output_Accumulator.Text = accumulator;
        }

        private void ClearDisplay()
        {
            Output_Output.Text = "";
            Output_Memory.Text = "";
            Output_Accumulator.Text = "";
        }

        // Button events

        private void Go_Click(object sender, RoutedEventArgs e)
        {
            interpreter.Run();
            UpdateDisplay();
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            interpreter.LoadIntoMemory();
            UpdateDisplay();
        }

        private void Step_Click(object sender, RoutedEventArgs e)
        {
            interpreter.RunStep();
            UpdateDisplay();
        }
    }
}
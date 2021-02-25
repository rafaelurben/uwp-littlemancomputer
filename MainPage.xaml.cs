using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace LittleManComputer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Input_MachineCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            LittleManInterpreter interpreter = new LittleManInterpreter(
                Regex.Split(Input_MachineCode.Text, "\r\n|\r|\n"),
                Regex.Split(Input_Input.Text, "\r\n|\r|\n")
            );

            Output_LineNumbers.Text = interpreter.GetLineNumbers();
            Output_MachineCodeEasy.Text = interpreter.GetEasyReadableCode();
        }

        private void Go_Click(object sender, RoutedEventArgs e)
        {
            LittleManInterpreter interpreter = new LittleManInterpreter(
               Regex.Split(Input_MachineCode.Text, "\r\n|\r|\n"),
               Regex.Split(Input_Input.Text, "\r\n|\r|\n")
            );
            string output = interpreter.Run();
            Output_Output.Text = output;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Win32;
using StreamCipher.Core;

namespace StreamCipher
{
    public class ApplicationViewModel : BaseViewModel
    {

        public string MessageText { get; set; }

        public string GeneratedKeyText { get; set; }

        public string ResultText { get; set; }

        public string InitialStateText { get; set; }

        public int State { get; set; } = 0;
        
        private string ChosenFile { get; set; }

        public string StateText { get; set; } = "Waiting for an action...";



        public ICommand DoWorkCommand { get { return new RelayCommand(DoWork); } }

        public ICommand ChooseFileCommand { get { return new RelayCommand(ChooseFile); } }



        private void ChooseFile()
        {
            OpenFileDialog file = new OpenFileDialog();

            file.ShowDialog();

            if (string.IsNullOrEmpty(file.FileName))
            {
                State = (int)CurrentAppState.LastOperationFailed;
                StateText = "There was a problem loading that file!";
                return;
            }

            ChosenFile = file.FileName;

        }

        private async void DoWork()
        {
            if(string.IsNullOrWhiteSpace(ChosenFile))
            {
                State = (int)CurrentAppState.LastOperationFailed;
                StateText = "No file is chosen!";
                return;
            }



            BitArray Message = new BitArray(File.ReadAllBytes(ChosenFile));

            var generator = new LFSR();

            if(!generator.Initialize(InitialStateText, Message.Count))
            {
                State = (int)CurrentAppState.LastOperationFailed;
                StateText = "Wrong initial state length!";
                return;
            }

            MessageText = BitArrayToString(Message);

            State = (int)CurrentAppState.Working;
            StateText = "Generating a key using LFSR...";

            var Key = new BitArray(Message.Length);

            await Task.Run(() => Key = generator.GenerateKey());

            GeneratedKeyText = BitArrayToString(Key);

            await Task.Run(() => ResultText = BitArrayToString(Message.Xor(Key)));

            State = (int)CurrentAppState.LastOperationCompleted;
            StateText = "Operation completed!";

        }

        public string BitArrayToString(BitArray array)
        {
            var builder = new StringBuilder();

            foreach (var bit in array.Cast<bool>())
                builder.Append(bit ? "1" : "0");

            return builder.ToString();
        }
    }
}

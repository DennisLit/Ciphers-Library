using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Win32;

namespace StreamCipher.Core
{
    /// <summary>
    /// Program to understand the basics of LFSR and Stream cipher. 
    /// All algorithms there may be unoptimized in sacrifise of understandability
    /// </summary>
    public class ApplicationViewModel : BaseViewModel
    {
        #region Public fields

        public string MessageText { get; set; }

        public string GeneratedKeyText { get; set; }

        public string ResultText { get; set; }

        public string InitialStateText { get; set; }

        public int State { get; set; } = 0;

        public bool IsNoActionRunning { get; set; } = true;

        public string StateText { get; set; } = "Waiting for an action...";

        #endregion

        #region Private fields

        private string ChosenFile { get; set; }

        #endregion

        #region Commands

        public ICommand DoWorkCommand { get { return new RelayCommand(DoWork, () => IsNoActionRunning); } }

        public ICommand ChooseFileCommand { get { return new RelayCommand(ChooseFile, () => IsNoActionRunning); } }

        #endregion

        #region Public methods
        /// <summary>
        /// Choosing the file
        /// </summary>
        public void ChooseFile()
        {
            try
            {
                IsNoActionRunning = false;

                OpenFileDialog file = new OpenFileDialog();

                file.ShowDialog();

                if (string.IsNullOrEmpty(file.FileName))
                {
                    State = (int)CurrentAppState.LastOperationFailed;
                    StateText = "There was a problem loading that file!";
                    return;
                }

                StateText = "Successfully opened.";
                State = (int)CurrentAppState.LastOperationCompleted;

                ChosenFile = file.FileName;
            }
            finally { IsNoActionRunning = true; }


        }

        /// <summary>
        /// Main method
        /// </summary>
        public async void DoWork()
        {
            try
            {
                IsNoActionRunning = false;

                if (string.IsNullOrWhiteSpace(ChosenFile))
                {
                    State = (int)CurrentAppState.LastOperationFailed;
                    StateText = "No file is chosen!";
                    return;
                }

                var generator = new LFSR();

                try
                {
                    // Read bytes from file and convert to the binary

                    var Message = await Task.Run(() => File.ReadAllBytes(ChosenFile));

                    if (!generator.Initialize(InitialStateText, Message.Length))
                    {
                        State = (int)CurrentAppState.LastOperationFailed;
                        StateText = "Wrong initial state length!";
                        return;
                    }

                    State = (int)CurrentAppState.Working;
                    StateText = "Generating a key using LFSR...";

                    var Key = new byte[Message.Length];

                    await Task.Run(() => Key = generator.GenerateKey());

                    var Result = await Task.Run(() => MainStreamCipher.EncryptDecrypt(Message, Key));

                    MessageText = GetFirst16Bits(Message);

                    GeneratedKeyText = await Task.Run(() => GetFirst16Bits(Key));

                    var OutputFile = ChosenFile.Substring(0, ChosenFile.IndexOf(".") + 1);
                    OutputFile = OutputFile.Insert(OutputFile.Length - 1, ".Encrypted");

                    ResultText = await Task.Run(() => GetFirst16Bits(Result));

                    State = (int)CurrentAppState.Working;
                    StateText = "Writing bytes to file!";

                    await Task.Run(() => File.WriteAllBytes(OutputFile, Result));

                    State = (int)CurrentAppState.LastOperationCompleted;
                    StateText = "Operation completed!";

                }
                catch(Exception ex)
                {
                    StateText = ex.Message;
                    return;
                }

            }

            finally { IsNoActionRunning = true; }
            
        }

        #endregion

        #region Helper methods
        /// <summary>
        /// Returns a string containing 16 bits
        /// of first 2 elements of the array
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        private string GetFirst16Bits(byte[] array)
        {
            var builder = new StringBuilder();

            for(int i = 0; i < 2; ++i)
            {
                builder.Append(Convert.ToString(array[i], 2).PadLeft(8,'0'));
            }

            return builder.ToString();
        }


        #endregion

    }
}

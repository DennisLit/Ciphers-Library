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

                //Initialize to check if all requerements met

                if (!generator.Initialize(InitialStateText, new FileInfo(ChosenFile).Length))
                {
                    State = (int)CurrentAppState.LastOperationFailed;
                    StateText = "Wrong initial state length / wrong input!";
                    return;
                }

                try
                {
                    //use 16 MB because it equals to LFSR period

                    var isOutputIteration = true;
                    var KeyBuf = new byte[1024 * 1024 * 16];
                    var MessageBuf = new byte[1024 * 1024 * 16];
                    var OutputBuf = new byte[1024 * 1024 * 16];

                    State = (int)CurrentAppState.Working;
                    StateText = "Working...";

                    //re-initialize to buffer size

                    generator.Initialize(InitialStateText, 1024 * 1024 * 16);

                    //key buffer

                    KeyBuf = await Task.Run(() => generator.GenerateKey());

                    using (var OutFStream = new FileStream(GetOutputPath(ChosenFile), FileMode.Create))
                    // Read bytes from file and convert to the binary
                    using (var secBinStream = new BinaryWriter(OutFStream))
                    {
                        using (var FsStream = new FileStream(ChosenFile, FileMode.Open))
                        using (var binStream = new BinaryReader(FsStream))
                        {
                            //read file till the end
                            while(binStream.BaseStream.Position != binStream.BaseStream.Length)
                            {
                                //message buffer

                                MessageBuf = await Task.Run(() => binStream.ReadBytes(1024 * 1024 * 16)); // 16 MB

                                //use same keyBuf because period 
                                //of our LFSR equals to 2^m where m is highest power in polynom

                                OutputBuf = await Task.Run(() => MainStreamCipher.EncryptDecrypt(MessageBuf, KeyBuf));

                                await Task.Run(() => secBinStream.Write(OutputBuf));

                                //Used to output data to user 
                                //(only on first iteration to prevent using large amounts of RAM )
                                if(isOutputIteration)
                                {
                                    MessageText = GetFirst8Bytes(MessageBuf);

                                    GeneratedKeyText = GetFirst8Bytes(KeyBuf);

                                    ResultText = GetFirst8Bytes(OutputBuf);
                                }

                                isOutputIteration = false;
                            }
                        }
                    }

                    GC.Collect();

                    State = (int)CurrentAppState.LastOperationCompleted;
                    StateText = "Operation completed!";

                }
                catch (OutOfMemoryException ex)
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
        /// Returns a string containing 8 bytes
        /// in bit representation of an array
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        private string GetFirst8Bytes(byte[] array)
        { 
            var builder = new StringBuilder();

            for(int i = 0; (i < 8) && (i < array.Length); ++i)
            {
                builder.Append(Convert.ToString(array[i], 2).PadLeft(8,'0'));
            }

            return builder.ToString();
        }

        /// <summary>
        /// Gets string with any extension; 
        /// Returns a string with .encrypted extension
        /// </summary>
        /// <param name="InputPath"></param>
        /// <returns></returns>
        private string GetOutputPath(string InputPath)
        {
            return InputPath.Insert(InputPath.LastIndexOf('.'), "Encrypted");
        } 


        #endregion

    }
}

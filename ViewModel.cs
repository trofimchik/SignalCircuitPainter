using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using StringToBinaryConverter;
using DrawingDigitalCodingOnCanvas;

namespace SignalCircuitPainter
{
    class ViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<LineByPoints> LinesCoordinates { get; set; } = BipolarCodeAMI.GetAllLinesCoordinates("10011001 1010011100011010110101");
        public ViewModel()
        {
            
            //if(test != null)
            //MessageBox.Show(test.Count.ToString() + " and " + test[2].First.X.ToString() + " " + test[2].First.Y.ToString());
        }
        //public Func<string, Encoding, string> GetEncodedString = BinaryConverterLogic.GetBinaryString;
        public Encoding EncodingProp { get; set; } = Encoding.UTF8;

        private IDelegateCommand setEncoding;
        public IDelegateCommand SetEncoding
        {
            get
            {
                return setEncoding ??= new DelegateCommand(
                   parameter =>
                   {
                       string encoding = parameter as string;
                       switch (encoding)
                       {

                           case "UTF7":
                               EncodingProp = Encoding.UTF7;
                               break;
                           case "UTF8":
                               EncodingProp = Encoding.UTF8;
                               break;
                           case "UTF16":
                               EncodingProp = Encoding.Unicode;
                               break;
                           case "UTF32":
                               EncodingProp = Encoding.UTF32;
                               break;
                           case "ASCII":
                               EncodingProp = Encoding.ASCII;
                               break;
                           default:
                               break;
                       }
                       OutputEncoding = BinaryConverterLogic.GetBinaryString(InputEncoding, EncodingProp);
                       OnPropertyChanged("OutputEncoding");
                   });
            }
        }

        private string inputEncoding;
        public string InputEncoding
        {
            private get { return inputEncoding; }
            set
            {
                inputEncoding = value;
                OutputEncoding = BinaryConverterLogic.GetBinaryString(InputEncoding, EncodingProp);
                OnPropertyChanged("OutputEncoding");
            }
        }

        public string OutputEncoding { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        
    }
}

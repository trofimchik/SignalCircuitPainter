using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using StringToBinaryConverter;
namespace SignalCircuitPainter
{
    class ViewModel : INotifyPropertyChanged
    {
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
            get { return inputEncoding; }
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

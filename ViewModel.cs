using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Drawing;
using StringToBinaryConverter;
using DrawingDigitalCodingOnCanvas;

namespace SignalCircuitPainter
{
    class ViewModel : INotifyPropertyChanged
    {
        public ViewModel()
        {
        }

        public ObservableCollection<LineByPoints> Layout { get; set; } = new ObservableCollection<LineByPoints>();
        public ObservableCollection<LineByPoints> LinesCoordinates { get; set; }
        Func<string, ObservableCollection<LineByPoints>> SignalProp { get; set; } = BipolarCodeAMI.GetAllLinesCoordinates;

        private IDelegateCommand setSignalType;
        public IDelegateCommand SetSignalType
        {
            get
            {
                return setSignalType ??= new DelegateCommand(
                   parameter =>
                   {
                       string type = parameter as string;
                       switch (type)
                       {
                           case "NRZ":
                               SignalProp = PotentialCodeNRZ.GetAllLinesCoordinates;
                               break;
                           case "AMI":
                               SignalProp = BipolarCodeAMI.GetAllLinesCoordinates;
                               break;
                           case "BipolarPulse":
                               SignalProp = BipolarPulseCode.GetAllLinesCoordinates;
                               break;
                           case "Manchester":
                               SignalProp = ManchesterCode.GetAllLinesCoordinates;
                               break;
                           case "2B1Q":
                               break;
                           default:
                               break;
                       }
                       if (InputEncoding != null) RefreshChanges();
                   });
            }
        }

        public char[] OutputEncodingCharArray { get; set; }
        public int CanvasWidth { get; set; }
        public int CanvasHeight { get; set; }

        public string OutputEncoding { get; set; }
        public Encoding EncodingProp { get; set; } = Encoding.UTF8;
        private string inputEncoding;
        public string InputEncoding
        {
            private get { return inputEncoding; }
            set
            {
                inputEncoding = value;

                RefreshChanges();
            }
        }

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
                       if (InputEncoding != null) RefreshChanges();
                   });
            }
        }

        private void RefreshChanges()
        {
            OutputEncoding = BinaryConverterLogic.GetBinaryString(InputEncoding, EncodingProp);

            OutputEncodingCharArray = OutputEncoding.Replace(" ", string.Empty).ToCharArray();

            LinesCoordinates = SignalProp(OutputEncoding);

            CanvasWidth = OutputEncodingCharArray.Length * Overall.HorizontalLineSize;
            CanvasHeight = Overall.VerticalLineSize * 4;

            CreateLayoutCoordinates();

            OnPropertyChanged("OutputEncoding");
            OnPropertyChanged("OutputEncodingCharArray");
            OnPropertyChanged("LinesCoordinates");
            OnPropertyChanged("CanvasWidth");
            OnPropertyChanged("CanvasHeight");
        }

        private void CreateLayoutCoordinates()
        {
            int temp = 0;
            foreach (char c in OutputEncoding.Replace(" ", string.Empty))
            {
                LinesCoordinates.Add(new LineByPoints(new System.Drawing.Point(temp, 0), new System.Drawing.Point(temp, CanvasHeight), 0.5));
                temp += Overall.HorizontalLineSize;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}

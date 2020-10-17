﻿using Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptographyLabs.GUI
{
    class Task7ViewModel : BaseViewModel
    {
        private string _a = "";
        public string A
        {
            get => _a;
            set
            {
                _a = value;
                NotifyPropertyChanged(nameof(A));
                Apply();
            }
        }

        private string _len = "";
        public string Len
        {
            get => _len;
            set
            {
                _len = value;
                NotifyPropertyChanged(nameof(Len));
                Apply();
            }
        }

        private string _n = "";
        public string N
        {
            get => _n;
            set
            {
                _n = value;
                NotifyPropertyChanged(nameof(N));
                Apply();
            }
        }

        private string _result = "";
        public string Result
        {
            get => _result;
            set
            {
                _result = value;
                NotifyPropertyChanged(nameof(Result));
            }
        }

        private void Apply()
        {
            if (Extended.TryParse(A, out uint a) && byte.TryParse(Len, out byte len) && byte.TryParse(N, out byte n))
            {
                Result = "0b" + Convert.ToString(Bitops.CycleShiftLeft(a, len, n), 2);
            }
            else
                Result = "-";
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CryptographyLabs.GUI
{
    public abstract class BaseTransformVM : BaseViewModel
    {
        protected CancellationTokenSource _cts = new CancellationTokenSource();

        #region Bindings

        private long _startTime = DateTime.Now.Ticks;
        public long StartTime => _startTime;

        private string _sourceFilePath = "";
        public string SourceFilePath
        {
            get => _sourceFilePath;
            set
            {
                _sourceFilePath = value;
                NotifyPropChanged(nameof(SourceFilePath));
            }
        }

        private string _destFilePath;
        public string DestFilePath
        {
            get => _destFilePath;
            set
            {
                _destFilePath = value;
                NotifyPropChanged(nameof(DestFilePath));
            }
        }

        private string _statusString = "aga";
        public string StatusString
        {
            get => _statusString;
            set
            {
                _statusString = value;
                NotifyPropChanged(nameof(StatusString));
            }
        }

        private double _cryptoProgress = 0;
        public double CryptoProgress
        {
            get => _cryptoProgress;
            set
            {
                if (value > 100)
                    _cryptoProgress = 100;
                else if (value < 0)
                    _cryptoProgress = 0;
                else
                    _cryptoProgress = value;
                NotifyPropChanged(nameof(CryptoProgress));
            }
        }

        private bool _isDone = false;
        public bool IsDone
        {
            get => _isDone;
            set
            {
                _isDone = value;
                NotifyPropChanged(nameof(IsDone));
            }
        }

        private string _cryptoName = "";
        public string CryptoName
        {
            get => _cryptoName;
            set
            {
                _cryptoName = value;
                NotifyPropChanged(nameof(CryptoName));
            }
        }

        private RelayCommand _cancelCmd;
        public RelayCommand CancelCmd
            => _cancelCmd ?? (_cancelCmd = new RelayCommand(_ => Cancel()));

        #endregion

        private bool _isDeleteAfter;

        public BaseTransformVM(bool isDeleteAfter)
        {
            _isDeleteAfter = isDeleteAfter;
        }

        protected async void Start()
        {
            try
            {
                await Process();
                if (_isDeleteAfter)
                {
                    StatusString = "Deleting file";
                    await Task.Run(() => File.Delete(SourceFilePath));
                }
                StatusString = "Done successfully";
            }
            catch (OperationCanceledException)
            {
                Reject();
                StatusString = "Canceled";
            }
            catch (Exception e)
            {
                Reject();
                StatusString = "Error: " + e.Message;
            }

            IsDone = true;
        }

        private void Cancel()
        {
            _cts.Cancel();
        }

        protected virtual void Reject()
        {
            try
            {
                if (File.Exists(DestFilePath))
                    File.Delete(DestFilePath);
            }
            catch { }
        }

        protected abstract Task Process();
    }
}
﻿using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Model.Kinect
{
    public abstract class KinectStream : INotifyPropertyChanged
    {

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        #region Property
        public KinectManager KinectManager 
        {
            get => _kinectManager; 
            protected set
            {
                if (_kinectManager != value)
                {
                    _kinectManager = value;
                    OnPropertyChanged();
                }
            }
        }
        private KinectManager _kinectManager = null;
        public ImageSource Bitmap 
        { 
            get => _bitmap;
            set
            {
                _bitmap = (WriteableBitmap)value;
                OnPropertyChanged();
            }
        }
        protected WriteableBitmap _bitmap = null;

        public Canvas Canva
        {
            get => _canva;
            set
            {
                _canva = value;
                OnPropertyChanged();
            }
        }
        protected Canvas _canva = null;

        #endregion

        public abstract void Start();
        public abstract void Stop();
        
    }
}

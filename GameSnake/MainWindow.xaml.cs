﻿using GameSnake.ViewModels;
using System.Windows;

namespace GameSnake
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainVM(this);
        }
    }
}

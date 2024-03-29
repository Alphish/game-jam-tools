﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Alphicsh.JamPlayer.Controls.Ranking
{
    /// <summary>
    /// Interaction logic for RankingList.xaml
    /// </summary>
    public partial class RankingList : UserControl
    {
        public RankingList()
        {
            InitializeComponent();
        }

        // ---------------------
        // Dependency properties
        // ---------------------

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
            nameof(Header), typeof(string), typeof(RankingList), new PropertyMetadata(defaultValue: string.Empty)
            );

        public string Header
        {
            get => (string)GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }
    }
}

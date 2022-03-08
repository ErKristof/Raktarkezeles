﻿using Raktarkezeles.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Raktarkezeles.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewPartPage : ContentPage
    {
        public NewPartPage()
        {
            InitializeComponent();
            this.BindingContext = new NewPartViewModel(Navigation);
        }
    }
}
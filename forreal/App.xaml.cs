﻿
using forreal.Models;
using forreal.ViewModels;

namespace forreal
{
    public partial class App : Application
    {
        private bool showFlyout;
        private AppShellViewModel shellViewModel;
        public bool ShowFlyouts { get => showFlyout; set { showFlyout = value; shellViewModel.Visible = value; } }
        public bool StartTimer { get; set; }
        public User User { get; set; }
        public TimeSpan Timeleft { get; set; }
        public bool ShowFlyouts2 { get { if (showFlyout)return false; return true; } set { shellViewModel.AntiVisible = value; } }
        public App(AppShellViewModel vm)
        {
            InitializeComponent();
            shellViewModel = vm;
            User = null;
            StartTimer = true;
            Timeleft=TimeSpan.Zero;
            MainPage = new AppShell(vm);
        }
    }
}
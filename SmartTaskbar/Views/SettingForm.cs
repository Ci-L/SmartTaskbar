﻿using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using System.Windows.Forms;
using ReactiveUI;
using SmartTaskbar.Core;
using SmartTaskbar.Core.Settings;

namespace SmartTaskbar.Views
{
    public partial class SettingForm : Form, IViewFor<AppViewModel>
    {
        private static readonly Lazy<SettingForm> LazyInstance =
            new Lazy<SettingForm>(() => new SettingForm(), LazyThreadSafetyMode.ExecutionAndPublication);

        public static SettingForm Instance = LazyInstance.Value;

        private SettingForm()
        {
            InitializeComponent();
            FormClosing += (sender, args) =>
            {
                if (args.CloseReason != CloseReason.UserClosing) return;

                args.Cancel = true;
                Hide();
            };

            #region Reactive Binding

            ViewModel = AppViewModel.Instance;

            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel,
                        viewModel => viewModel.IconStyle,
                        view => view.Icon,
                        vmToViewConverterOverride: new IconStyleIconConverter())
                    .DisposeWith(disposables);

                this.OneWayBind(ViewModel, model => model.SettingMode,
                        view => view.groupBox_AutoMode.Text)
                    .DisposeWith(disposables);

                this.OneWayBind(ViewModel, model => model.SettingDisable,
                        view => view.radioButtonDisableMode.Text)
                    .DisposeWith(disposables);

                this.OneWayBind(ViewModel, model => model.SettingForegroundMode,
                        view => view.radioButtonForegroundMode.Text)
                    .DisposeWith(disposables);

                this.OneWayBind(ViewModel, model => model.SettingBlacklistMode,
                        view => view.radioButtonBlacklistMode.Text)
                    .DisposeWith(disposables);

                this.OneWayBind(ViewModel, model => model.SettingWhitelistMode,
                        view => view.radioButtonWhitelistMode.Text)
                    .DisposeWith(disposables);

                this.WhenAnyValue(x => x.radioButtonDisableMode.Checked)
                    .Where(x => x)
                    .Subscribe(_ => InvokeMethods.AutoModeSet(AutoModeType.Disable));

                this.WhenAnyValue(x => x.radioButtonForegroundMode.Checked)
                    .Where(x => x)
                    .Subscribe(_ => InvokeMethods.AutoModeSet(AutoModeType.ForegroundMode));

                this.WhenAnyValue(x => x.radioButtonBlacklistMode.Checked)
                    .Where(x => x)
                    .Subscribe(_ => InvokeMethods.AutoModeSet(AutoModeType.BlacklistMode));

                this.WhenAnyValue(x => x.radioButtonWhitelistMode.Checked)
                    .Where(x => x)
                    .Subscribe(_ => InvokeMethods.AutoModeSet(AutoModeType.WhitelistMode));
            });

            #endregion
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (AppViewModel)value;
        }

        public AppViewModel ViewModel { get; set; }
    }
}
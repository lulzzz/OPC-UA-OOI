﻿//___________________________________________________________________________________
//
//  Copyright (C) 2018, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community at GITTER: https://gitter.im/mpostol/OPC-UA-OOI
//___________________________________________________________________________________

using System;
using System.ComponentModel.Composition;
using System.Windows.Input;
using UAOOI.Networking.ReferenceApplication.Core.MvvmLight;

namespace UAOOI.Networking.ReferenceApplication.Core
{

  /// <summary>
  /// class ProducerViewModel - defines a ViewModel part to be used by the producer to expose diagnostic information on the UI.
  /// </summary>
  [Export()]
  [PartCreationPolicy(CreationPolicy.Shared)]
  public class ProducerViewModel : ObservableObject
  {

    #region API
    /// <summary>
    /// Initializes a new instance of the <see cref="ProducerViewModel"/> class.
    /// </summary>
    public ProducerViewModel()
    {
      ProducerRestartCommand = new DelegateCommand(() => { });
    }
    /// <summary>
    /// Gets or sets the producer error message.
    /// </summary>
    /// <value>The producer error message.</value>
    public string ProducerErrorMessage
    {
      get
      {
        return b_ProducerErrorMessage;
      }
      set
      {
        b_ProducerErrorMessage = value;
        RaisePropertyChanged<string>("ProducerErrorMessage", b_ProducerErrorMessage, value);
      }
    }
    /// <summary>
    /// Gets or sets the producer restart command. While assigned by 
    /// </summary>
    /// <value>The producer restart command.</value>
    public ICommand ProducerRestartCommand
    {
      get
      {
        return b_ProducerRestartCommand;
      }
      set
      {
        b_ProducerRestartCommand = value;
        RaisePropertyChanged<ICommand>("ProducerRestartCommand", b_ProducerRestartCommand, value);
      }
    }
    /// <summary>
    /// Changes the producer command.
    /// </summary>
    /// <param name="action">The action to be executed in case of <see cref="ProducerViewModel.ProducerRestartCommand"/> event.</param>
    public void ChangeProducerCommand(Action action)
    {
      ProducerRestartCommand = new DelegateCommand(action);
    }
    #endregion

    #region private
    private string b_ProducerErrorMessage;
    private ICommand b_ProducerRestartCommand;
    #endregion

  }
}
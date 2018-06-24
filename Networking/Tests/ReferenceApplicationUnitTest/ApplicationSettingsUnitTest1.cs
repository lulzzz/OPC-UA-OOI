﻿using CommonServiceLocator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics;
using System.Linq;
using UAOOI.Networking.DataLogger;
using UAOOI.Networking.ReferenceApplication.MEF;
using UAOOI.Networking.SimulatorInteroperabilityTest;

namespace UAOOI.Networking.ReferenceApplication.UnitTest
{
  [TestClass]
  public class ApplicationSettingsUnitTest1
  {
    [TestMethod]
    public void ApplicationSettingsMEFCompositionMethod()
    {
      using (AggregateCatalog newCatalog = DefaultServiceRegistrar.RegisterServices(null))
      {
        using (CompositionContainer _container = new CompositionContainer(newCatalog))
        {
          foreach (ComposablePartDefinition _part in _container.Catalog.Parts)
            foreach (var export in _part.ExportDefinitions)
              Debug.WriteLine(string.Format("Part contract name => '{0}'", export.ContractName));
          Assert.AreEqual<int>(15, _container.Catalog.Parts.Count());
          string _ProducerConfigurationFileName = _container.GetExportedValue<string>(SimulatorCompositionSettings.ConfigurationFileNameContract);
          Assert.AreEqual<string>("ConfigurationDataProducer.xml", _ProducerConfigurationFileName, $"_ProducerConfigurationFileName = {_ProducerConfigurationFileName}");
          string _ConsumerConfigurationFileName = _container.GetExportedValue<string>(ConsumerCompositionSettings.ConfigurationFileNameContract);
          Assert.AreEqual<string>("ConfigurationDataConsumer.xml", _ConsumerConfigurationFileName, $"_ProducerConfigurationFileName = {_ConsumerConfigurationFileName}");
          ApplicationSettings _ApplicationSettings = _container.GetExportedValue<ApplicationSettings>();
          Assert.IsNotNull(_ApplicationSettings);
          ApplicationSettings _ApplicationSettings2 = _container.GetExportedValue<ApplicationSettings>();
          Assert.AreSame(_ApplicationSettings, _ApplicationSettings2);
        }
      }
    }
    [TestMethod]
    public void ApplicationSettingsISLCompositionMethod()
    {
      using (AggregateCatalog newCatalog = DefaultServiceRegistrar.RegisterServices(null))
      {
        using (CompositionContainer _container = new CompositionContainer(newCatalog))
        {
          IServiceLocator _serviceLocator = new ServiceLocatorAdapter(_container);
          ServiceLocator.SetLocatorProvider(() => _serviceLocator);
          Assert.IsNotNull(_serviceLocator);
          string _ProducerConfigurationFileName = _serviceLocator.GetInstance<string>(SimulatorCompositionSettings.ConfigurationFileNameContract);
          Assert.AreEqual<string>("ConfigurationDataProducer.xml", _ProducerConfigurationFileName, $"_ProducerConfigurationFileName = {_ProducerConfigurationFileName}");
          string _ConsumerConfigurationFileName = _serviceLocator.GetInstance<string>(ConsumerCompositionSettings.ConfigurationFileNameContract);
          Assert.AreEqual<string>("ConfigurationDataConsumer.xml", _ConsumerConfigurationFileName, $"_ProducerConfigurationFileName = {_ConsumerConfigurationFileName}");
          ApplicationSettings _ApplicationSettings = _serviceLocator.GetInstance<ApplicationSettings>();
          Assert.IsNotNull(_ApplicationSettings);
          ApplicationSettings _ApplicationSettings2 = _serviceLocator.GetInstance<ApplicationSettings>();
          Assert.AreSame(_ApplicationSettings, _ApplicationSettings2);
        }
      }
    }

  }
}

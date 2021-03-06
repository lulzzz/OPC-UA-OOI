﻿//___________________________________________________________________________________
//
//  Copyright (C) 2019, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community at GITTER: https://gitter.im/mpostol/OPC-UA-OOI
//___________________________________________________________________________________

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UAOOI.Configuration.Networking.Serialization;
using UAOOI.Networking.Core;
using UAOOI.Networking.SemanticData.DataRepository;
using UAOOI.Networking.SemanticData.MessageHandling;
using UAOOI.Networking.SemanticData.UnitTest.Helpers;

namespace UAOOI.Networking.SemanticData.UnitTest
{

  [TestClass]
  public class MessageWriterBaseTest
  {

    [TestMethod]
    [TestCategory("DataManagement_MessageWriter")]
    public void CreatorTestMethod1()
    {
      TypesMessageWriter _bmw = new TypesMessageWriter();
      Assert.IsNotNull(_bmw);
      _bmw.AttachToNetwork();
      Assert.IsTrue(_bmw.State.State == HandlerState.Operational);
    }
    [TestMethod]
    [TestCategory("DataManagement_MessageWriter")]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void ObjectTestMethod()
    {
      TypesMessageWriter _bmw = new TypesMessageWriter();
      _bmw.AttachToNetwork();
      ProducerBinding _binding = new ProducerBinding
      {
        Value = new TestClass()
      };
      ((IMessageWriter)_bmw).Send(x => _binding, 1, ulong.MaxValue, FieldEncodingEnum.VariantFieldEncoding, TestDataSelector, 0, DateTime.UtcNow,
        new ConfigurationVersionDataType() { MajorVersion = 0, MinorVersion = 0 });
    }
    [TestMethod]
    [TestCategory("DataManagement_MessageWriter")]
    [ExpectedException(typeof(NullReferenceException))]
    public void NullableTestMethod()
    {
      TypesMessageWriter _bmw = new TypesMessageWriter();
      _bmw.AttachToNetwork();
      Assert.IsTrue(_bmw.State.State == HandlerState.Operational);
      ProducerBinding _binding = new ProducerBinding(BuiltInType.Float)
      {
        Value = new Nullable<float>()
      };
      ((IMessageWriter)_bmw).Send(x => _binding, 1, ulong.MaxValue, FieldEncodingEnum.VariantFieldEncoding, TestDataSelector, 0, DateTime.UtcNow, new ConfigurationVersionDataType() { MajorVersion = 0, MinorVersion = 0 });
    }
    [TestMethod]
    [TestCategory("DataManagement_MessageWriter")]
    public void SendTestMethod()
    {
      TypesMessageWriter _bmw = new TypesMessageWriter();
      _bmw.AttachToNetwork();
      Assert.IsTrue(_bmw.State.State == HandlerState.Operational);
      ProducerBinding _binding = new ProducerBinding
      {
        Value = string.Empty
      };
      int _sentItems = 0;
      ((IMessageWriter)_bmw).Send((x) => { _binding.Value = CommonDefinitions.TestValues[x]; _sentItems++; return _binding; },
                                   Convert.ToUInt16(CommonDefinitions.TestValues.Length),
                                   ulong.MaxValue,
                                   FieldEncodingEnum.VariantFieldEncoding,
                                   TestDataSelector,
                                   0,
                                   DateTime.UtcNow,
                                   new ConfigurationVersionDataType() { MajorVersion = 0, MinorVersion = 0 }
                                   );
      Assert.AreEqual(CommonDefinitions.TestValues.Length, _sentItems);
    }

    private class TestClass { }
    private readonly DataSelector TestDataSelector = new DataSelector() { PublisherId = Guid.NewGuid(), DataSetWriterId = ushort.MaxValue };
    private class ProducerBinding : IProducerBinding
    {

      internal object Value;
      private readonly BuiltInType _builtInType;

      public ProducerBinding(BuiltInType builtInType)
      {
        _builtInType = builtInType;
      }
      public ProducerBinding() { }

      #region IProducerBinding
      public bool NewValue => true;
      public object GetFromRepository()
      {
        return Value;
      }
      public IValueConverter Converter
      {
        set => throw new NotImplementedException();
      }
      public UATypeInfo Encoding
      {
        get
        {
          if (Value == null)
            return new UATypeInfo(_builtInType);
          switch (Type.GetTypeCode(Value.GetType()))
          {
            case TypeCode.Boolean:
              return new UATypeInfo(BuiltInType.Boolean);
            case TypeCode.SByte:
              return new UATypeInfo(BuiltInType.SByte);
            case TypeCode.Byte:
              return new UATypeInfo(BuiltInType.Byte);
            case TypeCode.Int16:
              return new UATypeInfo(BuiltInType.Int16);
            case TypeCode.UInt16:
              return new UATypeInfo(BuiltInType.UInt16);
            case TypeCode.Int32:
              return new UATypeInfo(BuiltInType.Int32);
            case TypeCode.UInt32:
              return new UATypeInfo(BuiltInType.UInt32);
            case TypeCode.Int64:
              return new UATypeInfo(BuiltInType.Int64);
            case TypeCode.UInt64:
              return new UATypeInfo(BuiltInType.UInt64);
            case TypeCode.Single:
              return new UATypeInfo(BuiltInType.Float);
            case TypeCode.Double:
              return new UATypeInfo(BuiltInType.Double);
            case TypeCode.DateTime:
              return new UATypeInfo(BuiltInType.DateTime);
            case TypeCode.String:
              return new UATypeInfo(BuiltInType.String);
            default:
              throw new ArgumentOutOfRangeException(nameof(Value));
          }
          throw new ArgumentOutOfRangeException(nameof(Value));
        }
      }
      public object Parameter
      {
        get => null;
        set { }
      }
      public System.Globalization.CultureInfo Culture
      {
        set => throw new NotImplementedException();
      }

      public object FallbackValue { set => throw new NotImplementedException(); }

      public void OnEnabling()
      {
        throw new NotImplementedException();
      }
      public void OnDisabling()
      {
        throw new NotImplementedException();
      }
      public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
      #endregion

    }
    private class TypesMessageWriter : MessageWriterBase
    {

      #region creator
      public TypesMessageWriter() : base(new Helpers.UABinaryEncoderImplementation())
      {
        State = new MyState();
      }
      #endregion

      #region BinaryMessageWriter
      public override IAssociationState State
      {
        get;
        set;
      }
      public override void AttachToNetwork()
      {
        Assert.AreNotEqual<HandlerState>(HandlerState.Operational, State.State);
        State.Enable();
      }
      public override void Write(ulong value)
      {
        Assert.IsInstanceOfType(value, typeof(ulong));
      }
      public override void Write(uint value)
      {
        Assert.IsInstanceOfType(value, typeof(uint));
      }
      public override void Write(ushort value)
      {
        Assert.IsInstanceOfType(value, typeof(ushort));
      }
      public override void Write(float value)
      {
        Assert.IsInstanceOfType(value, typeof(float));
      }
      public override void Write(sbyte value)
      {
        Assert.IsInstanceOfType(value, typeof(sbyte));
      }
      public override void Write(long value)
      {
        Assert.IsInstanceOfType(value, typeof(long));
      }
      public override void Write(int value)
      {
        Assert.IsInstanceOfType(value, typeof(int));
      }
      public override void Write(short value)
      {
        Assert.IsInstanceOfType(value, typeof(short));
      }
      public override void Write(double value)
      {
        Assert.IsInstanceOfType(value, typeof(double));
      }
      public override void Write(byte value)
      {
        Assert.IsInstanceOfType(value, typeof(byte));
      }
      public override void Write(bool value)
      {
        Assert.IsInstanceOfType(value, typeof(bool));
      }
      public override void Write(byte[] value)
      {
        Assert.IsInstanceOfType(value, typeof(byte[]));
      }
      protected internal override void CreateMessage
        (FieldEncodingEnum encoding, Guid producerId, ushort dataSetWriterId, ushort fieldCount, ushort sequenceNumber, DateTime timeStamp, ConfigurationVersionDataType configurationVersion)
      {
        MassageCreated = true;
      }
      protected override void SendMessage() { }
      #endregion

      #region test infrastructure
      internal bool MassageCreated = false;
      #endregion

    }

  }
}

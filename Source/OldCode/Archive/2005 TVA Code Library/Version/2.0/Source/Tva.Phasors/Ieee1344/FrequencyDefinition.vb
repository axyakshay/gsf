'*******************************************************************************************************
'  FrequencyDefinition.vb - IEEE 1344 Frequency definition
'  Copyright � 2005 - TVA, all rights reserved - Gbtc
'
'  Build Environment: VB.NET, Visual Studio 2005
'  Primary Developer: James R Carroll, Operations Data Architecture [TVA]
'      Office: COO - TRNS/PWR ELEC SYS O, CHATTANOOGA, TN - MR 2W-C
'       Phone: 423/751-2827
'       Email: jrcarrol@tva.gov
'
'  Code Modification History:
'  -----------------------------------------------------------------------------------------------------
'  11/12/2004 - James R Carroll
'       Initial version of source generated
'
'*******************************************************************************************************

Imports Tva.Collections.Common
Imports Tva.Interop
Imports Tva.Interop.Bit

Namespace Ieee1344

    <CLSCompliant(False)> _
    Public Class FrequencyDefinition

        Inherits FrequencyDefinitionBase

        Private m_statusFlags As Int16

        Public Sub New(ByVal parent As ConfigurationCell)

            MyBase.New(parent)
            MyBase.DataFormat = Phasors.DataFormat.FixedInteger
            ScalingFactor = 1000
            DfDtScalingFactor = 100

        End Sub

        Public Sub New(ByVal parent As ConfigurationCell, ByVal binaryImage As Byte(), ByVal startIndex As Int32)

            MyBase.New(parent, binaryImage, startIndex)
            MyBase.DataFormat = Phasors.DataFormat.FixedInteger
            ScalingFactor = 1000
            DfDtScalingFactor = 100

        End Sub

        Public Sub New(ByVal parent As ConfigurationCell, ByVal dataFormat As DataFormat, ByVal index As Int32, ByVal label As String, ByVal scale As Int32, ByVal offset As Single, ByVal dfdtScale As Int32, ByVal dfdtOffset As Single)

            MyBase.New(parent, dataFormat, index, label, scale, offset, dfdtScale, dfdtOffset)

        End Sub

        Public Sub New(ByVal frequencyDefinition As IFrequencyDefinition)

            MyBase.New(frequencyDefinition)

        End Sub

        Friend Shared Function CreateNewFrequencyDefintion(ByVal parent As IConfigurationCell, ByVal binaryImage As Byte(), ByVal startIndex As Int32) As IFrequencyDefinition

            Return New FrequencyDefinition(parent, binaryImage, startIndex)

        End Function

        Public Overrides ReadOnly Property InheritedType() As System.Type
            Get
                Return Me.GetType
            End Get
        End Property

        Public Property FrequencyAvailable() As Boolean
            Get
                Return (m_statusFlags And Bit8) = 0
            End Get
            Set(ByVal value As Boolean)
                If value Then
                    m_statusFlags = m_statusFlags And Not Bit8
                Else
                    m_statusFlags = m_statusFlags Or Bit8
                End If
            End Set
        End Property

        Public Property DfDtAvailable() As Boolean
            Get
                Return (m_statusFlags And Bit9) = 0
            End Get
            Set(ByVal value As Boolean)
                If value Then
                    m_statusFlags = m_statusFlags And Not Bit9
                Else
                    m_statusFlags = m_statusFlags Or Bit9
                End If
            End Set
        End Property

        ' IEEE 1344 only supports scaled data
        Public Overrides Property DataFormat() As DataFormat
            Get
                Return Phasors.DataFormat.FixedInteger
            End Get
            Set(ByVal value As DataFormat)
                If value = Phasors.DataFormat.FixedInteger Then
                    MyBase.DataFormat = value
                Else
                    Throw New InvalidOperationException("IEEE 1344 only supports scaled integers - floating points are not allowed")
                End If
            End Set
        End Property

        Protected Overrides ReadOnly Property BodyLength() As UInt16
            Get
                Return 2
            End Get
        End Property

        Protected Overrides ReadOnly Property BodyImage() As Byte()
            Get
                If NominalFrequency = LineFrequency.Hz50 Then
                    m_statusFlags = m_statusFlags Or Bit0
                Else
                    m_statusFlags = m_statusFlags And Not Bit0
                End If

                Return EndianOrder.BigEndian.GetBytes(m_statusFlags)
            End Get
        End Property

        Protected Overrides Sub ParseBodyImage(ByVal state As IChannelParsingState, ByVal binaryImage() As Byte, ByVal startIndex As Int32)

            m_statusFlags = EndianOrder.BigEndian.ToInt16(binaryImage, startIndex)
            Parent.NominalFrequency = IIf((m_statusFlags And Bit0) > 0, LineFrequency.Hz50, LineFrequency.Hz60)

        End Sub

    End Class

End Namespace
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gurux.MBus.AddIn
{
	public class VariableDataVIF
	{
		public VariableDataVIF()
		{

		}

		public VariableDataVIF(byte messagePart)
		{
			Multiplier = 0;
			UnitAndMultiplierByte = (byte)(messagePart & 0x7F);
			ExtensionBit = (byte)((messagePart & 0x80) >> 7);
		}

		public byte UnitAndMultiplierByte
		{
			get
			{
				return (byte)(MultiplierBits + UnitInfo);
			}
			set
			{
				if ((value & (byte)VariableValueInformationType.ExtendedFB) == (byte)VariableValueInformationType.ExtendedFB)
				{
					UnitInfo = VariableValueInformationType.ExtendedFB;
					MultiplierBits = 0;
					Unit = string.Empty;
					Mask = 0xFF;
				}
				else if ((value & (byte)VariableValueInformationType.ExtendedFD) == (byte)VariableValueInformationType.ExtendedFD)
				{
					UnitInfo = VariableValueInformationType.ExtendedFD;
					MultiplierBits = 0;
					Unit = string.Empty;
					Mask = 0xFF;
				}
				else if ((value & (byte)VariableValueInformationType.BusAddress) == (byte)VariableValueInformationType.BusAddress)
				{
					UnitInfo = VariableValueInformationType.BusAddress;
					MultiplierBits = 0;
					Unit = string.Empty;
					Mask = 0xFF;
				}
				else if ((value & (byte)VariableValueInformationType.Enhanced) == (byte)VariableValueInformationType.Enhanced)
				{
					UnitInfo = VariableValueInformationType.Enhanced;
					MultiplierBits = 0;
					Unit = string.Empty;
					Mask = 0xFF;
				}
				else if ((value & (byte)VariableValueInformationType.FabricationNumber) == (byte)VariableValueInformationType.FabricationNumber)
				{
					UnitInfo = VariableValueInformationType.FabricationNumber;
					MultiplierBits = 0;
					Unit = string.Empty;
					Mask = 0xFF;
				}
				else if ((value & (byte)VariableValueInformationType.ActualityDuration) == (byte)VariableValueInformationType.ActualityDuration)
				{
					UnitInfo = VariableValueInformationType.ActualityDuration;
					MultiplierBits = (byte)(value & 0x03);
					Unit = GetTimeUnit(MultiplierBits);
					Mask = 0xFC;
				}
				else if ((value & (byte)VariableValueInformationType.AveragingDuration) == (byte)VariableValueInformationType.AveragingDuration)
				{
					UnitInfo = VariableValueInformationType.AveragingDuration;
					MultiplierBits = (byte)(value & 0x03);
					Unit = GetTimeUnit(MultiplierBits);
					Mask = 0xFC;
				}
				else if ((value & (byte)VariableValueInformationType.Reserved) == (byte)VariableValueInformationType.Reserved)
				{
					UnitInfo = VariableValueInformationType.Reserved;
					MultiplierBits = 0;
					Unit = string.Empty;
					Mask = 0xFF;
				}
				else if ((value & (byte)VariableValueInformationType.UnitsForHCA) == (byte)VariableValueInformationType.UnitsForHCA)
				{
					UnitInfo = VariableValueInformationType.UnitsForHCA;
					MultiplierBits = 0;
					Unit = string.Empty;
					Mask = 0xFF;
				}
				else if ((value & (byte)VariableValueInformationType.TimePoint) == (byte)VariableValueInformationType.TimePoint)
				{
					UnitInfo = VariableValueInformationType.TimePoint;
					MultiplierBits = (byte)(value & 0x01);
					if (MultiplierBits == 0)
					{
						Unit = "date";
					}
					else
					{
						Unit = "time & date";
					}
					Mask = 0xFE;
				}
				else if ((value & (byte)VariableValueInformationType.Pressure) == (byte)VariableValueInformationType.Pressure)
				{
					UnitInfo = VariableValueInformationType.Pressure;
					MultiplierBits = (byte)(value & 0x03);
					Multiplier = Math.Pow(10, MultiplierBits - 3);
					Unit = "bar";
					Mask = 0xFC;
				}
				else if ((value & (byte)VariableValueInformationType.ExternalTemperature) == (byte)VariableValueInformationType.ExternalTemperature)
				{
					UnitInfo = VariableValueInformationType.ExternalTemperature;
					MultiplierBits = (byte)(value & 0x03);
					Multiplier = Math.Pow(10, MultiplierBits - 3);
					Unit = "°C";
					Mask = 0xFC;
				}
				else if ((value & (byte)VariableValueInformationType.TemperatureDifference) == (byte)VariableValueInformationType.TemperatureDifference)
				{
					UnitInfo = VariableValueInformationType.TemperatureDifference;
					MultiplierBits = (byte)(value & 0x03);
					Multiplier = Math.Pow(10, MultiplierBits - 3);
					Unit = "K";
					Mask = 0xFC;
				}
				else if ((value & (byte)VariableValueInformationType.ReturnTemperature) == (byte)VariableValueInformationType.ReturnTemperature)
				{
					UnitInfo = VariableValueInformationType.ReturnTemperature;
					MultiplierBits = (byte)(value & 0x03);
					Unit = "°C";
					Mask = 0xFC;
				}
				else if ((value & (byte)VariableValueInformationType.FlowTemperature) == (byte)VariableValueInformationType.FlowTemperature)
				{
					UnitInfo = VariableValueInformationType.FlowTemperature;
					MultiplierBits = (byte)(value & 0x03);
					Multiplier = Math.Pow(10, MultiplierBits - 3);
					Unit = "°C";
					Mask = 0xFC;
				}
				else if ((value & (byte)VariableValueInformationType.MassFlow) == (byte)VariableValueInformationType.MassFlow)
				{
					UnitInfo = VariableValueInformationType.MassFlow;
					MultiplierBits = (byte)(value & 0x07);
					Multiplier = Math.Pow(10, MultiplierBits - 3);
					Unit = "kg/h";
					Mask = 0xF8;
				}
				else if ((value & (byte)VariableValueInformationType.VolumeFlow_m3_per_s) == (byte)VariableValueInformationType.VolumeFlow_m3_per_s)
				{
					UnitInfo = VariableValueInformationType.VolumeFlow_m3_per_s;
					MultiplierBits = (byte)(value & 0x07);
					Multiplier = Math.Pow(10, MultiplierBits - 9);
					Unit = "m^3/s";
					Mask = 0xF8;
				}
				else if ((value & (byte)VariableValueInformationType.VolumeFlow_m3_per_min) == (byte)VariableValueInformationType.VolumeFlow_m3_per_min)
				{
					UnitInfo = VariableValueInformationType.VolumeFlow_m3_per_min;
					MultiplierBits = (byte)(value & 0x07);
					Multiplier = Math.Pow(10, MultiplierBits - 7);
					Unit = "m^3/min";
					Mask = 0xF8;
				}
				else if ((value & (byte)VariableValueInformationType.VolumeFlow_m3_per_h) == (byte)VariableValueInformationType.VolumeFlow_m3_per_h)
				{
					UnitInfo = VariableValueInformationType.VolumeFlow_m3_per_h;
					MultiplierBits = (byte)(value & 0x07);
					Multiplier = Math.Pow(10, MultiplierBits - 6);
					Unit = "m^3/h";
					Mask = 0xF8;
				}
				else if ((value & (byte)VariableValueInformationType.Power_J_per_h) == (byte)VariableValueInformationType.Power_J_per_h)
				{
					UnitInfo = VariableValueInformationType.Power_J_per_h;
					MultiplierBits = (byte)(value & 0x07);
					Multiplier = Math.Pow(10, MultiplierBits);
					Unit = "J/h";
					Mask = 0xF8;
				}
				else if ((value & (byte)VariableValueInformationType.Power_W) == (byte)VariableValueInformationType.Power_W)
				{
					UnitInfo = VariableValueInformationType.Power_W;
					MultiplierBits = (byte)(value & 0x07);
					Multiplier = Math.Pow(10, MultiplierBits - 3);
					Unit = "W";
					Mask = 0xF8;
				}
				else if ((value & (byte)VariableValueInformationType.OperatingTime) == (byte)VariableValueInformationType.OperatingTime)
				{
					UnitInfo = VariableValueInformationType.OperatingTime;
					MultiplierBits = (byte)(value & 0x03);
					Unit = GetTimeUnit(MultiplierBits);
					Mask = 0xFC;
				}
				else if ((value & (byte)VariableValueInformationType.OnTime) == (byte)VariableValueInformationType.OnTime)
				{
					UnitInfo = VariableValueInformationType.OnTime;
					MultiplierBits = (byte)(value & 0x03);
					Unit = GetTimeUnit(MultiplierBits);
					Mask = 0xFC;
				}
				else if ((value & (byte)VariableValueInformationType.Mass) == (byte)VariableValueInformationType.Mass)
				{
					UnitInfo = VariableValueInformationType.Mass;
					MultiplierBits = (byte)(value & 0x07);
					Multiplier = Math.Pow(10, MultiplierBits - 3);
					Unit = "kg";
					Mask = 0xFC;
				}
				else if ((value & (byte)VariableValueInformationType.Volume) == (byte)VariableValueInformationType.Volume)
				{
					UnitInfo = VariableValueInformationType.Volume;
					MultiplierBits = (byte)(value & 0x07);
					Multiplier = Math.Pow(10, MultiplierBits - 6);
					Unit = "m^3";
					Mask = 0xFC;
				}
				else if ((value & (byte)VariableValueInformationType.Energy_J) == (byte)VariableValueInformationType.Energy_J)
				{
					UnitInfo = VariableValueInformationType.Energy_J;
					MultiplierBits = (byte)(value & 0x07);
					Multiplier = Math.Pow(10, MultiplierBits);
					Unit = "J";
					Mask = 0xFC;
				}
				else if ((value & (byte)VariableValueInformationType.Energy_Wh) == (byte)VariableValueInformationType.Energy_Wh)
				{
					UnitInfo = VariableValueInformationType.Energy_Wh;
					MultiplierBits = (byte)(value & 0x07);
					Multiplier = Math.Pow(10, MultiplierBits - 3);
					Unit = "Wh";
					Mask = 0xFC;
				}
			}
		}

		public static string GetTimeUnit(byte multiplier)
		{
			switch (multiplier)
			{
				case 0:
					return "seconds";					
				case 1:
					return "minutes";					
				case 2:
					return "hours";					
				case 3:
					return "days";					
				default:
					return string.Empty;					
			}
		}

		public byte MultiplierBits
		{
			get;
			set;
		}

		public double Multiplier
		{
			get;
			set;
		}

		public VariableValueInformationType UnitInfo
		{
			get;
			set;
		}

		public string Unit
		{
			get;
			set;
		}

		public byte ExtensionBit
		{
			get;
			set;
		}

		public byte Mask
		{
			get;
			set;
		}

		public byte[] ToByteArray()
		{
			byte result = (byte)(UnitAndMultiplierByte + (ExtensionBit << 7));
			return new byte[] { result };
		}
	}
}

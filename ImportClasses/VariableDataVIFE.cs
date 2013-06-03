using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gurux.MBus.AddIn
{
	public class VariableDataVIFE
	{
		public VariableDataVIFE()
		{

		}

		public VariableDataVIFE(byte messagePart)
		{
			UnitExtension = (byte)(messagePart & 0x7F);
			ExtensionBit = (byte)((messagePart & 0x80) >> 7);

		}

		/// <summary>
		/// 8.4.5
		/// </summary>
		public byte UnitExtension
		{
			get;
			set;
		}

		public byte ExtensionBit
		{
			get;
			set;
		}

		public byte[] ToByteArray()
		{
			byte result = (byte)(UnitExtension + (ExtensionBit << 7));
			return new byte[] { result };
		}

		#region Extension enum handlers

		public void GetVariableInfo0xFD(out VariableValueAlternateInformation0xFD unitInfo, out byte mask, out byte multiplierBits, out string unit, out double multiplier)
		{
			multiplier = 0;
			mask = 0x7F;
			if ((UnitExtension & (byte)VariableValueAlternateInformation0xFD.DateTimeOfBatteryChange) == (byte)VariableValueAlternateInformation0xFD.DateTimeOfBatteryChange)
			{
				unitInfo = VariableValueAlternateInformation0xFD.DateTimeOfBatteryChange;
				multiplierBits = 0;
				unit = string.Empty;
			}
			else if ((UnitExtension & (byte)VariableValueAlternateInformation0xFD.OperatingTimeBattery) == (byte)VariableValueAlternateInformation0xFD.OperatingTimeBattery)
			{
				unitInfo = VariableValueAlternateInformation0xFD.OperatingTimeBattery;
				multiplierBits = (byte)(UnitExtension & 0x03);
				unit = GetTimeUnitHoursYears(multiplierBits);
			}
			else if ((UnitExtension & (byte)VariableValueAlternateInformation0xFD.DurationSinceLastCumulation) == (byte)VariableValueAlternateInformation0xFD.DurationSinceLastCumulation)
			{
				unitInfo = VariableValueAlternateInformation0xFD.DurationSinceLastCumulation;
				multiplierBits = (byte)(UnitExtension & 0x03);
				unit = GetTimeUnitHoursYears(multiplierBits);
			}
			else if ((UnitExtension & (byte)VariableValueAlternateInformation0xFD.SpecialSupplierInformation) == (byte)VariableValueAlternateInformation0xFD.SpecialSupplierInformation)
			{
				unitInfo = VariableValueAlternateInformation0xFD.SpecialSupplierInformation;
				multiplierBits = 0;
				unit = string.Empty;
			}
			else if ((UnitExtension & (byte)VariableValueAlternateInformation0xFD.ParameterActivationState) == (byte)VariableValueAlternateInformation0xFD.ParameterActivationState)
			{
				unitInfo = VariableValueAlternateInformation0xFD.ParameterActivationState;
				multiplierBits = 0;
				unit = string.Empty;
			}
			else if ((UnitExtension & (byte)VariableValueAlternateInformation0xFD.TimePointOfDayChange) == (byte)VariableValueAlternateInformation0xFD.TimePointOfDayChange)
			{
				unitInfo = VariableValueAlternateInformation0xFD.TimePointOfDayChange;
				multiplierBits = 0;
				unit = string.Empty;
			}
			else if ((UnitExtension & (byte)VariableValueAlternateInformation0xFD.WeekNumber) == (byte)VariableValueAlternateInformation0xFD.WeekNumber)
			{
				unitInfo = VariableValueAlternateInformation0xFD.WeekNumber;
				multiplierBits = 0;
				unit = string.Empty;
			}
			else if ((UnitExtension & (byte)VariableValueAlternateInformation0xFD.DayOfWeek) == (byte)VariableValueAlternateInformation0xFD.DayOfWeek)
			{
				unitInfo = VariableValueAlternateInformation0xFD.DayOfWeek;
				multiplierBits = 0;
				unit = string.Empty;
			}
			else if ((UnitExtension & (byte)VariableValueAlternateInformation0xFD.ControlSignal) == (byte)VariableValueAlternateInformation0xFD.ControlSignal)
			{
				unitInfo = VariableValueAlternateInformation0xFD.ControlSignal;
				multiplierBits = 0;
				unit = string.Empty;
			}
			else if ((UnitExtension & (byte)VariableValueAlternateInformation0xFD.CumulationCounter) == (byte)VariableValueAlternateInformation0xFD.CumulationCounter)
			{
				unitInfo = VariableValueAlternateInformation0xFD.CumulationCounter;
				multiplierBits = 0;
				unit = string.Empty;
			}
			else if ((UnitExtension & (byte)VariableValueAlternateInformation0xFD.ResetCounter) == (byte)VariableValueAlternateInformation0xFD.ResetCounter)
			{
				unitInfo = VariableValueAlternateInformation0xFD.ResetCounter;
				multiplierBits = 0;
				unit = string.Empty;
			}
			else if ((UnitExtension & (byte)VariableValueAlternateInformation0xFD.Volts_Minus_9) == (byte)VariableValueAlternateInformation0xFD.Volts_Minus_9)
			{
				unitInfo = VariableValueAlternateInformation0xFD.Volts_Minus_9;
				multiplierBits = (byte)(UnitExtension & 0x0F);
				multiplier = Math.Pow(10, multiplierBits - 9);
				unit = "V";
			}
			else if ((UnitExtension & (byte)VariableValueAlternateInformation0xFD.Amperes_Minus_12) == (byte)VariableValueAlternateInformation0xFD.Amperes_Minus_12)
			{
				unitInfo = VariableValueAlternateInformation0xFD.Amperes_Minus_12;
				multiplierBits = (byte)(UnitExtension & 0x0F);
				multiplier = Math.Pow(10, multiplierBits - 12);
				unit = "A";
			}
			else if ((UnitExtension & (byte)VariableValueAlternateInformation0xFD.DimensionlessOrNoVIF) == (byte)VariableValueAlternateInformation0xFD.DimensionlessOrNoVIF)
			{
				unitInfo = VariableValueAlternateInformation0xFD.DimensionlessOrNoVIF;
				multiplierBits = 0;
				unit = string.Empty;
			}
			else if ((UnitExtension & (byte)VariableValueAlternateInformation0xFD.PeriodOfTariffYears) == (byte)VariableValueAlternateInformation0xFD.PeriodOfTariffYears)
			{
				unitInfo = VariableValueAlternateInformation0xFD.PeriodOfTariffYears;
				multiplierBits = 0;
				unit = "years";
			}
			else if ((UnitExtension & (byte)VariableValueAlternateInformation0xFD.PeriodOfTariffMonths) == (byte)VariableValueAlternateInformation0xFD.PeriodOfTariffMonths)
			{
				unitInfo = VariableValueAlternateInformation0xFD.PeriodOfTariffMonths;
				multiplierBits = 0;
				unit = "months";
			}
			else if ((UnitExtension & (byte)VariableValueAlternateInformation0xFD.PeriodOfTariff) == (byte)VariableValueAlternateInformation0xFD.PeriodOfTariff)
			{
				unitInfo = VariableValueAlternateInformation0xFD.PeriodOfTariff;
				multiplierBits = (byte)(UnitExtension & 0x03);
				unit = GetTimeUnitSecondsDays(multiplierBits);
			}
			else if ((UnitExtension & (byte)VariableValueAlternateInformation0xFD.StartDateTimeOfTariff) == (byte)VariableValueAlternateInformation0xFD.StartDateTimeOfTariff)
			{
				//DurationOfTariff
				multiplierBits = (byte)(UnitExtension & 0x03);
				if (multiplierBits == 0)
				{
					unitInfo = VariableValueAlternateInformation0xFD.StartDateTimeOfTariff;
					unit = string.Empty;
				}
				else
				{
					unitInfo = VariableValueAlternateInformation0xFD.DurationOfTariff;
					unit = GetTimeUnitSecondsDays(multiplierBits);
				}
			}
			else if ((UnitExtension & (byte)VariableValueAlternateInformation0xFD.DurationSinceLastReadout) == (byte)VariableValueAlternateInformation0xFD.DurationSinceLastReadout)
			{
				unitInfo = VariableValueAlternateInformation0xFD.DurationSinceLastReadout;
				multiplierBits = (byte)(UnitExtension & 0x03);
				unit = GetTimeUnitSecondsDays(multiplierBits);
			}
			else if ((UnitExtension & (byte)VariableValueAlternateInformation0xFD.StorageInterval_Years) == (byte)VariableValueAlternateInformation0xFD.StorageInterval_Years)
			{
				unitInfo = VariableValueAlternateInformation0xFD.StorageInterval_Years;
				multiplierBits = 0;
				unit = "years";
			}
			else if ((UnitExtension & (byte)VariableValueAlternateInformation0xFD.StorageInterval_Months) == (byte)VariableValueAlternateInformation0xFD.StorageInterval_Months)
			{
				unitInfo = VariableValueAlternateInformation0xFD.StorageInterval_Months;
				multiplierBits = 0;
				unit = "months";
			}
			else if ((UnitExtension & (byte)VariableValueAlternateInformation0xFD.StorageInterval) == (byte)VariableValueAlternateInformation0xFD.StorageInterval)
			{
				unitInfo = VariableValueAlternateInformation0xFD.StorageInterval;
				multiplierBits = (byte)(UnitExtension & 0x03);
				unit = GetTimeUnitSecondsDays(multiplierBits);
			}
			else if ((UnitExtension & (byte)VariableValueAlternateInformation0xFD.SizeOfStorageBlock) == (byte)VariableValueAlternateInformation0xFD.SizeOfStorageBlock)
			{
				unitInfo = VariableValueAlternateInformation0xFD.SizeOfStorageBlock;
				multiplierBits = 0;
				unit = string.Empty;
			}
			else if ((UnitExtension & (byte)VariableValueAlternateInformation0xFD.LastStorageNumberForCyclicStorage) == (byte)VariableValueAlternateInformation0xFD.LastStorageNumberForCyclicStorage)
			{
				unitInfo = VariableValueAlternateInformation0xFD.LastStorageNumberForCyclicStorage;
				multiplierBits = 0;
				unit = string.Empty;
			}
			else if ((UnitExtension & (byte)VariableValueAlternateInformation0xFD.FirstStorageNumberForCyclicStorage) == (byte)VariableValueAlternateInformation0xFD.FirstStorageNumberForCyclicStorage)
			{
				unitInfo = VariableValueAlternateInformation0xFD.FirstStorageNumberForCyclicStorage;
				multiplierBits = 0;
				unit = string.Empty;
			}
			else if ((UnitExtension & (byte)VariableValueAlternateInformation0xFD.Retry) == (byte)VariableValueAlternateInformation0xFD.Retry)
			{
				unitInfo = VariableValueAlternateInformation0xFD.Retry;
				multiplierBits = 0;
				unit = string.Empty;
			}
			else if ((UnitExtension & (byte)VariableValueAlternateInformation0xFD.ResponseDelayTime) == (byte)VariableValueAlternateInformation0xFD.ResponseDelayTime)
			{
				unitInfo = VariableValueAlternateInformation0xFD.ResponseDelayTime;
				multiplierBits = 0;
				unit = string.Empty;
			}
			else if ((UnitExtension & (byte)VariableValueAlternateInformation0xFD.Baudrate) == (byte)VariableValueAlternateInformation0xFD.Baudrate)
			{
				unitInfo = VariableValueAlternateInformation0xFD.Baudrate;
				multiplierBits = 0;
				unit = string.Empty;
			}
			else if ((UnitExtension & (byte)VariableValueAlternateInformation0xFD.DigitalInoput) == (byte)VariableValueAlternateInformation0xFD.DigitalInoput)
			{
				unitInfo = VariableValueAlternateInformation0xFD.DigitalInoput;
				multiplierBits = 0;
				unit = string.Empty;
			}
			else if ((UnitExtension & (byte)VariableValueAlternateInformation0xFD.DigitalOutput) == (byte)VariableValueAlternateInformation0xFD.DigitalOutput)
			{
				unitInfo = VariableValueAlternateInformation0xFD.DigitalOutput;
				multiplierBits = 0;
				unit = string.Empty;
			}
			else if ((UnitExtension & (byte)VariableValueAlternateInformation0xFD.ErrorMask) == (byte)VariableValueAlternateInformation0xFD.ErrorMask)
			{
				unitInfo = VariableValueAlternateInformation0xFD.ErrorMask;
				multiplierBits = 0;
				unit = string.Empty;
			}
			else if ((UnitExtension & (byte)VariableValueAlternateInformation0xFD.ErrorFlags) == (byte)VariableValueAlternateInformation0xFD.ErrorFlags)
			{
				unitInfo = VariableValueAlternateInformation0xFD.ErrorFlags;
				multiplierBits = 0;
				unit = string.Empty;
			}
			else if ((UnitExtension & (byte)VariableValueAlternateInformation0xFD.Password) == (byte)VariableValueAlternateInformation0xFD.Password)
			{
				unitInfo = VariableValueAlternateInformation0xFD.Password;
				multiplierBits = 0;
				unit = string.Empty;
			}
			else if ((UnitExtension & (byte)VariableValueAlternateInformation0xFD.AccessCodeDeveloper) == (byte)VariableValueAlternateInformation0xFD.AccessCodeDeveloper)
			{
				unitInfo = VariableValueAlternateInformation0xFD.AccessCodeDeveloper;
				multiplierBits = 0;
				unit = string.Empty;
			}
			else if ((UnitExtension & (byte)VariableValueAlternateInformation0xFD.AccessCodeSystemOperator) == (byte)VariableValueAlternateInformation0xFD.AccessCodeSystemOperator)
			{
				unitInfo = VariableValueAlternateInformation0xFD.AccessCodeSystemOperator;
				multiplierBits = 0;
				unit = string.Empty;
			}
			else if ((UnitExtension & (byte)VariableValueAlternateInformation0xFD.AccessCodeOperator) == (byte)VariableValueAlternateInformation0xFD.AccessCodeOperator)
			{
				unitInfo = VariableValueAlternateInformation0xFD.AccessCodeOperator;
				multiplierBits = 0;
				unit = string.Empty;
			}
			else if ((UnitExtension & (byte)VariableValueAlternateInformation0xFD.AccessCodeUser) == (byte)VariableValueAlternateInformation0xFD.AccessCodeUser)
			{
				unitInfo = VariableValueAlternateInformation0xFD.AccessCodeUser;
				multiplierBits = 0;
				unit = string.Empty;
			}
			else if ((UnitExtension & (byte)VariableValueAlternateInformation0xFD.Customer) == (byte)VariableValueAlternateInformation0xFD.Customer)
			{
				unitInfo = VariableValueAlternateInformation0xFD.Customer;
				multiplierBits = 0;
				unit = string.Empty;
			}
			else if ((UnitExtension & (byte)VariableValueAlternateInformation0xFD.CustomerLocation) == (byte)VariableValueAlternateInformation0xFD.CustomerLocation)
			{
				unitInfo = VariableValueAlternateInformation0xFD.CustomerLocation;
				multiplierBits = 0;
				unit = string.Empty;
			}
			else if ((UnitExtension & (byte)VariableValueAlternateInformation0xFD.SoftwareVersion) == (byte)VariableValueAlternateInformation0xFD.SoftwareVersion)
			{
				unitInfo = VariableValueAlternateInformation0xFD.SoftwareVersion;
				multiplierBits = 0;
				unit = string.Empty;
			}
			else if ((UnitExtension & (byte)VariableValueAlternateInformation0xFD.FirmwareVersion) == (byte)VariableValueAlternateInformation0xFD.FirmwareVersion)
			{
				unitInfo = VariableValueAlternateInformation0xFD.FirmwareVersion;
				multiplierBits = 0;
				unit = string.Empty;
			}
			else if ((UnitExtension & (byte)VariableValueAlternateInformation0xFD.HardwareVersion) == (byte)VariableValueAlternateInformation0xFD.HardwareVersion)
			{
				unitInfo = VariableValueAlternateInformation0xFD.HardwareVersion;
				multiplierBits = 0;
				unit = string.Empty;
			}
			else if ((UnitExtension & (byte)VariableValueAlternateInformation0xFD.ModelOrVersion) == (byte)VariableValueAlternateInformation0xFD.ModelOrVersion)
			{
				unitInfo = VariableValueAlternateInformation0xFD.ModelOrVersion;
				multiplierBits = 0;
				unit = string.Empty;
			}
			else if ((UnitExtension & (byte)VariableValueAlternateInformation0xFD.ParameterSetIdentification) == (byte)VariableValueAlternateInformation0xFD.ParameterSetIdentification)
			{
				unitInfo = VariableValueAlternateInformation0xFD.ParameterSetIdentification;
				multiplierBits = 0;
				unit = string.Empty;
			}
			else if ((UnitExtension & (byte)VariableValueAlternateInformation0xFD.Manufacturer) == (byte)VariableValueAlternateInformation0xFD.Manufacturer)
			{
				unitInfo = VariableValueAlternateInformation0xFD.Manufacturer;
				multiplierBits = 0;
				unit = string.Empty;
			}
			else if ((UnitExtension & (byte)VariableValueAlternateInformation0xFD.Medium) == (byte)VariableValueAlternateInformation0xFD.Medium)
			{
				unitInfo = VariableValueAlternateInformation0xFD.Medium;
				multiplierBits = 0;
				unit = string.Empty;
			}
			else if ((UnitExtension & (byte)VariableValueAlternateInformation0xFD.TransmissionAccessCount) == (byte)VariableValueAlternateInformation0xFD.TransmissionAccessCount)
			{
				unitInfo = VariableValueAlternateInformation0xFD.TransmissionAccessCount;
				multiplierBits = 0;
				unit = string.Empty;
			}
			else if ((UnitExtension & (byte)VariableValueAlternateInformation0xFD.DebitOfNominalLocalLegalCurrencyUnits) == (byte)VariableValueAlternateInformation0xFD.DebitOfNominalLocalLegalCurrencyUnits)
			{
				unitInfo = VariableValueAlternateInformation0xFD.DebitOfNominalLocalLegalCurrencyUnits;
				multiplierBits = (byte)(UnitExtension & 0x03);
				multiplier = Math.Pow(10, multiplierBits - 3);
				unit = string.Empty;
			}
			else if ((UnitExtension & (byte)VariableValueAlternateInformation0xFD.ModelOrVersion) == (byte)VariableValueAlternateInformation0xFD.ModelOrVersion)
			{
				unitInfo = VariableValueAlternateInformation0xFD.CreditOfNominalLocalLegalCurrencyUnits;
				multiplierBits = (byte)(UnitExtension & 0x03);
				multiplier = Math.Pow(10, multiplierBits - 3);
				unit = string.Empty;
			}
			else if ((UnitExtension & (byte)VariableValueAlternateInformation0xFD.ModelOrVersion) == (byte)VariableValueAlternateInformation0xFD.ModelOrVersion)
			{
				unitInfo = VariableValueAlternateInformation0xFD.ModelOrVersion;
				multiplierBits = 0;
				unit = string.Empty;
			}
			else
			{
				throw new Exception("Unknown type in VIFE");
			}
		}

		public void GetVariableInfo0xFB(out VariableValueAlternateInformation0xFB unitInfo, out byte mask, out byte multiplierBits, out string unit, out double multiplier)
		{
			multiplier = 0;
			mask = 0x7F;
			if ((UnitExtension & (byte)VariableValueAlternateInformation0xFB.CumulativeMaxPower_W) == (byte)VariableValueAlternateInformation0xFB.CumulativeMaxPower_W)
			{
				unitInfo = VariableValueAlternateInformation0xFB.CumulativeMaxPower_W;
				multiplierBits = (byte)(UnitExtension & 0x07);
				multiplier = Math.Pow(10, multiplierBits - 3);
				unit = "W";
			}
			else if ((UnitExtension & (byte)VariableValueAlternateInformation0xFB.TemperatureLimit_C) == (byte)VariableValueAlternateInformation0xFB.TemperatureLimit_C)
			{
				unitInfo = VariableValueAlternateInformation0xFB.TemperatureLimit_C;
				multiplierBits = (byte)(UnitExtension & 0x03);
				multiplier = Math.Pow(10, multiplierBits - 3);
				unit = "°C";
			}
			else if ((UnitExtension & (byte)VariableValueAlternateInformation0xFB.TemperatureLimit_F) == (byte)VariableValueAlternateInformation0xFB.TemperatureLimit_F)
			{
				unitInfo = VariableValueAlternateInformation0xFB.TemperatureLimit_F;
				multiplierBits = (byte)(UnitExtension & 0x03);
				multiplier = Math.Pow(10, multiplierBits - 3);
				unit = "°F";
			}
			else if ((UnitExtension & (byte)VariableValueAlternateInformation0xFB.ExternalTemperature) == (byte)VariableValueAlternateInformation0xFB.ExternalTemperature)
			{
				unitInfo = VariableValueAlternateInformation0xFB.ExternalTemperature;
				multiplierBits = (byte)(UnitExtension & 0x03);
				multiplier = Math.Pow(10, multiplierBits - 3);
				unit = "°F";
			}
			else if ((UnitExtension & (byte)VariableValueAlternateInformation0xFB.TemperatureDifference) == (byte)VariableValueAlternateInformation0xFB.TemperatureDifference)
			{
				unitInfo = VariableValueAlternateInformation0xFB.TemperatureDifference;
				multiplierBits = (byte)(UnitExtension & 0x03);
				multiplier = Math.Pow(10, multiplierBits - 3);
				unit = "°F";
			}
			else if ((UnitExtension & (byte)VariableValueAlternateInformation0xFB.ReturnTemperature) == (byte)VariableValueAlternateInformation0xFB.ReturnTemperature)
			{
				unitInfo = VariableValueAlternateInformation0xFB.ReturnTemperature;
				multiplierBits = (byte)(UnitExtension & 0x03);
				multiplier = Math.Pow(10, multiplierBits - 3);
				unit = "°F";
			}
			else if ((UnitExtension & (byte)VariableValueAlternateInformation0xFB.FlowTemperature) == (byte)VariableValueAlternateInformation0xFB.FlowTemperature)
			{
				unitInfo = VariableValueAlternateInformation0xFB.FlowTemperature;
				multiplierBits = (byte)(UnitExtension & 0x03);
				multiplier = Math.Pow(10, multiplierBits - 3);
				unit = "°F";
			}
			else if ((UnitExtension & (byte)VariableValueAlternateInformation0xFB.Power_GJ_per_h) == (byte)VariableValueAlternateInformation0xFB.Power_GJ_per_h)
			{
				unitInfo = VariableValueAlternateInformation0xFB.Power_GJ_per_h;
				multiplierBits = (byte)(UnitExtension & 0x01);
				multiplier = Math.Pow(10, multiplierBits - 1);
				unit = "GJ/h";
			}
			else if ((UnitExtension & (byte)VariableValueAlternateInformation0xFB.Power_MW) == (byte)VariableValueAlternateInformation0xFB.Power_MW)
			{
				unitInfo = VariableValueAlternateInformation0xFB.Power_MW;
				multiplierBits = (byte)(UnitExtension & 0x01);
				multiplier = Math.Pow(10, multiplierBits - 1);
				unit = "MW";
			}
			else if ((UnitExtension & (byte)VariableValueAlternateInformation0xFB.VolumeFlow_american_gallon_per_hour) == (byte)VariableValueAlternateInformation0xFB.VolumeFlow_american_gallon_per_hour)
			{
				unitInfo = VariableValueAlternateInformation0xFB.VolumeFlow_american_gallon_per_hour;
				multiplierBits = 0;
				unit = "american gallon/h";
			}
			else if ((UnitExtension & (byte)VariableValueAlternateInformation0xFB.VolumeFlow_american_gallon_per_min) == (byte)VariableValueAlternateInformation0xFB.VolumeFlow_american_gallon_per_min)
			{
				unitInfo = VariableValueAlternateInformation0xFB.VolumeFlow_american_gallon_per_min;
				multiplierBits = 0;
				unit = "american gallon/min";
			}
			else if ((UnitExtension & (byte)VariableValueAlternateInformation0xFB.VolumeFlow_american_gallon_per_min_one_per_thousand) == (byte)VariableValueAlternateInformation0xFB.VolumeFlow_american_gallon_per_min_one_per_thousand)
			{
				unitInfo = VariableValueAlternateInformation0xFB.VolumeFlow_american_gallon_per_min_one_per_thousand;
				multiplierBits = 0;
				multiplier = Math.Pow(10, -3);
				unit = "american gallon/min";
			}
			else if ((UnitExtension & (byte)VariableValueAlternateInformation0xFB.Volume_american_gallon) == (byte)VariableValueAlternateInformation0xFB.Volume_american_gallon)
			{
				unitInfo = VariableValueAlternateInformation0xFB.Volume_american_gallon;
				multiplierBits = 0;
				unit = "american gallon";
			}
			else if ((UnitExtension & (byte)VariableValueAlternateInformation0xFB.Volume_american_gallon_point_one) == (byte)VariableValueAlternateInformation0xFB.Volume_american_gallon_point_one)
			{
				unitInfo = VariableValueAlternateInformation0xFB.Volume_american_gallon_point_one;
				multiplierBits = 0;
				multiplier = Math.Pow(10, -1);
				unit = "american gallon";
			}
			else if ((UnitExtension & (byte)VariableValueAlternateInformation0xFB.Volume_feet3) == (byte)VariableValueAlternateInformation0xFB.Volume_feet3)
			{
				unitInfo = VariableValueAlternateInformation0xFB.Volume_feet3;
				multiplierBits = 0;
				multiplier = Math.Pow(10, -1);
				unit = "feet^3";
			}
			else if ((UnitExtension & (byte)VariableValueAlternateInformation0xFB.Mass_t) == (byte)VariableValueAlternateInformation0xFB.Mass_t)
			{
				unitInfo = VariableValueAlternateInformation0xFB.Mass_t;
				multiplierBits = (byte)(UnitExtension & 0x01);
				multiplier = Math.Pow(10, multiplierBits - 2);
				unit = "t";
			}
			else if ((UnitExtension & (byte)VariableValueAlternateInformation0xFB.Volume_m3) == (byte)VariableValueAlternateInformation0xFB.Volume_m3)
			{
				unitInfo = VariableValueAlternateInformation0xFB.TemperatureLimit_C;
				multiplierBits = (byte)(UnitExtension & 0x01);
				multiplier = Math.Pow(10, multiplierBits - 2);
				unit = "m^3";
			}
			else if ((UnitExtension & (byte)VariableValueAlternateInformation0xFB.Energy_GJ) == (byte)VariableValueAlternateInformation0xFB.Energy_GJ)
			{
				unitInfo = VariableValueAlternateInformation0xFB.Energy_GJ;
				multiplierBits = (byte)(UnitExtension & 0x01);
				multiplier = Math.Pow(10, multiplierBits - 1);
				unit = "GJ";
			}
			else if ((UnitExtension & (byte)VariableValueAlternateInformation0xFB.Energy_MWh) == (byte)VariableValueAlternateInformation0xFB.Energy_MWh)
			{
				unitInfo = VariableValueAlternateInformation0xFB.Energy_MWh;
				multiplierBits = (byte)(UnitExtension & 0x01);
				multiplier = Math.Pow(10, multiplierBits - 1);
				unit = "MWh";
			}
			else
			{
				throw new Exception("Unknown type in VIFE");
			}
		}

		public void GetExtendedVariableInfo(out VariableValueInformationExtensionType unitInfo, out byte mask, out string unitExtension, out double multiplier)
		{
			multiplier = 0;
			mask = 0x7F;
			if ((UnitExtension & (byte)VariableValueInformationExtensionType.FutureValue) == (byte)VariableValueInformationExtensionType.FutureValue)
			{
				unitInfo = VariableValueInformationExtensionType.FutureValue;
				unitExtension = string.Empty;
			}
			else if ((UnitExtension & (byte)VariableValueInformationExtensionType.MultiplicativeCorrectionFactor10InPowerOf3) == (byte)VariableValueInformationExtensionType.MultiplicativeCorrectionFactor10InPowerOf3)
			{
				unitInfo = VariableValueInformationExtensionType.MultiplicativeCorrectionFactor10InPowerOf3;
				multiplier = Math.Pow(10, 3);
				unitExtension = "";
			}
			else if ((UnitExtension & (byte)VariableValueInformationExtensionType.AdditiveCorretcionConstant) == (byte)VariableValueInformationExtensionType.AdditiveCorretcionConstant)
			{
				unitInfo = VariableValueInformationExtensionType.AdditiveCorretcionConstant;
				int val = (UnitExtension & 0x03) - 3;
				unitExtension = " +10^" + val.ToString();
			}
			else if ((UnitExtension & (byte)VariableValueInformationExtensionType.MultiplicativeCorretionFactorMinusSix) == (byte)VariableValueInformationExtensionType.MultiplicativeCorretionFactorMinusSix)
			{
				unitInfo = VariableValueInformationExtensionType.MultiplicativeCorretionFactorMinusSix;
				int val = (UnitExtension & 0x07) - 6;
				multiplier = Math.Pow(10, val);
				unitExtension = "";
			}
			else if ((UnitExtension & (byte)VariableValueInformationExtensionType.DateTimeOfDataRecord) == (byte)VariableValueInformationExtensionType.DateTimeOfDataRecord)
			{
				unitInfo = VariableValueInformationExtensionType.DateTimeOfDataRecord;
				unitExtension = " Date/Time of Data Record";
			}
			else if ((UnitExtension & (byte)VariableValueInformationExtensionType.DurationOfDataRecord) == (byte)VariableValueInformationExtensionType.DurationOfDataRecord)
			{
				unitInfo = VariableValueInformationExtensionType.DurationOfDataRecord;
				unitExtension = " Duration of Data Record";
			}
			else if ((UnitExtension & (byte)VariableValueInformationExtensionType.DurationOfLimitExceed) == (byte)VariableValueInformationExtensionType.DurationOfLimitExceed)
			{
				unitInfo = VariableValueInformationExtensionType.DurationOfLimitExceed;
				unitExtension = " Duration of Limit Exceed";
			}
			else if ((UnitExtension & (byte)VariableValueInformationExtensionType.DateTimeOfLimitExceed) == (byte)VariableValueInformationExtensionType.DateTimeOfLimitExceed)
			{
				unitInfo = VariableValueInformationExtensionType.DateTimeOfLimitExceed;
				unitExtension = " Date/Time of Limit Exceed";
			}
			else if ((UnitExtension & (byte)VariableValueInformationExtensionType.NumberOfLimitExceeds) == (byte)VariableValueInformationExtensionType.NumberOfLimitExceeds)
			{
				unitInfo = VariableValueInformationExtensionType.NumberOfLimitExceeds;
				unitExtension = " Number of Limit Exceeds";
			}
			else if ((UnitExtension & (byte)VariableValueInformationExtensionType.LimitValue) == (byte)VariableValueInformationExtensionType.LimitValue)
			{
				unitInfo = VariableValueInformationExtensionType.LimitValue;
				unitExtension = "Limit Value";
			}

			else if ((UnitExtension & (byte)VariableValueInformationExtensionType.OnlyAbsoluteOfNegativeContributionAccumulation) == (byte)VariableValueInformationExtensionType.OnlyAbsoluteOfNegativeContributionAccumulation)
			{
				unitInfo = VariableValueInformationExtensionType.OnlyAbsoluteOfNegativeContributionAccumulation;
				unitExtension = string.Empty;
			}
			else if ((UnitExtension & (byte)VariableValueInformationExtensionType.OnlyPositiveContributionAccumulation) == (byte)VariableValueInformationExtensionType.OnlyPositiveContributionAccumulation)
			{
				unitInfo = VariableValueInformationExtensionType.OnlyPositiveContributionAccumulation;
				unitExtension = string.Empty;
			}
			else if ((UnitExtension & (byte)VariableValueInformationExtensionType.VIFContainstUncorrectedUnit) == (byte)VariableValueInformationExtensionType.VIFContainstUncorrectedUnit)
			{
				unitInfo = VariableValueInformationExtensionType.VIFContainstUncorrectedUnit;
				unitExtension = string.Empty;
			}
			else if ((UnitExtension & (byte)VariableValueInformationExtensionType.StartDateTimeOfDataRecord) == (byte)VariableValueInformationExtensionType.StartDateTimeOfDataRecord)
			{
				unitInfo = VariableValueInformationExtensionType.StartDateTimeOfDataRecord;
				unitExtension = string.Empty;
			}
			else if ((UnitExtension & (byte)VariableValueInformationExtensionType.MultipliedBySekPerAmpere) == (byte)VariableValueInformationExtensionType.MultipliedBySekPerAmpere)
			{
				unitInfo = VariableValueInformationExtensionType.MultipliedBySekPerAmpere;
				unitExtension = "*s/A";
			}
			else if ((UnitExtension & (byte)VariableValueInformationExtensionType.MultipliedBySekPerVolt) == (byte)VariableValueInformationExtensionType.MultipliedBySekPerVolt)
			{
				unitInfo = VariableValueInformationExtensionType.MultipliedBySekPerVolt;
				unitExtension = "*s/V";
			}
			else if ((UnitExtension & (byte)VariableValueInformationExtensionType.MultipliedBySek) == (byte)VariableValueInformationExtensionType.MultipliedBySek)
			{
				unitInfo = VariableValueInformationExtensionType.MultipliedBySek;
				unitExtension = "*s";
			}
			else if ((UnitExtension & (byte)VariableValueInformationExtensionType.Per_Ampere) == (byte)VariableValueInformationExtensionType.Per_Ampere)
			{
				unitInfo = VariableValueInformationExtensionType.Per_Ampere;
				unitExtension = "/A";
			}
			else if ((UnitExtension & (byte)VariableValueInformationExtensionType.Per_Volt) == (byte)VariableValueInformationExtensionType.Per_Volt)
			{
				unitInfo = VariableValueInformationExtensionType.Per_Volt;
				unitExtension = "/V";
			}
			else if ((UnitExtension & (byte)VariableValueInformationExtensionType.Per_Kelvin_x_Liter) == (byte)VariableValueInformationExtensionType.Per_Kelvin_x_Liter)
			{
				unitInfo = VariableValueInformationExtensionType.Per_Kelvin_x_Liter;
				unitExtension = "K/l";
			}
			else if ((UnitExtension & (byte)VariableValueInformationExtensionType.Per_kW) == (byte)VariableValueInformationExtensionType.Per_kW)
			{
				unitInfo = VariableValueInformationExtensionType.Per_kW;
				unitExtension = "/kW";
			}
			else if ((UnitExtension & (byte)VariableValueInformationExtensionType.Per_GJ) == (byte)VariableValueInformationExtensionType.Per_GJ)
			{
				unitInfo = VariableValueInformationExtensionType.Per_GJ;
				unitExtension = "/GJ";
			}
			else if ((UnitExtension & (byte)VariableValueInformationExtensionType.Per_kWh) == (byte)VariableValueInformationExtensionType.Per_kWh)
			{
				unitInfo = VariableValueInformationExtensionType.Per_kWh;
				unitExtension = "/kWh";
			}
			else if ((UnitExtension & (byte)VariableValueInformationExtensionType.Per_K) == (byte)VariableValueInformationExtensionType.Per_K)
			{
				unitInfo = VariableValueInformationExtensionType.Per_K;
				unitExtension = "/K";
			}
			else if ((UnitExtension & (byte)VariableValueInformationExtensionType.Per_kg) == (byte)VariableValueInformationExtensionType.Per_kg)
			{
				unitInfo = VariableValueInformationExtensionType.Per_kg;
				unitExtension = "/kg";
			}
			else if ((UnitExtension & (byte)VariableValueInformationExtensionType.Per_m3) == (byte)VariableValueInformationExtensionType.Per_m3)
			{
				unitInfo = VariableValueInformationExtensionType.Per_m3;
				unitExtension = "/m3";
			}
			else if ((UnitExtension & (byte)VariableValueInformationExtensionType.Per_liter) == (byte)VariableValueInformationExtensionType.Per_liter)
			{
				unitInfo = VariableValueInformationExtensionType.Per_liter;
				unitExtension = "/l";
			}
			else if ((UnitExtension & (byte)VariableValueInformationExtensionType.IncrementPerOutputPulseOrChannel) == (byte)VariableValueInformationExtensionType.IncrementPerOutputPulseOrChannel)
			{
				unitInfo = VariableValueInformationExtensionType.IncrementPerOutputPulseOrChannel;
				unitExtension = "Increment per Output Pulse or Channel";
			}
			else if ((UnitExtension & (byte)VariableValueInformationExtensionType.IncrementPerInputPulseOrChannel) == (byte)VariableValueInformationExtensionType.IncrementPerInputPulseOrChannel)
			{
				unitInfo = VariableValueInformationExtensionType.IncrementPerInputPulseOrChannel;
				unitExtension = "Increment per Input Pulse or Channel";
			}
			else if ((UnitExtension & (byte)VariableValueInformationExtensionType.Per_revolution) == (byte)VariableValueInformationExtensionType.Per_revolution)
			{
				unitInfo = VariableValueInformationExtensionType.Per_revolution;
				unitExtension = "/1";
			}
			else if ((UnitExtension & (byte)VariableValueInformationExtensionType.Per_year) == (byte)VariableValueInformationExtensionType.Per_year)
			{
				unitInfo = VariableValueInformationExtensionType.Per_year;
				unitExtension = "/a";
			}
			else if ((UnitExtension & (byte)VariableValueInformationExtensionType.Per_month) == (byte)VariableValueInformationExtensionType.Per_month)
			{
				unitInfo = VariableValueInformationExtensionType.Per_month;
				unitExtension = "/month";
			}
			else if ((UnitExtension & (byte)VariableValueInformationExtensionType.Per_week) == (byte)VariableValueInformationExtensionType.Per_week)
			{
				unitInfo = VariableValueInformationExtensionType.Per_week;
				unitExtension = "/week";
			}
			else if ((UnitExtension & (byte)VariableValueInformationExtensionType.Per_day) == (byte)VariableValueInformationExtensionType.Per_day)
			{
				unitInfo = VariableValueInformationExtensionType.Per_day;
				unitExtension = "/d";
			}
			else if ((UnitExtension & (byte)VariableValueInformationExtensionType.Per_hour) == (byte)VariableValueInformationExtensionType.Per_hour)
			{
				unitInfo = VariableValueInformationExtensionType.Per_hour;
				unitExtension = "/h";
			}
			else if ((UnitExtension & (byte)VariableValueInformationExtensionType.Per_minute) == (byte)VariableValueInformationExtensionType.Per_minute)
			{
				unitInfo = VariableValueInformationExtensionType.Per_minute;
				unitExtension = "/min";
			}
			else if ((UnitExtension & (byte)VariableValueInformationExtensionType.Per_second) == (byte)VariableValueInformationExtensionType.Per_second)
			{
				unitInfo = VariableValueInformationExtensionType.Per_second;
				unitExtension = "/s";
			}
			else
			{
				throw new Exception("Unknown type in VIFE");
			}
		}

		private string GetTimeUnitSecondsDays(byte multiplier)
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

        private string GetTimeUnitHoursYears(byte multiplier)
        {
            switch (multiplier)
            {
                case 0:
                    return "hours";
                case 1:
                    return "days";
                case 2:
                    return "months";
                case 3:
                    return "years";
                default:
                    return string.Empty;
            }
        }
		#endregion
	}
}

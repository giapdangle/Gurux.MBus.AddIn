using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gurux.MBus.AddIn
{
	public enum FrameType
	{
		None = 0,
		SingleCharacterFrame,
		ShortFrame,
		LongFrame,
	}

	#region CField
	[Flags]
	public enum CFieldRequest
	{
		Function0 = 0x1,
		Function1 = 0x2,
		Function2 = 0x4,
		Function3 = 0x8,
		FrameCountBitValid = 0x10,
		FrameCountBit = 0x20,
		/// <summary>
		/// Should always be 1.
		/// </summary>
		IsRequest = 0x40,
	}

	/// <summary>
	/// Table 1 Control Codes of the M-Bus Protocol (F : FCB-Bit, A : ACD-Bit, D : DFC-Bit)
	/// MBus specification page 24.
	/// </summary>
	[Flags]
	public enum CFieldReply
	{
		Function0 = 0x1,
		Function1 = 0x2,
		Function2 = 0x4,

		/// <summary>
		/// Data Transfer from Slave to Master after Request
		/// </summary>
		Function3 = 0x8,
		/// <summary>
		/// Client cannot accept futher data.
		/// </summary>
		DataFlowControl = 0x10,
		/// <summary>
		/// Client wants to transmit Class 1 data, masters needs to request it ASAP.
		/// </summary>
		AccessDemand = 0x20,
		/// <summary>
		/// Should always be 0.
		/// </summary>
		IsRequest = 0x40,
	}

	[Flags]
	public enum CFieldFunctions
	{
		/// <summary>
		/// SND_NKE
		/// </summary>
		SendSlaveInit = 0x40,
		/// <summary>
		/// SND_UD
		/// </summary>
		SendUserData = 0x53,
		/// <summary>
		/// REQ_UD2
		/// </summary>
		RequestClass2Data = 0x5B,
		/// <summary>
		/// REQ_UD1
		/// </summary>
		RequestClass1Data = 0x5A,
		/// <summary>
		/// RSP_DATA
		/// </summary>
		ReplyData = 8,
	}
	#endregion

	#region CIField

	[Flags]
	public enum CIField
	{
		/// <summary>
		/// Not recommended
		/// </summary>
		Mode2 = 0x04,
		DataSend = 0x51,
		SelectSlaves = 0x52,
		ApplicationRset = 0x50,
		SynchronizeAction = 0x54,
		SetBaudrate300 = 0xB8,
		SetBaudrate600 = 0xB9,
		SetBaudrate1200 = 0xBA,
		SetBaudrate2400 = 0xBB,
		SetBaudrate4800 = 0xBC,
		SetBaudrate9600 = 0xBD,
		SetBaudrate19200 = 0xBE,
		SetBaudrate38400 = 0xBF,
		RequestCompleteRAM = 0xB1,
		/// <summary>
		/// Not standardized RAM Write
		/// </summary>
		SendUserData = 0xB2,
		InitTestCalibrationMode = 0xB3,
		ReadEEPROM = 0xB4,
		StartSoftwareTest =0xB6,
		HashingCode0 = 0x90,
		HashingCode1 = 0x91,
		HashingCode2 = 0x92,
		HashingCode3 = 0x93,
		HashingCode4 = 0x94,
		HashingCode5 = 0x95,
		HashingCode6 = 0x96,
		HashingCode7 = 0x97,
	}

	[Flags]
	public enum CIFieldReply
	{
		Mode1 = 0x04,
		ApplicationError = 0x70,
		AlarmStatus = 0x71,
		VariableDataReply = 0x72,
		FixedDataReply = 0x73,
	}

	[Flags]
	public enum CIFieldResetCodes
	{
		All = 0x00,
		UserData = 0x01,
		SimpleBilling = 0x02,
		EnhancedBilling = 0x03,
		MultiTariffBilling = 0x04,
		InstantenousValues = 0x05,
		LoadManagementValues = 0x06,
		InstallationAndStartup = 0x08,
		Testing = 0x09,
		Calibration = 0x0A,
		Manufactureing = 0x0B,
		Development = 0x0C,
		Selftest = 0x0D,
	}

	#endregion

	#region Data Structures

	[Flags]
	public enum FixedDataStatus
	{
		BCD = 0x01,
		FixedDate = 0x02,
		PowerLow = 0x05,
		PermanentError = 0x08,
		TemporaryError = 0x10,
		Manufacturer1 = 0x20,
		Manufacturer2 = 0x40,
		Manufacturer3 = 0x80,
	}

	public enum FixedDataMedium
	{
		Other = 0x00,
		Oil = 0x01,
		Electricity = 0x02,
		Gas = 0x03,
		Heat = 0x04,
		Steam = 0x05,
		HotWater = 0x06,
		Water = 0x07,
		HCA = 0x08,
		Reserved = 0x09,
		GasMode2 = 0x0A,
		HeatMode2 = 0x0B,
		HotWaterMode2 = 0x0C,
		WaterMode2 = 0x0D,
		HCAMode2 = 0x0E,
		Reserved2 = 0x0F
	}

	public enum FixedDataPhysicalUnit
	{
		HourMinuteSecond = 0x00,
		DayMonthYear = 0x01,
		Wh = 0x02,
		WhTimes10 = 0x03,
		WhTimes100 = 0x04,
		kWh = 0x05,
		kWhTimes10 = 0x06,
		kWhTimes100 = 0x07,
		MWh = 0x08,
		MWhTimes10 = 0x09,
		MWhTimes100 = 0x0A,
		kJ = 0x0B,
		kJTimes10 = 0x0C,
		kJTimes100 = 0x0D,
		MJ = 0x0E,
		MJTimes10 = 0x0F,
		MJTimes100 = 0x10,
		GJ = 0x11,
		GJTimes10 = 0x12,
		GJTimes100 = 0x13,
		W = 0x14,
		WTimes10 = 0x15,
		WTimes100 = 0x16,
		kW = 0x17,
		kWTimes10 = 0x18,
		kWTimes100 = 0x19,
		MW = 0x1A,
		MWTimes10 = 0x1B,
		MWTimes100 = 0x1C,
		kJPerh = 0x1D,
		kJPerhTimes10 = 0x1E,
		kJPerhTimes100 = 0x1F,
		MJPerh = 0x20,
		MJPerhTimes10 = 0x21,
		MJPerhTimes100 = 0x22,
		GJPerh = 0x23,
		GJPerhTimes10 = 0x24,
		GJPerhTimes100 = 0x25,
		ml = 0x26,
		mlTimes10 = 0x27,
		mlTimes100 = 0x28,
		l = 0x29,
		lTimes10 = 0x2A,
		lTimes100 = 0x2B,
		m3 = 0x2C,
		m3Times10 = 0x2D,
		m3Times100 = 0x2E,
		mlPerh = 0x2F,
		mlPerhTimes10 = 0x30,
		mlPerhTimes100 = 0x31,
		lPerh = 0x32,
		lPerhTimes10 = 0x33,
		lPerhTimes100 = 0x34,
		m3Perh = 0x35,
		m3PerhTiems10 = 0x36,
		m3PerhTiems100 = 0x37,
		DegreesCelciusTimes10ToMinus3 = 0x38,
		UnitsForHCA = 0x39,
		Reserved1 = 0x3A,
		Reserved2 = 0x3B,
		Reserved3 = 0x3C,
		Reserved4 = 0x3D,
		SameButHistoric = 0x3E,
		WithoutUnit = 0x3F
	}

	public enum VariableDataFunction
	{
		Instantaneous = 0x00,
		Maximum = 0x01,
		Minimum = 0x02,
		ErrorValue = 0x03,
	}

	/// <summary>
	/// For a detailed description of data types refer to appendix 8.2 “ Coding of data records“
	/// </summary>
	public enum VariableDataType
	{
		NoData = 0x00,
		Int8 = 0x01,
		Int16 = 0x02,
		Int24 = 0x03,
		Int32 = 0x04,
		Real32 = 0x05,
		Int48 = 0x06,
		Int64 = 0x07,
		SelectionForReadout = 0x08,
		BCD2 = 0x09,
		BCD4 = 0x0a,
		BCD6 = 0x0b,
		BCD8 = 0x0c,
		/// <summary>
		/// TODO: Requires special handling, page 39 of the mbus pdf specification.
		/// </summary>
		VariableLength = 0x0d,
		BCD12 = 0x0e,
		/// TODO: Requires special handling, page 41 of the mbus pdf specification.
		SpecialFunctions = 0x0f,
	}

	public enum VariableValueInformationType
	{
		Energy_Wh = 0x00,
		Energy_J = 0x08,
		Volume = 0x10,
		Mass = 0x18,
		OnTime = 0x20,
		OperatingTime = 0x24,
		Power_W = 0x28,
		Power_J_per_h = 0x30,
		VolumeFlow_m3_per_h = 0x38,
		VolumeFlow_m3_per_min = 0x40,
		VolumeFlow_m3_per_s = 0x48,
		MassFlow = 0x50,
		FlowTemperature = 0x58,
		ReturnTemperature = 0x5C,
		TemperatureDifference = 0x60,
		ExternalTemperature = 0x64,
		Pressure = 0x68,
		TimePoint = 0x6C,
		UnitsForHCA = 0x6E,
		Reserved = 0x6F,
		AveragingDuration = 0x70,
		ActualityDuration = 0x74,
		FabricationNumber = 0x78,
		Enhanced = 0x79,
		BusAddress = 0x7A,
		/// <summary>
		/// Page 83 of the mbus pdf specification.
		/// </summary>
		ExtendedFB = 0xFB,
		/// <summary>
		/// Page 80 of the mbus pdf specification.
		/// </summary>
		ExtendedFD = 0xFD,
	}

	public enum VariableValueInformationExtensionType
	{
		Per_second = 0x20,
		Per_minute = 0x21,
		Per_hour = 0x22,
		Per_day = 0x23,
		Per_week = 0x24,
		Per_month = 0x25,
		Per_year = 0x26,
		Per_revolution = 0x27,
		IncrementPerInputPulseOrChannel = 0x28,
		IncrementPerOutputPulseOrChannel = 0x2A,
		Per_liter = 0x2C,
		Per_m3 = 0x2D,
		Per_kg = 0x2E,
		Per_K = 0x2F,
		Per_kWh = 0x30,
		Per_GJ = 0x31,
		Per_kW = 0x32,
		Per_Kelvin_x_Liter = 0x33,
		Per_Volt = 0x34,
		Per_Ampere = 0x35,
		MultipliedBySek = 0x36,
		MultipliedBySekPerVolt = 0x37,
		MultipliedBySekPerAmpere = 0x38,
		StartDateTimeOfDataRecord = 0x39,
		VIFContainstUncorrectedUnit = 0x3A,
		OnlyPositiveContributionAccumulation = 0x3B,
		OnlyAbsoluteOfNegativeContributionAccumulation = 0x3C,
		LimitValue = 0x40,
		NumberOfLimitExceeds = 0x41,
		DateTimeOfLimitExceed = 0x42,
		DurationOfLimitExceed = 0x50,
		DurationOfDataRecord = 0x60,
		DateTimeOfDataRecord = 0x6A,
		MultiplicativeCorretionFactorMinusSix = 0x70,
		AdditiveCorretcionConstant = 0x78,
		MultiplicativeCorrectionFactor10InPowerOf3 = 0x7D,
		FutureValue = 0x7E,
		NextVIFEIsManufacturer = 0x7F,
	}

	public enum VariableValueAlternateInformation0xFB
	{
		Energy_MWh = 0x00,
		Energy_GJ = 0x08,
		Volume_m3 = 0x10,
		Mass_t = 0x18,
		Volume_feet3 = 0x21,
		Volume_american_gallon_point_one = 0x22,
		Volume_american_gallon = 0x23,
		VolumeFlow_american_gallon_per_min_one_per_thousand = 0x24,
		VolumeFlow_american_gallon_per_min = 0x25,
		VolumeFlow_american_gallon_per_hour = 0x26,
		Power_MW = 0x28,
		Power_GJ_per_h = 0x30,
		FlowTemperature = 0x58,
		ReturnTemperature = 0x5C,
		TemperatureDifference = 0x60,
		ExternalTemperature = 0x64,
		TemperatureLimit_F = 0x70,
		TemperatureLimit_C = 0x74,
		CumulativeMaxPower_W = 0x78,
	}

	public enum VariableValueAlternateInformation0xFD
	{
		CreditOfNominalLocalLegalCurrencyUnits = 0x00,
		DebitOfNominalLocalLegalCurrencyUnits = 0x04,
		TransmissionAccessCount = 0x08,
		Medium = 0x09,
		Manufacturer = 0x0A,
		ParameterSetIdentification = 0x0B,
		ModelOrVersion = 0x0C,
		HardwareVersion = 0x0D,
		FirmwareVersion = 0x0E,
		SoftwareVersion = 0x0F,
		CustomerLocation = 0x10,
		Customer = 0x11,
		AccessCodeUser = 0x12,
		AccessCodeOperator = 0x13,
		AccessCodeSystemOperator = 0x14,
		AccessCodeDeveloper = 0x15,
		Password = 0x16,
		ErrorFlags = 0x17,
		ErrorMask = 0x18,
		DigitalOutput = 0x1A,
		DigitalInoput = 0x1B,
		Baudrate = 0x1C,
		ResponseDelayTime = 0x1D,
		Retry = 0x1E,
		FirstStorageNumberForCyclicStorage = 0x20,
		LastStorageNumberForCyclicStorage = 0x21,
		SizeOfStorageBlock = 0x22,
		StorageInterval = 0x24,
		StorageInterval_Months = 0x28,
		StorageInterval_Years = 0x29,
		DurationSinceLastReadout = 0x2C,
		StartDateTimeOfTariff = 0x30,
		DurationOfTariff = 0x30,
		PeriodOfTariff = 0x34,
		PeriodOfTariffMonths = 0x38,
		PeriodOfTariffYears = 0x39,
		DimensionlessOrNoVIF = 0x3A,
		Volts_Minus_9 = 0x40,
		Amperes_Minus_12 = 0x50,
		ResetCounter = 0x60,
		CumulationCounter = 0x61,
		ControlSignal = 0x62,
		DayOfWeek = 0x63,
		WeekNumber = 0x64,
		TimePointOfDayChange = 0x65,
		ParameterActivationState = 0x66,
		SpecialSupplierInformation = 0x67,
		DurationSinceLastCumulation = 0x68,
		OperatingTimeBattery = 0x6C,
		DateTimeOfBatteryChange = 0x70,
	}
	#endregion


}

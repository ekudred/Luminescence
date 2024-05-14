//**************************** Команды (совпадают с Режимами работы)******************************
#define cNone 	 					0x00 	//нет команды (для NextCommand)
#define cMeasure 	 				0x01 	//Команда измерения ТЛ
#define cIdle 						0x20	//холостой режим, ничего не делаем, но 1 раз в секунду
											//отправляем стандартную посылку

//********************************************* структуры репортов ***************************************************
#pragma pack(push, 1)
typedef struct {         //репорт приема команд и данных от ПК
	uint8_t ID_Report;              //00  //номер типа репорта repStdOutPC = 1
	uint8_t Command;                //01  //команда
	uint8_t Parameter0;          	//02  //параметр 0
	uint8_t Parameter1;           	//03  //параметр 1

	uint8_t HeaterMode;           	//04  //режим нагрева (0 – отключение, 1 – линейный нагрев, 2 – поддержание)
	uint8_t LEDMode;           		//05  //режим работы светодиодов для ОСЛ (0 – отключение, 1 – линейное увеличение тока, 2 – поддержание тока)
	uint8_t PEMMode;           		//06  //режим работы ФЭУ (0 – ФЭУ выключен, 1 – авто режим, 2 – постоянное Ufeu)
	uint8_t HeatRate; 				//07  //скорость нагрева (фикс точка 1 знак после запятой (входное число делим на 10): 1 - 0,1C°/с, 10 - 1C°/с, 20 - 2C°/с ... )

	uint16_t StartTemperature;		//08-09	//начальная температура, С°
	uint16_t EndTemperature;		//10-11	//конечная температура, С°

	uint16_t StartLEDCurrent;		//12-13	//начальный ток, мА
	uint16_t EndLEDCurrent;			//14-15	//конечный ток, мА

	uint8_t LEDCurrentRate;			//16-17	//скорость роста тока (фикс точка 1 знак после запятой (входное число делим на 10): 1 - 0,1мА/с, 10 - 1мА/с, 20 - 2мА/с ... )
	uint8_t Upem; 					//18	//управляющее напряжение на ФЭУ (фикс точка 1 знак после запятой (входное число делим на 10): 5 - 0,5В,  ... , 11 - 1.1В)

	//коэф.

	uint8_t Data[LengthDataInRepStdOutPC];           			//19-62   LengthDataInRepStdOutPC = 43
	uint8_t fError;						//63	//Ошибка
} tReportStdOutPC;
#pragma pack(pop)

#pragma pack(push, 1)
typedef struct {        //стандартный репорт передачи данных от МК
	uint8_t ID_Report;              //00  //номер типа репорта tReportStdInPC = 3
	uint8_t Mode;                   //01  //режим
	uint8_t Phase;           		//02  //фаза режима
	uint8_t Parameter0;          	//03  //параметр 0

	uint32_t Counter;            	//04-07	//кол-во тиков после запуска нагрева

	uint8_t HeaterMode;            	//08    //режим нагрева
	uint8_t HeatingRate;        	//09 	//скорость нагрева, C°/с
	uint16_t LEDCurrentRate;        //10-11	//скорость увеличения тока, 0,1мА/с (фикс точка 1 знак после запятой (входное число делим на 10): 1 - 0,1мА/с, 10 - 1мА/с, 20 - 2мА/с ... )

	float OpTemperature; 			//12-15	//опорная температура
	float Temperature;				//16-19	//текущая температура
	float LEDCurrent;				//20-23	//текущый ток, мА

	int32_t Intensity;				//24-27	//интенсивность свечения
	uint8_t LEDMode;            	//28    //режим работы светодиодов для ОСЛ
	uint8_t Upem; 					//29	//управляющее напряжение на ФЭУ (фикс точка 1 знак после запятой (входное число делим на 10): 5 - 0,5В,  ... , 11 - 1.1В)

	uint8_t Data[LengthDataInRepStdInPC];	//30-62        LengthDataInRepStdInPC = 33
	uint8_t fError;					//63	//Ошибка
} tReportStdInPC;
#pragma pack(pop)

Luminescence (solution)/               
    ├── Luminescence (project)/ 
    	├── Assets/
        ├── Styles/          
        ├── Views/                
        └── ...  
    ├── Luminescence.Services (project)/
    	├── App/    
        └── ...  
    ├── Luminescence.Shared (project)/
        ├── Dialog/
	├── Form/
        ├── UsbHid/         
        ├── Utils/          
        └── ...  
    └── Luminescence.ViewModels (project)/  
        ├── Dialogs/
        ├── Main/                   
        └── ...  
























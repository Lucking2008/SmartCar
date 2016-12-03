#include <SoftwareSerial.h>

SoftwareSerial bt(0, 1);

int motor[10];
double fuerza[10];
int nMotores = 2;
int difFuerza;

void setup()
{
	// put your setup code here, to run once:
	Serial.begin(9600);
	delay(50);
	/*bt.begin(9600);

	//se configura los pines de entrada y salida   
	pinMode(A2, OUTPUT);

	difFuerza = 1;

	motor[0] = 9;
	motor[1] = 10;

	for (int i = 0; i<nMotores; i++)
	{
		pinMode(motor[i], OUTPUT);
		fuerza[i] = 0;
	}*/
}

void loop()
{
	/*// put your main code here, to run repeatedly:
	if (bt.available() > 0)
	{
		int btData = bt.parseInt();

		if (btData > 100)
			aumentarFuerza(0);
		else
			reducirFuerza(0);

		Serial.println(btData);
		delay(100);

	}*/
	if (Serial.available())
	{
		String s = Serial.readString();
		if (s.compareTo("hello") == 0)
		{
			Serial.println("Arduino: hello!");
		}
		delay(100);
	}

}

void aumentarFuerza(int nMotor)
{
	fuerza[nMotor] = min(3000, fuerza[nMotor] + difFuerza);
	analogWrite(motor[nMotor], 250);
}
void reducirFuerza(int nMotor)
{
	fuerza[nMotor] = max(0, fuerza[nMotor] - difFuerza);
	analogWrite(motor[nMotor], 0);
}
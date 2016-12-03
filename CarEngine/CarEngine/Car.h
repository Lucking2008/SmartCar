#pragma once

#include "Polygon2D.h"

class Car
{
public:

	Polygon2D body;

public:
	Car();
	~Car();

	void Rotate(double theta);

};


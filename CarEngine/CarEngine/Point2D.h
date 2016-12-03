#pragma once

#include "Transformable.h"
#include <cmath>

class Point2D : public Transformable
{
private:
	double x;
	double y;

public:
	Point2D();
	Point2D(double x, double y);

	~Point2D();

	void Translate(double dx, double dy) override;
	void Escale(double sx, double sy) override;
	void Rotate(double theta) override;
	void Rotate(double px, double py, double theta) override;

	inline double getX() { return x; }
	inline double getY() { return y; }

};


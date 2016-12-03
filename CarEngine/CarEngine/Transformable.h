#pragma once
class Transformable
{
public:
	virtual ~Transformable() {}

	virtual void Translate(double dx, double dy) = 0;
	virtual void Escale(double dx, double dy) = 0;
	virtual void Rotate(double theta) = 0;
	virtual void Rotate(double px, double py, double theta) = 0;

};


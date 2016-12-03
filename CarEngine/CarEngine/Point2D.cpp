#include "Point2D.h"



Point2D::Point2D()
{
	x = 0;
	y = 0;
}

Point2D::Point2D(double x, double y)
{
	this->x = x;
	this->y = y;
}

Point2D::~Point2D()
{
}

void Point2D::Translate(double dx, double dy)
{
	x += dx;
	y += dy;
}

void Point2D::Escale(double sx, double sy)
{
	x *= sx;
	y *= sy;
}

void Point2D::Rotate(double theta)
{
	x = x * cos(theta) - y * sin(theta);
	y = y * cos(theta) + x * sin(theta);
}

void Point2D::Rotate(double px, double py, double theta)
{
	x = cos(theta) * (x - px) - sin(theta) * (y - py) + px;
	y = sin(theta) * (x - px) + cos(theta) * (y - py) + py;
}
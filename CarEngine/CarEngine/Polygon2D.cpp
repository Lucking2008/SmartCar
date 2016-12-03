#include "Polygon2D.h"



Polygon2D::Polygon2D()
{
}


Polygon2D::~Polygon2D()
{
}

void Polygon2D::AddPoint(Point2D point)
{
	points.push_back(point);
}

void Polygon2D::Translate(double dx, double dy)
{
	for (auto& it : points)
		it.Translate(dx, dy);
}

void Polygon2D::Escale(double sx, double sy)
{
	for (auto& it : points)
		it.Escale(sx, sy);
}

void Polygon2D::Rotate(double theta)
{
	for (auto& it : points)
		it.Rotate(theta);
}

void Polygon2D::Rotate(double px, double py, double theta)
{
	for (auto& it : points)
		it.Rotate(px, py, theta);
}
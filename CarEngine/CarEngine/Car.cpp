#include "Car.h"



Car::Car()
{
	Point2D p1(15, 30);
	Point2D p2(30, 30);
	Point2D p3(30, 15);
	Point2D p4(15, 15);

	body.AddPoint(p1);
	body.AddPoint(p2);
	body.AddPoint(p3);
	body.AddPoint(p4);

}


Car::~Car()
{
}

void Car::Rotate(double theta)
{
	Point2D center = body.getCenter();
	body.Rotate(center.getX(), center.getY(), theta);
}
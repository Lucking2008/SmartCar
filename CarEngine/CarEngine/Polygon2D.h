#pragma once

#include "Transformable.h"
#include "Point2D.h"
#include <vector>

class Polygon2D : public Transformable
{
private:
	std::vector<Point2D> points;

public:
	Polygon2D();
	~Polygon2D();


	void AddPoint(Point2D point);

	void Translate(double dx, double dy) override;
	void Escale(double sx, double sy) override;
	void Rotate(double theta) override;
	void Rotate(double px, double py, double theta) override;

	inline Point2D getCenter() {
		double x = 0.0;
		double y = 0.0;
		for (int i = 0; i < points.size(); i++) {
			Point2D point = points[i];
			x += point.getX();
			y += point.getY();
		}

		x = x / points.size();
		y = y / points.size();

		return Point2D(x, y);
	}

	inline size_t getSize() { return points.size(); }
	inline Point2D operator[](size_t i) { return points[i]; }

};


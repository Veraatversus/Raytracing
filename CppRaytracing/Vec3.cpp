#include <iostream>
#include <sstream>
#include <cmath>

#include "Vec3.h"
#include "Rand.h"

Vec3::Vec3() :
	e{ 0,0,0 }
{}

Vec3::Vec3(float x, float y, float z) :
	e{ x,y,z }
{}

Vec3::Vec3(const Vec3& other) :
	e{ other.e }
{}

const std::string Vec3::toString() const {
	std::stringstream s;
	s << "[" << e[0] << ", " << e[1] << ", " << e[2] << "]";
	return s.str();
}

float Vec3::x() const {
	return e[0];
}

float Vec3::y() const {
	return e[1];
}

float Vec3::z() const {
	return e[2];
}

float Vec3::r() const {
	return e[0];
}

float Vec3::g() const {
	return e[1];
}

float Vec3::b() const {
	return e[2];
}

Vec3 Vec3::operator+(const Vec3& val) const {
	return Vec3{ e[0] + val.e[0],
				e[1] + val.e[1],
				e[2] + val.e[2] };
}

Vec3 Vec3::operator-(const Vec3& val) const {
	return Vec3{ e[0] - val.e[0],
				e[1] - val.e[1],
				e[2] - val.e[2] };
}

Vec3 Vec3::operator*(const Vec3& val) const {
	return Vec3{ e[0] * val.e[0],
				e[1] * val.e[1],
				e[2] * val.e[2] };
}

Vec3 Vec3::operator/(const Vec3& val) const {
	return Vec3{ e[0] / val.e[0],
				e[1] / val.e[1],
				e[2] / val.e[2] };
}

Vec3 Vec3::operator*(const float& val) const {
	return Vec3{ e[0] * val,
				e[1] * val,
				e[2] * val };
}

Vec3 Vec3::operator/(const float& val) const {
	return Vec3{ e[0] / val,
				e[1] / val,
				e[2] / val };
}

float Vec3::length() const {
	return sqrt(sqlength());
}

float Vec3::sqlength() const {
	return e[0] * e[0] + e[1] * e[1] + e[2] * e[2];
}

float Vec3::dot(const Vec3& a, const Vec3& b) {
	return a.e[0] * b.e[0] + a.e[1] * b.e[1] + a.e[2] * b.e[2];
}

Vec3 Vec3::cross(const Vec3& a, const Vec3& b) {
	return Vec3{ a.e[1] * b.e[2] - a.e[2] * b.e[1],
				a.e[2] * b.e[0] - a.e[0] * b.e[2],
				a.e[0] * b.e[1] - a.e[1] * b.e[0] };
}

Vec3 Vec3::normalize(const Vec3& a) {
	return a / a.length();
}

Vec3 Vec3::reflect(const Vec3& a, const Vec3& n) {
	return a - n * dot(a, n) * 2;
}

Vec3 Vec3::refract(const Vec3& a, const Vec3& n,
	const float index) {
	const float cosTheta = fmin(-dot(a, n), 1.0f);
	const Vec3 rOutParallel{ (a + n * cosTheta) * index };
	const Vec3 rOutPerpendicular{ n * -sqrt(1.0f - fmin(rOutParallel.sqlength(), 1.0f)) };
	return rOutParallel + rOutPerpendicular;
}

Vec3 Vec3::random() {
	return Vec3{ Rand::rand01(),Rand::rand01(),Rand::rand01() };
}

Vec3 Vec3::randomPointInSphere() {
	while (true) {
		Vec3 p{ Rand::rand11(),Rand::rand11(),Rand::rand11() };
		if (p.sqlength() > 1) continue;
		return p;
	}
}

Vec3 Vec3::randomPointInDisc() {
	while (true) {
		Vec3 p{ Rand::rand01(),Rand::rand01(),0 };
		if (p.sqlength() > 1) continue;
		return p;
	}
}

Vec3 Vec3::randomUnitVector() {
	const float a = Rand::rand0Pi();
	const float z = Rand::rand11();
	const float r = sqrt(1.0f - z * z);

	return Vec3{ r * float(cos(a)), r * float(sin(a)), z };
}
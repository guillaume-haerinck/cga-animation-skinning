﻿using System;
using AlgeoSharp.Exceptions;
using UnityEngine;

namespace AlgeoSharp
{
    public static class IPNS
    {
        public static MultiVector CreatePoint(MultiVector v)
        {
            if (IPNS.GetGeometricEntity(v) != GeometricEntity.Vector)
                throw new InvalidEntityException();

            return v + 0.5f * MultiVector.ScalarProduct(v, v) * Basis.E8 + Basis.E0;
        }

        public static MultiVector CreatePoint(float e1, float e2, float e3)
        {
            MultiVector v = MultiVector.Vector(e1, e2, e3);
            return v + 0.5f * MultiVector.ScalarProduct(v, v) * Basis.E8 + Basis.E0;
        }

        public static MultiVector CreateSphere(MultiVector c, float r)
        {
            return IPNS.CreatePoint(c) - 0.5f * r * r * Basis.E8;
        }

        public static MultiVector CreateSphere(float e1, float e2, float e3, float r)
        {
            return IPNS.CreatePoint(e1, e2, e3) - 0.5f * r * r * Basis.E8;
        }

        public static MultiVector CreateSphere(MultiVector p1, MultiVector p2, MultiVector p3, MultiVector p4)
        {
            return (p1 ^ p2 ^ p3 ^ p4).Dual;
        }

        public static MultiVector CreatePlane(MultiVector n, float d)
        {
            return n + d * Basis.E8;
        }

        public static MultiVector CreatePlane(MultiVector p1, MultiVector p2, MultiVector p3)
        {
            return (p1 ^ p2 ^ p3 ^ Basis.E8).Dual;
        }

        public static MultiVector CreateLine(MultiVector t, MultiVector d)
        {
            return d * Basis.E123 + (MultiVector.CrossProduct(d, t) ^ Basis.E8);
        }

        public static MultiVector CreateLine2(MultiVector p1, MultiVector p2)
        {
            return (p1 ^ p2 ^ Basis.E8).Dual;
        }

        public static MultiVector CreateCircle(MultiVector n, MultiVector c, float r)
        {
            float d = (float)MultiVector.ScalarProduct(c, n);
            return IPNS.CreatePlane(n, d) ^ IPNS.CreateSphere(c, r);
        }

        public static MultiVector CreateCircle(MultiVector p1, MultiVector p2, MultiVector p3)
        {
            return (p1 ^ p2 ^ p3).Dual;
        }

        public static MultiVector CreatePointPair(MultiVector p1, MultiVector p2)
        {
            return (p1 ^ p2).Dual;
        }


        public static void GetPointParams(MultiVector obj, out MultiVector x)
        {
            MultiVector temp;
            float r;

            IPNS.GetSphereParams(obj, out temp, out r);

            if (r != 0f)
                throw new InvalidEntityException();

            x = temp;
        }

        public static void GetSphereParams(MultiVector obj, out MultiVector c, out float r)
        {
            if (!obj.ContainsOnly(1) || obj.E0 == 0.0)
                throw new InvalidEntityException();

            obj /= obj.E0;
            c = MultiVector.Vector(obj.E1, obj.E2, obj.E3);
            r = Mathf.Sqrt(Math.Abs(2 * obj.E8 - MultiVector.ScalarProduct(c, c)));

            // HACK
            if (Math.Abs(r) < 1E-3)
                r = 0f;
        }

        public static void GetPlaneParams(MultiVector obj, out MultiVector n, out float d)
        {
            if (!obj.ContainsOnly(1) || obj.E0 != 0.0)
                throw new InvalidEntityException();

            n = MultiVector.Vector(obj.E1, obj.E2, obj.E3);

            float norm = n.Length;

            n /= norm;
            d = obj.E8 / norm;
        }

        public static void GetLineParams(MultiVector obj, out MultiVector t, out MultiVector d)
        {
            if (!obj.ContainsOnly(2))
                throw new InvalidEntityException();

            d = MultiVector.Vector(
                -obj[Basis.E23],
                obj[Basis.E13],
                -obj[Basis.E12]);

            float norm = d.Length;

            if (norm == 0.0)
                throw new InvalidEntityException();

            d /= norm;

            // d cross t
            MultiVector dct = MultiVector.Vector(
                obj[Basis.E1 ^ Basis.E8],
                obj[Basis.E2 ^ Basis.E8],
                obj[Basis.E3 ^ Basis.E8]) / norm;

            t = MultiVector.CrossProduct(dct, d);
        }

        public static void GetCircleParams(MultiVector obj, out MultiVector n, out MultiVector c, out float r)
        {
            if (!obj.ContainsOnly(2))
                throw new InvalidEntityException();

            // c cross n
            MultiVector ccd = MultiVector.Vector(
                -obj[Basis.E23],
                obj[Basis.E13],
                -obj[Basis.E12]);

            // c dot n
            float d = obj[Basis.EPLANE];

            n = MultiVector.Vector(
                obj[Basis.E1 ^ Basis.E0],
                obj[Basis.E2 ^ Basis.E0],
                obj[Basis.E3 ^ Basis.E0]);

            c = MultiVector.CrossProduct(n, ccd) + n * d;
            c /= (float)MultiVector.ScalarProduct(n, n);

            float x;
            if (n.E1 != 0) x = (obj[Basis.E1 ^ Basis.E8] + (float)d * c.E1) / n.E1;
            else if (n.E2 != 0) x = (obj[Basis.E2 ^ Basis.E8] + (float)d * c.E2) / n.E2;
            else if (n.E3 != 0) x = (obj[Basis.E3 ^ Basis.E8] + (float)d * c.E3) / n.E3;
            else throw new InvalidEntityException();

            r = Mathf.Sqrt(MultiVector.ScalarProduct(c, c) - 2 * x);
            n /= n.Length;
        }

        public static void GetPointPairParams(MultiVector obj, out MultiVector p1, out MultiVector p2)
        {
            if (!obj.ContainsOnly(3))
                throw new InvalidEntityException();

            obj = obj.Dual;

            float delta = Mathf.Sqrt(Mathf.Abs((float) MultiVector.InnerProduct(obj, obj)));

            try
            {
                MultiVector temp = MultiVector.InnerProduct(Basis.E8, obj);
                p1 = (+delta + obj) / temp;
                p2 = (-delta + obj) / temp;
            }
            catch (InvalidOperationException)
            {
                throw new InvalidEntityException();
            }
        }

        public static GeometricEntity GetGeometricEntity(MultiVector obj)
        {
            if (obj.ContainsOnly(0))
            {
                return GeometricEntity.Scalar;
            }

            if (obj.ContainsOnly(1))
            {
                if (obj.E0 == 0 && obj.E8 == 0)
                    return GeometricEntity.Vector;

                if (obj.E0 == 0)
                    return GeometricEntity.Plane;

                MultiVector center; float radius;
                GetSphereParams(obj, out center, out radius);

                if (radius == 0)
                    return GeometricEntity.Point;

                return GeometricEntity.Sphere;
            }

            if (obj.ContainsOnly(2))
            {
                if (obj.ContainsOnly(Basis.E12, Basis.E13, Basis.E23))
                    return GeometricEntity.Bivector;

                if (MultiVector.InnerProduct(Basis.E8, obj) == 0f)
                    return GeometricEntity.Line;

                return GeometricEntity.Circle;
            }

            if (obj.ContainsOnly(3))
            {
                if (obj.ContainsOnly(Basis.E123))
                    return GeometricEntity.Trivector;

                try
                {
                    MultiVector p1, p2;
                    GetPointPairParams(obj, out p1, out p2);

                    return GeometricEntity.PointPair;
                }
                catch (InvalidEntityException)
                {
                    return GeometricEntity.Unknown;
                }
            }

            return GeometricEntity.Unknown;
        }
    }
}

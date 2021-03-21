using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AlgeoSharp
{
    public class MultiVector
    {
        public static readonly MultiVector Zero = new MultiVector();


        public static MultiVector Vector(float e1, float e2, float e3)
        {
            MultiVector v = new MultiVector();
            v.E1 = e1;
            v.E2 = e2;
            v.E3 = e3;
            return v;
        }


        public MultiVector()
        {
        }


        Dictionary<Basis, Blade> blades = new Dictionary<Basis, Blade>();


        public float this[Basis basis]
        {
            get
            {
                if (blades.ContainsKey(basis))
                    return blades[basis].Value;
                else
                    return 0f;
            }
            set
            {
                // HACK
                if (Math.Abs(value) < 1E-3)
                {
                    if (blades.ContainsKey(basis))
                        blades.Remove(basis);
                }
                else
                {
                    if (blades.ContainsKey(basis))
                        blades[basis] = new Blade(basis, value);
                    else
                        blades.Add(basis, new Blade(basis, value));
                }
            }
        }

        public float this[Blade blade]
        {
            get
            {
                return blade.Value * this[blade.Basis];
            }
            set
            {
                this[blade.Basis] = blade.Value * value;
            }
        }

        #region Grade 1 Blades

        public float E1 { get { return this[Basis.E1]; } set { this[Basis.E1] = value; } }
        public float E2 { get { return this[Basis.E2]; } set { this[Basis.E2] = value; } }
        public float E3 { get { return this[Basis.E3]; } set { this[Basis.E3] = value; } }

        public float E0 { get { return this[Basis.E0]; } set { this[Basis.E0] = value; } }
        public float E8 { get { return this[Basis.E8]; } set { this[Basis.E8] = value; } }

        #endregion

        public Blade[] Blades
        {
            get
            {
                return blades.Values.ToArray();
            }
        }

        public int Grade
        {
            get
            {
                if (this.Blades.Length == 0)
                    return 0;

                return this.Blades.Max(x => x.Basis.Grade);
            }
        }

        public bool ContainsOnly(params Basis[] bases)
        {
            foreach (Blade b in this.Blades)
            {
                if (!bases.Contains(b.Basis))
                    return false;
            }

            return true;
        }

        public bool ContainsOnly(int grade)
        {
            foreach (Blade b in this.Blades)
            {
                if (b.Basis.Grade != grade)
                    return false;
            }

            return true;
        }


        public bool IsHomogeneous
        {
            get
            {
                if (this.Blades.Length == 0)
                    return true;

                int grade = this.Blades[0].Basis.Grade;

                for (int i = 1; i < this.Blades.Length; i++)
                {
                    if (this.Blades[i].Basis.Grade != grade)
                        return false;
                }

                return true;
            }
        }

        public MultiVector Dual
        {
            get
            {
                return MultiVector.InnerProduct(this, -Basis.PS);
            }
        }

        public MultiVector Reverse
        {
            get
            {
                MultiVector ret = new MultiVector();

                foreach (Blade blade in this.Blades)
                {
                    if ((blade.Basis.Grade / 2) % 2 == 0)
                        ret[blade.Basis] = blade.Value;
                    else
                        ret[blade.Basis] = -blade.Value;
                }

                return ret;
            }
        }

        public MultiVector Conjugate
        {
            get
            {
                MultiVector ret = new MultiVector();

                foreach (Blade blade in this.Blades)
                {
                    float reverse;

                    if ((blade.Basis.Grade / 2) % 2 == 0)
                        reverse = blade.Value;
                    else
                        reverse = -blade.Value;

                    if (blade.Basis.Grade % 2 == 0)
                        ret[blade.Basis] = reverse;
                    else
                        ret[blade.Basis] = -reverse;
                }

                return ret;
            }
        }

        public MultiVector Involution
        {
            get
            {
                MultiVector ret = new MultiVector();

                foreach (Blade blade in this.Blades)
                {
                    if (blade.Basis.Grade % 2 == 0)
                        ret += blade;
                    else
                        ret += -blade;
                }

                return ret;
            }
        }

        public MultiVector Inverse
        {
            get
            {
                try
                {
                    return this / (float)(this * this.Reverse);
                }
                catch (InvalidCastException)
                {
                    throw new InvalidOperationException();
                }
            }
        }

        public float Length
        {
            get
            {
                float sum = 0f;
                foreach (Blade b in this.Blades)
                    sum += b.Value * b.Value;
                return Mathf.Sqrt(sum);
            }
        }


        public static implicit operator MultiVector(Basis e)
        {
            return new Blade(e, 1f);
        }

        public static implicit operator MultiVector(float value)
        {
            return new Blade(Basis.S, value);
        }

        public static implicit operator MultiVector(Blade blade)
        {
            MultiVector result = new MultiVector();
            result[blade.Basis] = blade.Value;
            return result;
        }

        public static explicit operator float(MultiVector v)
        {
            return (float)((Blade)v);
        }

        public static explicit operator Blade(MultiVector v)
        {
            if (v.blades.Count == 0)
                return Blade.Zero;

            if (v.blades.Count == 1)
                return v.Blades.First();

            throw new InvalidCastException();
        }


        public static MultiVector operator +(MultiVector v1, MultiVector v2)
        {
            MultiVector result = v1.Clone();
            foreach (Blade blade in v2.Blades)
                result[blade.Basis] += blade.Value;
            return result;
        }

        public static MultiVector operator -(MultiVector v1, MultiVector v2)
        {
            MultiVector result = v1.Clone();
            foreach (Blade blade in v2.Blades)
                result[blade.Basis] -= blade.Value;
            return result;
        }

        public static MultiVector operator -(MultiVector v)
        {
            MultiVector result = new MultiVector();
            foreach (Blade blade in v.Blades)
                result[blade.Basis] = -blade.Value;
            return result;
        }

        public static MultiVector operator *(float f, MultiVector v)
        {
            MultiVector result = new MultiVector();
            foreach (Blade b in v.Blades)
                result[b.Basis] = f * b.Value;
            return result;
        }

        public static MultiVector operator *(MultiVector v, float f)
        {
            return f * v;
        }

        public static MultiVector operator /(MultiVector v, float f)
        {
            return (1f / f) * v;
        }

        public static MultiVector operator *(MultiVector v1, MultiVector v2)
        {
            return MultiVector.GeometricProduct(v1, v2);
        }

        public static MultiVector operator /(MultiVector v1, MultiVector v2)
        {
            return v1 * v2.Inverse;
        }

        public static MultiVector operator ^(MultiVector v1, MultiVector v2)
        {
            return MultiVector.OuterProduct(v1, v2);
        }


        public static bool operator ==(MultiVector v1, MultiVector v2)
        {
            List<Basis> toCheck = v1.blades.Keys.Union(v2.blades.Keys).ToList();

            foreach (Basis b in toCheck)
            {
                if (v1[b] != v2[b])
                    return false;
            }

            return true;
        }

        public static bool operator !=(MultiVector v1, MultiVector v2)
        {
            return !(v1 == v2);
        }

        public override int GetHashCode()
        {
            int hash = 0;
            foreach (Blade b in this.Blades)
                hash = hash ^ b.GetHashCode();
            return hash;
        }

        public override bool Equals(object obj)
        {
            if (obj is MultiVector)
                return this == (obj as MultiVector);
            else
                return false;
        }


        public static MultiVector GeometricProduct(MultiVector v1, MultiVector v2)
        {
            MultiVector result = new MultiVector();

            foreach (Blade b1 in v1.Blades)
                foreach (Blade b2 in v2.Blades)
                    result += Blade.GeometricProduct(b1, b2);

            return result;
        }

        public static MultiVector OuterProduct(MultiVector v1, MultiVector v2)
        {
            MultiVector result = new MultiVector();

            foreach (Blade b1 in v1.Blades)
                foreach (Blade b2 in v2.Blades)
                    result += Blade.OuterProduct(b1, b2);

            return result;
        }

        public static MultiVector InnerProduct(MultiVector v1, MultiVector v2)
        {
            MultiVector result = new MultiVector();

            foreach (Blade b1 in v1.Blades)
                foreach (Blade b2 in v2.Blades)
                    result += Blade.InnerProduct(b1, b2);

            return result;
        }

        public static MultiVector CrossProduct(MultiVector v1, MultiVector v2)
        {
            return (v2 ^ v1) * Basis.E123;
        }

        public static float ScalarProduct(MultiVector v1, MultiVector v2)
        {
            float result = 0;

            foreach (Blade b in v1.Blades)
            {
                result += b.Value * v2[b.Basis];
            }

            return result;
        }


        public MultiVector Clone()
        {
            MultiVector result = new MultiVector();
            foreach (Blade blade in this.Blades)
                result[blade.Basis] = blade.Value;
            return result;
        }

        public MultiVector ToMinkowskiMetric()
        {
            MultiVector result = new MultiVector();
            foreach (Blade blade in this.Blades)
                result += blade.ToMinkowskiMetric();
            return result;
        }

        public MultiVector ToNullMetric()
        {
            MultiVector result = new MultiVector();
            foreach (Blade blade in this.Blades)
                result += blade.ToNullMetric();
            return result;
        }

        public override string ToString()
        {
            List<Blade> blades = this.Blades.ToList();
            blades.Sort((x, y) => Basis.DisplayOrder(x.Basis, y.Basis));

            return String.Join(" + ", blades.ToArray());
        }
    }
}

﻿// © Alexander Kozlenko. Licensed under the MIT License.

using System.Data.JsonRpc.Resources;

namespace System.Data.JsonRpc
{
    /// <summary>Represents a JSON-RPC message identifier.</summary>
    public readonly struct JsonRpcId : IEquatable<JsonRpcId>, IComparable<JsonRpcId>
    {
        private readonly double _valueFloat;
        private readonly long _valueInteger;
        private readonly string _valueString;
        private readonly JsonRpcIdType _type;

        /// <summary>Initializes a new instance of the <see cref="JsonRpcId" /> structure.</summary>
        /// <param name="value">The identifier value.</param>
        /// <exception cref="ArgumentNullException"><paramref name="value" /> is <see langword="null" />.</exception>
        public JsonRpcId(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            _type = JsonRpcIdType.String;
            _valueString = value;
            _valueInteger = default;
            _valueFloat = default;
        }

        /// <summary>Initializes a new instance of the <see cref="JsonRpcId" /> structure.</summary>
        /// <param name="value">The identifier value.</param>
        public JsonRpcId(long value)
        {
            _type = JsonRpcIdType.Integer;
            _valueString = default;
            _valueInteger = value;
            _valueFloat = default;
        }

        /// <summary>Initializes a new instance of the <see cref="JsonRpcId" /> structure.</summary>
        /// <param name="value">The identifier value.</param>
        /// <exception cref="ArgumentException"><paramref name="value" /> is <see cref="double.NaN" />, or <see cref="double.NegativeInfinity" />, or <see cref="double.PositiveInfinity" />.</exception>
        public JsonRpcId(double value)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                throw new ArgumentException(Strings.GetString("id.invalid_float"), nameof(value));
            }

            _type = JsonRpcIdType.Float;
            _valueString = default;
            _valueInteger = default;
            _valueFloat = value;
        }

        bool IEquatable<JsonRpcId>.Equals(JsonRpcId other)
        {
            return Equals(in other);
        }

        int IComparable<JsonRpcId>.CompareTo(JsonRpcId other)
        {
            return CompareTo(in other);
        }

        internal string UnsafeAsString()
        {
            return _valueString;
        }

        internal long UnsafeAsInteger()
        {
            return _valueInteger;
        }

        internal double UnsafeAsFloat()
        {
            return _valueFloat;
        }

        /// <summary>Compares the current <see cref="JsonRpcId" /> with another <see cref="JsonRpcId" /> and returns an integer that indicates whether the current <see cref="JsonRpcId" /> precedes, follows, or occurs in the same position in the sort order as the other <see cref="JsonRpcId" />.</summary>
        /// <param name="other">A <see cref="JsonRpcId" /> to compare with the current <see cref="JsonRpcId" />.</param>
        /// <returns>A value that indicates the relative order of the objects being compared.</returns>
        [CLSCompliant(false)]
        public int CompareTo(in JsonRpcId other)
        {
            var result = ((int)_type).CompareTo((int)other._type);

            if (result != 0)
            {
                return result;
            }

            switch (_type)
            {
                case JsonRpcIdType.String:
                    {
                        return string.CompareOrdinal(_valueString, other._valueString);
                    }
                case JsonRpcIdType.Integer:
                    {
                        return _valueInteger.CompareTo(other._valueInteger);
                    }
                case JsonRpcIdType.Float:
                    {
                        return _valueFloat.CompareTo(other._valueFloat);
                    }
                default:
                    {
                        return 0;
                    }
            }
        }

        /// <summary>Indicates whether the current <see cref="JsonRpcId" /> is equal to another <see cref="JsonRpcId" />.</summary>
        /// <param name="other">A <see cref="JsonRpcId" /> to compare with the current <see cref="JsonRpcId" />.</param>
        /// <returns><see langword="true" /> if the current <see cref="JsonRpcId" /> is equal to the other <see cref="JsonRpcId" />; otherwise, <see langword="false" />.</returns>
        [CLSCompliant(false)]
        public bool Equals(in JsonRpcId other)
        {
            if (_type != other._type)
            {
                return false;
            }

            switch (_type)
            {
                case JsonRpcIdType.String:
                    {
                        return string.Equals(_valueString, other._valueString);
                    }
                case JsonRpcIdType.Integer:
                    {
                        return _valueInteger.Equals(other._valueInteger);
                    }
                case JsonRpcIdType.Float:
                    {
                        return _valueFloat.Equals(other._valueFloat);
                    }
                default:
                    {
                        return true;
                    }
            }
        }

        /// <summary>Indicates whether the current <see cref="JsonRpcId" /> is equal to the specified object.</summary>
        /// <param name="obj">The object to compare with the current <see cref="JsonRpcId" />.</param>
        /// <returns><see langword="true" /> if the current <see cref="JsonRpcId" /> is equal to the specified object; otherwise, <see langword="false" />.</returns>
        public override bool Equals(object obj)
        {
            switch (obj)
            {
                case JsonRpcId other:
                    {
                        return Equals(in other);
                    }
                case string other:
                    {
                        return (_type == JsonRpcIdType.String) && string.Equals(_valueString, other);
                    }
                case long other:
                    {
                        return (_type == JsonRpcIdType.Integer) && _valueInteger.Equals(other);
                    }
                case double other:
                    {
                        return (_type == JsonRpcIdType.Float) && _valueFloat.Equals(other);
                    }
                default:
                    {
                        return false;
                    }
            }
        }

        /// <summary>Returns the hash code for the current <see cref="JsonRpcId" />.</summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int)2166136261;

                hashCode ^= _type.GetHashCode();
                hashCode *= 16777619;

                switch (_type)
                {
                    case JsonRpcIdType.String:
                        {
                            hashCode ^= _valueString.GetHashCode();
                            hashCode *= 16777619;
                        }
                        break;
                    case JsonRpcIdType.Integer:
                        {
                            hashCode ^= _valueInteger.GetHashCode();
                            hashCode *= 16777619;
                        }
                        break;
                    case JsonRpcIdType.Float:
                        {
                            hashCode ^= _valueFloat.GetHashCode();
                            hashCode *= 16777619;
                        }
                        break;
                }

                return hashCode;
            }
        }

        /// <summary>Converts the current <see cref="JsonRpcId" /> to its equivalent string representation.</summary>
        /// <returns>The string representation of the current <see cref="JsonRpcId" />.</returns>
        public override string ToString()
        {
            return ToString(null);
        }

        /// <summary>Converts the current <see cref="JsonRpcId" /> to its equivalent string representation.</summary>
        /// <param name="provider">An <see cref="IFormatProvider" /> that supplies culture-specific formatting information.</param>
        /// <returns>The string representation of the current <see cref="JsonRpcId" />.</returns>
        public string ToString(IFormatProvider provider)
        {
            switch (_type)
            {
                case JsonRpcIdType.String:
                    {
                        return _valueString;
                    }
                case JsonRpcIdType.Integer:
                    {
                        return _valueInteger.ToString(provider);
                    }
                case JsonRpcIdType.Float:
                    {
                        return _valueFloat.ToString(provider);
                    }
                default:
                    {
                        return string.Empty;
                    }
            }
        }

        /// <summary>Indicates whether the left <see cref="JsonRpcId" /> is equal to the right <see cref="JsonRpcId" />.</summary>
        /// <param name="obj1">The left <see cref="JsonRpcId" /> operand.</param>
        /// <param name="obj2">The right <see cref="JsonRpcId" /> operand.</param>
        /// <returns><see langword="true" /> if the left <see cref="JsonRpcId" /> is equal to the right <see cref="JsonRpcId" />; otherwise, <see langword="false" />.</returns>
        public static bool operator ==(in JsonRpcId obj1, in JsonRpcId obj2)
        {
            return obj1.Equals(in obj2);
        }

        /// <summary>Indicates whether the left <see cref="JsonRpcId" /> is not equal to the right <see cref="JsonRpcId" />.</summary>
        /// <param name="obj1">The left <see cref="JsonRpcId" /> operand.</param>
        /// <param name="obj2">The right <see cref="JsonRpcId" /> operand.</param>
        /// <returns><see langword="true" /> if the left <see cref="JsonRpcId" /> is not equal to the right <see cref="JsonRpcId" />; otherwise, <see langword="false" />.</returns>
        public static bool operator !=(in JsonRpcId obj1, in JsonRpcId obj2)
        {
            return !obj1.Equals(in obj2);
        }

        /// <summary>Performs an implicit conversion from <see cref="string" /> to <see cref="JsonRpcId" />.</summary>
        /// <param name="value">The value to create a <see cref="JsonRpcId" /> from.</param>
        public static implicit operator JsonRpcId(string value)
        {
            return new JsonRpcId(value);
        }

        /// <summary>Performs an implicit conversion from <see cref="ulong" /> to <see cref="JsonRpcId" />.</summary>
        /// <param name="value">The value to create a <see cref="JsonRpcId" /> from.</param>
        public static implicit operator JsonRpcId(long value)
        {
            return new JsonRpcId(value);
        }

        /// <summary>Performs an implicit conversion from <see cref="double" /> to <see cref="JsonRpcId" />.</summary>
        /// <param name="value">The value to create a <see cref="JsonRpcId" /> from.</param>
        public static implicit operator JsonRpcId(double value)
        {
            return new JsonRpcId(value);
        }

        /// <summary>Performs an implicit conversion from <see cref="JsonRpcId" /> to <see cref="string" />.</summary>
        /// <param name="value">The identifier to get a <see cref="string" /> value from.</param>
        /// <exception cref="InvalidCastException">The underlying value is not of type <see cref="string" />.</exception>
        public static explicit operator string(in JsonRpcId value)
        {
            if (value._type != JsonRpcIdType.String)
            {
                throw new InvalidCastException(string.Format(Strings.GetString("id.invalid_cast"), typeof(JsonRpcId), typeof(string)));
            }

            return value._valueString;
        }

        /// <summary>Performs an implicit conversion from <see cref="JsonRpcId" /> to <see cref="long" />.</summary>
        /// <param name="value">The identifier to get a <see cref="long" /> value from.</param>
        /// <exception cref="InvalidCastException">The underlying value is not of type <see cref="long" />.</exception>
        public static explicit operator long(in JsonRpcId value)
        {
            if (value._type != JsonRpcIdType.Integer)
            {
                throw new InvalidCastException(string.Format(Strings.GetString("id.invalid_cast"), typeof(JsonRpcId), typeof(long)));
            }

            return value._valueInteger;
        }

        /// <summary>Performs an implicit conversion from <see cref="JsonRpcId" /> to <see cref="double" />.</summary>
        /// <param name="value">The identifier to get a <see cref="double" /> value from.</param>
        /// <exception cref="InvalidCastException">The underlying value is not of type <see cref="double" />.</exception>
        public static explicit operator double(in JsonRpcId value)
        {
            if (value._type != JsonRpcIdType.Float)
            {
                throw new InvalidCastException(string.Format(Strings.GetString("id.invalid_cast"), typeof(JsonRpcId), typeof(double)));
            }

            return value._valueFloat;
        }

        /// <summary>Gets the JSON-RPC message identifier type.</summary>
        public JsonRpcIdType Type
        {
            get => _type;
        }
    }
}
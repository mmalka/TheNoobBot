using System;
using System.ComponentModel;
using System.Globalization;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using nManager.Wow.Enums;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;

namespace nManager.Wow.Class
{
    /// <summary>
    ///     Represents a 128-bit signed integer.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    [TypeConverter(typeof (UInt128Converter))]
    public struct UInt128 : IComparable<UInt128>, IComparable, IEquatable<UInt128>, IConvertible, IFormattable
    {
        private ulong _hi;
        private ulong _lo;

        public ulong High
        {
            get { return _hi; }
            set { _hi = value; }
        }

        public ulong Low
        {
            get { return _lo; }
            set { _lo = value; }
        }

        /// <summary>
        ///     The number of bytes this type will take.
        /// </summary>
        public const int SizeOf = 16;

        /// <summary>
        ///     A Zero UInt128 value.
        /// </summary>
        public static readonly UInt128 Zero = 0;

        /// <summary>
        ///     A One UInt128 value.
        /// </summary>
        public static readonly UInt128 One = 1;

        /// <summary>
        ///     Represents the smallest possible value of an UInt128.
        /// </summary>
        public static readonly UInt128 MinValue = 0;

        /// <summary>
        ///     Represents the largest possible value of an UInt128.
        /// </summary>
        public static readonly UInt128 MaxValue = new UInt128(ulong.MaxValue, ulong.MaxValue);

        public GuidType GetWoWType
        {
            get { return (GuidType) (_hi >> 58); }
            set { _hi |= (ulong) value << 58; }
        }

        public GuidSubType GetWoWSubType
        {
            get { return (GuidSubType) (_lo >> 56); }
            set { _lo |= (ulong) value << 56; }
        }

        public ushort GetWoWRealmId
        {
            get { return (ushort) ((_hi >> 42) & 0x1FFF); }
            set { _hi |= (ulong) value << 42; }
        }

        public ushort GetWoWServerId
        {
            get { return (ushort) ((_lo >> 40) & 0x1FFF); }
            set { _lo |= (ulong) value << 40; }
        }

        public ushort GetWoWMapId
        {
            get { return (ushort) ((_hi >> 29) & 0x1FFF); }
            set { _hi |= (ulong) value << 29; }
        }

        public uint GetWoWId
        {
            get { return (uint) (_hi & 0xFFFFFF) >> 6; }
            set { _hi |= (ulong) value << 6; }
        }

        public ulong CreationBits
        {
            get { return _lo & 0xFFFFFFFFFF; }
            set { _lo |= value; }
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="UInt128" /> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        public UInt128(byte value)
        {
            _hi = 0;
            _lo = value;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="UInt128" /> struct.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public UInt128(bool value)
        {
            _hi = 0;
            _lo = (ulong) (value ? 1 : 0);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="UInt128" /> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        public UInt128(char value)
        {
            _hi = 0;
            _lo = value;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="UInt128" /> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        public UInt128(decimal value)
        {
            int[] bits = decimal.GetBits(value);
            _hi = (uint) bits[2];
            _lo = (uint) bits[0] | (ulong) bits[1] << 32;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="UInt128" /> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        public UInt128(double value)
            : this((decimal) value)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="UInt128" /> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        public UInt128(float value)
            : this((decimal) value)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="UInt128" /> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        public UInt128(short value)
        {
            _hi = 0;
            _lo = (ulong) value;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="UInt128" /> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        public UInt128(int value)
        {
            _hi = 0;
            _lo = (ulong) value;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="UInt128" /> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        public UInt128(long value)
        {
            _hi = 0;
            _lo = (ulong) value;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="UInt128" /> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        public UInt128(sbyte value)
        {
            _hi = 0;
            _lo = (ulong) value;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="UInt128" /> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        public UInt128(ushort value)
        {
            _hi = 0;
            _lo = value;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="UInt128" /> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        public UInt128(uint value)
        {
            _hi = 0;
            _lo = value;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="UInt128" /> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        public UInt128(ulong value)
        {
            _hi = 0;
            _lo = value;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="UInt128" /> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        public UInt128(Guid value)
            : this(value.ToByteArray())
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="UInt128" /> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        public UInt128(byte[] value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            if (value.Length != 16)
                throw new ArgumentException(null, "value");

            _hi = BitConverter.ToUInt64(value, 8);
            _lo = BitConverter.ToUInt64(value, 0);
        }

        /// <summary>
        ///     Creates a value using two 64 bit values.
        /// </summary>
        /// <param name="hi">The most significant 64 bits of the value.</param>
        /// <param name="lo">The least significant 64 bits of the value.</param>
        private UInt128(ulong hi, ulong lo)
        {
            _hi = hi;
            _lo = lo;
        }


        /// <summary>
        ///     Conversion of a <see cref="BigInteger" /> object to an unsigned 128-bit integer value.
        /// </summary>
        /// <param name="value">The value to convert to an unsigned 128-bit integer.</param>
        /// <exception cref="OverflowException">
        ///     The <paramref name="value" /> parameter represents a number less than
        ///     <see cref="UInt128.MinValue" /> or greater than <see cref="UInt128.MaxValue" />.
        /// </exception>
        public UInt128(BigInteger value)
            : this((ulong) (value >> 64), (ulong) (value & ulong.MaxValue))
        {
        }

        /// <summary>
        ///     Defines an explicit conversion of a <see cref="BigInteger" /> object to an unsigned 128-bit integer value.
        /// </summary>
        /// <param name="value">The value to convert to an unsigned 128-bit integer.</param>
        /// <returns>The 128 bit value created by equivalent to <paramref name="value" />.</returns>
        /// <exception cref="OverflowException">
        ///     The <paramref name="value" /> parameter represents a number less than
        ///     <see cref="UInt128.MinValue" /> or greater than <see cref="UInt128.MaxValue" />.
        /// </exception>
        public static explicit operator UInt128(BigInteger value)
        {
            return new UInt128(value);
        }

        /// <summary>
        ///     Converts the 128 bits unsigned integer to a <see cref="BigInteger" />.
        /// </summary>
        /// <param name="value">The 128 bit value to convert.</param>
        /// <returns>The <see cref="BigInteger" /> value converted from the 128 bit value.</returns>
        public static implicit operator BigInteger(UInt128 value)
        {
            return value.ToBigInteger();
        }

        private BigInteger ToBigInteger()
        {
            BigInteger value = _hi;
            value <<= 64;
            value += _lo;
            return value;
        }

        /// <summary>
        ///     Converts a 64 bit unsigned integer to a 128 bit unsigned integer by taking all the 64 bits.
        /// </summary>
        /// <param name="value">The 64 bit value to convert.</param>
        /// <returns>The 128 bit value created by taking all the 64 bits of the 64 bit value.</returns>
        public static implicit operator UInt128(ulong value)
        {
            return new UInt128(0, value);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="UInt128" /> struct.
        /// </summary>
        /// <param name="ints">The ints.</param>
        public UInt128(uint[] ints)
        {
            if (ints == null)
                throw new ArgumentNullException("ints");

            var lo = new byte[8];
            var hi = new byte[8];

            if (ints.Length > 0)
            {
                Array.Copy(BitConverter.GetBytes(ints[0]), 0, lo, 0, 4);
                if (ints.Length > 1)
                {
                    Array.Copy(BitConverter.GetBytes(ints[1]), 0, lo, 4, 4);
                    if (ints.Length > 2)
                    {
                        Array.Copy(BitConverter.GetBytes(ints[2]), 0, hi, 0, 4);
                        if (ints.Length > 3)
                        {
                            Array.Copy(BitConverter.GetBytes(ints[3]), 0, hi, 4, 4);
                        }
                    }
                }
            }

            _lo = BitConverter.ToUInt64(lo, 0);
            _hi = BitConverter.ToUInt64(hi, 0);
        }

        /// <summary>
        ///     Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        ///     A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            return _hi.GetHashCode() ^ _lo.GetHashCode();
        }

        /// <summary>
        ///     Returns a value indicating whether this instance is equal to a specified object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>
        ///     true if obj has the same value as this instance; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            return (obj is UInt128) && base.Equals(obj);
        }

        /// <summary>
        ///     Returns a value indicating whether this instance is equal to a specified Int64 value.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>
        ///     true if obj has the same value as this instance; otherwise, false.
        /// </returns>
        public bool Equals(UInt128 obj)
        {
            return _hi == obj._hi && _lo == obj._lo;
        }

        /// <summary>
        /// Converts the numeric value of the current <see cref="UInt128"/> object to its equivalent string representation by using the specified format and culture-specific format information.
        /// </summary>
        /// <param name="format">A standard or custom numeric format string.</param>
        /// <param name="formatProvider">An object that supplies culture-specific formatting information.</param>
        /// <returns>The string representation of the current <see cref="UInt128"/> value as specified by the <paramref name="format"/> and <paramref name="formatProvider"/> parameters.</returns>
        /// <exception cref="FormatException"><paramref name="format"/> is not a valid format string.</exception>
        /// <remarks>
        /// The <paramref name="format"/> parameter can be any valid standard numeric format specifier, or any combination of custom numeric format specifiers. 
        /// If <paramref name="format"/> is equal to <see cref="String.Empty"/> or is <see langword="null"/>, the return value of the current <see cref="UInt128"/> object is formatted with the general format specifier ("G"). 
        /// If <paramref name="format"/> is any other value, the method throws a <see cref="FormatException"/>.
        /// <para>
        /// The <paramref name="formatProvider"/> parameter is an <see cref="IFormatProvider"/> implementation. 
        /// Its <see cref="IFormatProvider.GetFormat"/> method returns a <see cref="NumberFormatInfo"/> object that provides culture-specific information about the format of the string returned by this method. 
        /// When the <see cref="ToString(String, IFormatProvider)"/> method is invoked, it calls the <paramref name="formatProvider"/> parameter's <see cref="IFormatProvider.GetFormat"/> method and passes it a <see cref="Type"/> object that represents the <see cref="NumberFormatInfo"/> type. 
        /// The <see cref="IFormatProvider.GetFormat"/> method then returns the <see cref="NumberFormatInfo"/> object that provides information for formatting the <see cref="UInt128"/> object, such as the negative sign symbol, the group separator symbol, or the decimal point symbol. 
        /// There are three ways to use the <paramref name="formatProvider"/> parameter to supply formatting information to the <see cref="ToString(String, IFormatProvider)"/> method: 
        /// <list type="bullet">
        ///   <item>
        ///   You can pass a <see cref="CultureInfo"/> object that represents the culture that provides numeric formatting information. 
        ///   Its <see cref="CultureInfo.GetFormat"/> method returns the <see cref="NumberFormatInfo"/> object that provides numeric formatting information.
        ///   </item>
        ///   <item>You can pass the actual <see cref="NumberFormatInfo"/> object that provides formatting information. (Its implementation of <see cref="NumberFormatInfo.GetFormat"/> just returns itself.)</item>
        ///   <item>
        ///   You can pas a custom object that implements <see cref="IFormatProvider"/> and uses the <see cref="IFormatProvider.GetFormat"/> method 
        ///   to instantiate and return the <see cref="NumberFormatInfo"/> object that provides formatting information.
        ///   </item>
        /// </list>
        /// If <paramref name="formatProvider"/> is <see langword="null"/>, the formatting of the returned string is based on the <see cref="NumberFormatInfo "/> object of the current culture.
        /// </para>
        /// </remarks>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return ((BigInteger)this).ToString(format, formatProvider);
        }

        /// <summary>
        /// Converts the numeric value of the current <see cref="UInt128"/> object to its equivalent string representation by using the specified format.
        /// Uses <see cref="CultureInfo.CurrentCulture"/> as the format provider.
        /// </summary>
        /// <param name="format">A standard or custom numeric format string.</param>
        /// <returns>The string representation of the current <see cref="UInt128"/> value as specified by the <paramref name="format"/> parameter.</returns>
        /// <exception cref="FormatException"><paramref name="format"/> is not a valid format string.</exception>
        /// <remarks>
        /// See <see cref="ToString(string, IFormatProvider)"/> for remarks.
        /// </remarks>
        public string ToString(string format)
        {
            return ToString(format, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Converts the numeric value of the current <see cref="UInt128"/> object to its equivalent string representation by using the specified culture-specific format information.
        /// Uses "G" format.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <returns>The string representation of the current <see cref="UInt128"/> value as specified by the <paramref name="provider"/> parameter.</returns>
        /// <remarks>
        /// See <see cref="ToString(string, IFormatProvider)"/> for remarks.
        /// </remarks>
        public string ToString(IFormatProvider provider)
        {
            return ToString("G", provider);
        }

        /// <summary>
        /// Converts the numeric value of the current <see cref="UInt128"/> object to its equivalent string representation.
        /// Uses "G" format.
        /// Uses <see cref="CultureInfo.CurrentCulture"/> as the format provider.
        /// </summary>
        /// <returns>The string representation of the current <see cref="UInt128"/> value.</returns>
        /// <remarks>
        /// See <see cref="ToString(string, IFormatProvider)"/> for remarks.
        /// </remarks>
        public override string ToString()
        {
            return ToString(CultureInfo.CurrentCulture);
        }

        /// <summary>
        ///     Returns the <see cref="T:System.TypeCode" /> for this instance.
        /// </summary>
        /// <returns>
        ///     The enumerated constant that is the <see cref="T:System.TypeCode" /> of the class or value type that implements
        ///     this interface.
        /// </returns>
        TypeCode IConvertible.GetTypeCode()
        {
            return TypeCode.Object;
        }

        /// <summary>
        ///     Converts the value of this instance to an equivalent Boolean value using the specified culture-specific formatting
        ///     information.
        /// </summary>
        /// <param name="provider">
        ///     An <see cref="T:System.IFormatProvider" /> interface implementation that supplies
        ///     culture-specific formatting information.
        /// </param>
        /// <returns>
        ///     A Boolean value equivalent to the value of this instance.
        /// </returns>
        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            return (bool) this;
        }

        /// <summary>
        ///     Converts the value of this instance to an equivalent 8-bit unsigned integer using the specified culture-specific
        ///     formatting information.
        /// </summary>
        /// <param name="provider">
        ///     An <see cref="T:System.IFormatProvider" /> interface implementation that supplies
        ///     culture-specific formatting information.
        /// </param>
        /// <returns>
        ///     An 8-bit unsigned integer equivalent to the value of this instance.
        /// </returns>
        byte IConvertible.ToByte(IFormatProvider provider)
        {
            return (byte) this;
        }

        /// <summary>
        ///     Converts the value of this instance to an equivalent Unicode character using the specified culture-specific
        ///     formatting information.
        /// </summary>
        /// <param name="provider">
        ///     An <see cref="T:System.IFormatProvider" /> interface implementation that supplies
        ///     culture-specific formatting information.
        /// </param>
        /// <returns>
        ///     A Unicode character equivalent to the value of this instance.
        /// </returns>
        char IConvertible.ToChar(IFormatProvider provider)
        {
            return (char) this;
        }

        /// <summary>
        ///     Converts the value of this instance to an equivalent <see cref="T:System.DateTime" /> using the specified
        ///     culture-specific formatting information.
        /// </summary>
        /// <param name="provider">
        ///     An <see cref="T:System.IFormatProvider" /> interface implementation that supplies
        ///     culture-specific formatting information.
        /// </param>
        /// <returns>
        ///     A <see cref="T:System.DateTime" /> instance equivalent to the value of this instance.
        /// </returns>
        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        /// <summary>
        ///     Converts the value of this instance to an equivalent <see cref="T:System.Decimal" /> number using the specified
        ///     culture-specific formatting information.
        /// </summary>
        /// <param name="provider">
        ///     An <see cref="T:System.IFormatProvider" /> interface implementation that supplies
        ///     culture-specific formatting information.
        /// </param>
        /// <returns>
        ///     A <see cref="T:System.Decimal" /> number equivalent to the value of this instance.
        /// </returns>
        decimal IConvertible.ToDecimal(IFormatProvider provider)
        {
            return (decimal) this;
        }

        /// <summary>
        ///     Converts the value of this instance to an equivalent double-precision floating-point number using the specified
        ///     culture-specific formatting information.
        /// </summary>
        /// <param name="provider">
        ///     An <see cref="T:System.IFormatProvider" /> interface implementation that supplies
        ///     culture-specific formatting information.
        /// </param>
        /// <returns>
        ///     A double-precision floating-point number equivalent to the value of this instance.
        /// </returns>
        double IConvertible.ToDouble(IFormatProvider provider)
        {
            return (double) this;
        }

        /// <summary>
        ///     Converts the value of this instance to an equivalent 16-bit signed integer using the specified culture-specific
        ///     formatting information.
        /// </summary>
        /// <param name="provider">
        ///     An <see cref="T:System.IFormatProvider" /> interface implementation that supplies
        ///     culture-specific formatting information.
        /// </param>
        /// <returns>
        ///     An 16-bit signed integer equivalent to the value of this instance.
        /// </returns>
        short IConvertible.ToInt16(IFormatProvider provider)
        {
            return (short) this;
        }

        /// <summary>
        ///     Converts the value of this instance to an equivalent 32-bit signed integer using the specified culture-specific
        ///     formatting information.
        /// </summary>
        /// <param name="provider">
        ///     An <see cref="T:System.IFormatProvider" /> interface implementation that supplies
        ///     culture-specific formatting information.
        /// </param>
        /// <returns>
        ///     An 32-bit signed integer equivalent to the value of this instance.
        /// </returns>
        int IConvertible.ToInt32(IFormatProvider provider)
        {
            return (int) this;
        }

        /// <summary>
        ///     Converts the value of this instance to an equivalent 64-bit signed integer using the specified culture-specific
        ///     formatting information.
        /// </summary>
        /// <param name="provider">
        ///     An <see cref="T:System.IFormatProvider" /> interface implementation that supplies
        ///     culture-specific formatting information.
        /// </param>
        /// <returns>
        ///     An 64-bit signed integer equivalent to the value of this instance.
        /// </returns>
        long IConvertible.ToInt64(IFormatProvider provider)
        {
            return (int) this;
        }

        /// <summary>
        ///     Converts the value of this instance to an equivalent 8-bit signed integer using the specified culture-specific
        ///     formatting information.
        /// </summary>
        /// <param name="provider">
        ///     An <see cref="T:System.IFormatProvider" /> interface implementation that supplies
        ///     culture-specific formatting information.
        /// </param>
        /// <returns>
        ///     An 8-bit signed integer equivalent to the value of this instance.
        /// </returns>
        sbyte IConvertible.ToSByte(IFormatProvider provider)
        {
            return (sbyte) this;
        }

        /// <summary>
        ///     Converts the value of this instance to an equivalent single-precision floating-point number using the specified
        ///     culture-specific formatting information.
        /// </summary>
        /// <param name="provider">
        ///     An <see cref="T:System.IFormatProvider" /> interface implementation that supplies
        ///     culture-specific formatting information.
        /// </param>
        /// <returns>
        ///     A single-precision floating-point number equivalent to the value of this instance.
        /// </returns>
        float IConvertible.ToSingle(IFormatProvider provider)
        {
            return (float) this;
        }

        /// <summary>
        ///     Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <returns>
        ///     A <see cref="System.String" /> that represents this instance.
        /// </returns>
        string IConvertible.ToString(IFormatProvider provider)
        {
            return ToString(null, provider);
        }

        /// <summary>
        ///     Converts the numeric value to an equivalent object. The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="conversionType">The target conversion type.</param>
        /// <param name="provider">An object that supplies culture-specific information about the conversion.</param>
        /// <param name="value">
        ///     When this method returns, contains the value that is equivalent to the numeric value, if the
        ///     conversion succeeded, or is null if the conversion failed. This parameter is passed uninitialized.
        /// </param>
        /// <returns>true if this value was converted successfully; otherwise, false.</returns>
        public bool TryConvert(Type conversionType, IFormatProvider provider, out object value)
        {
            if (conversionType == typeof (bool))
            {
                value = (bool) this;
                return true;
            }

            if (conversionType == typeof (byte))
            {
                value = (byte) this;
                return true;
            }

            if (conversionType == typeof (char))
            {
                value = (char) this;
                return true;
            }

            if (conversionType == typeof (decimal))
            {
                value = (decimal) this;
                return true;
            }

            if (conversionType == typeof (double))
            {
                value = (double) this;
                return true;
            }

            if (conversionType == typeof (short))
            {
                value = (short) this;
                return true;
            }

            if (conversionType == typeof (int))
            {
                value = (int) this;
                return true;
            }

            if (conversionType == typeof (long))
            {
                value = (long) this;
                return true;
            }

            if (conversionType == typeof (sbyte))
            {
                value = (sbyte) this;
                return true;
            }

            if (conversionType == typeof (float))
            {
                value = (float) this;
                return true;
            }

            if (conversionType == typeof (string))
            {
                value = ToString(null, provider);
                return true;
            }

            if (conversionType == typeof (ushort))
            {
                value = (ushort) this;
                return true;
            }

            if (conversionType == typeof (uint))
            {
                value = (uint) this;
                return true;
            }

            if (conversionType == typeof (ulong))
            {
                value = (ulong) this;
                return true;
            }

            if (conversionType == typeof (byte[]))
            {
                value = ToByteArray();
                return true;
            }

            if (conversionType == typeof (Guid))
            {
                value = new Guid(ToByteArray());
                return true;
            }

            value = null;
            return false;
        }

        /// <summary>
        ///     Converts the string representation of a number to its UInt128 equivalent.
        /// </summary>
        /// <param name="value">A string that contains a number to convert.</param>
        /// <returns>
        ///     A value that is equivalent to the number specified in the value parameter.
        /// </returns>
        public static UInt128 Parse(string value)
        {
            return Parse(value, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        /// <summary>
        ///     Converts the string representation of a number in a specified style format to its UInt128 equivalent.
        /// </summary>
        /// <param name="value">A string that contains a number to convert.</param>
        /// <param name="style">A bitwise combination of the enumeration values that specify the permitted format of value.</param>
        /// <returns>
        ///     A value that is equivalent to the number specified in the value parameter.
        /// </returns>
        public static UInt128 Parse(string value, NumberStyles style)
        {
            return Parse(value, style, NumberFormatInfo.CurrentInfo);
        }

        /// <summary>
        ///     Converts the string representation of a number in a culture-specific format to its UInt128 equivalent.
        /// </summary>
        /// <param name="value">A string that contains a number to convert.</param>
        /// <param name="provider">An object that provides culture-specific formatting information about value.</param>
        /// <returns>
        ///     A value that is equivalent to the number specified in the value parameter.
        /// </returns>
        public static UInt128 Parse(string value, IFormatProvider provider)
        {
            return Parse(value, NumberStyles.Integer, provider);
        }

        /// <summary>
        ///     Converts the string representation of a number in a specified style and culture-specific format to its UInt128
        ///     equivalent.
        /// </summary>
        /// <param name="value">A string that contains a number to convert.</param>
        /// <param name="style">A bitwise combination of the enumeration values that specify the permitted format of value.</param>
        /// <param name="provider">An object that provides culture-specific formatting information about value.</param>
        /// <returns>A value that is equivalent to the number specified in the value parameter.</returns>
        public static UInt128 Parse(string value, NumberStyles style, IFormatProvider provider)
        {
            BigInteger bigIntegerValue = BigInteger.Parse(value, style, provider);
            if (bigIntegerValue < 0 || bigIntegerValue > MaxValue)
                throw new OverflowException("Value was either too large or too small for an UInt128.");

            return (UInt128) bigIntegerValue;
        }

        /// <summary>
        ///     Tries to convert the string representation of a number to its UInt128 equivalent, and returns a value that
        ///     indicates
        ///     whether the conversion succeeded..
        /// </summary>
        /// <param name="value">The string representation of a number.</param>
        /// <param name="result">
        ///     When this method returns, contains the UInt128 equivalent to the number that is contained in value,
        ///     or UInt128.Zero if the conversion failed. This parameter is passed uninitialized.
        /// </param>
        /// <returns>
        ///     true if the value parameter was converted successfully; otherwise, false.
        /// </returns>
        public static bool TryParse(string value, out UInt128 result)
        {
            return TryParse(value, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
        }

        /// <summary>
        ///     Tries to convert the string representation of a number in a specified style and culture-specific format to its
        ///     UInt128 equivalent, and returns a value that indicates whether the conversion succeeded..
        /// </summary>
        /// <param name="value">
        ///     The string representation of a number. The string is interpreted using the style specified by
        ///     style.
        /// </param>
        /// <param name="style">
        ///     A bitwise combination of enumeration values that indicates the style elements that can be present
        ///     in value. A typical value to specify is NumberStyles.Integer.
        /// </param>
        /// <param name="provider">An object that supplies culture-specific formatting information about value.</param>
        /// <param name="result">
        ///     When this method returns, contains the UInt128 equivalent to the number that is contained in value,
        ///     or UInt128.Zero if the conversion failed. This parameter is passed uninitialized.
        /// </param>
        /// <returns>true if the value parameter was converted successfully; otherwise, false.</returns>
        public static bool TryParse(string value, NumberStyles style, IFormatProvider provider, out UInt128 result)
        {
            BigInteger bigIntegerValue;
            bool success = BigInteger.TryParse(value, style, provider, out bigIntegerValue);
            if (success && (bigIntegerValue < 0 || bigIntegerValue > MaxValue))
            {
                result = Zero;
                return false;
            }
            result = (UInt128) bigIntegerValue;
            return success;
        }

        /// <summary>
        ///     Converts the value of this instance to an <see cref="T:System.Object" /> of the specified
        ///     <see cref="T:System.Type" /> that has an equivalent value, using the specified culture-specific formatting
        ///     information.
        /// </summary>
        /// <param name="conversionType">The <see cref="T:System.Type" /> to which the value of this instance is converted.</param>
        /// <param name="provider">
        ///     An <see cref="T:System.IFormatProvider" /> interface implementation that supplies
        ///     culture-specific formatting information.
        /// </param>
        /// <returns>
        ///     An <see cref="T:System.Object" /> instance of type <paramref name="conversionType" /> whose value is equivalent to
        ///     the value of this instance.
        /// </returns>
        public object ToType(Type conversionType, IFormatProvider provider)
        {
            object value;
            if (TryConvert(conversionType, provider, out value))
                return value;

            throw new InvalidCastException();
        }

        /// <summary>
        ///     Converts the value of this instance to an equivalent 16-bit unsigned integer using the specified culture-specific
        ///     formatting information.
        /// </summary>
        /// <param name="provider">
        ///     An <see cref="T:System.IFormatProvider" /> interface implementation that supplies
        ///     culture-specific formatting information.
        /// </param>
        /// <returns>
        ///     An 16-bit unsigned integer equivalent to the value of this instance.
        /// </returns>
        ushort IConvertible.ToUInt16(IFormatProvider provider)
        {
            if (_hi != 0)
                throw new OverflowException();

            return Convert.ToUInt16(_lo);
        }

        /// <summary>
        ///     Converts the value of this instance to an equivalent 32-bit unsigned integer using the specified culture-specific
        ///     formatting information.
        /// </summary>
        /// <param name="provider">
        ///     An <see cref="T:System.IFormatProvider" /> interface implementation that supplies
        ///     culture-specific formatting information.
        /// </param>
        /// <returns>
        ///     An 32-bit unsigned integer equivalent to the value of this instance.
        /// </returns>
        uint IConvertible.ToUInt32(IFormatProvider provider)
        {
            if (_hi != 0)
                throw new OverflowException();

            return Convert.ToUInt32(_lo);
        }

        /// <summary>
        ///     Converts the value of this instance to an equivalent 64-bit unsigned integer using the specified culture-specific
        ///     formatting information.
        /// </summary>
        /// <param name="provider">
        ///     An <see cref="T:System.IFormatProvider" /> interface implementation that supplies
        ///     culture-specific formatting information.
        /// </param>
        /// <returns>
        ///     An 64-bit unsigned integer equivalent to the value of this instance.
        /// </returns>
        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            if (_hi != 0)
                throw new OverflowException();

            return _lo;
        }

        /// <summary>
        ///     Compares the current instance with another object of the same type and returns an integer that indicates whether
        ///     the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>
        ///     A value that indicates the relative order of the objects being compared. The return value has these meanings: Value
        ///     Meaning Less than zero This instance is less than <paramref name="obj" />. Zero This instance is equal to
        ///     <paramref name="obj" />. Greater than zero This instance is greater than <paramref name="obj" />.
        /// </returns>
        /// <exception cref="T:System.ArgumentException">
        ///     <paramref name="obj" /> is not the same type as this instance.
        /// </exception>
        int IComparable.CompareTo(object obj)
        {
            return Compare(this, obj);
        }

        /// <summary>
        ///     Compares two UInt128 values and returns an integer that indicates whether the first value is less than, equal to,
        ///     or
        ///     greater than the second value.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns>A signed integer that indicates the relative values of left and right, as shown in the following table.</returns>
        public static int Compare(UInt128 left, object right)
        {
            if (right is UInt128)
                return Compare(left, (UInt128) right);

            // NOTE: this could be optimized type per type
            if (right is bool)
                return Compare(left, new UInt128((bool) right));

            if (right is byte)
                return Compare(left, new UInt128((byte) right));

            if (right is char)
                return Compare(left, new UInt128((char) right));

            if (right is decimal)
                return Compare(left, new UInt128((decimal) right));

            if (right is double)
                return Compare(left, new UInt128((double) right));

            if (right is short)
                return Compare(left, new UInt128((short) right));

            if (right is int)
                return Compare(left, new UInt128((int) right));

            if (right is long)
                return Compare(left, new UInt128((long) right));

            if (right is sbyte)
                return Compare(left, new UInt128((sbyte) right));

            if (right is float)
                return Compare(left, new UInt128((float) right));

            if (right is ushort)
                return Compare(left, new UInt128((ushort) right));

            if (right is uint)
                return Compare(left, new UInt128((uint) right));

            if (right is ulong)
                return Compare(left, new UInt128((ulong) right));

            var bytes = right as byte[];
            if ((bytes != null) && (bytes.Length != 16))
                return Compare(left, new UInt128(bytes));

            if (right is Guid)
                return Compare(left, new UInt128((Guid) right));

            throw new ArgumentException();
        }

        /// <summary>
        ///     Converts an UInt128 value to a byte array.
        /// </summary>
        /// <returns>The value of the current UInt128 object converted to an array of bytes.</returns>
        public byte[] ToByteArray()
        {
            var bytes = new byte[16];
            Buffer.BlockCopy(BitConverter.GetBytes(_lo), 0, bytes, 0, 8);
            Buffer.BlockCopy(BitConverter.GetBytes(_hi), 0, bytes, 8, 8);
            return bytes;
        }

        /// <summary>
        ///     Compares two 128-bit signed integer values and returns an integer that indicates whether the first value is less
        ///     than, equal to, or greater than the second value.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns>
        ///     A signed number indicating the relative values of this instance and value.
        /// </returns>
        public static int Compare(UInt128 left, UInt128 right)
        {
            if (left._hi != right._hi)
                return left._hi.CompareTo(right._hi);
            return left._lo.CompareTo(right._lo);
        }

        /// <summary>
        ///     Compares this instance to a specified 128-bit signed integer and returns an indication of their relative values.
        /// </summary>
        /// <param name="value">An integer to compare.</param>
        /// <returns>A signed number indicating the relative values of this instance and value.</returns>
        public int CompareTo(UInt128 value)
        {
            return Compare(this, value);
        }

        /// <summary>
        ///     Performs an implicit conversion from <see cref="System.Boolean" /> to <see cref="UInt128" />.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns>
        ///     The result of the conversion.
        /// </returns>
        public static implicit operator UInt128(bool value)
        {
            return new UInt128(value);
        }

        /// <summary>
        ///     Performs an implicit conversion from <see cref="System.Byte" /> to <see cref="UInt128" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///     The result of the conversion.
        /// </returns>
        public static implicit operator UInt128(byte value)
        {
            return new UInt128(value);
        }

        /// <summary>
        ///     Performs an implicit conversion from <see cref="System.Char" /> to <see cref="UInt128" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///     The result of the conversion.
        /// </returns>
        public static implicit operator UInt128(char value)
        {
            return new UInt128(value);
        }

        /// <summary>
        ///     Performs an explicit conversion from <see cref="System.Decimal" /> to <see cref="UInt128" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///     The result of the conversion.
        /// </returns>
        public static explicit operator UInt128(decimal value)
        {
            return new UInt128(value);
        }

        /// <summary>
        ///     Performs an explicit conversion from <see cref="System.Double" /> to <see cref="UInt128" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///     The result of the conversion.
        /// </returns>
        public static explicit operator UInt128(double value)
        {
            return new UInt128(value);
        }

        /// <summary>
        ///     Performs an implicit conversion from <see cref="System.Int16" /> to <see cref="UInt128" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///     The result of the conversion.
        /// </returns>
        public static implicit operator UInt128(short value)
        {
            return new UInt128(value);
        }

        /// <summary>
        ///     Performs an implicit conversion from <see cref="System.Int32" /> to <see cref="UInt128" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///     The result of the conversion.
        /// </returns>
        public static implicit operator UInt128(int value)
        {
            return new UInt128(value);
        }

        /// <summary>
        ///     Performs an implicit conversion from <see cref="System.Int64" /> to <see cref="UInt128" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///     The result of the conversion.
        /// </returns>
        public static implicit operator UInt128(long value)
        {
            return new UInt128(value);
        }

        /// <summary>
        ///     Performs an implicit conversion from <see cref="System.SByte" /> to <see cref="UInt128" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///     The result of the conversion.
        /// </returns>
        public static implicit operator UInt128(sbyte value)
        {
            return new UInt128(value);
        }

        /// <summary>
        ///     Performs an explicit conversion from <see cref="System.Single" /> to <see cref="UInt128" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///     The result of the conversion.
        /// </returns>
        public static explicit operator UInt128(float value)
        {
            return new UInt128(value);
        }

        /// <summary>
        ///     Performs an implicit conversion from <see cref="System.UInt16" /> to <see cref="UInt128" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///     The result of the conversion.
        /// </returns>
        public static implicit operator UInt128(ushort value)
        {
            return new UInt128(value);
        }

        /// <summary>
        ///     Performs an implicit conversion from <see cref="System.UInt32" /> to <see cref="UInt128" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///     The result of the conversion.
        /// </returns>
        public static implicit operator UInt128(uint value)
        {
            return new UInt128(value);
        }

        /// <summary>
        ///     Performs an explicit conversion from <see cref="UInt128" /> to <see cref="System.Boolean" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///     The result of the conversion.
        /// </returns>
        public static explicit operator bool(UInt128 value)
        {
            return value != 0;
        }

        /// <summary>
        ///     Performs an explicit conversion from <see cref="UInt128" /> to <see cref="System.Byte" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///     The result of the conversion.
        /// </returns>
        public static explicit operator byte(UInt128 value)
        {
            if (value == 0)
                return 0;

            if (value._lo > 0xFF)
                throw new OverflowException();

            return (byte) value._lo;
        }

        /// <summary>
        ///     Performs an explicit conversion from <see cref="UInt128" /> to <see cref="System.Char" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///     The result of the conversion.
        /// </returns>
        public static explicit operator char(UInt128 value)
        {
            if (value == 0)
                return (char) 0;

            if (value._lo > 0xFFFF)
                throw new OverflowException();

            return (char) (ushort) value._lo;
        }

        /// <summary>
        ///     Performs an explicit conversion from <see cref="UInt128" /> to <see cref="System.Decimal" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///     The result of the conversion.
        /// </returns>
        public static explicit operator decimal(UInt128 value)
        {
            if (value == 0)
                return 0;

            return new decimal((int) (value._lo & 0xFFFFFFFF), (int) (value._lo >> 32), (int) (value._hi & 0xFFFFFFFF), false, 0);
        }

        /// <summary>
        ///     Performs an explicit conversion from <see cref="UInt128" /> to <see cref="System.Double" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///     The result of the conversion.
        /// </returns>
        public static explicit operator double(UInt128 value)
        {
            if (value == 0)
                return 0;

            double d;
            NumberFormatInfo nfi = CultureInfo.InvariantCulture.NumberFormat;
            if (!double.TryParse(value.ToString(nfi), NumberStyles.Number, nfi, out d))
                throw new OverflowException();

            return d;
        }

        /// <summary>
        ///     Performs an explicit conversion from <see cref="UInt128" /> to <see cref="System.Single" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///     The result of the conversion.
        /// </returns>
        public static explicit operator float(UInt128 value)
        {
            if (value == 0)
                return 0;

            float f;
            NumberFormatInfo nfi = CultureInfo.InvariantCulture.NumberFormat;
            if (!float.TryParse(value.ToString(nfi), NumberStyles.Number, nfi, out f))
                throw new OverflowException();

            return f;
        }

        /// <summary>
        ///     Performs an explicit conversion from <see cref="UInt128" /> to <see cref="System.Int16" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///     The result of the conversion.
        /// </returns>
        public static explicit operator short(UInt128 value)
        {
            if (value == 0)
                return 0;

            if (value._lo > 32767)
                throw new OverflowException();

            return (short) ((int) value._lo);
        }

        /// <summary>
        ///     Performs an explicit conversion from <see cref="UInt128" /> to <see cref="System.Int32" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///     The result of the conversion.
        /// </returns>
        public static explicit operator int(UInt128 value)
        {
            if (value == 0)
                return 0;

            if (value._lo > int.MaxValue)
                throw new OverflowException();

            return ((int) value._lo);
        }

        /// <summary>
        ///     Performs an explicit conversion from <see cref="UInt128" /> to <see cref="System.Int64" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///     The result of the conversion.
        /// </returns>
        public static explicit operator long(UInt128 value)
        {
            if (value == 0)
                return 0;

            if (value._lo > long.MaxValue)
                throw new OverflowException();

            return ((long) value._lo);
        }

        /// <summary>
        ///     Performs an explicit conversion from <see cref="UInt128" /> to <see cref="System.UInt32" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///     The result of the conversion.
        /// </returns>
        public static explicit operator uint(UInt128 value)
        {
            if (value == 0)
                return 0;

            if ((value._lo > uint.MaxValue))
                throw new OverflowException();

            return (uint) value._lo;
        }

        /// <summary>
        ///     Performs an explicit conversion from <see cref="UInt128" /> to <see cref="System.UInt16" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///     The result of the conversion.
        /// </returns>
        public static explicit operator ushort(UInt128 value)
        {
            if (value == 0)
                return 0;

            if (value._lo > ushort.MaxValue)
                throw new OverflowException();

            return (ushort) value._lo;
        }

        /// <summary>
        ///     Performs an explicit conversion from <see cref="UInt128" /> to <see cref="System.UInt64" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///     The result of the conversion.
        /// </returns>
        public static explicit operator ulong(UInt128 value)
        {
            if ((value._hi != 0))
                throw new OverflowException("Value was too large for a UInt64.");

            return value._lo;
        }

        /// <summary>
        ///     Implements the operator &gt;.
        /// </summary>
        /// <param name="left">The x.</param>
        /// <param name="right">The y.</param>
        /// <returns>
        ///     The result of the operator.
        /// </returns>
        public static bool operator >(UInt128 left, UInt128 right)
        {
            return Compare(left, right) > 0;
        }

        /// <summary>
        ///     Implements the operator &lt;.
        /// </summary>
        /// <param name="left">The x.</param>
        /// <param name="right">The y.</param>
        /// <returns>
        ///     The result of the operator.
        /// </returns>
        public static bool operator <(UInt128 left, UInt128 right)
        {
            return Compare(left, right) < 0;
        }

        /// <summary>
        ///     Implements the operator &gt;=.
        /// </summary>
        /// <param name="left">The x.</param>
        /// <param name="right">The y.</param>
        /// <returns>
        ///     The result of the operator.
        /// </returns>
        public static bool operator >=(UInt128 left, UInt128 right)
        {
            return Compare(left, right) >= 0;
        }

        /// <summary>
        ///     Implements the operator &lt;=.
        /// </summary>
        /// <param name="left">The x.</param>
        /// <param name="right">The y.</param>
        /// <returns>
        ///     The result of the operator.
        /// </returns>
        public static bool operator <=(UInt128 left, UInt128 right)
        {
            return Compare(left, right) <= 0;
        }

        /// <summary>
        ///     Implements the operator !=.
        /// </summary>
        /// <param name="left">The x.</param>
        /// <param name="right">The y.</param>
        /// <returns>
        ///     The result of the operator.
        /// </returns>
        public static bool operator !=(UInt128 left, UInt128 right)
        {
            return Compare(left, right) != 0;
        }

        /// <summary>
        ///     Implements the operator ==.
        /// </summary>
        /// <param name="left">The x.</param>
        /// <param name="right">The y.</param>
        /// <returns>
        ///     The result of the operator.
        /// </returns>
        public static bool operator ==(UInt128 left, UInt128 right)
        {
            return Compare(left, right) == 0;
        }

        /// <summary>
        ///     Implements the operator +.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///     The result of the operator.
        /// </returns>
        public static UInt128 operator +(UInt128 value)
        {
            return value;
        }

        /// <summary>
        ///     Implements the operator -.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///     The result of the operator.
        /// </returns>
        public static UInt128 operator -(UInt128 value)
        {
            return Negate(value);
        }

        /// <summary>
        ///     Negates a specified UInt128 value.
        /// </summary>
        /// <param name="value">The value to negate.</param>
        /// <returns>The result of the value parameter multiplied by negative one (-1).</returns>
        public static UInt128 Negate(UInt128 value)
        {
            return new UInt128(~value._hi, ~value._lo) + 1;
        }

        /// <summary>
        ///     Gets the absolute value this object.
        /// </summary>
        /// <returns>The absolute value.</returns>
        public UInt128 ToAbs()
        {
            return Abs(this);
        }

        /// <summary>
        ///     Gets the absolute value of an UInt128 object.
        /// </summary>
        /// <param name="value">A number.</param>
        /// <returns>
        ///     The absolute value.
        /// </returns>
        public static UInt128 Abs(UInt128 value)
        {
            return value;
        }

        /// <summary>
        ///     Implements the operator +.
        /// </summary>
        /// <param name="left">The x.</param>
        /// <param name="right">The y.</param>
        /// <returns>
        ///     The result of the operator.
        /// </returns>
        public static UInt128 operator +(UInt128 left, UInt128 right)
        {
            return Add(left, right);
        }

        /// <summary>
        ///     Implements the operator -.
        /// </summary>
        /// <param name="left">The x.</param>
        /// <param name="right">The y.</param>
        /// <returns>
        ///     The result of the operator.
        /// </returns>
        public static UInt128 operator -(UInt128 left, UInt128 right)
        {
            return Subtract(left, right);
        }

        /// <summary>
        ///     Adds two UInt128 values and returns the result.
        /// </summary>
        /// <param name="left">The first value to add.</param>
        /// <param name="right">The second value to add.</param>
        /// <returns>The sum of left and right.</returns>
        public static UInt128 Add(UInt128 left, UInt128 right)
        {
            ulong leastSignificant = left._lo + right._lo;
            bool overflow = (leastSignificant < Math.Max(left._lo, right._lo));
            return new UInt128(left._hi + right._hi + (ulong) (overflow ? 1 : 0), leastSignificant);
        }

        /// <summary>
        ///     Subtracts one UInt128 value from another and returns the result.
        /// </summary>
        /// <param name="left">The value to subtract from (the minuend).</param>
        /// <param name="right">The value to subtract (the subtrahend).</param>
        /// <returns>The result of subtracting right from left.</returns>
        public static UInt128 Subtract(UInt128 left, UInt128 right)
        {
            ulong leastSignificant = left._lo - right._lo;
            bool overflow = (leastSignificant > left._lo);
            return new UInt128(left._hi - right._hi - (ulong) (overflow ? 1 : 0), leastSignificant);
        }

        /// <summary>
        ///     Divides one UInt128 value by another and returns the result.
        /// </summary>
        /// <param name="dividend">The value to be divided.</param>
        /// <param name="divisor">The value to divide by.</param>
        /// <returns>The quotient of the division.</returns>
        public static UInt128 Divide(UInt128 dividend, UInt128 divisor)
        {
            UInt128 integer;
            return DivRem(dividend, divisor, out integer);
        }

        /// <summary>
        ///     Divides one UInt128 value by another, returns the result, and returns the remainder in an output parameter.
        /// </summary>
        /// <param name="dividend">The value to be divided.</param>
        /// <param name="divisor">The value to divide by.</param>
        /// <param name="remainder">
        ///     When this method returns, contains an UInt128 value that represents the remainder from the
        ///     division. This parameter is passed uninitialized.
        /// </param>
        /// <returns>
        ///     The quotient of the division.
        /// </returns>
        public static UInt128 DivRem(UInt128 dividend, UInt128 divisor, out UInt128 remainder)
        {
            if (divisor == 0)
                throw new DivideByZeroException();

            uint[] quotient;
            uint[] rem;
            DivRem(dividend.ToUIn32Array(), divisor.ToUIn32Array(), out quotient, out rem);
            remainder = new UInt128(rem);
            return new UInt128(quotient); // toCheck
        }

        private static void DivRem(uint[] dividend, uint[] divisor, out uint[] quotient, out uint[] remainder)
        {
            const ulong hiBit = 0x100000000;
            int divisorLen = GetLength(divisor);
            int dividendLen = GetLength(dividend);
            if (divisorLen <= 1)
            {
                ulong rem = 0;
                uint div = divisor[0];
                quotient = new uint[dividendLen];
                remainder = new uint[1];
                for (int i = dividendLen - 1; i >= 0; i--)
                {
                    rem *= hiBit;
                    rem += dividend[i];
                    ulong q = rem/div;
                    rem -= q*div;
                    quotient[i] = (uint) q;
                }
                remainder[0] = (uint) rem;
                return;
            }

            if (dividendLen >= divisorLen)
            {
                int shift = GetNormalizeShift(divisor[divisorLen - 1]);
                var normDividend = new uint[dividendLen + 1];
                var normDivisor = new uint[divisorLen];
                Normalize(dividend, dividendLen, normDividend, shift);
                Normalize(divisor, divisorLen, normDivisor, shift);
                quotient = new uint[(dividendLen - divisorLen) + 1];
                for (int j = dividendLen - divisorLen; j >= 0; j--)
                {
                    ulong dx = (hiBit*normDividend[j + divisorLen]) + normDividend[(j + divisorLen) - 1];
                    ulong qj = dx/normDivisor[divisorLen - 1];
                    dx -= qj*normDivisor[divisorLen - 1];
                    do
                    {
                        if ((qj < hiBit) && ((qj*normDivisor[divisorLen - 2]) <= ((dx*hiBit) + normDividend[(j + divisorLen) - 2])))
                            break;

                        qj -= 1L;
                        dx += normDivisor[divisorLen - 1];
                    } while (dx < hiBit);

                    long di = 0;
                    long dj;
                    int index = 0;
                    while (index < divisorLen)
                    {
                        ulong dqj = normDivisor[index]*qj;
                        dj = (normDividend[index + j] - ((uint) dqj)) - di;
                        normDividend[index + j] = (uint) dj;
                        dqj = dqj >> 32;
                        dj = dj >> 32;
                        di = ((long) dqj) - dj;
                        index++;
                    }

                    dj = normDividend[j + divisorLen] - di;
                    normDividend[j + divisorLen] = (uint) dj;
                    quotient[j] = (uint) qj;

                    if (dj < 0)
                    {
                        quotient[j]--;
                        ulong sum = 0;
                        for (index = 0; index < divisorLen; index++)
                        {
                            sum = (normDivisor[index] + normDividend[j + index]) + sum;
                            normDividend[j + index] = (uint) sum;
                            sum = sum >> 32;
                        }
                        sum += normDividend[j + divisorLen];
                        normDividend[j + divisorLen] = (uint) sum;
                    }
                }
                remainder = Unnormalize(normDividend, shift);
                return;
            }

            quotient = new uint[0];
            remainder = dividend;
        }

        private static int GetLength(uint[] uints)
        {
            int index = uints.Length - 1;
            while ((index >= 0) && (uints[index] == 0))
            {
                index--;
            }
            return index + 1;
        }

        private static int GetNormalizeShift(uint ui)
        {
            int shift = 0;
            if ((ui & 0xffff0000) == 0)
            {
                ui = ui << 16;
                shift += 16;
            }

            if ((ui & 0xff000000) == 0)
            {
                ui = ui << 8;
                shift += 8;
            }

            if ((ui & 0xf0000000) == 0)
            {
                ui = ui << 4;
                shift += 4;
            }

            if ((ui & 0xc0000000) == 0)
            {
                ui = ui << 2;
                shift += 2;
            }

            if ((ui & 0x80000000) == 0)
            {
                shift++;
            }
            return shift;
        }

        private static uint[] Unnormalize(uint[] normalized, int shift)
        {
            int len = GetLength(normalized);
            var unormalized = new uint[len];
            if (shift > 0)
            {
                int rshift = 32 - shift;
                uint r = 0;
                for (int i = len - 1; i >= 0; i--)
                {
                    unormalized[i] = (normalized[i] >> shift) | r;
                    r = normalized[i] << rshift;
                }
            }
            else
            {
                for (int j = 0; j < len; j++)
                {
                    unormalized[j] = normalized[j];
                }
            }
            return unormalized;
        }

        private static void Normalize(uint[] unormalized, int len, uint[] normalized, int shift)
        {
            int i;
            uint n = 0;
            if (shift > 0)
            {
                int rshift = 32 - shift;
                for (i = 0; i < len; i++)
                {
                    normalized[i] = (unormalized[i] << shift) | n;
                    n = unormalized[i] >> rshift;
                }
            }
            else
            {
                i = 0;
                while (i < len)
                {
                    normalized[i] = unormalized[i];
                    i++;
                }
            }

            while (i < normalized.Length)
            {
                normalized[i++] = 0;
            }

            if (n != 0)
            {
                normalized[len] = n;
            }
        }

        /// <summary>
        ///     Performs integer division on two UInt128 values and returns the remainder.
        /// </summary>
        /// <param name="dividend">The value to be divided.</param>
        /// <param name="divisor">The value to divide by.</param>
        /// <returns>The remainder after dividing dividend by divisor.</returns>
        public static UInt128 Remainder(UInt128 dividend, UInt128 divisor)
        {
            UInt128 remainder;
            DivRem(dividend, divisor, out remainder);
            return remainder;
        }

        /// <summary>
        ///     Implements the operator %.
        /// </summary>
        /// <param name="dividend">The dividend.</param>
        /// <param name="divisor">The divisor.</param>
        /// <returns>
        ///     The result of the operator.
        /// </returns>
        public static UInt128 operator %(UInt128 dividend, UInt128 divisor)
        {
            return Remainder(dividend, divisor);
        }

        /// <summary>
        ///     Implements the operator /.
        /// </summary>
        /// <param name="dividend">The dividend.</param>
        /// <param name="divisor">The divisor.</param>
        /// <returns>
        ///     The result of the operator.
        /// </returns>
        public static UInt128 operator /(UInt128 dividend, UInt128 divisor)
        {
            return Divide(dividend, divisor);
        }

        /// <summary>
        ///     Converts an int128 value to an unsigned long array.
        /// </summary>
        /// <returns>
        ///     The value of the current UInt128 object converted to an array of unsigned integers.
        /// </returns>
        public ulong[] ToUIn64Array()
        {
            return new[] {_hi, _lo};
        }

        /// <summary>
        ///     Converts an int128 value to an unsigned integer array.
        /// </summary>
        /// <returns>The value of the current UInt128 object converted to an array of unsigned integers.</returns>
        public uint[] ToUIn32Array()
        {
            var ints = new uint[4];
            byte[] lob = BitConverter.GetBytes(_lo);
            byte[] hib = BitConverter.GetBytes(_hi);

            Buffer.BlockCopy(lob, 0, ints, 0, 4);
            Buffer.BlockCopy(lob, 4, ints, 4, 4);
            Buffer.BlockCopy(hib, 0, ints, 8, 4);
            Buffer.BlockCopy(hib, 4, ints, 12, 4);
            return ints;
        }

        /// <summary>
        ///     Returns the product of two UInt128 values.
        /// </summary>
        /// <param name="left">The first number to multiply.</param>
        /// <param name="right">The second number to multiply.</param>
        /// <returns>The product of the left and right parameters.</returns>
        public static UInt128 Multiply(UInt128 left, UInt128 right)
        {
            uint[] xInts = left.ToUIn32Array();
            uint[] yInts = right.ToUIn32Array();
            var mulInts = new uint[8];

            for (int i = 0; i < xInts.Length; i++)
            {
                int index = i;
                ulong remainder = 0;
                foreach (uint yi in yInts)
                {
                    remainder = remainder + (ulong) xInts[i]*yi + mulInts[index];
                    mulInts[index++] = (uint) remainder;
                    remainder = remainder >> 32;
                }

                while (remainder != 0)
                {
                    remainder += mulInts[index];
                    mulInts[index++] = (uint) remainder;
                    remainder = remainder >> 32;
                }
            }
            return new UInt128(mulInts); //toCheck
        }

        /// <summary>
        ///     Implements the operator *.
        /// </summary>
        /// <param name="left">The x.</param>
        /// <param name="right">The y.</param>
        /// <returns>
        ///     The result of the operator.
        /// </returns>
        public static UInt128 operator *(UInt128 left, UInt128 right)
        {
            return Multiply(left, right);
        }

        /// <summary>
        ///     Implements the operator &gt;&gt;.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="shift">The shift.</param>
        /// <returns>The result of the operator.</returns>
        public static UInt128 operator >>(UInt128 value, int shift)
        {
            return RightShift(value, shift);
        }

        /// <summary>
        ///     Implements the operator &lt;&lt;.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="shift">The shift.</param>
        /// <returns>The result of the operator.</returns>
        public static UInt128 operator <<(UInt128 value, int shift)
        {
            return LeftShift(value, shift);
        }


        /// <summary>
        ///     Shifts its first operand right by the number of bits specified by its second operand.
        /// </summary>
        /// <param name="value">The value to shift.</param>
        /// <param name="numberOfBits">The number of bits to shift.</param>
        /// <returns>The value after it was shifted by the given number of bits.</returns>
        public static UInt128 RightShift(UInt128 value, int numberOfBits)
        {
            if (numberOfBits >= 128)
                return Zero;
            if (numberOfBits >= 64)
                return new UInt128(0, value._hi >> (numberOfBits - 64));
            if (numberOfBits == 0)
                return value;
            return new UInt128(value._hi >> numberOfBits, (value._lo >> numberOfBits) + (value._hi << (64 - numberOfBits)));
        }

        /// <summary>
        ///     Shifts its first operand left by the number of bits specified by its second operand.
        /// </summary>
        /// <param name="value">The value to shift.</param>
        /// <param name="numberOfBits">The number of bits to shift.</param>
        /// <returns>The value after it was shifted by the given number of bits.</returns>
        public static UInt128 LeftShift(UInt128 value, int numberOfBits)
        {
            numberOfBits %= 128;
            if (numberOfBits >= 64)
                return new UInt128(value._lo << (numberOfBits - 64), 0);
            if (numberOfBits == 0)
                return value;
            return new UInt128((value._hi << numberOfBits) + (value._lo >> (64 - numberOfBits)), value._lo << numberOfBits);
        }

        /// <summary>
        ///     Implements the operator |.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static UInt128 operator |(UInt128 left, UInt128 right)
        {
            if (left == 0)
                return right;

            if (right == 0)
                return left;

            UInt128 result = left;
            result._hi |= right._hi;
            result._lo |= right._lo;
            return result;
        }

        /// <summary>
        ///     Implements the operator &amp;.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static UInt128 operator &(UInt128 left, UInt128 right)
        {
            return BitwiseAnd(left, right);
        }

        /// <summary>
        ///     Bitwise ands between two values.
        /// </summary>
        /// <param name="left">The first value to do bitwise and.</param>
        /// <param name="right">The second value to do bitwise and.</param>
        /// <returns>The two values after they were bitwise anded.</returns>
        public static UInt128 BitwiseAnd(UInt128 left, UInt128 right)
        {
            return new UInt128(left._hi & right._hi, left._lo & right._lo);
        }

        /// <summary>
        ///     Converts a String type to a UInt128 type, and vice versa.
        /// </summary>
        public class UInt128Converter : TypeConverter
        {
            /// <summary>
            ///     Returns whether this converter can convert an object of the given type to the type of this converter, using the
            ///     specified context.
            /// </summary>
            /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
            /// <param name="sourceType">A <see cref="T:System.Type" /> that represents the type you want to convert from.</param>
            /// <returns>
            ///     true if this converter can perform the conversion; otherwise, false.
            /// </returns>
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                if (sourceType == typeof (string))
                    return true;

                return base.CanConvertFrom(context, sourceType);
            }

            /// <summary>
            ///     Converts the given object to the type of this converter, using the specified context and culture information.
            /// </summary>
            /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
            /// <param name="culture">The <see cref="T:System.Globalization.CultureInfo" /> to use as the current culture.</param>
            /// <param name="value">The <see cref="T:System.Object" /> to convert.</param>
            /// <returns>
            ///     An <see cref="T:System.Object" /> that represents the converted value.
            /// </returns>
            /// <exception cref="T:System.NotSupportedException">
            ///     The conversion cannot be performed.
            /// </exception>
            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                if (value != null)
                {
                    UInt128 i;
                    if (TryParse(string.Format("{0}", value), out i))
                        return i;
                }
                return new UInt128();
            }

            /// <summary>
            ///     Returns whether this converter can convert the object to the specified type, using the specified context.
            /// </summary>
            /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
            /// <param name="destinationType">A <see cref="T:System.Type" /> that represents the type you want to convert to.</param>
            /// <returns>
            ///     true if this converter can perform the conversion; otherwise, false.
            /// </returns>
            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                if (destinationType == typeof (string))
                    return true;

                return base.CanConvertTo(context, destinationType);
            }

            /// <summary>
            ///     Converts the given value object to the specified type, using the specified context and culture information.
            /// </summary>
            /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
            /// <param name="culture">
            ///     A <see cref="T:System.Globalization.CultureInfo" />. If null is passed, the current culture is
            ///     assumed.
            /// </param>
            /// <param name="value">The <see cref="T:System.Object" /> to convert.</param>
            /// <param name="destinationType">The <see cref="T:System.Type" /> to convert the <paramref name="value" /> parameter to.</param>
            /// <returns>
            ///     An <see cref="T:System.Object" /> that represents the converted value.
            /// </returns>
            /// <exception cref="T:System.ArgumentNullException">
            ///     The <paramref name="destinationType" /> parameter is null.
            /// </exception>
            /// <exception cref="T:System.NotSupportedException">
            ///     The conversion cannot be performed.
            /// </exception>
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            {
                if (destinationType == typeof (string))
                    return string.Format("{0}", value);

                return base.ConvertTo(context, culture, value, destinationType);
            }
        }
    }
}